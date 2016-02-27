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
    class Program
    {
        private static Table _table;

        private static bool _isDragging;
        private static Card _cardToDrag;

        static void Main(string[] args)
        {
            Utility.PrintControls();

            // Open a 1280x720 window @ 60fps
            RenderWindow window = new RenderWindow(new VideoMode(1280, 720, 32), "Cards!");
            window.SetFramerateLimit(60);

            window.Closed += Exit;
            window.MouseButtonPressed += MouseButtonPressed;
            window.MouseButtonReleased += MouseButtonReleased;
            window.KeyReleased += KeyReleased;

            _table = new Table(window.Size.X, window.Size.Y);

            // Main loop
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Utility.BackgroundColor);

                if (_isDragging)
                {
                    _cardToDrag.Move(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y);
                }

                window.Draw(_table);

                window.Display();
            }
        }

        private static void Exit(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }

        private static void MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            // Only using this for drag events.

            // Check for a card at this position
            Object objectAtPosition = _table.CheckPos(e.X, e.Y);
            if (objectAtPosition != null && objectAtPosition.GetType() == typeof(Card))
            {
                // Enable dragging, pick up the card, move it to the "top" of the table
                Card c = (Card)objectAtPosition;
                _isDragging = true;
                _table.MoveCardToTop(c);
                _cardToDrag = c;
            }
        }

        private static void MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                // Drop the card
                _isDragging = false;
            }
            else
            {
                // Check mouse position for the deck; draw if true
                Object objectAtPosition = _table.CheckPos(e.X, e.Y);
                if (objectAtPosition != null && objectAtPosition.GetType() == typeof(Deck))
                {
                    _table.DrawFromDeck();
                }
            }
        }

        private static void KeyReleased(object sender, KeyEventArgs e)
        {
            if(_isDragging)
            {
                switch(e.Code)
                {
                    case Keyboard.Key.LShift:
                        _cardToDrag.Flip(false);
                        break;
                    case Keyboard.Key.LControl:
                        _table.PlaceCardInDeck(_cardToDrag);
                        break;
                }
            }
            else
            {
                switch(e.Code)
                {
                    case Keyboard.Key.Space:
                        _table.ShuffleDeck();
                        break;
                    case Keyboard.Key.R:
                        _table.ReplaceAllCards();
                        break;
                    case Keyboard.Key.S:
                        _table.SpillCards();
                        break;
                    case Keyboard.Key.F:
                        _table.FlipAll();
                        break;
                    case Keyboard.Key.Escape:
                        Exit(sender, null);
                        break;
                }
            }
        }
    }
}
