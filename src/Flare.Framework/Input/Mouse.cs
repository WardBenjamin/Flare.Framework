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

using Flare.SDL2;

namespace Flare.Input
{
    /// <summary>
    /// Allows reading position and button click information from mouse.
    /// </summary>
    public static class Mouse
    {
        /// <summary>
        /// Gets mouse state information that includes position and button
        /// presses for the provided window
        /// </summary>
        /// <returns>Current state of the mouse.</returns>
        public static MouseState GetState()
        {
            return SDL2_MouseUtil.GetState();
        }


        /// <summary>
        /// Sets mouse cursor's relative position to game-window.
        /// </summary>
        /// <param name="x">Relative horizontal position of the cursor.</param>
        /// <param name="y">Relative vertical position of the cursor.</param>
        public static void SetPosition(int x, int y)
        {
            SDL2_MouseUtil.SetPosition(x, y);
        }
    }

    /// <summary>
    /// Represents a mouse state with cursor position and button press information.
    /// </summary>
    public struct MouseState
    {
        #region Public Properties

        /// <summary>
        /// Gets horizontal position of the cursor.
        /// </summary>
        public int X
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets vertical position of the cursor.
        /// </summary>
        public int Y
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets state of the left mouse button.
        /// </summary>
        public ButtonState LeftButton
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets state of the middle mouse button.
        /// </summary>
        public ButtonState MiddleButton
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets state of the right mouse button.
        /// </summary>
        public ButtonState RightButton
        {
            get;
            internal set;
        }

        /// <summary>
        /// Returns cumulative scroll wheel value since the game start.
        /// </summary>
        public int ScrollWheelValue
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets state of the XButton1.
        /// </summary>
        public ButtonState XButton1
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets state of the XButton2.
        /// </summary>
        public ButtonState XButton2
        {
            get;
            internal set;
        }

        #endregion

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the MouseState.
        /// </summary>
        /// <param name="x">Horizontal position of the mouse.</param>
        /// <param name="y">Vertical position of the mouse.</param>
        /// <param name="scrollWheel">Mouse scroll wheel's value.</param>
        /// <param name="leftButton">Left mouse button's state.</param>
        /// <param name="middleButton">Middle mouse button's state.</param>
        /// <param name="rightButton">Right mouse button's state.</param>
        /// <param name="xButton1">XBUTTON1's state.</param>
        /// <param name="xButton2">XBUTTON2's state.</param>
        /// <remarks>
        /// Normally <see cref="Mouse.GetState()"/> should be used to get mouse current
        /// state. The constructor is provided for simulating mouse input.
        /// </remarks>
        public MouseState(
            int x,
            int y,
            int scrollWheel,
            ButtonState leftButton,
            ButtonState middleButton,
            ButtonState rightButton,
            ButtonState xButton1,
            ButtonState xButton2
        ) : this()
        {
            X = x;
            Y = y;
            ScrollWheelValue = scrollWheel;
            LeftButton = leftButton;
            MiddleButton = middleButton;
            RightButton = rightButton;
            XButton1 = xButton1;
            XButton2 = xButton2;
        }

        #endregion

        #region Public Static Operators and Override Methods

        /// <summary>
        /// Compares whether two MouseState instances are equal.
        /// </summary>
        /// <param name="left">MouseState instance on the left of the equal sign.</param>
        /// <param name="right">MouseState instance on the right of the equal sign.</param>
        /// <returns>true if the instances are equal; false otherwise.</returns>
        public static bool operator ==(MouseState left, MouseState right)
        {
            return (left.X == right.X &&
                    left.Y == right.Y &&
                    left.LeftButton == right.LeftButton &&
                    left.MiddleButton == right.MiddleButton &&
                    left.RightButton == right.RightButton &&
                    left.ScrollWheelValue == right.ScrollWheelValue);
        }

        /// <summary>
        /// Compares whether two MouseState instances are not equal.
        /// </summary>
        /// <param name="left">MouseState instance on the left of the equal sign.</param>
        /// <param name="right">MouseState instance on the right of the equal sign.</param>
        /// <returns>true if the objects are not equal; false otherwise.</returns>
        public static bool operator !=(MouseState left, MouseState right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified object.
        /// </summary>
        /// <param name="obj">The MouseState to compare.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (obj is MouseState) && (this == (MouseState)obj);
        }

        /// <summary>
        /// Gets the hash code for MouseState instance.
        /// </summary>
        /// <returns>Hash code of the object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}