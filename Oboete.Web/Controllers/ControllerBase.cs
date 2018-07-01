using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Oboete.Database;
using Oboete.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oboete.Web.API
{
    [Produces("application/json")]
    public class ControllerBase<T> : Controller
        where T : BaseEntity
    {
        protected readonly OboeteContext DbContext;
        public ControllerBase(OboeteContext dataToken) => DbContext = dataToken;

        [HttpGet]
        public async Task<ActionResult<IQueryable<T>>> GetAll()
        {
            return Ok(await DbContext.Set<T>().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IQueryable>> GetEntity(int id)
        {
            var flashcard = await DbContext.FindAsync<T>(id);

            if (flashcard == null)
                return NotFound();

            return Ok(flashcard);
        }

        [HttpPost]
        public async Task<ActionResult<T>> AddEntity([FromBody] T entity)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            DbContext.Add(entity);

            try {
                await DbContext.SaveChangesAsync();
            } catch (DbUpdateException ex) {
                // TODO: Log the error with a proper logger. 
                Console.WriteLine(ex.ToString());

                // Return a 422 error to let the requester know it could not be procesed. 
                // In the future there may be a proper 'Validate' to give more descriptive errors
                return StatusCode(422, $"Entity could not be processed likely due to an incorrect foreign key: {ex.ToString()}");
            }

            // TODO: make this return the endpoint for the entity that was created
            //return Created($"/api/", entity);
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEntity(int id)
        {
            T entity = await DbContext.FindAsync<T>(id);

            if (entity == null)
                return NotFound();

            try {
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync();
            } catch (Exception ex) {
                // TODO: Log the error with a proper logger. 
                Console.WriteLine(ex.ToString());
                return StatusCode(422, $"Entity could not be processed: {ex.ToString()}");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<T>> UpdateFlashcard(int id, [FromBody] T item)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            // TODO: Validate that the ID passed is the same as in the body item
            var entity = await DbContext.FindAsync<T>(id);
            var set = DbContext.Set<T>().Update(item);

            try {
                DbContext.Update(item);
                await DbContext.SaveChangesAsync();
                return Ok(entity);
            } catch (Exception ex) {
                // TODO: Log the error with a proper logger. 
                Console.WriteLine(ex.ToString());
                return StatusCode(422, $"Entity could not be processed: {ex.ToString()}");
            }
        }

        
        [HttpPatch("{id}")]
        public async Task<ActionResult<T>> PatchFlashcard(int id, [FromBody] JsonPatchDocument<T> flashcardPatch)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var flashcard = await DbContext.FindAsync<T>(id);
            if (flashcard == null)
                return NotFound();

            try {
                flashcardPatch.ApplyTo(flashcard);
                await DbContext.SaveChangesAsync();
            } catch (Exception ex) {
                // TODO: Log the error with a proper logger. 
                Console.WriteLine(ex.ToString());
                return StatusCode(422, $"Entity could not be processed: {ex.ToString()}");
            }

            return Ok(flashcard);
        }
    }
}
