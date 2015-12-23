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
    class Demo
    {
        private SpriteBatch spriteBatch;
        private Sprite sprite;
        private Text text;
        private ClockDisplay fpsCounter;

        public Demo()
        {
            Game.Load += OnLoad;
            Game.RenderFrame += OnRenderFrame;
            Game.UpdateFrame += OnUpdateFrame;
            Game.Run(60.0f);
        }

        protected void OnLoad(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch();
            sprite = new Sprite(new Texture(new Bitmap(@"Content/test.png")), new Vector2(256, 0));
            text = new Text(new BitmapFont("Content/arial_test"), "The quick brown fox \njumps over the lazy dog.", new Vector2(0, 256));
            fpsCounter = new ClockDisplay(Game.Clock, new BitmapFont(@"Content/fps_font"), Vector2.Zero);

            //clockHUD = new ClockHUD(Width, Height, Vector2.Zero);
        }

        protected void OnRenderFrame(object sender, FrameEventArgs e)
        {
            Game.Clock.BeginFrame();
            Game.Clear(Color.CornflowerBlue);

            spriteBatch.Add(sprite);
            spriteBatch.AddString(text);
            spriteBatch.Draw();
            spriteBatch.DrawStrings();

            spriteBatch.Draw(fpsCounter);

            Game.SwapBuffers();
            Game.Clock.EndFrame();
        }

        protected void OnUpdateFrame(object sender, FrameEventArgs e)
        {
        }
    }
}
