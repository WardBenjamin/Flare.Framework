#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Runtime.InteropServices;

namespace Flare
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X, Y, Z, W;

        #region Static Constructors
        private static readonly Vector4 identity = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        public static Vector4 Identity
        {
            get { return identity; }
        }

        private static readonly Vector4 zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        public static Vector4 Zero
        {
            get { return zero; }
        }

        public static Vector4 One
        {
            get { return Identity; }
        }

        private static readonly Vector4 unitx = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
        public static Vector4 UnitX
        {
            get { return new Vector4(1.0f, 0.0f, 0.0f, 0.0f); }
        }

        private static readonly Vector4 unity = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
        public static Vector4 UnitY
        {
            get { return unity; }
        }

        private static readonly Vector4 unitz = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
        public static Vector4 UnitZ
        {
            get { return unitz; }
        }

        private static readonly Vector4 unitw = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        public static Vector4 UnitW
        {
            get { return unitw; }
        }
        #endregion

        #region Operators
        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W);
        }

        public static Vector4 operator +(Vector4 v, float s)
        {
            return new Vector4(v.X + s, v.Y + s, v.Z + s, v.W + s);
        }

        public static Vector4 operator +(float s, Vector4 v)
        {
            return new Vector4(v.X + s, v.Y + s, v.Z + s, v.W + s);
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);
        }

        public static Vector4 operator -(Vector4 v, float s)
        {
            return new Vector4(v.X - s, v.Y - s, v.Z - s, v.W - s);
        }

        public static Vector4 operator -(float s, Vector4 v)
        {
            return new Vector4(s - v.X, s - v.Y, s - v.Z, s - v.W);
        }

        public static Vector4 operator -(Vector4 v)
        {
            return new Vector4(-v.X, -v.Y, -v.Z, -v.W);
        }

        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z, v1.W * v2.W);
        }

        public static Vector4 operator *(float s, Vector4 v)
        {
            return new Vector4(v.X * s, v.Y * s, v.Z * s, v.W * s);
        }

        public static Vector4 operator *(Vector4 v, float s)
        {
            return new Vector4(v.X * s, v.Y * s, v.Z * s, v.W * s);
        }

        public static Vector4 operator /(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z, v1.W / v2.W);
        }

        public static Vector4 operator /(float s, Vector4 v)
        {
            return new Vector4(s / v.X, s / v.Y, s / v.Z, s / v.W);
        }

        public static Vector4 operator /(Vector4 v, float s)
        {
            return new Vector4(v.X / s, v.Y / s, v.Z / s, v.W / s);
        }

        public static bool operator ==(Vector4 v1, Vector4 v2)
        {
            return (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z && v1.W == v2.W);
        }

        public static bool operator !=(Vector4 v1, Vector4 v2)
        {
            return (v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z || v1.W != v2.W);
        }
        #endregion

        #region Constructors
        /// <summary>Create a Vector4 structure.</summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        /// <param name="w">w value</param>
        public Vector4(float x, float y, float z, float w)
        {
            this.X = x; this.Y = y; this.Z = z; this.W = w;
        }

        /// <summary>Creates a Vector4 structure.  Casted to floats for OpenGL.</summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        /// <param name="w">w value</param>
        public Vector4(double x, double y, double z, double w)
        {
            this.X = (float)x; this.Y = (float)y; this.Z = (float)z; this.W = (float)w;
        }

        /// <summary>Creates a Vector4 structure based on a Vector3 and w.</summary>
        /// <param name="v">Vector3 to make up X,Y,Z</param>
        /// <param name="w">Double to make up the w component</param>
        public Vector4(Vector3 v, float w)
        {
            X = v.X; Y = v.Y; Z = v.Z; this.W = w;
        }

        /// <summary>
        /// Accepts a 24 bit color value and assumes an alpha of 1.0f.
        /// </summary>
        /// <param name="RGBByte">24bit color value</param>
        public Vector4(byte[] RGBByte)
        {
            if (RGBByte.Length < 3) throw new Exception("Color data was not 24bit as expected.");
            X = (float)(RGBByte[0] / 256.0); Y = (float)(RGBByte[1] / 256.0); Z = (float)(RGBByte[2] / 256.0); W = 1.0f;
        }

        /// <summary>Creates a Vector4 tructure from a float array (assuming the float array is of length 4).</summary>
        /// <param name="vector">The float array to convert to a Vector4.</param>
        public Vector4(float[] vector)
        {
            if (vector.Length != 4) throw new Exception(string.Format("float[] vector was of length {0}.  Was expecting a length of 4.", vector.Length));
            this.X = vector[0]; this.Y = vector[1]; this.Z = vector[2]; this.W = vector[3];
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4)) return false;

            return this.Equals((Vector4)obj);
        }

        public bool Equals(Vector4 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "{" + X + ", " + Y + ", " + Z + ", " + W + "}";
        }

        /// <summary>
        /// Parses a JSON stream and produces a Vector4 struct.
        /// </summary>
        public static Vector4 Parse(string text)
        {
            string[] split = text.Trim(new char[] { '{', '}' }).Split(',');
            if (split.Length != 4) return Vector4.Zero;

            return new Vector4(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
        }

        public float this[int a]
        {
            get { return (a == 0) ? X : (a == 1) ? Y : (a == 2) ? Z : W; }
            set
            {
                if (a == 0) X = value;
                else if (a == 1) Y = value;
                else if (a == 2) Z = value;
                else if (a == 3) W = value;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get the length of the Vector4 structure.
        /// </summary>
        public float Length
        {
            get { return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
        }

        /// <summary>
        /// Get the squared length of the Vector4 structure.
        /// </summary>
        public float SquaredLength
        {
            get { return X * X + Y * Y + Z * Z + W * W; }
        }

        /// <summary>
        /// Gets or sets an OpenGl.Types.Vector2 with the X and Y components of this instance.
        /// </summary>
        public Vector2 Xy { get { return new Vector2(X, Y); } set { X = value.X; Y = value.Y; } }

        /// <summary>
        /// Gets or sets an OpenGl.Types.Vector3 with the X, Y and Z components of this instance.
        /// </summary>
        public Vector3 Xyz { get { return new Vector3(X, Y, Z); } set { X = value.X; Y = value.Y; Z = value.Z; } }
        #endregion

        #region Methods
        /// <summary>
        /// Vector4 scalar dot product.
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>Scalar dot product value</returns>
        public static float Dot(Vector4 v1, Vector4 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        }

        /// <summary>
        /// Vector4 scalar dot product.
        /// </summary>
        /// <param name="v">Second vector</param>
        /// <returns>Vector4.Dot(this, v)</returns>
        public float Dot(Vector4 v)
        {
            return Vector4.Dot(this, v);
        }

        /// <summary>
        /// Converts a Vector4 to a float array.  Useful for vector commands in GL.
        /// </summary>
        /// <returns>Float array representation of a Vector4</returns>
        public float[] ToFloat()
        {
            return new float[] { X, Y, Z, W };
        }

        /// <summary>
        /// Normalizes the Vector4 structure to have a peak value of one.
        /// </summary>
        /// <returns>if (Length = 0) return Zero; else return Vector4(X,Y,Z,w)/Length</returns>
        public Vector4 Normalize()
        {
            if (Length == 0) return Zero;
            else return new Vector4(X, Y, Z, W) / Length;
        }

        /// <summary>
        /// Checks to see if any value (X, Y, Z, w) are within 0.0001 of 0.
        /// If so this method truncates that value to zero.
        /// </summary>
        /// <returns>A truncated Vector4</returns>
        public Vector4 Truncate()
        {
            float _x = (System.Math.Abs(X) - 0.0001 < 0) ? 0 : X;
            float _y = (System.Math.Abs(Y) - 0.0001 < 0) ? 0 : Y;
            float _z = (System.Math.Abs(Z) - 0.0001 < 0) ? 0 : Z;
            float _w = (System.Math.Abs(W) - 0.0001 < 0) ? 0 : W;
            return new Vector4(_x, _y, _z, _w);
        }

        /// <summary>
        /// Store the minimum values of X, Y, Z, and w between the two vectors.
        /// </summary>
        /// <param name="v">Vector to check against</param>
        public void TakeMin(Vector4 v)
        {
            if (v.X < X) X = v.X;
            if (v.Y < Y) Y = v.Y;
            if (v.Z < Z) Z = v.Z;
            if (v.W < W) W = v.W;
        }

        /// <summary>
        /// Store the maximum values of X, Y, Z, and w  between the two vectors.
        /// </summary>
        /// <param name="v">Vector to check against</param>
        public void TakeMax(Vector4 v)
        {
            if (v.X > X) X = v.X;
            if (v.Y > Y) Y = v.Y;
            if (v.Z > Z) Z = v.Z;
            if (v.W > W) W = v.W;
        }

        /// <summary>
        /// Linear interpolates between two vectors to get a new vector.
        /// </summary>
        /// <param name="v1">Initial vector (amount = 0).</param>
        /// <param name="v2">Final vector (amount = 1).</param>
        /// <param name="amount">Amount of each vector to consider (0->1).</param>
        /// <returns>A linear interpolated Vector3.</returns>
        public static Vector4 Lerp(Vector4 v1, Vector4 v2, float amount)
        {
            return v1 + (v2 - v1) * amount;
        }

        /// <summary>
        /// Swaps two Vector4 structures by passing via reference.
        /// </summary>
        /// <param name="v1">The first Vector4 structure.</param>
        /// <param name="v2">The second Vector4 structure.</param>
        public static void Swap(ref Vector4 v1, ref Vector4 v2)
        {
            Vector4 t = v1;
            v1 = v2;
            v2 = t;
        }
        #endregion
    }
}
