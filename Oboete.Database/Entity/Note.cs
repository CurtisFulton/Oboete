using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Oboete.Database.Entity
{
    [Table("Note")]
    public class Note : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteID { get; private set; }
        public int DeckID { get; private set; }
        public int NoteTypeID { get; private set; }

        public Deck Deck { get; private set; }
        public NoteType NoteType { get; private set; }

        private ICollection<NoteFieldValue> _noteValues;
        public IEnumerable<NoteFieldValue> NoteValues => _noteValues;
        
        private ICollection<Flashcard> _flashcards;
        public IEnumerable<Flashcard> Flashcards => _flashcards;

        #region Constructors

        private Note() { }
        internal Note(int noteTypeID, int deckID = 0)
        {
            DeckID = deckID;
            NoteTypeID = noteTypeID;

            _noteValues = new List<NoteFieldValue>();
            _flashcards = new List<Flashcard>();
        }

        #endregion

        #region Public Accessors

        public NoteFieldValue AddValue(int noteTypeFieldDefinitionId, string noteValue, DbContext context = null)
        {
            NoteFieldValue newValue;

            if (_noteValues != null) {
                newValue = new NoteFieldValue(noteTypeFieldDefinitionId, noteValue);
                _noteValues.Add(newValue);
            } else if (context == null) {
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(NoteValues)}' collection isn't valid");
            } else if (context.Entry(this).IsKeySet) {
                newValue = new NoteFieldValue(noteTypeFieldDefinitionId, noteValue, NoteID);
                context.Add(newValue);
            } else {
                throw new InvalidOperationException("Could not add a new Note Type Value");
            }

            return newValue;
        }

        public void RemoveValue(NoteFieldValue value, DbContext context = null)
        {
            if (_noteValues != null)
                _noteValues.Remove(value);
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(Flashcards)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Remove(value);
            else
                throw new InvalidOperationException("Could not remove a new Note Type Value");
        }

        public Flashcard AddFlashcard(int flashcardTemplateID, DbContext context = null)
        {
            Flashcard newValue;

            if (_flashcards != null) {
                newValue = new Flashcard(flashcardTemplateID);
                _flashcards.Add(newValue);
            } else if (context == null) {
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(Flashcards)}' collection isn't valid");
            } else if (context.Entry(this).IsKeySet) {
                newValue = new Flashcard(flashcardTemplateID, NoteID);
                context.Add(newValue);
            } else {
                throw new InvalidOperationException("Could not add a new Note Type Value");
            }

            return newValue;
        }

        public void RemoveFlashcard(Flashcard value, DbContext context = null)
        {
            if (_flashcards != null)
                _flashcards.Remove(value);
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(NoteValues)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Remove(value);
            else
                throw new InvalidOperationException("Could not remove a new Note Type Value");
        }

        public void UpdateDeckID(int newDeck) => DeckID = newDeck;

        #endregion
    }
}
