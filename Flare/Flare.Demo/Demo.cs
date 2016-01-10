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
        private Texture texture;
        private Sprite sprite;
        private BitmapFont arial;
        private Text text;
        private BitmapFont fpsFont;
        private ClockDisplay fpsCounter;

        public Demo()
        {
            Game.Load += OnLoad;
            Game.RenderFrame += OnRenderFrame;
            Game.UpdateFrame += OnUpdateFrame;
            Game.Unload += OnUnload;
            Game.Run(60.0f);
            Game.UpdateFrame += (sender, e) => { GC.Collect(); };
        }

        private void OnUnload(object sender, EventArgs e)
        {
            sprite.Dispose();
            texture.Dispose();
            text.Dispose();
            arial.Dispose();
            fpsCounter.Dispose();
            fpsFont.Dispose();
        }

        protected void OnLoad(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch();
            texture = new Texture(new Bitmap(@"Content/test.png"));
            sprite = new Sprite(texture);
            arial = new BitmapFont("Content/arial_test");
            text = new Text(arial, "The quick brown fox \njumps over the lazy dog.", new Vector2(0, 256), Color.Black);
            fpsFont = new BitmapFont(@"Content/fps_font");
            fpsCounter = new ClockDisplay(Game.Clock, fpsFont, Vector2.Zero, Color.White); // White is also default
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
