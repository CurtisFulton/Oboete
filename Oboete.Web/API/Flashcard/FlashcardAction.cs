using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Oboete.Web.API
{
    public partial class FlashcardController : Controller
    {
        [HttpPost("{id}/ReviewCard")]
        public ActionResult ReviewCard(int id)
        {
            return Ok($"Card {id} was reviewed");
        }
    }
}