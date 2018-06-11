using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("FlashcardType")]
    public class FlashcardType : TypeEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlashcardTypeId { get; set; }
        public string FlashcardTypeName { get; set; }

        public IEnumerable<Flashcard> Flashcards { get; set; }
    }
}