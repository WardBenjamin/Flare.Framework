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

namespace Flare
{
    /// <summary>
    /// Describes a 2D-rectangle.
    /// </summary>
    [Serializable]
    public struct Rectangle : IEquatable<Rectangle>
    {
        #region Public Properties

        /// <summary>
        /// Returns the X coordinate of the left edge of this <see cref="Rectangle"/>.
        /// </summary>
        public int Left
        {
            get
            {
                return X;
            }
        }

        /// <summary>
        /// Returns the X coordinate of the right edge of this <see cref="Rectangle"/>.
        /// </summary>
        public int Right
        {
            get
            {
                return (X + Width);
            }
        }

        /// <summary>
        /// Returns the Y coordinate of the top edge of this <see cref="Rectangle"/>.
        /// </summary>
        public int Top
        {
            get
            {
                return Y;
            }
        }

        /// <summary>
        /// Returns the Y coordinate of the bottom edge of this <see cref="Rectangle"/>.
        /// </summary>
        public int Bottom
        {
            get
            {
                return (Y + Height);
            }
        }

        /// <summary>
        /// The top-left coordinates of this <see cref="Rectangle"/>.
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// The width and height of this <see cref="Rectangle"/>.
        /// </summary>
        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.Width, this.Height);
            }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        /// <summary>
        /// A <see cref="Point"/> located in the center of this <see cref="Rectangle"/>'s bounds.
        /// </summary>
        /// <remarks>
        /// If <see cref="Width"/> or <see cref="Height"/> is an odd number,
        /// the center point will be rounded down.
        /// </remarks>
        public Point Center
        {
            get
            {
                return new Point(
                    X + (Width / 2),
                    Y + (Height / 2)
                );
            }
        }

        /// <summary>
        /// Whether or not this <see cref="Rectangle"/> has a width and
        /// height of 0, and a position of (0, 0).
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((Width == 0) &&
                        (Height == 0) &&
                        (X == 0) &&
                        (Y == 0));
            }
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Returns a <see cref="Rectangle"/> with X=0, Y=0, Width=0, and Height=0.
        /// </summary>
        public static Rectangle Empty
        {
            get
            {
                return emptyRectangle;
            }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    X.ToString(), " ",
                    Y.ToString(), " ",
                    Width.ToString(), " ",
                    Height.ToString()
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// The X coordinate of the top-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y coordinate of the top-left corner of this <see cref="Rectangle"/>.
        /// </summary>
        public int Y;

        /// <summary>
        /// The width of this <see cref="Rectangle"/>.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of this <see cref="Rectangle"/>.
        /// </summary>
        public int Height;

        #endregion

        #region Private Static Fields

        private static Rectangle emptyRectangle = new Rectangle();

        #endregion

        #region Public Constructors

        /// <summary>
        /// Creates a <see cref="Rectangle"/> with the specified
        /// position, width, and height.
        /// </summary>
        /// <param name="x">The X coordinate of the top-left corner of the created <see cref="Rectangle"/>.</param>
        /// <param name="y">The Y coordinate of the top-left corner of the created <see cref="Rectangle"/>.</param>
        /// <param name="width">The width of the created <see cref="Rectangle"/>.</param>
        /// <param name="height">The height of the created <see cref="Rectangle"/>.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a <see cref="Rectangle"/> with the specified
        /// position and size.
        /// </summary>
        /// <param name="position">The X and Y position of the created <see cref="Rectangle"/>.</param>
        /// <param name="size">The width and height of the created <see cref="Rectangle"/>.</param>
        public Rectangle(Point position, System.Drawing.Size size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.Width;
            Height = size.Height;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="x">The X coordinate of the point to check for containment.</param>
        /// <param name="y">The Y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="Rectangle"/>. <c>false</c> otherwise.</returns>
        public bool Contains(int x, int y)
        {
            return ((this.X <= x) &&
                    (x < (this.X + this.Width)) &&
                    (this.Y <= y) &&
                    (y < (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="Rectangle"/>. <c>false</c> otherwise.</returns>
        public bool Contains(Point value)
        {
            return ((this.X <= value.X) &&
                    (value.X < (this.X + this.Width)) &&
                    (this.Y <= value.Y) &&
                    (value.Y < (this.Y + this.Height)));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Rectangle"/> lies within the bounds of this <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="value">The <see cref="Rectangle"/> to check for inclusion in this <see cref="Rectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Rectangle"/>'s bounds lie entirely inside this <see cref="Rectangle"/>. <c>false</c> otherwise.</returns>
        public bool Contains(Rectangle value)
        {
            return ((this.X <= value.X) &&
                    ((value.X + value.Width) <= (this.X + this.Width)) &&
                    (this.Y <= value.Y) &&
                    ((value.Y + value.Height) <= (this.Y + this.Height)));
        }

        public void Contains(ref Point value, out bool result)
        {
            result = ((this.X <= value.X) &&
                    (value.X < (this.X + this.Width)) &&
                    (this.Y <= value.Y) &&
                    (value.Y < (this.Y + this.Height)));
        }

        public void Contains(ref Rectangle value, out bool result)
        {
            result = ((this.X <= value.X) &&
                    (value.X < (this.X + this.Width)) &&
                    (this.Y <= value.Y) &&
                    (value.Y < (this.Y + this.Height)));
        }

        /// <summary>
        /// Increments this <see cref="Rectangle"/>'s <see cref="Location"/> by the
        /// X and Y components of the provided <see cref="Point"/>.
        /// </summary>
        /// <param name="offset">The X and Y components to add to this <see cref="Rectangle"/>'s <see cref="Position"/>.</param>
        public void Offset(Point offset)
        {
            X += offset.X;
            Y += offset.Y;
        }

        /// <summary>
        /// Increments this <see cref="Rectangle"/>'s <see cref="Location"/> by the
        /// provided X and Y coordinates.
        /// </summary>
        /// <param name="offsetX">The X coordinate to add to this <see cref="Rectangle"/>'s <see cref="Location"/>.</param>
        /// <param name="offsetY">The Y coordinate to add to this <see cref="Rectangle"/>'s <see cref="Location"/>.</param>
        public void Offset(int offsetX, int offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        public void Inflate(int horizontalValue, int verticalValue)
        {
            X -= horizontalValue;
            Y -= verticalValue;
            Width += horizontalValue * 2;
            Height += verticalValue * 2;
        }

        /// <summary>
        /// Checks whether or not this <see cref="Rectangle"/> is equivalent
        /// to a provided <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Rectangle"/> to test for equality.</param>
        /// <returns>
        /// <c>true</c> if this <see cref="Rectangle"/>'s X coordinate, Y coordinate, width, and height
        /// match the values for the provided <see cref="Rectangle"/>. <c>false</c> otherwise.
        /// </returns>
        public bool Equals(Rectangle other)
        {
            return this == other;
        }

        /// <summary>
        /// Checks whether or not this <see cref="Rectangle"/> is equivalent
        /// to a provided object.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to test for equality.</param>
        /// <returns>
        /// <c>true</c> if the provided object is a <see cref="Rectangle"/>, and this
        /// <see cref="Rectangle"/>'s X coordinate, Y coordinate, width, and height
        /// match the values for the provided <see cref="Rectangle"/>. <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is Rectangle) && this == ((Rectangle)obj);
        }

        public override string ToString()
        {
            return (
                "{X:" + X.ToString() +
                " Y:" + Y.ToString() +
                " Width:" + Width.ToString() +
                " Height:" + Height.ToString() +
                "}"
            );
        }

        public override int GetHashCode()
        {
            return (this.X ^ this.Y ^ this.Width ^ this.Height);
        }

        /// <summary>
        /// Gets whether or not the other <see cref="Rectangle"/> intersects with this rectangle.
        /// </summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <returns><c>true</c> if other <see cref="Rectangle"/> intersects with this rectangle; <c>false</c> otherwise.</returns>
        public bool Intersects(Rectangle value)
        {
            return (value.Left < Right &&
                    Left < value.Right &&
                    value.Top < Bottom &&
                    Top < value.Bottom);
        }

        /// <summary>
        /// Gets whether or not the other <see cref="Rectangle"/> intersects with this rectangle.
        /// </summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="Rectangle"/> intersects with this rectangle; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref Rectangle value, out bool result)
        {
            result = (value.Left < Right &&
                    Left < value.Right &&
                    value.Top < Bottom &&
                    Top < value.Bottom);
        }

        #endregion

        #region Public Static Methods

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return ((a.X == b.X) &&
                    (a.Y == b.Y) &&
                    (a.Width == b.Width) &&
                    (a.Height == b.Height));
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        public static Rectangle Intersect(Rectangle value1, Rectangle value2)
        {
            Rectangle rectangle;
            Intersect(ref value1, ref value2, out rectangle);
            return rectangle;
        }

        public static void Intersect(
            ref Rectangle value1,
            ref Rectangle value2,
            out Rectangle result
        )
        {
            if (value1.Intersects(value2))
            {
                int right_side = Math.Min(
                    value1.X + value1.Width,
                    value2.X + value2.Width
                );
                int left_side = Math.Max(value1.X, value2.X);
                int top_side = Math.Max(value1.Y, value2.Y);
                int bottom_side = Math.Min(
                    value1.Y + value1.Height,
                    value2.Y + value2.Height
                );
                result = new Rectangle(
                    left_side,
                    top_side,
                    right_side - left_side,
                    bottom_side - top_side
                );
            }
            else
            {
                result = new Rectangle(0, 0, 0, 0);
            }
        }

        public static Rectangle Union(Rectangle value1, Rectangle value2)
        {
            int x = Math.Min(value1.X, value2.X);
            int y = Math.Min(value1.Y, value2.Y);
            return new Rectangle(
                x,
                y,
                Math.Max(value1.Right, value2.Right) - x,
                Math.Max(value1.Bottom, value2.Bottom) - y
            );
        }

        public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            result.X = Math.Min(value1.X, value2.X);
            result.Y = Math.Min(value1.Y, value2.Y);
            result.Width = Math.Max(value1.Right, value2.Right) - result.X;
            result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
        }

        #endregion

        #region Implicit Type Conversions

        public static implicit operator System.Drawing.Rectangle(Rectangle d)
        {
            return new System.Drawing.Rectangle(d.Location, d.Size);
        }

        public static implicit operator Rectangle(System.Drawing.Rectangle d)
        {
            return new Rectangle(d.Location, d.Size);
        }

        #endregion
    }
}
