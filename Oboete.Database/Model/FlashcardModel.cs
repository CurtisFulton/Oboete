using Oboete.Database;
using Oboete.Database.Entity;
using System;

namespace Oboete.Database.Model
{
    public class FlashcardModel 
    {
        #region Private Variables

        private FlashcardEntity FlashcardEntity { get; set; }
        
        #endregion

        #region Public Variables

        public double DaysToNextReview { get => (FlashcardEntity.NextReviewDateTime - DateTime.UtcNow).TotalDays; }

        public double EaseFactor {
            get => FlashcardEntity.EaseFactor;
            private set => FlashcardEntity.EaseFactor = value;
        }

        public int IntervalIndex {
            get => FlashcardEntity.IntervalIndex;
            private set => FlashcardEntity.IntervalIndex = value;
        }

        #endregion

        #region Constructors

        public FlashcardModel() => FlashcardEntity = new FlashcardEntity();
        public FlashcardModel(FlashcardEntity flashcard) => FlashcardEntity = flashcard;

        #endregion

        #region Public Functions

        public void UpdateEaseFactor(AnswerGrade grade) => UpdateEaseFactor((int)grade);
        public void UpdateEaseFactor(int answerQuality)
        {
            // Reduced form of EaseFactor + (0.1 - (5 - answerQuality) * (0.08 + (5 - answerQuality) * 0.02))
            EaseFactor = EaseFactor - 0.8 + (0.28 * answerQuality) - (0.02 * answerQuality * answerQuality);

            // Ease factor cannot go below 1.3
            if (EaseFactor < 1.3)
                EaseFactor = 1.3;
            
            if (answerQuality < 3) {
                IntervalIndex = 1;
            } else {
                IntervalIndex++;
            }
        }
        
        #endregion

        #region Private Functions

        public static double GetRepetitionInterval(int index, double easeFactor)
        {
            if (index < 1)
                throw new System.ArgumentOutOfRangeException("Repetition Index cannot be less than 1");
            if (index == 1)
                return 1;
            if (index == 2)
                return 6;

            return GetRepetitionInterval(index - 1, easeFactor) * easeFactor;
        }

        #endregion
    }
}