using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flare.Framework;
using OpenTK;
using Flare.Framework.Graphics;
using System.Drawing;
using Flare.Framework.Graphics.Fonts;
using OpenTK.Graphics.OpenGL;
using Flare.GUI;

namespace Flare.Demo
{
    class Game : GameBase
    {
        private SpriteBatch spriteBatch;
        private Sprite sprite;
        private Text text;
        private ClockDisplay fpsCounter;

        public Game() : base()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            spriteBatch = new SpriteBatch();
            sprite = new Sprite(new Texture(new Bitmap(@"Content/test.png")), new Vector2(256, 0));
            text = new Text(new BitmapFont("Content/arial_test"), "The quick brown fox \njumps over the lazy dog.", new Vector2(0, 256));
            fpsCounter = new ClockDisplay(Clock, new BitmapFont(@"Content/fps_font"), Vector2.Zero);

            //clockHUD = new ClockHUD(Width, Height, Vector2.Zero);

            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
        {
            Clock.BeginFrame();
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            spriteBatch.Add(sprite);
            spriteBatch.AddString(text);
            spriteBatch.Draw();
            spriteBatch.DrawStrings();

            spriteBatch.Draw(fpsCounter);

            SwapBuffers();
            Clock.EndFrame();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
    }
}
