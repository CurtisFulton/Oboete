using Oboete.Database;
using Oboete.Database.Entity;
using System;

namespace Oboete.Logic.Action
{
    public class DeckAction : ActionBase
    {
        #region Constructors

        public DeckAction(OboeteContext context) : base(context) { }

        #endregion
        
        #region Public Functions

        public void SetDeckParent(Deck deck, Deck parentDeck)
        {
            if (deck == null) 
                throw new ArgumentNullException(nameof(deck), $"Child Deck cannot be null when nesting decks");
            if (parentDeck == null) 
                throw new ArgumentNullException(nameof(parentDeck), $"The Parent deck cannot be null when nesting decks");
            
            // TODO
        }

        #endregion
    }
}