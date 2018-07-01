using Oboete.Core;
using Oboete.Database;
using Oboete.Database.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Oboete.Logic
{
    public class FlashcardAction : ActionBase
    {
        #region Constructors 

        public FlashcardAction(OboeteContext context) : base(context) { }

        #endregion

        #region Public Functions

        public void ReviewCard(int cardID, AnswerGrade grade) => ReviewCard(cardID, (int)grade);
        public void ReviewCard(int cardID, int grade)
        {
            var card = DbContext.Flashcards.Find(cardID);

            // Check the card actually exists
            if (card == null) {
                throw new ArgumentException($"Card '{cardID}' does not exist in the database", nameof(cardID));
            }

            ReviewCard(card, grade);
        }
        public void ReviewCard(Flashcard card, AnswerGrade grade) => ReviewCard(card, (int)grade);
        public void ReviewCard(Flashcard card, int grade)
        {
            if (card == null) {
                throw new ArgumentNullException(nameof(card), "Card cannot be null");
            }

            // Validate the grade is within the correct range
            if (grade < 0 || grade > 5) {
                throw new ArgumentOutOfRangeException(nameof(grade), $"Grade has to be between 0 and 5 but a grade of '{grade}' was passed");
            }

            // Reduced form of EaseFactor + (0.1 - (5 - answerQuality) * (0.08 + (5 - answerQuality) * 0.02))
            var easeFactor = card.EaseFactor - 0.8 + (0.28 * grade) - (0.02 * grade * grade);

            // Clamp the ease factor to a minimum of 1.3
            if (easeFactor < 1.3)
                easeFactor = 1.3;

            var intervalIndex = card.IntervalIndex;

            // Update the interval index depending on the answer grade
            if (grade < 3) {
                intervalIndex = 1;
            } else {
                intervalIndex++;
            }

            // Set all the values
            card.UpdateEaseFactor(easeFactor);
            card.UpdateIntervalIndex(intervalIndex);
            card.UpdateNextReviewDateTime(DateTimeOffset.UtcNow.AddDays(GetReviewIntervalDays(intervalIndex, easeFactor)));
            card.UpdateLastReviewDateTime(DateTimeOffset.UtcNow);
        }

        public double GetDaysToNextReview(int cardID)
        {
            var card = DbContext.Flashcards.Find(cardID);

            // Check the card actually exists
            if (card == null) {
                throw new ArgumentException($"Card '{cardID}' does not exist in the database", nameof(cardID));
            }
            
            return GetDaysToNextReview(card);
        }
        public double GetDaysToNextReview(Flashcard card) => (card.NextReviewDateTime - DateTimeOffset.UtcNow).TotalDays;

        #endregion

        #region Static Functions

        public static double GetReviewIntervalDays(int index, double easeFactor)
        {
            if (index < 1)
                throw new System.ArgumentOutOfRangeException("Repetition Index cannot be less than 1");
            if (index == 1)
                return 1;
            if (index == 2)
                return 6;

            return GetReviewIntervalDays(index - 1, easeFactor) * easeFactor;
        }

        #endregion  
    }
}