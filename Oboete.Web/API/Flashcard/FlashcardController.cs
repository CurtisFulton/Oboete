using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oboete.Database;
using Oboete.Database.Entity;

namespace Oboete.Web.API
{
    [Produces("application/json")]
    [Route("api/Flashcard")]
    public partial class FlashcardController : Controller
    {
        private readonly OboeteContext DataToken;

        public FlashcardController(OboeteContext dataToken) => DataToken = dataToken;

        [HttpGet]
        public async Task<ActionResult<List<Flashcard>>> Get()
        {
            return await Task.Run(() => DataToken.Flashcards.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flashcard>> Get(int id)
        {
            var flashcard = await DataToken.Flashcards.FindAsync(id);

            if (flashcard == null)
                return NotFound();

            return Ok(flashcard);
        }

        [HttpPost]
        public async Task<ActionResult<Flashcard>> AddFlashcard([FromBody] Flashcard flashcard)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (await DataToken.Notes.FindAsync(flashcard.NoteId) == null)
                return StatusCode(422, "Note specified by the NoteID does not exist");

            await Task.Run(() => DataToken.Flashcards.Add(flashcard));
            await DataToken.SaveChangesAsync();

            return Created($"/api/Note/{flashcard.FlashCardId}", flashcard);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Flashcard>> UpdateNote(int id, Flashcard item)
        {
            var flashcard = await DataToken.Flashcards.FindAsync(id);
            item.FlashCardId = id;

            if (flashcard == null)
                return NotFound();
            
            await Task.Run(() => DataToken.Entry(flashcard).CurrentValues.SetValues(item));
            await DataToken.SaveChangesAsync();

            return Ok(flashcard);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Flashcard>> DeleteNote(int id)
        {
            var flashcard = await DataToken.Flashcards.FindAsync(id);

            if (flashcard == null)
                return NotFound();

            await Task.Run(() => DataToken.Flashcards.Remove(flashcard));
            await DataToken.SaveChangesAsync();

            return NoContent();
        }
    }
}