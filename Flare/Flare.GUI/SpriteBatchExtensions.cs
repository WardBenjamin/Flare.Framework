using Flare.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare.GUI
{
    public static class SpriteBatchExtensions
    {
        static SpriteBatchExtensions() { }
        public static void Draw(this SpriteBatch spriteBatch, ClockDisplay display)
        {
            display.Draw(spriteBatch);
        }
    }
}
