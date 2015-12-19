using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Flare.Framework.Graphics.Fonts
{
    public class BitmapFontRegion
    {
        public char Character { get; set; }
        public Rectangle Rect { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int XAdvance { get; set; }
    }
}
