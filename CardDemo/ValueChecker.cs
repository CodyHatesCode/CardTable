using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace CardDemo
{
    public class ValueChecker : Drawable
    {
        public Vector2f Position { get; private set; }

        private RectangleShape _shape;
        private Text _valueNumber;

        private Table _parent;

        public ValueChecker(float x, float y, Table parent)
        {
            _parent = parent;

            Position = new Vector2f(x, y);

            // Create a little box on the screen that will count any cards within it.
            _shape = new RectangleShape(new Vector2f(175, 175));
            _shape.FillColor = Color.Transparent;
            _shape.OutlineColor = Color.White;
            _shape.OutlineThickness = 2.25f;

            // The text will display the value of the cards.
            _valueNumber = new Text("0", Utility.GlobalFont, 30);
            _valueNumber.Color = Color.White;
        }

        /// <summary>
        /// Update the position, and recalculate the value
        /// </summary>
        public void Update()
        {
            _shape.Position = Position;

            _valueNumber.Position = new Vector2f(
                                            Position.X + (_shape.Size.X / 1.75f),
                                            Position.Y + _shape.Size.Y + 10);

            // Ask the table for all the cards within this boundary
            List<Card> cardsWithin = _parent.CardsWithin(GetBounds());

            int cardValueQty = 0;
            
            // Add their values (+1 because of array)
            foreach (Card c in cardsWithin)
            {
                cardValueQty += (Utility.GetTrueValue((int)c.Value + 1));
            }

            _valueNumber.DisplayedString = cardValueQty.ToString();
        }

        /// <summary>
        /// Get the boundaries of the shape
        /// </summary>
        public FloatRect GetBounds()
        {
            return _shape.GetGlobalBounds();
        }

        /// <summary>
        /// Called by RenderWindow.Draw()
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            Update();

            target.Draw(_shape);
            target.Draw(_valueNumber);
        }
    }
}
