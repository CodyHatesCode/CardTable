using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CardDemo
{
    public class Table : Drawable
    {
        private Deck _deck;

        private List<Card> _cards;
        private List<ValueChecker> _valueCheckers;

        private Vector2f _windowSize;

        public Table(float windowX, float windowY)
        {
            _deck = new Deck(50, 50);

            _valueCheckers = new List<ValueChecker>
            {
                new ValueChecker(windowX - 200, windowY - 250, this),
                new ValueChecker(windowX - 500, windowY - 250, this)
            };

            _cards = new List<Card>();

            _windowSize = new Vector2f(windowX, windowY);
        }

        /// <summary>
        /// Removes a card from the deck and places it onto the table
        /// </summary>
        public Card DrawFromDeck()
        {
            Card c = _deck.DrawCard();

            // Drawables are drawn in descending order, so the end of the list is the "top" layer.
            if(c != null)
            {
                _cards.Insert(_cards.Count, c);
                c.Move(_deck.Position.X, _deck.Position.Y);
            }

            return c;
        }

        /// <summary>
        /// Attempts to find a deck or card at the given coordinates
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns></returns>
        public Object CheckPos(int x, int y)
        {
            // Find cards first (on top of deck)
            Card cToReturn = null;

            // Get all this cards at this position (up from the bottom layer), always return the last one
            var cardsAtPosition = from c in _cards where c.GetBounds().Contains(x, y) select c;
            if (cardsAtPosition.Count() > 0)
            {
                cToReturn = cardsAtPosition.Last();
            }


            // If there were no cards at this position, look for the deck
            if(cToReturn == null)
            {
                if(_deck.GetBounds().Contains(x, y))
                {
                    return _deck;
                }
            }

            return cToReturn;
        }

        /// <summary>
        /// Moves a stray card to the top layer
        /// </summary>
        /// <param name="c">The card to move</param>
        public void MoveCardToTop(Card c)
        {
            if(_cards.Remove(c))
            {
                _cards.Add(c);
            }
        }

        /// <summary>
        /// Places a card back into the deck
        /// </summary>
        /// <param name="c">The card to replace</param>
        public void PlaceCardInDeck(Card c)
        {
            if(_cards.Remove(c))
            {
                c.Flip(true);
                _deck.ReplaceCard(c);
            }
        }

        /// <summary>
        /// Spills all cards out of the deck onto the table
        /// </summary>
        public void SpillCards()
        {
            Random rng = new Random();

            while(_deck.Count > 0)
            {
                Card c = DrawFromDeck();

                // Choose a random position on the table to move this card to
                Vector2f nextPos = new Vector2f(rng.Next(0, (int)_windowSize.X), rng.Next(0, (int)_windowSize.Y));
                c.Move(nextPos.X, nextPos.Y);

                // Randomly decide whether to flip it or not
                if(rng.Next(0, 101) > 50)
                {
                    c.Flip(false);
                }
            }
        }

        /// <summary>
        /// Replaces all cards on the table into the deck.
        /// </summary>
        public void ReplaceAllCards()
        {
            // Need to work with a duplicate of the card list or the program will bomb during Foreach.
            List<Card> temporaryCardList = new List<Card>(_cards);

            // Reverse it to pick up from the top of the layer.
            temporaryCardList.Reverse();

            foreach(Card c in temporaryCardList)
            {
                PlaceCardInDeck(c);
            }
        }

        /// <summary>
        /// Flips all cards on the table
        /// </summary>
        public void FlipAll()
        {
            foreach(Card c in _cards)
            {
                c.Flip(false);
            }
        }

        /// <summary>
        /// Calls Shuffle() on the associated deck
        /// </summary>
        public void ShuffleDeck()
        {
            _deck.Shuffle();
        }

        public List<Card> CardsWithin(FloatRect area)
        {
            var cardsIntersecting = from c in _cards where area.Intersects(c.GetBounds()) select c;

            return cardsIntersecting.ToList<Card>();
        }

        /// <summary>
        /// Draws everything on the table (called by RenderWindow.Draw())
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_deck);

            foreach(ValueChecker vC in _valueCheckers)
            {
                target.Draw(vC);
            }

            foreach(Card c in _cards)
            {
                target.Draw(c);
            }
        }
    }
}
