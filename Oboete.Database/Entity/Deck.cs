using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Deck")]
    public class Deck : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeckID { get; set; }
        [Required]
        public string DeckName { get; set; }
        public int OboeteUserID { get; private set; }

        public OboeteUser OboeteUser { get; private set; }

        private ICollection<Note> _notes;
        public IEnumerable<Note> Notes => _notes;

        #region Constructors

        private Deck() { }
        public Deck(string deckName, int oboeteUserID, int deckID = 0)
        {
            if (string.IsNullOrWhiteSpace(deckName))
                throw new ArgumentNullException(nameof(deckName), $"'{nameof(DeckName)}' cannot be Null or white space");

            DeckName = deckName;
            DeckID = deckID;
            OboeteUserID = oboeteUserID;

            _notes = new List<Note>();
        }

        #endregion

        #region Public Accessors

        public Note AddNote(int noteTypeID, DbContext context = null)
        {
            Note newNote = null;

            if (_notes != null) {
                newNote = new Note(noteTypeID);
                _notes.Add(newNote);
            } else if (context == null) {
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(Notes)}' collection isn't valid");
            } else if (context.Entry(this).IsKeySet) {
                newNote = new Note(noteTypeID, DeckID);
                context.Add(newNote);
            } else {
                throw new InvalidOperationException("Could not add a new Note Type Value");
            }

            return newNote;
        }

        public void RemoveNote(Note value, DbContext context = null)
        {
            if (_notes != null)
                _notes.Remove(value);
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(Notes)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Remove(value);
            else
                throw new InvalidOperationException("Could not remove a new Note Type Value");
        }

        #endregion
    }
}
