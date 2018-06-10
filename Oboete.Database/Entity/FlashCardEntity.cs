using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Flashcard")]
    public partial class FlashcardEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int FlashCardId { get; set; }

        public int NoteId { get; set; }
        public double EaseFactor { get; set; } = 2.5;
        public int IntervalIndex { get; set; } = 1;
        public DateTimeOffset NextReviewDateTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastReviewDateTime { get; set; } = DateTimeOffset.UtcNow;

        public NoteEntity Note { get; set; }
    }
}
