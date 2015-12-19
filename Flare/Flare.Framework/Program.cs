using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Flare.Framework.Test
{
    class Program
    {
        //[STAThread]
        public static void Main()
        {
            using (var game = new Game())// new Examples.Tutorial.TextRendering())
            {
                game.Run(60);
            }
            Console.ReadKey();
        }
    }
}