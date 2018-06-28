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
    [Produces("application/json")]
    [Route("api/Flashcard")]
    public partial class FlashcardController : Controller
    {
        private readonly OboeteContext DataToken;

        public FlashcardController(OboeteContext dataToken) => DataToken = dataToken;

        [HttpGet]
        public async Task<ActionResult<IQueryable<FlashcardAction>>> Get()
        {
            return Ok(await FlashcardAction.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<FlashcardAction> Get(int id)
        {
            var flashcard = new FlashcardAction(id);

            if (flashcard == null)
                return NotFound();

            return Ok(flashcard);
        }

        [HttpPost]
        public async Task<ActionResult<Flashcard>> AddFlashcard([FromBody] Flashcard entity)
        {
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //}

            //var flashcardNote = new NoteLogic(entity.NoteId);

            //if (flashcardNote == null)
            //    return StatusCode(422, "Note specified by the NoteID does not exist");

            //var flashcard = new FlashcardLogic(entity);
            //await flashcard.AddToDb();

            //return Created($"/api/Note/{entity.FlashCardId}", entity);
            return null;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var flashcard = new FlashcardAction(id);

            if (flashcard == null)
                return NotFound();

            await flashcard.RemoveFromDb();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FlashcardAction>> UpdateFlashcard(int id, [FromBody] FlashcardAction item)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var flashcard = new FlashcardAction(id);

            if (await flashcard.UpdateEntity(item)) {
                return Ok(flashcard);
            } else {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<NoteAction>> PatchFlashcard(int id, [FromBody] JsonPatchDocument flashcardPatch)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var flashcard = new NoteAction(id);

            await flashcard.PatchEntity(flashcardPatch);

            return Ok(flashcard);
        }
    }
}