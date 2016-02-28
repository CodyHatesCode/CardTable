using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CardDemo
{
    public static class Utility
    {
        // probably won't scale well if this is changed
        public const float CARD_WIDTH = 100;
        public const float CARD_HEIGHT = 130;

        public static uint WindowWidth = 1280;
        public static uint WindowHeight = 720;

        public static Font GlobalFont = new Font("C:\\Windows\\Fonts\\arial.ttf");
        public static Color BackgroundColor = new Color(0, 151, 51);


        public static void PrintControls()
        {
            Console.WriteLine("SFML.NET Cards Demo");
            Console.WriteLine("By Cody - Feb 25, 2016");
            Console.WriteLine("=========================");
            Console.WriteLine("Space\t\tShuffle deck");
            Console.WriteLine("S\t\tSpill cards");
            Console.WriteLine("R\t\tReplace all cards");
            Console.WriteLine("F\t\tFlip all cards");
            Console.WriteLine("-- While dragging: ------");
            Console.WriteLine("Ctrl\t\tReplace card");
            Console.WriteLine("Shift\t\tFlip card over");
            Console.WriteLine("-------------------------");
            Console.WriteLine("Esc\t\tQuit");
            Console.WriteLine("=========================");
            Console.WriteLine("Drop cards within the white box to indicate their combined values.");
        }

        /// <summary>
        /// Uses the command line arguments to determine the resolution of the window/table
        /// </summary>
        /// <param name="args"></param>
        public static void ParseResolutionArgs(string[] args)
        {
            uint x;
            uint y;

            if(args.Length == 2)
            {
                if(uint.TryParse(args[0], out x) && uint.TryParse(args[1], out y))
                {
                    WindowWidth = x;
                    WindowHeight = y;
                }
            }
        }
    }
}
