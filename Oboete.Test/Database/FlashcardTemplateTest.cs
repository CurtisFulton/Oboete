using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oboete.Database;
using Oboete.Database.Entity;

namespace Oboete.Test.Database
{
    [TestClass]
    public class FlashcardTemplateTest 
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FlashcardWithEmptyTemplateName()
        {
            var template = new FlashcardTemplate(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FlashcardWithNullTemplateName()
        {
            var template = new FlashcardTemplate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FlashcardWithWhiteSpaceName()
        {
            var template = new FlashcardTemplate(" ");
        }

        [TestMethod]
        public void FlashcardTemplateAddedToExistingNoteType()
        {
            using (var factory = new DbContextFactory()) {
                using (var context = factory.CreateContext()) {
                    var noteType = context.NoteTypes.FirstOrDefault();
                    noteType.AddFlashcardTemplate("Test Template");

                    context.SaveChanges();
                }

                using (var context = factory.CreateContext()) {
                    Assert.AreEqual("Test Template", context.FlashCardTemplates.LastOrDefault().FlashcardTemplateName);
                }
            }
        }

        [TestMethod]
        public void FlashcardTemplateAddedToNewNoteType()
        {
            using (var factory = new DbContextFactory()) {
                using (var context = factory.CreateContext()) {
                    var noteType = new NoteType("New Note Type", 5);
                    context.NoteTypes.Add(noteType);

                    noteType.AddFlashcardTemplate("Test Template");

                    context.SaveChanges();
                }

                using (var context = factory.CreateContext()) {
                    Assert.AreEqual("Test Template", context.FlashCardTemplates.LastOrDefault().FlashcardTemplateName);
                }
            }
        }
    }
}
