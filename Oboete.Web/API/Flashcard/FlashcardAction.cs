using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Oboete.Logic;

namespace Oboete.Web.API
{
    public partial class FlashcardController : Controller
    {
        [HttpPost("{id}/ReviewCard")]
        public async Task<ActionResult> ReviewCard(int id, int? grade)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (grade == null || grade < 0 || grade > 5)
                return BadRequest($"To review a card you must supply a url parameter '{nameof(grade)}' between 0 and 5");
            
            FlashcardAction flashcard = new FlashcardAction(id);

            await flashcard.ReviewCardAsync((int)grade);

            return Ok(flashcard);
        }
    }
}