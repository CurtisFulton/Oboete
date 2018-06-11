using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    [Table("Document")]
    public class Document : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }

        public Guid DocumentName { get; private set; } = Guid.NewGuid();
        [MaxLength(75)]
        public string FileName { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}