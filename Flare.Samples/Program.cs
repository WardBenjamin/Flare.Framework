using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Flare;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("Flare Samples", 800, 600);
            //game.Window.IsFullscreen = true;
            Game.Draw += Game_Draw;
            Game.Update += Game_Update;
            game.Run();
        }

        private static void Game_Update(GameTime time)
        {
            Console.WriteLine("Update: " + time);
        }

        private static void Game_Draw(GameTime time)
        {
            Console.WriteLine("Draw: " + time);
        }
    }
}
