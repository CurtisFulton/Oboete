using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oboete.Database;
using Oboete.Database.Entity;

namespace Oboete.Web.API
{
    [Route("API/[controller]")]
    [Authorize]
    public class FlashcardController : ControllerBase<Flashcard>
    {
        public FlashcardController(OboeteContext context) : base(context) { }

        [HttpGet("[action]")]
        public IActionResult Test()
        {
            var test = HttpContext.User;

            return Ok("Logged in");
        }
    }
}