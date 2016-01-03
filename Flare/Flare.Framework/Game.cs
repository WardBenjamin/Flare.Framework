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
        /// FrameClock holding data framerate and other information. 
        /// Note: Update using BeginFrame() and EndFrame() in your draw method.
        /// </summary>
        public static Utility.FrameClock Clock { get; private set; }


        #region Window Size & Position Properties

        /// <summary>
        /// Gets or sets a Rectangle structure that specifies the external bounds of this window, in screen coordinates.
        /// The coordinates are specified in device-independant points and include the title bar, borders, and drawing area of the window.
        /// </summary>
        public static Rectangle Bounds
        {
            get { return Window.Bounds; }
            set { Window.Bounds = value; }
        }

        /// <summary>
        /// Gets or sets a Point structure that contains the location of this window on the desktop.
        /// </summary>
        public static Point Location
        {
            get { return Window.Location; }
            set { Window.Location = value; }
        }

        /// <summary>
        /// Gets or sets a Size structure that contains the external size of this window.
        /// </summary>
        public static Size Size
        {
            get { return Window.Size; }
            set { Window.Size = value; }
        }

        /// <summary>
        /// Gets or sets a Rectangle structure that specifies the bounds of the OpenGL surface, in window coordinates.
        /// The coordinates are specified in device-independant pixels.
        /// </summary>
        public static Rectangle ClientRectangle
        {
            get { return Window.ClientRectangle; }
            set { Window.ClientRectangle = value; }
        }

        /// <summary>
        /// Gets or sets a Size structure that defines the size of the OpenGL surface, in window coordinates.
        /// The coordinates are specified in device-dependant pixels.
        /// </summary>
        public static Size ClientSize
        {
            get { return Window.ClientSize; }
            set { Window.ClientSize = value; GL.Viewport(0, 0, Window.Width, Window.Height); }
        }

        /// <summary>
        /// Gets or sets the width of the OpenGL surface in window coordinates. The coordinates are specified in device-dependant pixels.
        /// </summary>
        public static int Width
        {
            get { return Window.Width; }
            set { Window.Width = value; GL.Viewport(0, 0, Window.Width, Window.Height); }
        }

        /// <summary>
        /// Gets or sets the height of the OpenGL surface in window coordinates. The coordinates are specified in device-dependant pixels.
        /// </summary>
        public static int Height
        {
            get { return Window.Height; }
            set { Window.Height = value; GL.Viewport(0, 0, Window.Width, Window.Height); }
        }

        /// <summary>
        /// Gets or sets the horizontal location of this window in screen coordinates. The coordinates are specified in device-independant pixels.
        /// </summary>
        public static int X
        {
            get { return Window.X; }
            set { Window.X = value; }
        }

        /// <summary>
        /// Gets or sets the vertical location of this window in screen coordinates. The coordinates are specified in device-independant pixels.
        /// </summary>
        public static int Y
        {
            get { return Window.Y; }
            set { Window.X = value; }
        }

        #endregion

        #region Window Information Properties

        /// <summary>
        /// Gets or sets the border of the NativeWindow
        /// </summary>
        public static WindowBorder WindowBorder
        {
            get { return Window.WindowBorder; }
            set { Window.WindowBorder = value; }
        }

        /// <summary>
        /// Gets the OpenTK.Platform.IWindowInfo of this window.
        /// </summary>
        public static OpenTK.Platform.IWindowInfo WindowInfo
        {
            get { return Window.WindowInfo; }
        }

        /// <summary>
        /// Gets or sets the state of the NativeWindow.
        /// </summary>
        public static WindowState WindowState
        {
            get { return Window.WindowState; }
            set { Window.WindowState = value; }
        }

        #endregion

        #region Misc. Window Properties

        /// <summary>
        /// Gets a value that indicates whether this window has input focus.
        /// </summary>
        public static bool Focused
        {
            get { return Window.Focused; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the the window is visible.
        /// </summary>
        public static bool Visible
        {
            get { return Window.Visible; }
            set { Window.Visible = value; }
        }

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public static string Title
        {
            get { return Window.Title; }
            set { Window.Title = value; }
        }

        /// <summary>
        /// Gets or sets the VSync mode.
        /// </summary>
        public static VSyncMode VSync
        {
            get { return Window.VSync; }
            set { Window.VSync = value; }
        }

        /// <summary>
        /// Gets or sets the System.Drawing.Icon for this window.
        /// </summary>
        public static Icon Icon
        {
            get { return Window.Icon; }
            set { Window.Icon = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this window exists.
        /// </summary>
        public static bool Exists
        {
            get { return Window.Exists; }
        }

        /// <summary>
        /// Gets a value indicating whether the shutdown sequence has been initiated for this window, 
        /// by calling Game.Exit() or clicking the "close" button. If this property is true, it is no longer
        /// safe to use any Flare.Graphics, OpenTK.Input, or OpenTK.Graphics.OpenGL functions or properties.
        /// </summary>
        public static bool IsExiting
        {
            get { return Window.IsExiting; }
        }

        /// <summary>
        /// Gets the OpenGL context associated with the current window.
        /// </summary>
        public static IGraphicsContext Context
        {
            get { return Window.Context; }
        }

        #endregion

        #region Render & Update Data Properties
        /*
        /// <summary>
        /// Gets a double representing the actual frequency of RenderFrame events, in hertz (i.e. fps or frames per second).
        /// </summary>
        public static double RenderFrequency
        {
            get { return Window.RenderFrequency; }
        }

        /// <summary>
        /// Gets a double representing the period of RenderFrame events, in seconds.
        /// </summary>
        public static double RenderPeriod
        {
            get { return Window.RenderPeriod; }
        }

            TODO: Fill these properties in
            Window.RenderFrequency;
            Window.RenderPeriod;

            Window.RenderTime;
            Window.TargetRenderFrequency;
            Window.TargetRenderPeriod;
            Window.TargetUpdateFrequency;
            Window.TargetUpdatePeriod;
            Window.UpdateFrequency;
            Window.UpdatePeriod;
            Window.UpdateTime;
        */
        #endregion

        #region Input Device Properties

        /// <summary>
        /// Gets or sets the MouseCursor for this window.
        /// </summary>
        public static MouseCursor Cursor
        {
            get { return Window.Cursor; }
            set { Window.Cursor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the mouse cursor is visible.
        /// </summary>
        public static bool CursorVisible
        {
            get { return Window.CursorVisible; }
            set { Window.CursorVisible = value; }
        }

        /// <summary>
        /// Gets the primary mouse device, or null if no Mouse exists.
        /// </summary>
        public static OpenTK.Input.MouseDevice Mouse
        {
            get { return Window.Mouse; }
        }

        /// <summary>
        /// Gets the primary Keyboard device, or null if no Keyboard exists.
        /// </summary>
        public static OpenTK.Input.KeyboardDevice Keyboard
        {
            get { return Window.Keyboard; }
        }

        // GameWindow Joysticks have been depricated in favor of OpenTK.Input.Joystick and OpenTK.Input.GamePad

        #endregion

        #region Event Handling

        public static event EventHandler<EventArgs> Closed;
        public static event EventHandler<System.ComponentModel.CancelEventArgs> Closing;
        public static event EventHandler<EventArgs> Disposed;
        public static event EventHandler<EventArgs> FocusedChanged;
        public static event EventHandler<EventArgs> IconChanged;
        public static event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> KeyDown;
        public static event EventHandler<KeyPressEventArgs> KeyPress;
        public static event EventHandler<OpenTK.Input.KeyboardKeyEventArgs> KeyUp;
        public static event EventHandler<EventArgs> Load;
        public static event EventHandler<OpenTK.Input.MouseButtonEventArgs> MouseDown;
        public static event EventHandler<EventArgs> MouseEnter;
        public static event EventHandler<EventArgs> MouseLeave;
        public static event EventHandler<OpenTK.Input.MouseMoveEventArgs> MouseMove;
        public static event EventHandler<OpenTK.Input.MouseButtonEventArgs> MouseUp;
        public static event EventHandler<OpenTK.Input.MouseWheelEventArgs> MouseWheel;
        public static event EventHandler<EventArgs> Move;
        public static event EventHandler<FrameEventArgs> RenderFrame;
        public static event EventHandler<EventArgs> Resize;
        public static event EventHandler<EventArgs> TitleChanged;
        public static event EventHandler<EventArgs> Unload;
        public static event EventHandler<FrameEventArgs> UpdateFrame;
        public static event EventHandler<EventArgs> VisibleChanged;
        public static event EventHandler<EventArgs> WindowBorderChanged;
        public static event EventHandler<EventArgs> WindowStateChanged;

        #endregion

        static Game()
        {
            Window = new GameWindow(800, 600, new GraphicsMode(32, 24, 0, 4), "Flare.Framework Game Window",
                GameWindowFlags.Default, DisplayDevice.Default, 3, 0,
                GraphicsContextFlags.Debug | GraphicsContextFlags.ForwardCompatible);

            #region Event Handler Registration

            Window.Closed += Window_Closed;
            Window.Closing += Window_Closing;
            Window.Disposed += Window_Disposed;
            Window.FocusedChanged += Window_FocusedChanged;
            Window.IconChanged += Window_IconChanged;
            Window.KeyDown += Window_KeyDown;
            Window.KeyPress += Window_KeyPress;
            Window.KeyUp += Window_KeyUp;
            Window.Load += Window_Load;
            Window.MouseDown += Window_MouseDown;
            Window.MouseEnter += Window_MouseEnter;
            Window.MouseLeave += Window_MouseLeave;
            Window.MouseMove += Window_MouseMove;
            Window.MouseUp += Window_MouseUp;
            Window.MouseWheel += Window_MouseWheel;
            Window.Move += Window_Move;
            Window.RenderFrame += Window_RenderFrame;
            Window.Resize += Window_Resize;
            Window.TitleChanged += Window_TitleChanged;
            Window.Unload += Window_Unload;
            Window.UpdateFrame += Window_UpdateFrame;
            Window.VisibleChanged += Window_VisibleChanged;
            Window.WindowBorderChanged += Window_WindowBorderChanged;
            Window.WindowStateChanged += Window_WindowStateChanged;

            #endregion

            Clock = new Utility.FrameClock();

            // Disable VSync for maximum possble framerate by default
            VSync = VSyncMode.Off;
        }

        #region Event Handler Methods

        private static void Window_Closed(object sender, EventArgs e)
        {
            if (Closed != null)
                Closed(sender, e);
        }

        private static void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Closing != null)
                Closing(sender, e);
        }

        private static void Window_Disposed(object sender, EventArgs e)
        {
            if (Disposed != null)
                Disposed(sender, e);
        }

        private static void Window_FocusedChanged(object sender, EventArgs e)
        {
            if (FocusedChanged != null)
                FocusedChanged(sender, e);
        }

        private static void Window_IconChanged(object sender, EventArgs e)
        {
            if (IconChanged != null)
                IconChanged(sender, e);
        }

        private static void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (KeyDown != null)
                KeyDown(sender, e);
        }

        private static void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPress != null)
                KeyPress(sender, e);
        }

        private static void Window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (KeyUp != null)
                KeyUp(sender, e);
        }

        private static void Window_Load(object sender, EventArgs e)
        {
            if (Load != null)
                Load(sender, e);
        }

        private static void Window_MouseEnter(object sender, EventArgs e)
        {
            if (MouseEnter != null)
                MouseEnter(sender, e);
        }

        private static void Window_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(sender, e);
        }

        private static void Window_MouseLeave(object sender, EventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(sender, e);
        }

        private static void Window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(sender, e);
        }

        private static void Window_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(sender, e);
        }

        private static void Window_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            if (MouseWheel != null)
                MouseWheel(sender, e);
        }

        private static void Window_Move(object sender, EventArgs e)
        {
            if (Move != null)
                Move(sender, e);
        }

        private static void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            if (RenderFrame != null)
                RenderFrame(sender, e);
        }

        private static void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Window.Width, Window.Height);
            if (Resize != null)
                Resize(sender, e);
        }

        private static void Window_TitleChanged(object sender, EventArgs e)
        {
            if (TitleChanged != null)
                TitleChanged(sender, e);
        }

        private static void Window_Unload(object sender, EventArgs e)
        {
            if (Unload != null)
                Unload(sender, e);
        }

        private static void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            if (UpdateFrame != null)
                UpdateFrame(sender, e);
        }

        private static void Window_VisibleChanged(object sender, EventArgs e)
        {
            if (VisibleChanged != null)
                VisibleChanged(sender, e);
        }

        private static void Window_WindowBorderChanged(object sender, EventArgs e)
        {
            if (WindowBorderChanged != null)
                WindowBorderChanged(sender, e);
        }

        private static void Window_WindowStateChanged(object sender, EventArgs e)
        {
            if (WindowStateChanged != null)
                WindowStateChanged(sender, e);
        }

        #endregion

        #region Run Methods

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

        #endregion

        #region Graphics Methods

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

        #endregion

        #region GameWindow Methods

        /// <summary>
        /// Closes the window. Equivalent to Close().
        /// </summary>
        public static void Exit()
        {
            Window.Exit();
        }

        /// <summary>
        /// Closes the window. Equivalent to Exit().
        /// </summary>
        public static void Close()
        {
            Window.Close();
        }

        /// <summary>
        /// Disposes the game, releasing all resources consumed by it.
        /// </summary>
        public static void Dispose()
        {
            Window.Dispose();
        }

        /// <summary>
        /// Make the GraphicsContext current on the calling thread. 
        /// </summary>
        public static void MakeCurrent()
        {
            Window.MakeCurrent();
        }

        /// <summary>
        /// Transforms the specified point from screen to client coordinates.
        /// </summary>
        /// <param name="p">Point in screen coordinates</param>
        /// <returns>Point in client coordinates</returns>
        public static Point PointToClient(Point p)
        {
            return Window.PointToClient(p);
        }

        /// <summary>
        /// Transforms the specified point from client to screen coordinates.
        /// </summary>
        /// <param name="p">Point in client coordinates</param>
        /// <returns>Point in screen coordinates</returns>
        public static Point PointToScreen(Point p)
        {
            return Window.PointToScreen(p);
        }

        /// <summary>
        /// Process operating system events until window becomes idle.
        /// </summary>
        public static void ProcessEvents()
        {
            Window.ProcessEvents();
        }

        #endregion
    }
}
