#region License
/* Flare - A Framework by Developers, for Developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.ComponentModel;

using SDL2;
#endregion

namespace Flare.SDL2
{
    class SDL2_GameWindow : GameWindow
    {
        #region Public GameWindow Properties

        [DefaultValue(false)]
        public override bool AllowUserResizing
        {
            /* FIXME: This change should happen immediately. However, SDL2 does
			 * not yet have an SDL_SetWindowResizable, so for now this is
			 * basically just a check for when the window is first made.
			 * -flibit
			 */
            get
            {
                return Environment.GetEnvironmentVariable(
                    "FLARE_WORKAROUND_WINDOW_RESIZABLE"
                ) == "1";
            }
            set
            {
                if (!_sdlWindow.Equals(IntPtr.Zero))
                    throw new Exception("SDL does not support changing resizable parameter of the window after it's already been created.");
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                Rectangle result;
                if (_isFullscreen)
                {
                    /* FIXME: SDL2 bug!
					 * SDL's a little weird about SDL_GetWindowSize.
					 * If you call it early enough (for example,
					 * Game.Initialize()), it reports outdated ints.
					 * So you know what, let's just use this.
					 * -flibit
					 */
                    SDL.SDL_DisplayMode mode;
                    SDL.SDL_GetCurrentDisplayMode(
                        SDL.SDL_GetWindowDisplayIndex(
                            _sdlWindow
                        ),
                        out mode
                    );
                    result.X = 0;
                    result.Y = 0;
                    result.Width = mode.w;
                    result.Height = mode.h;
                }
                else
                {
                    SDL.SDL_GetWindowPosition(
                        _sdlWindow,
                        out result.X,
                        out result.Y
                    );
                    SDL.SDL_GetWindowSize(
                        _sdlWindow,
                        out result.Width,
                        out result.Height
                    );
                }
                return result;
            }
        }

        public override IntPtr Handle
        {
            get
            {
                return _sdlWindow;
            }
        }

        public override bool IsBorderless
        {
            get
            {
                return ((SDL.SDL_GetWindowFlags(_sdlWindow) & (uint)SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS) != 0);
            }
            set
            {
                SDL.SDL_SetWindowBordered(
                    _sdlWindow,
                    value ? SDL.SDL_bool.SDL_FALSE : SDL.SDL_bool.SDL_TRUE
                );
            }
        }

        public override bool IsFullscreen
        {
            get
            {
                return _isFullscreen;
            }
            set
            {
                // Make the Platform device changes.
                BeginScreenDeviceChange(
                    value
                );
                EndScreenDeviceChange(
                    Title,
                    Width,
                    Height
                );
            }
        }

        #endregion

        #region Private SDL2 Window Variables

        private IntPtr _sdlWindow;

        private bool _isFullscreen;
        private bool _wantsFullscreen;

        private string _deviceName;

        private Point _lastWindowPosition;

        #endregion

        #region  Constructor

        internal SDL2_GameWindow(string title, int width, int height) : base(title, width, height)
        {
            SDL.SDL_WindowFlags initFlags = (
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL |
                SDL.SDL_WindowFlags.SDL_WINDOW_HIDDEN |
                SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS |
                SDL.SDL_WindowFlags.SDL_WINDOW_MOUSE_FOCUS
            );

            // FIXME: Once we have SDL_SetWindowResizable, remove this. -flibit
            if (AllowUserResizing)
            {
                initFlags |= SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
            }

            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_ALPHA_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DEPTH_SIZE, 24);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);

            SDL.SDL_GL_SetAttribute(
                SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION,
                3
            );
            SDL.SDL_GL_SetAttribute(
                SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION,
                2
            );
            SDL.SDL_GL_SetAttribute(
                SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK,
                (int)SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE
            );

#if DEBUG
            SDL.SDL_GL_SetAttribute(
                SDL.SDL_GLattr.SDL_GL_CONTEXT_FLAGS,
                (int)SDL.SDL_GLcontext.SDL_GL_CONTEXT_DEBUG_FLAG
            );
#endif

            _sdlWindow = SDL.SDL_CreateWindow(
                title,
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                width, 
                height,
                initFlags
            );
            _SetIcon(title);

            _isFullscreen = false;
            _wantsFullscreen = false;
            _lastWindowPosition = new Point(SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED);
        }

        #endregion

        #region Public GameWindow Methods

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            _wantsFullscreen = willBeFullScreen;
        }

        public override void EndScreenDeviceChange(
            string screenDeviceName,
            int clientWidth,
            int clientHeight
        )
        {
            // Set screen device name, not that we use it...
            _deviceName = screenDeviceName;

            // Fullscreen
            if (_wantsFullscreen &&
                (SDL.SDL_GetWindowFlags(_sdlWindow) & (uint)SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN) == 0)
            {
                /* FIXME: SDL2/OSX bug!
				 * For whatever reason, Spaces windows on OSX
				 * like to be high-DPI if you set fullscreen
				 * while the window is hidden. But, if you just
				 * show the window first, everything is fine.
				 * -flibit
				 */
                SDL.SDL_ShowWindow(_sdlWindow);
            }
            SDL.SDL_SetWindowFullscreen(
                _sdlWindow,
                _wantsFullscreen ?
                    (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP :
                    0
            );

            /* Because Mac windows resize from the bottom, we have to get the
			 * position before changing the size so we can keep the window
			 * centered when resizing in windowed mode.
			 * -Nick
			 */
            Rectangle prevBounds = Rectangle.Empty;
            if (!_wantsFullscreen)
            {
                prevBounds = ClientBounds;
            }

            // Window bounds
            SDL.SDL_SetWindowSize(_sdlWindow, clientWidth, clientHeight);

            // Window position
            if (_isFullscreen && !_wantsFullscreen)
            {
                // If exiting fullscreen, just center the window on the desktop.
                SDL.SDL_SetWindowPosition(
                    _sdlWindow,
                    _lastWindowPosition.X,
                    _lastWindowPosition.Y
                );
            }
            else if (!_wantsFullscreen)
            {
                // Store the window position before switching to fullscreen
                _lastWindowPosition.X = prevBounds.X + ((prevBounds.Width - clientWidth) / 2);
                _lastWindowPosition.Y = prevBounds.Y + ((prevBounds.Height - clientHeight) / 2);

                SDL.SDL_SetWindowPosition(
                    _sdlWindow,
                    Math.Max(
                        _lastWindowPosition.X,
                        0
                    ),
                    Math.Max(
                        _lastWindowPosition.Y,
                        0
                    )
                );
            }

            // Current window state has just been updated.
            _isFullscreen = _wantsFullscreen;
        }

        #endregion

        #region  Methods

        internal void _ClientSizeChanged()
        {
            OnClientSizeChanged();
        }

        #endregion

        #region Protected GameWindow Methods

        protected override void SetTitle(string title)
        {
            SDL.SDL_SetWindowTitle(
                _sdlWindow,
                title
            );
        }

        #endregion

        #region Private Window Icon Method

        private void _SetIcon(string title)
        {
            string fileIn = String.Empty;

            /* If the game's using SDL2_image, provide the option to use a PNG
			 * instead of a BMP. Nice for anyone who cares about transparency.
			 * -flibit
			 */
            try
            {
                fileIn = _GetIconName(title, ".png");
                if (!String.IsNullOrEmpty(fileIn))
                {
                    IntPtr icon = SDL_image.IMG_Load(fileIn);
                    SDL.SDL_SetWindowIcon(_sdlWindow, icon);
                    SDL.SDL_FreeSurface(icon);
                    return;
                }
            }
            catch (DllNotFoundException)
            {
                // Not that big a deal guys.
            }

            fileIn = _GetIconName(title, ".bmp");
            if (!String.IsNullOrEmpty(fileIn))
            {
                IntPtr icon = SDL.SDL_LoadBMP(fileIn);
                SDL.SDL_SetWindowIcon(_sdlWindow, icon);
                SDL.SDL_FreeSurface(icon);
            }
        }

        #endregion

        #region Private Static Icon Filename Method

        private static string _GetIconName(string title, string extension)
        {
            string fileIn = String.Empty;
            if (System.IO.File.Exists(title + extension))
            {
                // If the title and filename work, it just works. Fine.
                fileIn = title + extension;
            }
            else
            {
                // But sometimes the title has invalid characters inside.

                /* In addition to the filesystem's invalid charset, we need to
				 * blacklist the Windows standard set too, no matter what.
				 * -flibit
				 */
                char[] hardCodeBadChars = new char[]
                {
                    '<',
                    '>',
                    ':',
                    '"',
                    '/',
                    '\\',
                    '|',
                    '?',
                    '*'
                };
                List<char> badChars = new List<char>();
                badChars.AddRange(System.IO.Path.GetInvalidFileNameChars());
                badChars.AddRange(hardCodeBadChars);

                string stripChars = title;
                foreach (char c in badChars)
                {
                    stripChars = stripChars.Replace(c.ToString(), "");
                }
                stripChars += extension;

                if (System.IO.File.Exists(stripChars))
                {
                    fileIn = stripChars;
                }
            }
            return fileIn;
        }

        #endregion
    }
}
