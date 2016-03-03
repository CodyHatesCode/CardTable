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
        private static RenderWindow _window;

        private static Table _table;

        private static bool _isDragging;
        private static Card _cardToDrag;

        static void Main(string[] args)
        {
            Utility.ParseResolutionArgs(args);

            Utility.PrintControls();

            // Open a 1280x720 window @ 60fps
            _window = new RenderWindow(new VideoMode(Utility.WindowWidth, Utility.WindowHeight, 32), "Cards!", Styles.Close);
            _window.SetFramerateLimit(60);

            _window.Closed += Exit;
            _window.MouseButtonPressed += MouseButtonPressed;
            _window.MouseButtonReleased += MouseButtonReleased;
            _window.KeyReleased += KeyReleased;

            _table = new Table(_window.Size.X, _window.Size.Y);

            // Main loop
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _window.Clear(Utility.BackgroundColor);

                if (_isDragging)
                {
                    _cardToDrag.Move(Mouse.GetPosition(_window).X, Mouse.GetPosition(_window).Y);
                }

                _window.Draw(_table);

                _window.Display();
            }
        }

        private static void Exit(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }

        private static void MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            // Check for any objects at this position
            Object objectAtPosition = _table.CheckPos(e.X, e.Y);
            if(objectAtPosition != null)
            {
                // Cards (1st priority)
                if(objectAtPosition.GetType() == typeof(Card))
                {
                    // Enable dragging, pick up the card, move it to the "top" of the table
                    Card c = (Card)objectAtPosition;
                    _isDragging = true;
                    _table.MoveCardToTop(c);
                    _cardToDrag = c;
                }
                // Decks
                else if (objectAtPosition != null && objectAtPosition.GetType() == typeof(Deck))
                {
                    // Immediately drag out the first card if it exists
                    Card testCard = _table.DrawFromDeck();
                    if (testCard != null)
                    {
                        _cardToDrag = testCard;
                        _isDragging = true;
                    }
                }
            }
        }

        private static void MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                // Drop the card
                _isDragging = false;
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
