using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Flashcard")]
    public class Flashcard : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashCardId { get; set; }

        public int NoteId { get; set; }
        public int FlashcardTypeId { get; set; }
        public double EaseFactor { get; set; } = 2.5;
        public int IntervalIndex { get; set; } = 1;
        public DateTimeOffset NextReviewDateTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastReviewDateTime { get; set; } = DateTimeOffset.UtcNow;

        public Note Note { get; set; }
        public FlashcardType FlashcardType { get; set; }
    }
}
