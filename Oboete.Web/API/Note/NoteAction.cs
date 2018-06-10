using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Oboete.Web.API
{
    public partial class NoteController : Controller
    {
        [HttpPost("{id}/DoSomething")]
        public ActionResult AddNote(int id)
        {
            return Ok("I did something");
        }
    }
}