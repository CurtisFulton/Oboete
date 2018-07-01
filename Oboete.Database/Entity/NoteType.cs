using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Oboete.Database.Entity
{
    [Table("NoteType")]
    public class NoteType : TypeEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteTypeID { get; private set; }
        [Required]
        public string NoteTypeName { get; private set; }
        public int OboeteUserID { get; private set; }

        public OboeteUser OboeteUser { get; private set; }

        private ICollection<NoteTypeFieldDefinition> _noteFields;
        public IEnumerable<NoteTypeFieldDefinition> NoteFields => _noteFields;

        private ICollection<FlashcardTemplate> _flashcardTemplates;
        public IEnumerable<FlashcardTemplate> FlashcardTemplates => _flashcardTemplates;

        #region Constructors

        private NoteType() { }
        public NoteType(string noteTypeName, int userID, int noteTypeID = 0)
        {
            if (string.IsNullOrWhiteSpace(noteTypeName))
                throw new ArgumentNullException(nameof(noteTypeName), $"'{nameof(NoteTypeName)}' cannot be Null or white space");

            NoteTypeName = noteTypeName;
            NoteTypeID = noteTypeID;
            OboeteUserID = userID;

            _noteFields = new List<NoteTypeFieldDefinition>();
            _flashcardTemplates = new List<FlashcardTemplate>();
        }

        #endregion

        #region Public Accessors

        public void AddNoteField(string noteFieldName, string noteFieldDisplay, int sequenceID, DbContext context = null)
        {
            if (_noteFields != null)
                _noteFields.Add(new NoteTypeFieldDefinition(noteFieldName, noteFieldDisplay, sequenceID));
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(NoteFields)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Add(new NoteTypeFieldDefinition(noteFieldName, noteFieldDisplay, sequenceID, NoteTypeID));
            else
                throw new InvalidOperationException("Could not add a new Note Type Value");
        }
        public void RemoveNoteField(NoteTypeFieldDefinition value, DbContext context = null)
        {
            if (_noteFields != null)
                _noteFields.Remove(value);
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(NoteFields)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Remove(value);
            else
                throw new InvalidOperationException("Could not remove a new Note Type Value");
        }

        public void AddFlashcardTemplate(string flashCardTypeName, DbContext context = null)
        {
            if (_flashcardTemplates != null)
                _flashcardTemplates.Add(new FlashcardTemplate(flashCardTypeName));
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(FlashcardTemplates)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Add(new FlashcardTemplate(flashCardTypeName, NoteTypeID));
            else
                throw new InvalidOperationException("Could not add a new Note Type Value");
        }
        public void AddFlashcardTemplate(string flashCardTypeName, string cardFrontTemplate, string cardBackTemplate, string cardStyling, DbContext context = null)
        {
            if (_flashcardTemplates != null)
                _flashcardTemplates.Add(new FlashcardTemplate(flashCardTypeName, 0, cardFrontTemplate, cardBackTemplate, cardStyling));
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(FlashcardTemplates)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Add(new FlashcardTemplate(flashCardTypeName, NoteTypeID, cardFrontTemplate, cardBackTemplate, cardStyling));
            else
                throw new InvalidOperationException("Could not add a new Note Type Value");
        }
        public void RemoveFlashcardTemplate(FlashcardTemplate value, DbContext context = null)
        {
            if (_flashcardTemplates != null)
                _flashcardTemplates.Remove(value);
            else if (context == null)
                throw new ArgumentNullException(nameof(context), $"You must provide a context if the the '{nameof(FlashcardTemplates)}' collection isn't valid");
            else if (context.Entry(this).IsKeySet)
                context.Remove(value);
            else
                throw new InvalidOperationException("Could not remove a new Note Type Value");
        }

        #endregion
    }
}