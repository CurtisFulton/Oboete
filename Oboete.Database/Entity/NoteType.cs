using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("NoteType")]
    public class NoteType : TypeEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteTypeId { get; set; }
        public string NoteTypeName { get; set; }

        public IEnumerable<Note> Notes { get; set; }
    }
}