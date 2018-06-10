using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Note")]
    public partial class NoteEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int NoteId { get; set; }

        public ICollection<FlashcardEntity> FlashCard { get; set; } = new HashSet<FlashcardEntity>();
    }
}
