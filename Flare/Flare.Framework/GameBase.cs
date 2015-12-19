using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;
using Flare.Framework.Graphics;
using Flare.Framework.Graphics.Fonts;
using OpenTK.Graphics;

namespace Flare.Framework
{
    public class GameBase : GameWindow
    {
        public static Vector2 TargetSize { get; set; }
        public static Utility.FrameClock Clock { get; private set; }

        public GameBase() : base(800, 600, new GraphicsMode(32, 24, 0, 4), "Flare.Framework Game Window",
            GameWindowFlags.Default, DisplayDevice.Default, 3, 0, 
            GraphicsContextFlags.Debug | GraphicsContextFlags.ForwardCompatible) {}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TargetSize = new Vector2(Width, Height);
            if (Clock == null)
                Clock = new Utility.FrameClock();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }
    }
}
