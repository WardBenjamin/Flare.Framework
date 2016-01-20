#region License
/* Flare - A Framework by Developers, for Developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion
using System;
using System.Diagnostics;

namespace Flare
{
    public abstract class Game
    {
        #region Internal Fields
        /* This variable solely exists for the VideoPlayer -flibit */
        internal static Game Instance = null;

        internal bool RunApplication;

        #endregion

        #region Private Fields

        #endregion

        #region Public Properties

        /// <summary>
        /// Reference to the current GameWindow implementation.
        /// </summary>
        public abstract GameWindow Window
        {
            get; internal set;
        }

        public abstract TimeSpan InactiveSleepTime
        {
            get; set;
        }

        public abstract bool IsActive
        {
            get;  internal set;
        }

        public abstract bool IsMouseVisible
        {
            get; set;
        }

        public abstract TimeSpan TargetElapsedTime
        {
            get; set;
        }

        public abstract bool IsFixedTimeStep
        {
            get; set;
        }

        #endregion


        #region Public Constructors

        public Game(string title, int width, int height)
        {
            Instance = this;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // TODO: Audio
            //AudioDevice.Initialize();

            // Ready to run the loop!
            RunApplication = true;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
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
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: Audio
                    //AudioDevice.Dispose();
                }

                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;

                _isDisposed = true;
                Instance = null;
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

        public abstract event EventHandler<EventArgs> Activated;
        public abstract event EventHandler<EventArgs> Deactivated;
        public abstract event EventHandler<EventArgs> Disposed;
        public abstract event EventHandler<EventArgs> Exiting;

        #endregion

        #region Public Methods

        public virtual void Exit()
        {
            RunApplication = false;
        }

        public abstract void ResetElapsedTime();

        public abstract void SuppressDraw();

        public abstract void RunOneFrame();

        public abstract void Run();

        public abstract void Tick();

        #endregion

        #region Protected Methods

        protected virtual bool BeginDraw()
        {
            return true;
        }

        protected virtual void EndDraw()
        {
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void EndRun()
        {
        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        protected virtual void Initialize()
        {
            // TODO: Make sure we have an OpenGL context?
                LoadContent();
        }

        protected virtual void Draw(GameTime gameTime)
        {

        }


        protected virtual void Update(GameTime gameTime)
        {
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
            Raise(Exiting, args);
        }

        protected virtual void OnActivated(object sender, EventArgs args)
        {
            AssertNotDisposed();
            Raise(Activated, args);
        }

        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
            AssertNotDisposed();
            Raise(Deactivated, args);
        }

        protected virtual bool ShowMissingRequirementMessage(Exception exception)
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

        /* FIXME: We should work toward eliminating internal methods. They
         * could eliminate the possibility that additional platforms could
         * be added by third parties without changing FNA itself.
         */

        protected virtual void DoUpdate(GameTime gameTime)
        {
            AssertNotDisposed();

            // TODO: Audio
            //AudioDevice.Update();

            Update(gameTime);
        }

        protected abstract void DoInitialize();

        protected void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
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
