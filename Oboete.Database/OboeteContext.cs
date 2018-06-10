using System;
using System.Linq;
using System.Threading.Tasks;
using Oboete.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Oboete.Database
{
    public class OboeteContext : DbContext
    {
        public OboeteContext (DbContextOptions<OboeteContext> options) : base(options) { }

        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<FlashcardEntity> Flashcards { get; set; }

        public override int SaveChanges()
        {
            OnSaveChanges();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            OnSaveChanges();
            return await base.SaveChangesAsync();
        }

        private void OnSaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities) {
                ((BaseEntity)entity.Entity).ModifiedDateTime = DateTimeOffset.UtcNow;
            }
        }
    }
}