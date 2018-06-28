using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("NoteTypeFieldDefinition")]
    public class NoteTypeFieldDefinition : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteTypeFieldDefinitionID { get; set; }
        public int NoteTypeID { get; set; }

        [Required]
        public string NoteFieldName { get; set; }
        [Required]
        public string NoteFieldDisplay { get; set; }
        public int SequenceID { get; set; } // Order it appears on the note

        public NoteType NoteType { get; set; }

        #region Constructors

        private NoteTypeFieldDefinition() { }
        internal NoteTypeFieldDefinition(string noteFieldName, string noteFieldDisplay, int sequenceID, int noteTypeID = 0, int noteTypeFieldDefinitionID = 0)
        {
            if (string.IsNullOrWhiteSpace(noteFieldName))
                throw new ArgumentNullException(nameof(noteFieldName), $"'{nameof(NoteFieldName)}' cannot be Null or white space");
            
            if (string.IsNullOrWhiteSpace(noteFieldDisplay))
                throw new ArgumentNullException(nameof(noteFieldDisplay), $"'{nameof(NoteFieldDisplay)}' cannot be Null or white space");

            NoteTypeFieldDefinitionID = noteTypeFieldDefinitionID;
            NoteFieldName = noteFieldName;
            NoteFieldDisplay = noteFieldDisplay;
            SequenceID = sequenceID;
            NoteTypeID = noteTypeID;
        }

        #endregion
    }
}