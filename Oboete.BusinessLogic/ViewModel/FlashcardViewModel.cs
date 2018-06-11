using Oboete.Database.Entity;
using System;

namespace Oboete.BusinessLogic.ViewModel
{
    public class FlashcardViewModel : BaseViewModel<Flashcard>
    {
        #region Private Properties

        private double CurrentReviewInterval { get; set; }

        #endregion

        #region Public Properties

        public double DaysToNextReview  => (Entity.NextReviewDateTime - DateTimeOffset.UtcNow).TotalDays; 
        
        public double EaseFactor {
            get => Entity.EaseFactor;
            private set => Entity.EaseFactor = value;
        }

        public int IntervalIndex {
            get => Entity.IntervalIndex;
            set {
                if (IntervalIndex == value)
                    return;

                Entity.IntervalIndex = value;
                CurrentReviewInterval = GetReviewInterval(IntervalIndex, EaseFactor);
            }
        }

        public int NoteID {
            get => Entity.NoteId;
            private set => Entity.NoteId = value;
        }

        public DateTimeOffset NextReviewDateTime {
            get => Entity.NextReviewDateTime;
            private set => Entity.NextReviewDateTime = value;
        }

        public DateTimeOffset LastReviewDateTime {
            get => Entity.LastReviewDateTime;
            private set => Entity.LastReviewDateTime = value;
        }

        #endregion

        #region Constructors 

        public FlashcardViewModel() : base() { }
        public FlashcardViewModel(Flashcard entity) : base(entity) { }

        #endregion

        #region Public Functions

        public void ReviewCard(AnswerGrade grade) => ReviewCard((int)grade);
        public void ReviewCard(int grade)
        {
            // Reduced form of EaseFactor + (0.1 - (5 - answerQuality) * (0.08 + (5 - answerQuality) * 0.02))
            EaseFactor = EaseFactor - 0.8 + (0.28 * grade) - (0.02 * grade * grade);

            // Ease factor cannot go below 1.3
            if (EaseFactor < 1.3)
                EaseFactor = 1.3;

            if (grade < 3) {
                IntervalIndex = 1;
            } else {
                IntervalIndex++;
            }

            NextReviewDateTime = DateTimeOffset.UtcNow.AddDays(CurrentReviewInterval);
            LastReviewDateTime = DateTimeOffset.UtcNow;
        }

        #endregion

        #region Private Functions
        
        private double GetReviewInterval(int index, double easeFactor)
        {
            if (index < 1)
                throw new System.ArgumentOutOfRangeException("Repetition Index cannot be less than 1");
            if (index == 1)
                return 1;
            if (index == 2)
                return 6;

            return GetReviewInterval(index - 1, easeFactor) * easeFactor;
        }

        #endregion
    }
}