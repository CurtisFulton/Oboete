using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("FlashcardTemplate")]
    public class FlashcardTemplate : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashcardTemplateID { get; private set; }
        public int NoteTypeID { get; private set; }

        [Required]
        public string FlashcardTemplateName { get; private set; }
        public string CardFrontTemplate { get; private set; }
        public string CardBackTemplate { get; private set; }
        public string CardStyling { get; private set; }

        public NoteType NoteType { get; private set; }
        
        #region Constructors

        private FlashcardTemplate() { }
        internal FlashcardTemplate(string flashcardTemplateName, int noteTypeId = 0, string cardFrontTemplate = "", string cardBackTemplate = "", string cardStyling = "", int flashcardTemplateID = 0)
        {
            if (string.IsNullOrWhiteSpace(flashcardTemplateName))
                throw new ArgumentNullException(nameof(flashcardTemplateName), $"'{nameof(FlashcardTemplateName)}' cannot be Null or white space");

            FlashcardTemplateID = flashcardTemplateID;
            FlashcardTemplateName = flashcardTemplateName;
            NoteTypeID = noteTypeId;
            CardFrontTemplate = cardFrontTemplate;
            CardBackTemplate = cardBackTemplate;
            CardStyling = cardStyling;
        }

        #endregion
        
    }
}