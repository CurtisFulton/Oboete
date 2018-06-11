using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Note")]
    public class Note : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        
        public string Kana { get; set; }
        public string Kanji { get; set; }
        public string ExampleSentenceJapanese { get; set; }
        
        public string EnglishMeaning { get; set; }
        public string ExampleSentenceEnglish { get; set; }

        public int WordAudioDocumentId { get; set; }
        [ForeignKey("WordAudioDocumentId")]
        public Document WordAudioDocument { get; set; }

        public ICollection<Flashcard> FlashCard { get; set; } = new HashSet<Flashcard>();
    }
}
