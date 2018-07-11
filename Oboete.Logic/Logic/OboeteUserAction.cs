using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oboete.Core;
using Oboete.Database;
using Oboete.Database.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Oboete.Logic.Logic
{
    public class OboeteUserAction : ActionBase
    {
        #region Constructors

        public OboeteUserAction(OboeteContext context) : base(context) { }

        #endregion

        #region Public Functions

        public IGenericErrorHandler<string> LoginUserIn(string username, string password)
        {
            var errors = new GenericErrorHandler<string>();

            // Check the username and password are correct. Return a generic error no matter which one is incorrect
            if (!CredentialsValid(username, password)) {
                errors.AddError("The username or password is incorrect");
                return errors;
            }

            errors.Result = GenerateAuthenticationToken(username);

            return errors;
        }

        public bool CredentialsValid(string username, string password)
        {
            // Credentials cannot be null or empty, so we automatically know these are invalid without a database check
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            OboeteUser user = GetUser(username);

            // Check a user with that username exists and password match
            if (user == null || !HashMatch(password, user.PasswordHash)) 
                return false;
            
            return true;
        }
        
        public string GenerateAuthenticationToken(string username)
        {
            // Get the user
            OboeteUser user = GetUser(username, true);

            // State the claims this token will have. 
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.OboeteUserID.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim("SecurityStamp", user.SecurityStamp.ToString())
            };

            // Specify the encoding
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            // Create the token object
            var token = new JwtSecurityToken(
                issuer: Config.Issuer,
                audience: Config.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: credentials
            );

            // Encrypt the token and return it as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRememberMeToken(string username)
        {
            OboeteUser user = GetUser(username, true);

            // Generate the GUIDs
            var newToken = Guid.NewGuid().ToString();
            var selector = Guid.NewGuid().ToString();

            // State the claims this token will have. We store the selector, token and security stamp
            var claims = new Claim[] {
                new Claim("SecurityStamp", user.SecurityStamp.ToString()),
                new Claim("Selector", selector),
                new Claim("Token", newToken),
                new Claim(ClaimsIdentity.DefaultNameClaimType, username)
            };

            // Specify the encoding
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            // Create the token object. The remember me token is valid for 12 months
            var token = new JwtSecurityToken(
                issuer: Config.Issuer,
                audience: Config.Audience,
                claims: claims,
                expires: DateTime.Now.AddMonths(12),
                signingCredentials: credentials
            );

            //TODO: Write the token to the database
            var userTokenRecord = new OboeteUserLoginToken(user.OboeteUserID, HashString(newToken), DateTime.Now.AddMonths(12));
            DbContext.OboeteUserLoginTokens.Add(userTokenRecord);
            DbContext.SaveChanges();

            // Encrypt the token and return it as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool RememberMeTokenValid(string cookieToken)
        {
            var claimsPrincipal = GetTokenClaimsPrincipal(cookieToken);

            // Check the token is valid
            if (claimsPrincipal == null) 
                return false;

            // Get the values from the token
            var securityStamp = new Guid(claimsPrincipal.FindFirst("SecurityStamp").Value);
            var selector = new Guid(claimsPrincipal.FindFirst(c => c.Type == "Selector").Value);
            var token = claimsPrincipal.FindFirst(c => c.Type == "Token").Value;
            var cookieUsername = claimsPrincipal.FindFirst(c => c.Type == "ClaimsIdentity.DefaultNameClaimType").Value;
            
            // Get the loginToken record relating to the selector.
            var loginToken = DbContext.OboeteUserLoginTokens.Where(t => t.LoginSelector == selector).Include(nameof(OboeteUserLoginToken.OboeteUser)).SingleOrDefault();

            // If the selector does not 
            if (loginToken == null)
                return false;

            // Check the hashes match
            if (!BCrypt.Net.BCrypt.Verify(token, loginToken.TokenHash)) {
                // TODO: If the selector is valid but the token is not, it is assumed to be theft.
                // Log the user out, remove all current remember me tokens and force a password reset
                return false;
            }

            // Check the security token is still the same
            if (loginToken.OboeteUser.SecurityStamp != securityStamp) {
                // Something on the user has changed since this cookie was issued
                return false;
            }

            return true;
        }
        
        private ClaimsPrincipal GetTokenClaimsPrincipal(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Set the validation params
            TokenValidationParameters validationParameters = new TokenValidationParameters() {
                ValidIssuer = Config.Issuer,
                ValidAudience = Config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.SecretKey))
            };

            // Check the token is in a valid format
            if (!tokenHandler.CanReadToken(tokenString))
                return null;

            // Check the token itself is valid
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);
            
            return claimsPrincipal;
        }

        public string GetUsernameFromRememberMeToken(string tokenString)
        {
            var claimsPrincipal = GetTokenClaimsPrincipal(tokenString);

            if (claimsPrincipal == null)
                return null;

            return claimsPrincipal.FindFirst(c => c.Type == "ClaimsIdentity.DefaultNameClaimType").Value;
        }

        public OboeteUser GetUser(string username, bool mustExist = false)
        {
            OboeteUser user;

            try {
                // Try to get the user
                user = DbContext.OboeteUsers.Where(usr => usr.UserName == username).SingleOrDefault();
            } catch (InvalidOperationException ex) {
                throw new ArgumentException($"The username '{username}' exists more than once in the database", nameof(username), ex);
            }
            
            // Check that the user exists if it is a required value
            if (mustExist && user == null) {
                throw new ArgumentException($"The username '{username}' does not exist in the database, but is required for the next operation", nameof(username));
            }

            return user;
        }

        private bool HashMatch(string value, string hashedValue) => BCrypt.Net.BCrypt.Verify(value + Config.GlobalHashSalt, hashedValue);
        public string HashString(string value, int workFactor = 13)
        {
            var uniqueSalt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);

            return BCrypt.Net.BCrypt.HashPassword(value + Config.GlobalHashSalt, uniqueSalt);
        }

        #endregion
    }
}