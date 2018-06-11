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

        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }

        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardType> FlashCardTypes { get; set; }

        public DbSet<Document> Documents { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlashcardType>().HasData(
                new FlashcardType() { FlashcardTypeId = 1, FlashcardTypeName = "Kana Recognition" },
                new FlashcardType() { FlashcardTypeId = 2, FlashcardTypeName = "Kana Production" }
            );

            modelBuilder.Entity<NoteType>().HasData(
                new NoteType() { NoteTypeId = 1, NoteTypeName = "Vocab" },
                new NoteType() { NoteTypeId = 2, NoteTypeName = "Grammar" }
            );

            modelBuilder.Entity<Document>().HasData(
                new Document() { DocumentId = 1, FileName = "TestDocument.txt" }
            );

            // TODO: Indexes??
        }
    }
}