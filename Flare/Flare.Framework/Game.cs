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
    public static class Game
    {
        public static GameWindow Window;

        // Store OpenGL states
        private static Color ClearColor;

        /// <summary>
        /// Window size. Same as Width and Height properties.
        /// </summary>
        public static Vector2 TargetSize
        {
            get
            {
                return new Vector2(Window.Width, Window.Height);
            }
            set
            {
                Window.Width = (int)value.X; Window.Height = (int)value.Y;
                GL.Viewport(0, 0, Window.Width, Window.Height);
            }
        }

        public static VSyncMode VSync
        {
            get { return Window.VSync; }
            set { Window.VSync = value; }
        }

        public static Icon Icon
        {
            get { return Window.Icon; }
            set { Window.Icon = value; }
        }

        public static event EventHandler<EventArgs> Load;
        public static event EventHandler<EventArgs> Resize;
        public static event EventHandler<FrameEventArgs> RenderFrame;
        public static event EventHandler<FrameEventArgs> UpdateFrame;
        public static event EventHandler<OpenTK.Input.MouseButtonEventArgs> MouseDown;
        public static event EventHandler<OpenTK.Input.MouseButtonEventArgs> MouseUp;
        public static event EventHandler<OpenTK.Input.MouseMoveEventArgs> MouseMove;
        public static event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> KeyDown;
        public static event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> KeyUp;
        public static event EventHandler<KeyPressEventArgs> KeyPress;

        public static Utility.FrameClock Clock { get; private set; }

        static Game()
        {
            Window = new GameWindow(800, 600, new GraphicsMode(32, 24, 0, 4), "Flare.Framework Game Window",
                GameWindowFlags.Default, DisplayDevice.Default, 3, 0,
                GraphicsContextFlags.Debug | GraphicsContextFlags.ForwardCompatible);

            Window.Load         += Window_Load;
            Window.RenderFrame  += Window_RenderFrame;
            Window.UpdateFrame  += Window_UpdateFrame;
            Window.Resize       += Window_Resize;
            Window.MouseDown    += Window_MouseDown;
            Window.MouseUp      += Window_MouseUp;
            Window.MouseMove    += Window_MouseMove;
            Window.KeyDown      += Window_KeyDown;
            Window.KeyUp        += Window_KeyUp;
            Window.KeyPress     += Window_KeyPress;

            Clock = new Utility.FrameClock();
        }

        public static void Run()
        {
            Window.Run();
            Window.Exit();
        }

        public static void Run(float updateRate)
        {
            Window.Run(updateRate);
            Window.Exit();
        }
        public static void Run(float updateRate, float frameRate)
        {
            Window.Run(updateRate, frameRate);
            Window.Exit();
        }

        public static void SwapBuffers()
        {
            Window.SwapBuffers();
        }

        public static void Clear(Color clearColor)
        {
            if (ClearColor != clearColor)
            {
                GL.ClearColor(clearColor);
                ClearColor = clearColor;
            }
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private static void Window_Load(object sender, EventArgs e)
        {
            if (Load != null)
                Load(sender, e);
        }

        private static void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            if (RenderFrame != null)
                RenderFrame(sender, e);
        }

        private static void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            if (UpdateFrame != null)
                UpdateFrame(sender, e);
        }

        private static void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            if (Resize != null)
                Resize(sender, e);
        }

        private static void Window_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(sender, e);
        }

        private static void Window_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(sender, e);
        }

        private static void Window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(sender, e);
        }


        private static void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (KeyDown != null)
                KeyDown(sender, e);
        }

        private static void Window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (KeyUp != null)
                KeyUp(sender, e);
        }

        private static void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPress != null)
                KeyPress(sender, e);
        }
    }
}
