using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Oboete.Database.Entity
{
    [Table("Flashcard")]
    public class Flashcard : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashcardID { get; private set; }
        public int FlashcardTemplateID { get; private set; }
        public int NoteID { get; private set; }

        public double EaseFactor { get; private set; } = 2.5;
        public int IntervalIndex { get; private set; } = 1;
        public DateTimeOffset NextReviewDateTime { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastReviewDateTime { get; private set; } = DateTimeOffset.UtcNow;

        [JsonIgnore] public Note Note { get; private set; }
        [JsonIgnore] public FlashcardTemplate FlashcardTemplate { get; private set; }

        #region Constructor

        private Flashcard() { }
        internal Flashcard(int flashcardTemplateID, int noteID = 0, int flashcardID = 0)
        {
            NoteID = noteID;
            FlashcardTemplateID = flashcardTemplateID;
            FlashcardID = flashcardID;
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
