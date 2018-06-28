using System;
using System.Linq;
using System.Threading.Tasks;
using Oboete.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Oboete.Database
{
    public class OboeteContext : DbContext
    {
        #region Db Tables

        public DbSet<Deck> Decks { get; set; }

        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }

        public DbSet<NoteTypeFieldDefinition> NoteTypeFieldDefinitions { get; set; }
        public DbSet<NoteTypeFieldValue> NoteTypeFieldValues { get; set; }

        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardTemplate> FlashCardTemplates { get; set; }

        #endregion  

        public OboeteContext (DbContextOptions<OboeteContext> options) : base(options) { }
        public OboeteContext(string connectionString) : base(GetOptions(connectionString)) { }
        
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

        #region On Model Creating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpDeleteBehaviours(modelBuilder);

            SetUpAggregationNavigationProperties(modelBuilder);
            SetUpUniqueColumns(modelBuilder);

            SetUpIndexes(modelBuilder);

            SeedValues(modelBuilder);
        }

        private void SetUpDeleteBehaviours(ModelBuilder modelBuilder)
        {
            // Need this or we get FK issues due to cascade delete
            modelBuilder.Entity<NoteTypeFieldDefinition>()
                        .HasOne(p => p.NoteType)
                        .WithMany(p => p.NoteFields)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NoteType>()
                        .HasMany(p => p.FlashcardTemplates)
                        .WithOne(p => p.NoteType)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        private void SetUpAggregationNavigationProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deck>().Metadata.FindNavigation(nameof(Deck.Notes))
                                                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            modelBuilder.Entity<NoteType>().Metadata.FindNavigation(nameof(NoteType.NoteFields))
                                                .SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<NoteType>().Metadata.FindNavigation(nameof(NoteType.FlashcardTemplates))
                                                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            modelBuilder.Entity<Note>().Metadata.FindNavigation(nameof(Note.NoteValues))
                                                .SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<Note>().Metadata.FindNavigation(nameof(Note.Flashcards))
                                                .SetPropertyAccessMode(PropertyAccessMode.Field);

        }

        private void SetUpUniqueColumns(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Deck>()
                        .HasIndex(p => p.DeckName)
                        .IsUnique();
            
            modelBuilder.Entity<NoteTypeFieldDefinition>()
                        .HasIndex(p => new { p.NoteTypeID, p.SequenceID })
                        .IsUnique();
            modelBuilder.Entity<NoteTypeFieldDefinition>()
                        .HasIndex(p => new { p.NoteTypeID, p.NoteFieldName })
                        .IsUnique();

            modelBuilder.Entity<NoteType>()
                        .HasIndex(p => p.NoteTypeName)
                        .IsUnique();
            
            modelBuilder.Entity<FlashcardTemplate>()
                        .HasIndex(p => new { p.NoteTypeID, p.FlashcardTemplateName })
                        .IsUnique();
        }

        private void SetUpIndexes(ModelBuilder modelBuilder)
        {
            // TODO
        }

        private void SeedValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deck>().HasData(
                new Deck("Default Deck", 1)
            );

            modelBuilder.Entity<NoteType>().HasData(
                new NoteType("Default Note", 1)
            );

            modelBuilder.Entity<NoteTypeFieldDefinition>().HasData(
                new NoteTypeFieldDefinition("Front", "Front", 1, 1, 1),
                new NoteTypeFieldDefinition("Back", "Back", 2, 1, 2)
            );

            modelBuilder.Entity<FlashcardTemplate>().HasData(
                new FlashcardTemplate("Default Flashcard", 1, "{{Front}}", "{{Back}}", "", 1)
            );
        }

        #endregion
        
        private static DbContextOptions GetOptions(string connectionString) => SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
    }
}