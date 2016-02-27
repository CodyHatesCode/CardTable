using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace CardDemo
{
    public class Deck : Drawable
    {
        public Vector2f Position { get; private set; }

        private List<Card> _cards;

        private RectangleShape _shape;
        private Text _quantityNumber;

        public int Count { get { return _cards.Count; } }

        public Deck(float x, float y)
        {
            Position = new Vector2f(x, y);

            _cards = new List<Card>();

            // for every suit, add cards with values 0 - 12 (A - K)
            foreach(Suit s in Enum.GetValues(typeof(Suit)))
            {
                for(uint i = 0; i < 13; i++)
                {
                    _cards.Add(new Card(i, s));
                }
            }

            // Create the shape, +5 sq px larger than a normal card
            _shape = new RectangleShape(new Vector2f(Utility.CARD_WIDTH + 5, Utility.CARD_HEIGHT + 5));
            _shape.Origin = new Vector2f(_shape.Size.X / 2.6f, _shape.Size.Y / 3.2f);
            _shape.FillColor = Color.Cyan;
            _shape.OutlineColor = Color.Black;
            _shape.OutlineThickness = 3;

            // Create the text that will display the number of cards remaining
            _quantityNumber = new Text(_cards.Count.ToString(), Utility.GlobalFont, 45);
            _quantityNumber.Style = Text.Styles.Bold;
            _quantityNumber.Color = Color.Black;

            Shuffle();
        }

        /// <summary>
        /// Updates the position of the deck and the numerical display
        /// </summary>
        private void Update()
        {
            _shape.Position = Position;
            _quantityNumber.Position = new Vector2f(Position.X - (Position.X * 0.1f), Position.Y);

            _quantityNumber.DisplayedString = _cards.Count.ToString();
        }

        /// <summary>
        /// Thanks StackOverflow!
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();

            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }

        /// <summary>
        /// Removes the card at the top of the deck
        /// </summary>
        /// <returns></returns>
        public Card DrawCard()
        {
            // sanity check - number of cards in the deck
            if(_cards.Count == 0)
            {
                return null;
            }
            else
            {
                Card drawnCard = _cards[0];
                _cards.Remove(drawnCard);
                return drawnCard;
            }
        }

        /// <summary>
        /// Places a card back on top of the deck
        /// </summary>
        /// <param name="c">The card to replace</param>
        public void ReplaceCard(Card c)
        {
            if(!_cards.Contains(c))
            {
                _cards.Insert(0, c);
            }
        }

        /// <summary>
        /// Gets the boundaries of the shape
        /// </summary>
        /// <returns></returns>
        public FloatRect GetBounds()
        {
            return _shape.GetGlobalBounds();
        }

        /// <summary>
        /// Draws the deck (called by RenderWindow.Draw())
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            Update();

            target.Draw(_shape);
            target.Draw(_quantityNumber);
        }
    }
}
