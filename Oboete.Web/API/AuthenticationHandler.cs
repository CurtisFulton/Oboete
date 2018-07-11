using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oboete.Database;
using Oboete.Database.Entity;
using Oboete.Logic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Oboete.Web.API
{
    public static class JwtAuthenticationExtensions
    {
        public static AuthenticationBuilder UseCustomJwtAuthentication(this AuthenticationBuilder builder, Action<JwtAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<JwtAuthenticationOptions, JwtAutenticationHandler>(JwtAuthenticationOptions.DefaultScheme, configureOptions);
        }
    }

    public class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Jwt Auth";
        public string Scheme => DefaultScheme;
    }

    public class JwtAutenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private OboeteContext DbContext { get; set; }

        public JwtAutenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, OboeteContext context) : base(options, logger, encoder, clock)
        {
            DbContext = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"];
            
            if (authHeader.Count == 0 || !IsTokenValid(authHeader))
                return AuthenticateResult.Fail("Failed to Authenticate");

            var claims = GetTokenClaimsPrincipal(authHeader);
            return AuthenticateResult.Success(new AuthenticationTicket(GetTokenClaimsPrincipal(authHeader), JwtAuthenticationOptions.DefaultScheme));
        }

        private bool IsTokenValid(string tokenString)
        {
            var claimsPrincipal = GetTokenClaimsPrincipal(tokenString);

            // Check the token is valid
            if (claimsPrincipal == null)
                return false;

            // Get the values from the token
            var securityStamp = new Guid(claimsPrincipal.FindFirst("SecurityStamp").Value);
            var tokenUsername = claimsPrincipal.FindFirst(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value;

            var dbSecurityStamp = DbContext.OboeteUsers.Where(user => user.UserName == tokenUsername).SingleOrDefault().SecurityStamp;

            // Check the security token is still the same
            if (dbSecurityStamp != securityStamp) {
                // Something on the user has changed since this cookie was issued
                return false;
            }

            return true;
        }

        private ClaimsPrincipal GetTokenClaimsPrincipal(string tokenString)
        {
            tokenString = tokenString.Replace("Bearer ", string.Empty);

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
    }
}
