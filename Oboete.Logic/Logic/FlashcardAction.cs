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

        public IGenericErrorHandler ReviewCard(int cardID, AnswerGrade grade) => ReviewCard(cardID, (int)grade);
        public IGenericErrorHandler ReviewCard(int cardID, int grade)
        {
            var card = DbContext.Flashcards.Find(cardID);
            var errorHandler = new GenericErrorHandler();

            // Check the card actually exists
            if (card == null) {
                errorHandler.AddError($"No Card with the ID {cardID} exists in the database");
                return errorHandler;
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
            
            try {
                // Set all the values
                card.UpdateEaseFactor(easeFactor);
                card.UpdateIntervalIndex(intervalIndex);
                card.UpdateNextReviewDateTime(DateTimeOffset.UtcNow.AddDays(GetReviewIntervalDays(intervalIndex, easeFactor)));
                card.UpdateLastReviewDateTime(DateTimeOffset.UtcNow);
            } catch (Exception ex) {
                // Add any unexpected errors to the error handler
                errorHandler.AddError($"An unexpected exception occured: {ex.ToString()}");
            }
            
            return errorHandler;
        }

        public IGenericErrorHandler<double> GetDaysToNextReview(int cardID)
        {
            var card = DbContext.Flashcards.Find(cardID);
            var errorHandler = new GenericErrorHandler<double>();

            // Check the card actually exists
            if (card == null) {
                errorHandler.AddError($"No Card with the ID {cardID} exists in the database");
                return errorHandler;
            }
            
            // Store the result in the error handler
            errorHandler.Result = (card.NextReviewDateTime - DateTimeOffset.UtcNow).TotalDays;
            return errorHandler;
        }

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