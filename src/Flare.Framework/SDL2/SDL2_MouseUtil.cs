#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flare.Input;
using SDL2;

namespace Flare.SDL2
{
    class SDL2_MouseUtil
    {
        #region Public Properties

        public static IntPtr WindowHandle
        {
            get;
            set;
        }

        #endregion

        #region Internal Variables

        internal static int _WindowWidth = 800;
        internal static int _WindowHeight = 600;

        internal static int _MouseWheel = 0;

        internal static bool _IsWarped = false;

        #endregion

        #region Private Variables

        private static MouseState state;

        #endregion

        #region Public Interface

        /// <summary>
        /// Gets mouse state information that includes position and button
        /// presses for the provided window
        /// </summary>
        /// <returns>Current state of the mouse.</returns>
        public static MouseState GetState()
        {
            int x, y;
            uint flags = SDL.SDL_GetMouseState(out x, out y);

            // If we warped the mouse, we've already done this in SetPosition.
            if (!_IsWarped)
            {
                // Scale the mouse coordinates for the faux-backbuffer
                state.X = (int)((double)x * Game.Instance.GraphicsDevice.GLDevice.Backbuffer.Width / _WindowWidth);
                state.Y = (int)((double)y * Game.Instance.GraphicsDevice.GLDevice.Backbuffer.Height / _WindowHeight);
            }

            state.LeftButton = (ButtonState)(flags & SDL.SDL_BUTTON_LMASK);
            state.MiddleButton = (ButtonState)((flags & SDL.SDL_BUTTON_MMASK) >> 1);
            state.RightButton = (ButtonState)((flags & SDL.SDL_BUTTON_RMASK) >> 2);
            state.XButton1 = (ButtonState)((flags & SDL.SDL_BUTTON_X1MASK) >> 3);
            state.XButton2 = (ButtonState)((flags & SDL.SDL_BUTTON_X2MASK) >> 4);

            state.ScrollWheelValue = _MouseWheel;

            return state;
        }

        /// <summary>
        /// Sets mouse cursor's relative position to game-window.
        /// </summary>
        /// <param name="x">Relative horizontal position of the cursor.</param>
        /// <param name="y">Relative vertical position of the cursor.</param>
        public static void SetPosition(int x, int y)
        {
            // The state should appear to be what they _think_ they're setting first.
            state.X = x;
            state.Y = y;

            // Scale the mouse coordinates for the faux-backbuffer
            x = (int)((double)x * _WindowWidth / Game.Instance.GraphicsDevice.GLDevice.Backbuffer.Width);
            y = (int)((double)y * _WindowHeight / Game.Instance.GraphicsDevice.GLDevice.Backbuffer.Height);

            SDL.SDL_WarpMouseInWindow(WindowHandle, x, y);
            _IsWarped = true;
        }

        #endregion
    }
}
