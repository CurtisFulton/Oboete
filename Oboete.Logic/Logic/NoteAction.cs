using Oboete.Database;
using Oboete.Database.Entity;
using System;

namespace Oboete.Logic.Action
{
    public class NoteAction : ActionBase
    {
        #region Constructors 

        public NoteAction(OboeteContext context) : base(context) { }

        #endregion

        #region Public Functions

        public void ChangeNoteDeck(Note note, Deck targetDeck)
        {
            if (note == null) 
                throw new ArgumentNullException(nameof(note), $"Note cannot be null when changing decks");
            if (targetDeck == null) 
                throw new ArgumentNullException(nameof(targetDeck), $"The target deck cannot be null when changing note decks");
            

            note.UpdateDeckID(targetDeck.DeckID);
        }

        #endregion
    }
}