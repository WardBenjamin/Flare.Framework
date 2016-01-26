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
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X, Y, Z;

        #region Static Constructors
        private static readonly Vector3 identity = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 Identity
        {
            get { return identity; }
        }

        private static readonly Vector3 zero = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3 Zero
        {
            get { return zero; }
        }

        public static Vector3 One
        {
            get { return Identity; }
        }

        private static readonly Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        public static Vector3 Up
        {
            get { return up; }
        }

        private static readonly Vector3 down = new Vector3(0.0f, -1.0f, 0.0f);
        public static Vector3 Down
        {
            get { return down; }
        }

        private static readonly Vector3 forward = new Vector3(0.0f, 0.0f, -1.0f);
        public static Vector3 Forward
        {
            get { return forward; }
        }

        private static readonly Vector3 backward = new Vector3(0.0f, 0.0f, 1.0f);
        public static Vector3 Backward
        {
            get { return backward; }
        }

        private static readonly Vector3 left = new Vector3(-1.0f, 0.0f, 0.0f);
        public static Vector3 Left
        {
            get { return left; }
        }

        private static readonly Vector3 right = new Vector3(1.0f, 0.0f, 0.0f);
        public static Vector3 Right
        {
            get { return right; }
        }

        private static readonly Vector3 unitx = new Vector3(1.0f, 0.0f, 0.0f);
        public static Vector3 UnitX
        {
            get { return unitx; }
        }

        private static readonly Vector3 unity = new Vector3(0.0f, 1.0f, 0.0f);
        public static Vector3 UnitY
        {
            get { return unity; }
        }

        private static readonly Vector3 unitz = new Vector3(0.0f, 0.0f, 1.0f);
        public static Vector3 UnitZ
        {
            get { return unitz; }
        }

        private static readonly Vector3 unitscale = new Vector3(1.0f, 1.0f, 1.0f);
        public static Vector3 UnitScale
        {
            get { return unitscale; }
        }
        #endregion

        #region Operators
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator +(Vector3 v, float s)
        {
            return new Vector3(v.X + s, v.Y + s, v.Z + s);
        }

        public static Vector3 operator +(float s, Vector3 v)
        {
            return new Vector3(v.X + s, v.Y + s, v.Z + s);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator -(Vector3 v, float s)
        {
            return new Vector3(v.X - s, v.Y - s, v.Z - s);
        }

        public static Vector3 operator -(float s, Vector3 v)
        {
            return new Vector3(s - v.X, s - v.Y, s - v.Z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        public static Vector3 operator *(Vector3 v, float s)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector3 operator /(float s, Vector3 v)
        {
            return new Vector3(s / v.X, s / v.Y, s / v.Z);
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            return new Vector3(v.X / s, v.Y / s, v.Z / s);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return (v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z);
        }
        #endregion

        #region Constructors
        /// <summary>Create a Vector3 structure, normally used to store Vertex positions.</summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public Vector3(float x, float y, float z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }

        /// <summary>Creates a Vector3 structure, normally used to store Vertex positions.  Casted to floats for OpenGL.</summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public Vector3(double x, double y, double z)
        {
            this.X = (float)x; this.Y = (float)y; this.Z = (float)z;
        }

        /// <summary>Creates a Vector3 tructure from a float array (assuming the float array is of length 3).</summary>
        /// <param name="vector">The float array to convert to a Vector3.</param>
        public Vector3(float[] vector)
        {
            if (vector.Length != 3) throw new Exception(string.Format("float[] vector was of length {0}.  Was expecting a length of 3.", vector.Length));
            this.X = vector[0]; this.Y = vector[1]; this.Z = vector[2];
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3)) return false;

            return this.Equals((Vector3)obj);
        }

        public bool Equals(Vector3 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "{" + X + ", " + Y + ", " + Z + "}";
        }

        /// <summary>
        /// Parses a JSON stream and produces a Vector3 struct.
        /// </summary>
        public static Vector3 Parse(string text)
        {
            string[] split = text.Trim(new char[] { '{', '}' }).Split(',');
            if (split.Length != 3) return Vector3.Zero;

            return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
        }

        public float this[int a]
        {
            get { return (a == 0) ? X : (a == 1) ? Y : Z; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts a Vector3 to a float array.  Useful for vector commands in GL.
        /// </summary>
        /// <returns>Float array representation of a Vector3</returns>
        public float[] ToFloat()
        {
            return new float[] { X, Y, Z };
        }

        /// <summary>
        /// Get the length of the Vector3 structure.
        /// </summary>
        public float Length
        {
            get { return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// Performs the Vector3 scalar dot product.
        /// </summary>
        /// <param name="v1">The left Vector3.</param>
        /// <param name="v2">The right Vector3.</param>
        /// <returns>Scalar dot product value</returns>
        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Performs the Vector3 scalar dot product.
        /// </summary>
        /// <param name="v">Second dot product term</param>
        /// <returns>Vector3.Dot(this, v)</returns>
        public float Dot(Vector3 v)
        {
            return Vector3.Dot(this, v);
        }

        /// <summary>
        /// Returns the squared length of the Vector3 structure.
        /// </summary>
        public float SquaredLength
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        /// <summary>
        /// Vector3 cross product
        /// </summary>
        /// <param name="v1">Vector1</param>
        /// <param name="v2">Vector2</param>
        /// <returns>Vector3 cross product value</returns>
        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        /// <summary>
        /// Vector3 cross product
        /// </summary>
        /// <param name="v">Second cross product term</param>
        /// <returns>this X v</returns>
        public Vector3 Cross(Vector3 v)
        {
            return Vector3.Cross(this, v);
        }

        /// <summary>
        /// Normalizes the Vector3 structure to have a peak value of one.
        /// </summary>
        /// <returns>if (Length = 0) return Zero; else return Vector3(X,Y,Z)/Length</returns>
        public Vector3 Normalize()
        {
            if (Length == 0) return Zero;
            else return new Vector3(X, Y, Z) / Length;
        }

        /// <summary>
        /// Checks to see if any value (X, Y, Z) are within 0.0001 of 0.
        /// If so this method truncates that value to zero.
        /// </summary>
        /// <returns>A truncated Vector3</returns>
        public Vector3 Truncate()
        {
            float _x = (System.Math.Abs(X) - 0.0001 < 0) ? 0 : X;
            float _y = (System.Math.Abs(Y) - 0.0001 < 0) ? 0 : Y;
            float _z = (System.Math.Abs(Z) - 0.0001 < 0) ? 0 : Z;
            return new Vector3(_x, _y, _z);
        }

        /// <summary>
        /// Store the minimum values of X, Y, and Z between the two vectors.
        /// </summary>
        /// <param name="v">Vector to check against</param>
        public void TakeMin(Vector3 v)
        {
            if (v.X < X) X = v.X;
            if (v.Y < Y) Y = v.Y;
            if (v.Z < Z) Z = v.Z;
        }

        /// <summary>
        /// Store the maximum values of X, Y, and Z between the two vectors.
        /// </summary>
        /// <param name="v">Vector to check against</param>
        public void TakeMax(Vector3 v)
        {
            if (v.X > X) X = v.X;
            if (v.Y > Y) Y = v.Y;
            if (v.Z > Z) Z = v.Z;
        }

        /// <summary>
        /// Returns the maximum component of the Vector3.
        /// </summary>
        /// <returns>The maximum component of the Vector3</returns>
        public float Max()
        {
            return (X >= Y && X >= Z) ? X : (Y >= Z) ? Y : Z;
        }

        /// <summary>
        /// Returns the minimum component of the Vector3.
        /// </summary>
        /// <returns>The minimum component of the Vector3</returns>
        public float Min()
        {
            return (X <= Y && X <= Z) ? X : (Y <= Z) ? Y : Z;
        }

        /// <summary>
        /// Linear interpolates between two vectors to get a new vector.
        /// </summary>
        /// <param name="v1">Initial vector (amount = 0).</param>
        /// <param name="v2">Final vector (amount = 1).</param>
        /// <param name="amount">Amount of each vector to consider (0->1).</param>
        /// <returns>A linear interpolated Vector3.</returns>
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float amount)
        {
            return v1 + (v2 - v1) * amount;
        }

        /// <summary>
        /// Calculates the angle (in radians) between two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>Angle (in radians) between the vectors.</returns>
        /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
        public static float CalculateAngle(Vector3 first, Vector3 second)
        {
            return (float)System.Math.Acos((Vector3.Dot(first, second)) / (first.Length * second.Length));
        }

        /// <summary>
        /// Calculates the angle (in radians) between two vectors.
        /// </summary>
        /// <param name="v">The second vector.</param>
        /// <returns>Angle (in radians) between the vectors.</returns>
        /// <remarks>Note that the returned angle is never bigger than the constant Pi.</remarks>
        public float CalculateAngle(Vector3 v)
        {
            return Vector3.CalculateAngle(this, v);
        }

        /// <summary>
        /// Swaps two Vector3 structures by passing via reference.
        /// </summary>
        /// <param name="v1">The first Vector3 structure.</param>
        /// <param name="v2">The second Vector3 structure.</param>
        public static void Swap(ref Vector3 v1, ref Vector3 v2)
        {
            Vector3 t = v1;
            v1 = v2;
            v2 = t;
        }

        /// <summary>
        /// Create a quaternion that represents the rotation vector between this
        /// Vector3 and a destination Vector3.
        /// </summary>
        /// <param name="destination">The vector we would like to rotate to.</param>
        /// <returns>A quaternion representing the axis of rotation between this vector 
        /// and the destination vector.</returns>
        public Quaternion GetRotationTo(Vector3 destination)
        {
            // Based on Stan Melax's algorithm in "Game Programming Gems"
            Vector3 t_source = this.Normalize();
            Vector3 t_dest = destination.Normalize();

            float d = t_source.Dot(t_dest);

            // if dot == 1 then the vectors are the same
            if (d >= 1.0f) return Quaternion.Identity;
            else if (d < (1e-6f - 1.0f))
            {
                Vector3 t_axis = Vector3.UnitX.Cross(this);
                if (t_axis.SquaredLength < (1e-12)) // pick another if colinear
                    t_axis = Vector3.UnitY.Cross(this);
                t_axis.Normalize();
                return Quaternion.FromAngleAxis((float)System.Math.PI, t_axis);
            }
            else
            {
                float t_sqrt = (float)System.Math.Sqrt((1 + d) * 2.0f);
                float t_invs = 1.0f / t_sqrt;

                Vector3 t_cross = t_source.Cross(t_dest);
                return new Quaternion(t_cross.X * t_invs, t_cross.Y * t_invs, t_cross.Z * t_invs, t_sqrt * 0.5f).Normalize();
            }
        }
        #endregion
    }
}
