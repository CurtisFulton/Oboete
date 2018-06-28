using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oboete.Database;
using Oboete.Database.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Oboete.Logic;

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
        public async Task<ActionResult<IQueryable<NoteAction>>> Get()
        {
            return Ok(await NoteAction.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<NoteAction> Get(int id)
        {
            var note = new NoteAction(id);

            if (note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<NoteAction>> AddNote([FromBody] Note entity)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var note = new NoteAction(entity);
            await note.AddToDb();

            return Created($"/api/Note/{note.NoteId}", note);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var note = new NoteAction(id);

            if (note == null)
                return NotFound();

            await note.RemoveFromDb();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteAction>> UpdateNote(int id, [FromBody] NoteAction item)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var note = new NoteAction(id);
            
            if (await note.UpdateEntity(item)) {
                return Ok(note);
            } else {
                return BadRequest();
            }
        }
        
        [HttpPatch("{id}")]
        public async Task<ActionResult<NoteAction>> PatchNote(int id, [FromBody] JsonPatchDocument notePatch)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var note = new NoteAction(id);

            await note.PatchEntity(notePatch);

            return Ok(note);
        }
    }
}