#region License
/* Flare - A Framework by Developers, for Developers.
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

namespace Flare.Input
{
    /// <summary>
    /// Defines a button state for buttons of mouse, gamepad or joystick.
    /// </summary>
    public enum ButtonState
    {
        /// <summary>
        /// The button is released.
        /// </summary>
        Released,
        /// <summary>
        /// The button is pressed.
        /// </summary>
        Pressed
    }
}
