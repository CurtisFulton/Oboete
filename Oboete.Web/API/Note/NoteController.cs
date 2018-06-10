using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oboete.Database;
using Oboete.Database.Model;
using Oboete.Database.Entity;

namespace Oboete.Web.API
{
    [Produces("application/json")]
    [Route("API/Note")]
    public partial class NoteController : Controller
    {
        private readonly OboeteContext DataToken;

        public NoteController(OboeteContext dataToken)
        {
            DataToken = dataToken;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<NoteEntity>>> Get()
        {
            return await Task.Run(() => DataToken.Notes.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteEntity>> Get(int id)
        {
            var note = await DataToken.Notes.FindAsync(id);

            if (note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<NoteEntity>> AddNote(NoteEntity note)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await DataToken.Notes.AddAsync(note);
            await DataToken.SaveChangesAsync();

            return Created($"/api/Note/{note.NoteId}", note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteEntity>> UpdateNote(int id, NoteEntity item)
        {
            var note = await DataToken.Notes.FindAsync(id);
            item.NoteId = id;

            if (note == null)
                return NotFound();

            await Task.Run(() => DataToken.Entry(note).CurrentValues.SetValues(item));
            await DataToken.SaveChangesAsync();

            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NoteEntity>> DeleteNote(int id)
        {
            var note = await DataToken.Notes.FindAsync(id);

            if (note == null)
                return NotFound();

            await Task.Run(() => DataToken.Notes.Remove(note));
            await DataToken.SaveChangesAsync();

            return NoContent();
        }
    }
}