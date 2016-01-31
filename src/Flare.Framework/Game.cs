#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

#region BASIC_PROFILER Option
// #define BASIC_PROFILER
/* Sometimes you need a really quick way to determine if performance is either
 * CPU- or GPU-bound. For XNA games, the fastest generic way is to just time the
 * Update() and Draw() functions, respectively. This is not to say that each
 * function can only have problems for either the CPU or GPU, but the graph can
 * say a lot about one of the two processes if either is notably slower than the
 * other one.
 *
 * This option will draw a rectangle on the right side of the screen. The two
 * colors indicate a rough percentage of time spent in both Update() and Draw().
 * Blue is Update(), Red is Draw(). There may be time spent in other parts of
 * the frame (usually GraphicsDevice.Present if you're faster than the display's
 * refresh rate), but compares to these two functions, the time spent is likely
 * marginal in comparison.
 *
 * If you want _real_ profile data, use a _real_ profiler!
 */
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flare.SDL2;

namespace Flare
{
    public class Game
    {
        #region Internal Properties

        internal static Game Instance { get; private set; }
        internal bool RunApplication { get; set; }

        #endregion

        #region Private Fields
        private GameWindow _window;

        private bool _isActive = true;
        private bool _isMouseVisible = false;

        private bool _initialized = false;
        private bool _isFixedTimeStep = true;

        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(166667); // 60fps
        private TimeSpan _inactiveSleepTime = TimeSpan.FromSeconds(0.02);
        private readonly TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);

        private bool _suppressDraw;

        #region BASIC_PROFILER Support
#if BASIC_PROFILER
		private long drawStart;
		private long drawTime;
		private long updateStart;
		private long updateTime;
		private BasicEffect profileEffect;
		private Matrix projection;
		private VertexPositionColor[] profilePrimitives;
#endif
        #endregion
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the underlying operating system window implementation.
        /// </summary>
        public GameWindow Window
        {
            get { return _window; }
            internal set
            {
                if (_window == null)
                {
                    SDL2_MouseUtil.WindowHandle = value.Handle;
                }
                _window = value;
            }
        }

        /// <summary>
        /// Gets or sets the time to sleep when the game is inactive. 
        /// TODO: Will be depricated.
        /// </summary>
        public TimeSpan InactiveSleepTime
        {
            get
            {
                return _inactiveSleepTime;
            }
            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        "The time must be positive.",
                        default(Exception)
                    );
                }
                if (_inactiveSleepTime != value)
                {
                    _inactiveSleepTime = value;
                }
            }
        }

        /// <summary>
        /// Indicates whether the game is currently the active application. 
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            internal set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    Raise(_isActive ? Activated : Deactivated, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse cursor should be visible. 
        /// </summary>
        public bool IsMouseVisible
        {
            get
            {
                return _isMouseVisible;
            }
            set
            {
                if (_isMouseVisible != value)
                {
                    _isMouseVisible = value;
                    SDL2_Platform.OnIsMouseVisibleChanged(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the target time between calls to Update when IsFixedTimeStep is true.
        /// TODO: Will be depricated.
        /// </summary>
        public TimeSpan TargetElapsedTime
        {
            get
            {
                return _targetElapsedTime;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        "The time must be positive and non-zero.",
                        default(Exception)
                    );
                }

                _targetElapsedTime = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use fixed time steps. 
        /// TODO: Will be depricated.
        /// </summary>
        public bool IsFixedTimeStep
        {
            get
            {
                return _isFixedTimeStep;
            }
            set
            {
                _isFixedTimeStep = value;
            }
        }

        #endregion

        #region Public Constructors

        public Game(string title, int width, int height)
        {
            SDL2_Platform.Init(this, title, width, height);
            Instance = this;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // TODO: Audio
            //AudioDevice.Initialize();

            // Ready to run the loop!
            RunApplication = true;
        }

        #endregion

        #region Exception Handling

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("An exception was not handled! " + e.ToString());
        }

        #endregion

        #region Deconstructor

        ~Game()
        {
            Dispose(false);
        }

        #endregion

        #region IDisposable Implementation

        private bool _isDisposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Raise(Disposed, EventArgs.Empty);
        }

        protected void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: Audio
                    //AudioDevice.Dispose();

                    SDL2_Platform.Dispose(this);
                    SDL2_MouseUtil.WindowHandle = IntPtr.Zero;
                }

                _isDisposed = true;
            }
        }

        [DebuggerNonUserCode]
        protected void AssertNotDisposed()
        {
            if (_isDisposed)
            {
                string name = GetType().Name;
                throw new ObjectDisposedException(
                    name,
                    string.Format(
                        "The {0} object was used after being Disposed.",
                        name
                    )
                );
            }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<EventArgs> Exiting;

        #endregion

        #region Public Methods

        public void Exit()
        {
            _suppressDraw = true;
        }

        public void ResetElapsedTime()
        {
            if (_initialized)
            {
                _gameTimer.Reset();
                _gameTimer.Start();
                _accumulatedElapsedTime = TimeSpan.Zero;
                _gameTime.ElapsedGameTime = TimeSpan.Zero;
                _previousTicks = 0L;
            }
        }

        public void SuppressDraw()
        {
            _suppressDraw = true;
        }

        public void RunOneFrame()
        {
            if (!_initialized)
            {
                DoInitialize();
                _gameTimer = Stopwatch.StartNew();
                _initialized = true;
            }

            BeginRun();

            // FIXME: Not quite right..
            Tick();

            EndRun();
        }

        public void Run()
        {
            AssertNotDisposed();

            if (!_initialized)
            {
                DoInitialize();
                _initialized = true;
            }

            BeginRun();
            _gameTimer = Stopwatch.StartNew();

            SDL2_Platform.RunLoop(this);

            EndRun();

            OnExiting(this, EventArgs.Empty);
        }

        private TimeSpan _accumulatedElapsedTime;
        private readonly GameTime _gameTime = new GameTime();
        private Stopwatch _gameTimer;
        private long _previousTicks = 0;
        private int _updateFrameLag;

        public void Tick()
        {
            /* NOTE: This code is very sensitive and can break very badly,
             * even with what looks like a safe change. Be sure to test
             * any change fully in both the fixed and variable timestep
             * modes across multiple devices and platforms.
             */

            RetryTick:

            // Advance the accumulated elapsed time.
            long currentTicks = _gameTimer.Elapsed.Ticks;
            _accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - _previousTicks);
            _previousTicks = currentTicks;

            /* If we're in the fixed timestep mode and not enough time has elapsed
             * to perform an update we sleep off the the remaining time to save battery
             * life and/or release CPU time to other threads and processes.
             */
            if (IsFixedTimeStep && _accumulatedElapsedTime < TargetElapsedTime)
            {
                int sleepTime = (
                    (int)(TargetElapsedTime - _accumulatedElapsedTime).TotalMilliseconds
                );

                /* NOTE: While sleep can be inaccurate in general it is
                 * accurate enough for frame limiting purposes if some
                 * fluctuation is an acceptable result.
                 */
                System.Threading.Thread.Sleep(sleepTime);

                goto RetryTick;
            }

            // Do not allow any update to take longer than our maximum.
            if (_accumulatedElapsedTime > _maxElapsedTime)
            {
                _accumulatedElapsedTime = _maxElapsedTime;
            }

            if (IsFixedTimeStep)
            {
                _gameTime.ElapsedGameTime = TargetElapsedTime;
                int stepCount = 0;

                // Perform as many full fixed length time steps as we can.
                while (_accumulatedElapsedTime >= TargetElapsedTime)
                {
                    _gameTime.TotalGameTime += TargetElapsedTime;
                    _accumulatedElapsedTime -= TargetElapsedTime;
                    stepCount += 1;

                    DoUpdate(_gameTime);
                }

                // Every update after the first accumulates lag
                _updateFrameLag += Math.Max(0, stepCount - 1);

                /* If we think we are running slowly, wait
                 * until the lag clears before resetting it
                 */
                if (_gameTime.IsRunningSlowly)
                {
                    if (_updateFrameLag == 0)
                    {
                        _gameTime.IsRunningSlowly = false;
                    }
                }
                else if (_updateFrameLag >= 5)
                {
                    /* If we lag more than 5 frames,
                     * start thinking we are running slowly.
                     */
                    _gameTime.IsRunningSlowly = true;
                }

                /* Every time we just do one update and one draw,
                 * then we are not running slowly, so decrease the lag.
                 */
                if (stepCount == 1 && _updateFrameLag > 0)
                {
                    _updateFrameLag -= 1;
                }

                /* Draw needs to know the total elapsed time
                 * that occured for the fixed length updates.
                 */
                _gameTime.ElapsedGameTime = TimeSpan.FromTicks(TargetElapsedTime.Ticks * stepCount);
            }
            else
            {
                // Perform a single variable length update.
                _gameTime.ElapsedGameTime = _accumulatedElapsedTime;
                _gameTime.TotalGameTime += _accumulatedElapsedTime;
                _accumulatedElapsedTime = TimeSpan.Zero;

                DoUpdate(_gameTime);
            }

            // Draw unless the update suppressed it.
            if (_suppressDraw)
            {
                _suppressDraw = false;
            }
            else
            {
                if (BeginDraw())
                {
                    Draw(_gameTime);
                    EndDraw();
                }
            }
        }

        #endregion

        #region Protected Methods

        protected bool BeginDraw()
        {
            #region BASIC_PROFILER Support
#if BASIC_PROFILER
			drawStart = _gameTimer.ElapsedTicks;
#endif
            #endregion
            return true;
        }

        protected void EndDraw()
        {
            #region BASIC_PROFILER Support
#if BASIC_PROFILER
			drawTime = _gameTimer.ElapsedTicks - drawStart;
			Viewport viewport = GraphicsDevice.Viewport;
			float top = 50;
			float bottom = viewport.Height - 50;
			float middle = 50 + (bottom - top) * (updateTime / (float) (updateTime + drawTime));
			float left = viewport.Width - 100;
			float right = left + 50;
			profilePrimitives[0].Position.X = left;
			profilePrimitives[0].Position.Y = top;
			profilePrimitives[1].Position.X = right;
			profilePrimitives[1].Position.Y = top;
			profilePrimitives[2].Position.X = left;
			profilePrimitives[2].Position.Y = middle;
			profilePrimitives[3].Position.X = right;
			profilePrimitives[3].Position.Y = middle;
			profilePrimitives[4].Position.X = left;
			profilePrimitives[4].Position.Y = middle;
			profilePrimitives[5].Position.X = right;
			profilePrimitives[5].Position.Y = top;
			profilePrimitives[6].Position.X = left;
			profilePrimitives[6].Position.Y = middle;
			profilePrimitives[7].Position.X = right;
			profilePrimitives[7].Position.Y = middle;
			profilePrimitives[8].Position.X = left;
			profilePrimitives[8].Position.Y = bottom;
			profilePrimitives[9].Position.X = right;
			profilePrimitives[9].Position.Y = bottom;
			profilePrimitives[10].Position.X = left;
			profilePrimitives[10].Position.Y = bottom;
			profilePrimitives[11].Position.X = right;
			profilePrimitives[11].Position.Y = middle;
			projection.M11 = (float) (2.0 / (double) viewport.Width);
			projection.M22 = (float) (-2.0 / (double) viewport.Height);
			profileEffect.Projection = projection;
			profileEffect.CurrentTechnique.Passes[0].Apply();
			GraphicsDevice.DrawUserPrimitives(
				PrimitiveType.TriangleList,
				profilePrimitives,
				0,
				12
			);
#endif
            #endregion
        }

        protected void BeginRun()
        {
            #region BASIC_PROFILER Support
#if BASIC_PROFILER
			profileEffect = new BasicEffect(GraphicsDevice);
			profileEffect.FogEnabled = false;
			profileEffect.LightingEnabled = false;
			profileEffect.TextureEnabled = false;
			profileEffect.VertexColorEnabled = true;
			projection = new Matrix(
				1337.0f,
				0.0f,
				0.0f,
				0.0f,
				0.0f,
				-1337.0f,
				0.0f,
				0.0f,
				0.0f,
				0.0f,
				1.0f,
				0.0f,
				-1.0f,
				1.0f,
				0.0f,
				1.0f
			);
			profilePrimitives = new VertexPositionColor[12];
			int i = 0;
			do
			{
				profilePrimitives[i].Position = Vector3.Zero;
				profilePrimitives[i].Color = Color.Blue;
			} while (++i < 6);
			do
			{
				profilePrimitives[i].Position = Vector3.Zero;
				profilePrimitives[i].Color = Color.Red;
			} while (++i < 12);
#endif
            #endregion
        }

        protected void EndRun()
        {
            #region BASIC_PROFILER Support
#if BASIC_PROFILER
			profileEffect.Dispose();
#endif
            #endregion
        }

        protected void Initialize()
        {
            // TODO: Event
        }

        protected virtual void Draw(GameTime gameTime)
        {
            // TODO: Event and DrawEventArgs
        }

        protected virtual void Update(GameTime gameTime)
        {
            // TODO: Event and UpdateEventArgs
        }

        protected void OnExiting(object sender, EventArgs args)
        {
            Raise(Exiting, args);
        }

        protected void OnActivated(object sender, EventArgs args)
        {
            AssertNotDisposed();
            Raise(Activated, args);
        }

        protected void OnDeactivated(object sender, EventArgs args)
        {
            AssertNotDisposed();
            Raise(Deactivated, args);
        }

        protected bool ShowMissingRequirementMessage(Exception exception)
        {
            // TODO: Audio
            /*if (exception is NoAudioHardwareException)
            {
                SDL2_Platform.ShowRuntimeError(
                    Window.Title,
                    "Could not find a suitable audio device. " +
                    " Verify that a sound card is\ninstalled," +
                    " and check the driver properties to make" +
                    " sure it is not disabled."
                );
                return true;
            }*/
            /* TODO: Load graphics methods by hand to catch errors?
            if (exception is NoSuitableGraphicsDeviceException)
            {
                SDL2_Platform.ShowRuntimeError(
                    Window.Title,
                    "Could not find a suitable graphics device." +
                    " More information:\n\n" + exception.Message
                );
                return true;
            }*/
            return false;
        }

        #endregion

        #region Private Methods

        private void DoUpdate(GameTime gameTime)
        {
            AssertNotDisposed();

            // TODO: Audio
            //AudioDevice.Update();

            Update(gameTime);
        }

        private void DoInitialize()
        {
            AssertNotDisposed();

            SDL2_Platform.BeforeInitialize();
            Initialize();
        }

        private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
    where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        // TODO: Some attention
        #region Input events

        /// <summary>
        /// Use this event to retrieve text for objects like textboxes.
        /// This event is not raised by noncharacter keys.
        /// This event also supports key repeat.
        /// For more information this event is based off:
        /// http://msdn.microsoft.com/en-AU/library/system.windows.forms.control.keypress.aspx
        /// </summary>
        public event Action<char> KeyPress;


        public void OnKeyPress(char c)
        {
            if (KeyPress != null)
            {
                KeyPress(c);
            }
        }
        #endregion

    }
}