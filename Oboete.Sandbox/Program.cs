using Microsoft.EntityFrameworkCore;
using Oboete.Database;
using Oboete.Database.Entity;
using Oboete.Logic;
using System;
using System.Linq;

namespace Oboete.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sandbox Started");

            Config.ConnectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=OboeteDB;User Id=Oboete;Password=Lolop109;Integrated Security=false;MultipleActiveResultSets=True;";

            using (OboeteContext context = new OboeteContext(Config.ConnectionString)) {
                context.Database.Migrate();

                Console.WriteLine("Database Migrated");

                CreateNewNoteTypeAndFlashcardTemplates(context);
                context.SaveChanges();

                CreateNewDeckAndAddValues(context);
                context.SaveChanges();
            }
            
            Console.WriteLine("Finished running");
            Console.ReadKey();
        }
        
        private static void CreateNewDeckAndAddValues(OboeteContext context)
        {
            // Create a new Deck. There is a unique contraint on the Deck Name so it will fail if this name is duplicated
            Deck deck = new Deck("New Deck");

            // Add a new note to the Deck
            Note note = deck.AddNote(1);

            // Add values for the fields on the note
            note.AddNoteValue(1, "New Front Example");
            note.AddNoteValue(2, "New Back Example");

            var firstFlashcardTemplate = context.FlashCardTemplates.FirstOrDefault();
            var lastFlashcardTemplate = context.FlashCardTemplates.LastOrDefault();

            // Add flashcard
            note.AddFlashcard(firstFlashcardTemplate.FlashcardTemplateID);
            note.AddFlashcard(lastFlashcardTemplate.FlashcardTemplateID);

            // Add the deck to the DB Set
            context.Decks.Add(deck);
        }

        private static void CreateNewNoteTypeAndFlashcardTemplates(OboeteContext context)
        {
            // Create the new Note type
            NoteType noteType = new NoteType("New Note Type");

            // Add a new Flashcard Template
            noteType.AddFlashcardTemplate("New Flashcard");

            // Add new fields to this note type
            noteType.AddNoteField("CardFront", "Card Front", 1);
            noteType.AddNoteField("CardBack", "Card Back", 2);
            noteType.AddNoteField("CardBack2", "Card Back", 3);

            // Add the note type to the DB Set
            context.NoteTypes.Add(noteType);
        }
    }
}
