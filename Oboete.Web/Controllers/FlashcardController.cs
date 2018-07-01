using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oboete.Database;
using Oboete.Database.Entity;
using Oboete.Logic;

namespace Oboete.Web.API
{
    [Route("api/[controller]")]
    public class FlashcardController : ControllerBase<Flashcard>
    {
        public FlashcardController(OboeteContext context) : base(context) { }

        
    }
}