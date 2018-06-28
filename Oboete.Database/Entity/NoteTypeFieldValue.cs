using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("NoteTypeFieldValue")]
    public class NoteTypeFieldValue : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteTypeFieldValueId { get; set; }
        public int NoteTypeFieldDefinitionId { get; set; }
        public int NoteId { get; set; }

        public string NoteValue { get; set; }

        public NoteTypeFieldDefinition NoteTypeFieldDefinition { get; set; }
        public Note Note { get; set; }

        #region Constructors 

        private NoteTypeFieldValue() { }
        internal NoteTypeFieldValue(int noteTypeFieldDefinitionId, string noteValue, int noteId = 0)
        {
            NoteTypeFieldDefinitionId = noteTypeFieldDefinitionId;
            NoteId = noteId;
            NoteValue = noteValue;
        }

        #endregion  

    }
}