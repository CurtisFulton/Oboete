using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("NoteFieldValue")]
    public class NoteFieldValue : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteFieldValueId { get; set; }
        public int NoteTypeFieldDefinitionId { get; set; }
        public int NoteId { get; set; }

        public string NoteValue { get; set; }

        public NoteTypeFieldDefinition NoteTypeFieldDefinition { get; set; }
        public Note Note { get; set; }

        #region Constructors 

        private NoteFieldValue() { }
        internal NoteFieldValue(int noteTypeFieldDefinitionId, string noteValue, int noteId = 0)
        {
            NoteTypeFieldDefinitionId = noteTypeFieldDefinitionId;
            NoteId = noteId;
            NoteValue = noteValue;
        }

        #endregion  

    }
}