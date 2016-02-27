using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CardDemo
{
    public class Card : Drawable
    {
        public uint Value { get; private set; }
        public Suit Suit { get; private set; }

        public Vector2f Position { get; private set; }

        private bool _isFaceUp;
        private RectangleShape _shape;
        private Text _faceValue;
        private Text _suitSymbol;
        
        public Card(uint value, Suit suit)
        {
            _isFaceUp = false;

            Value = value;
            Suit = suit;

            // _shape of the card
            _shape = new RectangleShape(new Vector2f(Utility.CARD_WIDTH, Utility.CARD_HEIGHT));
            _shape.Origin = new Vector2f(_shape.Size.X / 2.5f, _shape.Size.Y / 3);
            _shape.OutlineColor = Color.Black;
            _shape.OutlineThickness = 3;

            // sanity check - if "greater" than a king, make it a king
            if(Value > 12) { Value = 12; }

            // create the string to be used on the card
            string valueText = string.Empty;
            switch(Value)
            {
                default:
                    valueText = (Value + 1).ToString();
                    break;
                case 0:
                    valueText = "A";
                    break;
                case 10:
                    valueText = "J";
                    break;
                case 11:
                    valueText = "Q";
                    break;
                case 12:
                    valueText = "K";
                    break;
            }

            // Figure out the suit and color
            string symbol = string.Empty;
            Color cardColor = Color.Black;
            switch(Suit)
            {
                case Suit.Clubs:
                    symbol = "♣";
                    break;
                case Suit.Diamonds:
                    symbol = "♦";
                    cardColor = Color.Red;
                    break;
                case Suit.Hearts:
                    symbol = "♥";
                    cardColor = Color.Red;
                    break;
                case Suit.Spades:
                    symbol = "♠";
                    break;
            }

            // Create the face text
            _faceValue = new Text(valueText, Utility.GlobalFont, 45);
            _faceValue.Style = Text.Styles.Bold;
            _faceValue.Color = cardColor;

            // Create the symbol text
            _suitSymbol = new Text(symbol, Utility.GlobalFont, 40);
            _suitSymbol.Style = Text.Styles.Bold;
            _suitSymbol.Color = cardColor;

            RecalculateShapes();
        }

        /// <summary>
        /// Update the positions, state, and text of the card
        /// </summary>
        private void RecalculateShapes()
        {
            _shape.Position = Position;
            _faceValue.Position = Position;
            _suitSymbol.Position = new Vector2f(
                                        Position.X - (Utility.CARD_WIDTH * 0.3f),
                                        Position.Y - (Utility.CARD_HEIGHT * 0.4f));

            if(_isFaceUp)
            {
                _shape.FillColor = Color.White;
            }
            else
            {
                _shape.FillColor = Color.Cyan;
            }

        }

        /// <summary>
        /// Moves the card to a new position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void Move(float x, float y)
        {
            Position = new Vector2f(x, y);
        }

        /// <summary>
        /// Gets the boundaries of the card
        /// </summary>
        /// <returns></returns>
        public FloatRect GetBounds()
        {
            return _shape.GetGlobalBounds();
        }
        
        /// <summary>
        /// Flips the card over
        /// </summary>
        /// <param name="autoFaceDown">Whether the card should automatically be face down</param>
        public void Flip(bool autoFaceDown)
        {
            // For replacing into the deck, can specify if the card should be face down.
            if(autoFaceDown)
            {
                _isFaceUp = false;
            }
            else
            {
                _isFaceUp = !_isFaceUp;
            }
        }

        /// <summary>
        /// Draws the card (called by RenderWindow.Draw())
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            RecalculateShapes();

            target.Draw(_shape);

            if(_isFaceUp)
            {
                target.Draw(_faceValue);
                target.Draw(_suitSymbol);
            }
        }
    }
}
