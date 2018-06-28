using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Flashcard")]
    public class Flashcard : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashCardID { get; private set; }
        public int FlashcardTemplateID { get; private set; }
        public int NoteID { get; private set; }

        public double EaseFactor { get; private set; } = 2.5;
        public int IntervalIndex { get; private set; } = 1;
        public DateTimeOffset NextReviewDateTime { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastReviewDateTime { get; private set; } = DateTimeOffset.UtcNow;

        public Note Note { get; private set; }
        public FlashcardTemplate FlashcardTemplate { get; private set; }

        #region Constructor

        private Flashcard() { }
        internal Flashcard(int flashcardTemplateID, int noteID = 0)
        {
            NoteID = noteID;
            FlashcardTemplateID = flashcardTemplateID;
        }

        #endregion

        #region Public Accessors

        public void UpdateEaseFactor(double easeFactor) => EaseFactor = easeFactor;
        public void UpdateIntervalIndex(int intervalIndex) => IntervalIndex = intervalIndex;
        public void UpdateNextReviewDateTime(DateTimeOffset nextReviewDateTime) => NextReviewDateTime = nextReviewDateTime;
        public void UpdateLastReviewDateTime(DateTimeOffset lastReviewDateTime) => LastReviewDateTime = lastReviewDateTime;

        #endregion  
    }
}
