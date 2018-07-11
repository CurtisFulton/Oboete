using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Oboete.Database;
using Oboete.Database.Entity;
using Oboete.Logic.Logic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Oboete.Web.API
{
    [Route("API/[controller]")]
    public class UserController : ControllerBase<OboeteUser>
    {
        public UserController(OboeteContext context) : base(context) { }

        [HttpGet("[action]")]
        public ActionResult Test()
        {
            var test = Request.Cookies;

            return Ok(test);
        }

        [HttpPost("[action]")]
        public ActionResult Login([FromBody] JObject data)
        {
            if (data == null) 
                return StatusCode(401, "No login credentials supplied");
            
            var userAction = new OboeteUserAction(DbContext);
            
            var username = data["Username"].ToObject<string>();
            var password = data["Password"].ToObject<string>();
            bool rememberMe = data.ContainsKey("RememberMe") ? data["RememberMe"].ToObject<bool>() : false;

            var loginResult = userAction.LoginUserIn(username, password);

            if (loginResult.HasErrors) {
                return StatusCode(401, loginResult.Errors[0]);
            }

            return Ok(new {
                token = loginResult.Result
            });
        }

        [HttpGet("[action]")]
        public ActionResult Signin()
        {
            var test = new JObject();
            test["Username"] = "Temp User";
            test["Password"] = "Zmxncbv109";

            return Login(test);
        }
    }
}