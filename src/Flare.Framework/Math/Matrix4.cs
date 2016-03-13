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
    public struct Matrix4 : IEquatable<Matrix4>
    {
        #region Public Fields

        /// <summary>
        /// The first row and first column value.
        /// </summary>
        public float M11;

        /// <summary>
        /// The first row and second column value.
        /// </summary>
        public float M12;

        /// <summary>
        /// The first row and third column value.
        /// </summary>
        public float M13;

        /// <summary>
        /// The first row and fourth column value.
        /// </summary>
        public float M14;

        /// <summary>
        /// The second row and first column value.
        /// </summary>
        public float M21;

        /// <summary>
        /// The second row and second column value.
        /// </summary>
        public float M22;

        /// <summary>
        /// The second row and third column value.
        /// </summary>
        public float M23;

        /// <summary>
        /// The second row and fourth column value.
        /// </summary>
        public float M24;

        /// <summary>
        /// The third row and first column value.
        /// </summary>
        public float M31;

        /// <summary>
        /// The third row and second column value.
        /// </summary>
        public float M32;

        /// <summary>
        /// The third row and third column value.
        /// </summary>
        public float M33;

        /// <summary>
        /// The third row and fourth column value.
        /// </summary>
        public float M34;

        /// <summary>
        /// The fourth row and first column value.
        /// </summary>
        public float M41;

        /// <summary>
        /// The fourth row and second column value.
        /// </summary>
        public float M42;

        /// <summary>
        /// The fourth row and third column value.
        /// </summary>
        public float M43;

        /// <summary>
        /// The fourth row and fourth column value.
        /// </summary>
        public float M44;

        #endregion

        #region Static Members

        private static Matrix4 identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        public static Matrix4 Identity
        {
            get { return identity; }
        }

        #endregion

        #region Operators

        public static Matrix4 operator +(Matrix4 m1, Matrix4 m2)
        {
            return new Matrix4
            (
                m1.M11 + m2.M11,
                m2.M12 + m2.M12,
                m2.M13 + m2.M13,
                m2.M14 + m2.M14,
                m2.M21 + m2.M21,
                m2.M22 + m2.M22,
                m2.M23 + m2.M23,
                m2.M24 + m2.M24,
                m2.M31 + m2.M31,
                m2.M32 + m2.M32,
                m2.M33 + m2.M33,
                m2.M34 + m2.M34,
                m2.M41 + m2.M41,
                m2.M42 + m2.M42,
                m2.M43 + m2.M43,
                m2.M44 + m2.M44
            );
        }

        public static Matrix4 operator -(Matrix4 m1, Matrix4 m2)
        {
            return new Matrix4
            (
                m1.M11 - m2.M11,
                m2.M12 - m2.M12,
                m2.M13 - m2.M13,
                m2.M14 - m2.M14,
                m2.M21 - m2.M21,
                m2.M22 - m2.M22,
                m2.M23 - m2.M23,
                m2.M24 - m2.M24,
                m2.M31 - m2.M31,
                m2.M32 - m2.M32,
                m2.M33 - m2.M33,
                m2.M34 - m2.M34,
                m2.M41 - m2.M41,
                m2.M42 - m2.M42,
                m2.M43 - m2.M43,
                m2.M44 - m2.M44
            );
        }

        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2)
        {
            return new Matrix4
            (
                m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41,
                m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42,
                m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43,
                m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44,

                m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41,
                m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42,
                m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43,
                m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44,

                m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41,
                m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42,
                m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43,
                m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44,

                m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41,
                m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42,
                m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43,
                m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44
            );
        }

        public static Matrix4 operator *(Matrix4 m1, float d)
        {
            Matrix4 ret = Matrix4.Identity;
            for (int i = 0; i < 16; i++)
                ret[i] = m1[i] * d;
            return ret;
        }

        public static Matrix4 operator /(Matrix4 m1, float d)
        {
            Matrix4 ret = Matrix4.Identity;
            for (int i = 0; i < 16; i++)
                ret[i] = m1[i] / d;
            return ret;
        }

        public static Vector3 operator *(Matrix4 m1, Vector3 v)
        {
            return new Vector3
            (
                m1.M11 * v.X + m1.M12 * v.Y + m1.M13 * v.Z,
                m1.M21 * v.X + m1.M22 * v.Y + m1.M23 * v.Z,
                m1.M31 * v.X + m1.M32 * v.Y + m1.M33 * v.Z
            );
        }

        public static Vector3 operator *(Vector3 v, Matrix4 m1)
        {
            return new Vector3
            (
                v.X * m1.M11 + v.Y * m1.M21 + v.Z * m1.M31,
                v.X * m1.M12 + v.Y * m1.M22 + v.Z * m1.M32,
                v.X * m1.M13 + v.Y * m1.M23 + v.Z * m1.M33
            );
        }

        public static Vector4 operator *(Matrix4 m1, Vector4 v)
        {
            return new Vector4
            (
                m1.M11 * v.X + m1.M12 * v.Y + m1.M13 * v.Z + m1.M14 * v.W,
                m1.M21 * v.X + m1.M22 * v.Y + m1.M23 * v.Z + m1.M24 * v.W,
                m1.M31 * v.X + m1.M32 * v.Y + m1.M33 * v.Z + m1.M34 * v.W,
                m1.M41 * v.X + m1.M42 * v.Y + m1.M43 * v.Z + m1.M44 * v.W
            );
        }

        public static Vector4 operator *(Vector4 v, Matrix4 m1)
        {
            return new Vector4
            (
                v.X * m1.M11 + v.Y * m1.M21 + v.Z * m1.M31 + v.W * m1.M41,
                v.X * m1.M12 + v.Y * m1.M22 + v.Z * m1.M32 + v.W * m1.M42,
                v.X * m1.M13 + v.Y * m1.M23 + v.Z * m1.M33 + v.W * m1.M43,
                v.X * m1.M14 + v.Y * m1.M24 + v.Z * m1.M34 + v.W * m1.M44
            );
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return M11;
                    case 1: return M12;
                    case 2: return M13;
                    case 3: return M14;
                    case 4: return M21;
                    case 5: return M22;
                    case 6: return M23;
                    case 7: return M24;
                    case 8: return M31;
                    case 9: return M32;
                    case 10: return M33;
                    case 11: return M34;
                    case 12: return M41;
                    case 13: return M42;
                    case 14: return M43;
                    case 15: return M44;
                }
                throw new ArgumentOutOfRangeException();
            }
            set
            {
                switch (index)
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M13 = value; break;
                    case 3: M14 = value; break;
                    case 4: M21 = value; break;
                    case 5: M22 = value; break;
                    case 6: M23 = value; break;
                    case 7: M24 = value; break;
                    case 8: M31 = value; break;
                    case 9: M32 = value; break;
                    case 10: M33 = value; break;
                    case 11: M34 = value; break;
                    case 12: M41 = value; break;
                    case 13: M42 = value; break;
                    case 14: M43 = value; break;
                    case 15: M44 = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public float this[int row, int column]
        {
            get
            {
                return this[(row * 4) + column];
            }

            set
            {
                this[(row * 4) + column] = value;
            }
        }

        public static bool operator ==(Matrix4 m1, Matrix4 m2)
        {
            return
            (
                m1.M11 == m2.M11 ||
                m1.M12 == m2.M12 ||
                m1.M13 == m2.M13 ||
                m1.M14 == m2.M14 ||
                m1.M21 == m2.M21 ||
                m1.M22 == m2.M22 ||
                m1.M23 == m2.M23 ||
                m1.M24 == m2.M24 ||
                m1.M31 == m2.M31 ||
                m1.M32 == m2.M32 ||
                m1.M33 == m2.M33 ||
                m1.M34 == m2.M34 ||
                m1.M41 == m2.M41 ||
                m1.M42 == m2.M42 ||
                m1.M43 == m2.M43 ||
                m1.M44 == m2.M44
            );
        }

        public static bool operator !=(Matrix4 m1, Matrix4 m2)
        {
            return
            (
                m1.M11 != m2.M11 ||
                m1.M12 != m2.M12 ||
                m1.M13 != m2.M13 ||
                m1.M14 != m2.M14 ||
                m1.M21 != m2.M21 ||
                m1.M22 != m2.M22 ||
                m1.M23 != m2.M23 ||
                m1.M24 != m2.M24 ||
                m1.M31 != m2.M31 ||
                m1.M32 != m2.M32 ||
                m1.M33 != m2.M33 ||
                m1.M34 != m2.M34 ||
                m1.M41 != m2.M41 ||
                m1.M42 != m2.M42 ||
                m1.M43 != m2.M43 ||
                m1.M44 != m2.M44
            );
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4)) return false;

            return this.Equals((Matrix4)obj);
        }

        public bool Equals(Matrix4 other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string c = ", ";
            return "[ "
                + M11 + c + M12 + c + M13 + c + M14 + " ] [ "
                + M21 + c + M22 + c + M23 + c + M24 + " ] [ "
                + M31 + c + M32 + c + M33 + c + M34 + " ] [ "
                + M41 + c + M42 + c + M43 + c + M44
                + " ]";
        }
        #endregion

        #region Constructors

        public Matrix4(float M11, float M12, float M13, float M14,
            float M21, float M22, float M23, float M24,
            float M31, float M32, float M33, float M34,
            float M41, float M42, float M43, float M44)
        {
            this.M11 = M11;
            this.M12 = M12;
            this.M13 = M13;
            this.M14 = M14;
            this.M21 = M21;
            this.M22 = M22;
            this.M23 = M23;
            this.M24 = M24;
            this.M31 = M31;
            this.M32 = M32;
            this.M33 = M33;
            this.M34 = M34;
            this.M41 = M41;
            this.M42 = M42;
            this.M43 = M43;
            this.M44 = M44;
        }

        public Matrix4(Matrix4 existingMatrix)
        {
            this.M11 = existingMatrix.M11;
            this.M12 = existingMatrix.M12;
            this.M13 = existingMatrix.M13;
            this.M14 = existingMatrix.M14;
            this.M21 = existingMatrix.M21;
            this.M22 = existingMatrix.M22;
            this.M23 = existingMatrix.M23;
            this.M24 = existingMatrix.M24;
            this.M31 = existingMatrix.M31;
            this.M32 = existingMatrix.M32;
            this.M33 = existingMatrix.M33;
            this.M34 = existingMatrix.M34;
            this.M41 = existingMatrix.M41;
            this.M42 = existingMatrix.M42;
            this.M43 = existingMatrix.M43;
            this.M44 = existingMatrix.M44;
        }

        public Matrix4(Vector4 v0, Vector4 v1, Vector4 v2, Vector4 v3) : this()
        {
            SetMatrix(v0, v1, v2, v3);
        }

        public Matrix4(float[] array)
        {
            this.M11 = array[0];
            this.M12 = array[1];
            this.M13 = array[2];
            this.M14 = array[3];
            this.M21 = array[4];
            this.M22 = array[5];
            this.M23 = array[6];
            this.M24 = array[7];
            this.M31 = array[8];
            this.M32 = array[9];
            this.M33 = array[10];
            this.M34 = array[11];
            this.M41 = array[12];
            this.M42 = array[13];
            this.M43 = array[14];
            this.M44 = array[15];
        }

        public Matrix4(double[] array)
        {
            this.M11 = (float)array[0];
            this.M12 = (float)array[1];
            this.M13 = (float)array[2];
            this.M14 = (float)array[3];
            this.M21 = (float)array[4];
            this.M22 = (float)array[5];
            this.M23 = (float)array[6];
            this.M24 = (float)array[7];
            this.M31 = (float)array[8];
            this.M32 = (float)array[9];
            this.M33 = (float)array[10];
            M34 = (float)array[11];
            M41 = (float)array[12];
            M42 = (float)array[13];
            M43 = (float)array[14];
            M44 = (float)array[15];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a matrix which contains information on how to translate.
        /// </summary>
        /// <param name="translation">Amount to translate by in the X, Y and Z direction.</param>
        /// <returns>A Matrix4 object that contains the translation matrix.</returns>
        public static Matrix4 CreateTranslation(Vector3 translation)
        {
            Matrix4 result = Matrix4.Identity;
            result.M41 = translation.X;
            result.M42 = translation.Y;
            result.M43 = translation.Z;
            result.M44 = 1.0f;
            return result;
        }

        /// <summary>
        /// Creates a matrix which contains information on how to rotate about the X axis.
        /// </summary>
        /// <param name="angle">Amount to rotate in radians (counter-clockwise).</param>
        /// <returns>A Matrix4 object that contains the rotation matrix.</returns>
        public static Matrix4 CreateRotationX(float angle)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            return new Matrix4
            (
                1, 0, 0, 0,
                0, cos, sin, 0,
                0, -sin, cos, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a matrix which contains information on how to rotate about the Y axis.
        /// </summary>
        /// <param name="angle">Amount to rotate in radians (counter-clockwise).</param>
        /// <returns>A Matrix4 object that contains the rotation matrix.</returns>
        public static Matrix4 CreateRotationY(float angle)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            return new Matrix4
            (
                cos, 0, -sin, 0,
                0, 1, 0, 0,
                sin, 0, cos, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Creates a matrix which contains information on how to rotate about the Z axis.
        /// </summary>
        /// <param name="angle">Amount to rotate in radians (counter-clockwise).</param>
        /// <returns>A Matrix4 object that contains the rotation matrix.</returns>
        public static Matrix4 CreateRotationZ(float angle)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            return new Matrix4
            (
                cos, sin, 0.0f, 0,
                -sin, cos, 0.0f, 0.0f,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Create a rotation matrix about an arbitrary axis.
        /// </summary>
        /// <param name="axis">Arbitrary axis for rotation.</param>
        /// <param name="angle">Amount to rotate in radians.</param>
        /// <returns>A Matrix4 object that contains the rotation matrix.</returns>
        public static Matrix4 CreateRotation(Vector3 axis, float angle)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);
            float tan = 1.0f - cos;

            return new Matrix4
            (
                tan * axis.X * axis.X + cos,
                tan * axis.X * axis.Y - sin * axis.Z,
                tan * axis.X * axis.Z + sin * axis.Y,
                0,

                tan * axis.Y * axis.X + sin * axis.Z,
                tan * axis.Y * axis.Y + cos,
                tan * axis.Y * axis.Z - sin * axis.X,
                0,

                tan * axis.Z * axis.X - sin * axis.Y,
                tan * axis.Z * axis.Y + sin * axis.X,
                tan * axis.Z * axis.Z + cos,
                0,

                0, 0, 0, 1
            );
        }

        /// <summary>
        /// Build a rotation matrix from the specified axis/angle rotation.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">Angle in radians to rotate counter-clockwise (looking in the direction of the given axis).</param>
        public static Matrix4 CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float cos = (float)System.Math.Cos(-angle);
            float sin = (float)System.Math.Sin(-angle);
            float t = 1.0f - cos;

            axis.Normalize();

            return new Matrix4(new Vector4(t * axis.X * axis.X + cos, t * axis.X * axis.Y - sin * axis.Z, t * axis.X * axis.Z + sin * axis.Y, 0.0f),
                                 new Vector4(t * axis.X * axis.Y + sin * axis.Z, t * axis.Y * axis.Y + cos, t * axis.Y * axis.Z - sin * axis.X, 0.0f),
                                 new Vector4(t * axis.X * axis.Z - sin * axis.Y, t * axis.Y * axis.Z + sin * axis.X, t * axis.Z * axis.Z + cos, 0.0f),
                                 Vector4.UnitW);
        }

        /// <summary>Creates a matrix which contains scale factor</summary>
        /// <param name="scale">Amount to scale by in the X, Y and Z direction</param>
        /// <returns>A Matrix4 object that contains the scaling matrix</returns>
        public static Matrix4 CreateScale(Vector3 scale)
        {
            return new Matrix4(new Vector4(scale.X, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, scale.Y, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, scale.Z, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        }

        /// <summary>
        /// Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="width">The width of the projection volume.</param>
        /// <param name="height">The height of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <rereturns>The resulting Matrix4 instance.</rereturns>
        public static Matrix4 CreateOrthographic(float width, float height, float zNear, float zFar)
        {
            return CreateOrthographicOffCenter(-width / 2, width / 2, -height / 2, height / 2, zNear, zFar);
        }

        /// <summary>
        /// Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            float invRL = 1 / (right - left);
            float invTB = 1 / (top - bottom);
            float invFN = 1 / (zFar - zNear);

            return new Matrix4(new Vector4(2 * invRL, 0, 0, 0), new Vector4(0, 2 * invTB, 0, 0), new Vector4(0, 0, -2 * invFN, 0),
                new Vector4(-(right + left) * invRL, -(top + bottom) * invTB, -(zFar + zNear) * invFN, 1));
        }

        /// <summary>
        /// Creates a perspective projection matrix.
        /// </summary>
        /// <param name="fovy">Angle of the field of view in the Y direction (in radians)</param>
        /// <param name="aspect">Aspect ratio of the view (width / height)</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        /// <list type="bullet">
        /// <item>fovy is zero, less than zero or larger than Math.PI</item>
        /// <item>aspect is negative or zero</item>
        /// <item>zNear is negative or zero</item>
        /// <item>zFar is negative or zero</item>
        /// <item>zNear is larger than zFar</item>
        /// </list>
        /// </exception>
        public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float zNear, float zFar)
        {
            if (fovy <= 0 || fovy > System.Math.PI)
                throw new ArgumentOutOfRangeException("fovy");
            if (aspect <= 0)
                throw new ArgumentOutOfRangeException("aspect");

            float yMax = zNear * (float)System.Math.Tan(0.5f * fovy);
            float yMin = -yMax;
            float xMin = yMin * aspect;
            float xMax = yMax * aspect;

            return CreatePerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar);
        }

        /// <summary>
        /// Creates a perspective projection matrix.
        /// </summary>
        /// <param name="left">Left edge of the view frustum</param>
        /// <param name="right">Right edge of the view frustum</param>
        /// <param name="bottom">Bottom edge of the view frustum</param>
        /// <param name="top">Top edge of the view frustum</param>
        /// <param name="zNear">Distance to the near clip plane</param>
        /// <param name="zFar">Distance to the far clip plane</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown under the following conditions:
        /// <list type="bullet">
        /// <item>zNear is negative or zero</item>
        /// <item>zFar is negative or zero</item>
        /// <item>zNear is larger than zFar</item>
        /// </list>
        /// </exception>
        public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            if (zNear <= 0)
                throw new ArgumentOutOfRangeException("zNear");
            if (zFar <= 0)
                throw new ArgumentOutOfRangeException("zFar");
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException("zNear");

            float x = (2.0f * zNear) / (right - left);
            float y = (2.0f * zNear) / (top - bottom);
            float a = (right + left) / (right - left);
            float b = (top + bottom) / (top - bottom);
            float c = -(zFar + zNear) / (zFar - zNear);
            float d = -(2.0f * zFar * zNear) / (zFar - zNear);

            return new Matrix4
            (
                x, 0, 0, 0,
                0, y, 0, 0,
                a, b, c, -1,
                0, 0, d, 0
            );
        }

        /// <summary>
        /// Build a world space to camera space matrix.
        /// </summary>
        /// <param name="eye">Eye (camera) position in world space</param>
        /// <param name="target">Target position in world space</param>
        /// <param name="up">Up vector in world space (should not be parallel to the camera direction, that is target - eye)</param>
        /// <returns>A Matrix4 that transforms world space to camera space</returns>
        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 z = (eye - target).Normalize();
            Vector3 x = Vector3.Cross(up, z).Normalize();
            Vector3 y = Vector3.Cross(z, x).Normalize();

            Matrix4 matrix = new Matrix4
            (
                x.X, y.X, z.X, 0,
                x.Y, y.Y, z.Y, 0,
                x.Z, y.Z, z.Z, 0,
                0, 0, 0, 1
            );

            return Matrix4.CreateTranslation(-eye) * matrix;
        }

        /// <summary>
        /// Creates the transpose of the current matrix.
        /// </summary>
        /// <returns>A Matrix4 object that contains the transposed matrix</returns>
        public Matrix4 Transpose()
        {
            return new Matrix4
            (
                M11, M21, M31, M41,
                M12, M22, M32, M42,
                M13, M23, M33, M43,
                M14, M24, M34, M44
            );
        }

        /// <summary>
        /// Creates the inverse matrix using Gauss-Jordan elimination with partial pivoting.
        /// </summary>
        /// <returns>A Matrix4 object that contains the inversed matrix</returns>
        public Matrix4 Inverse()
        {
            Matrix4 original = new Matrix4(this);
            Matrix4 identity = Matrix4.Identity;
            int k;

            // loop over columns from left to right eliminating above and below diagonal
            for (int j = 0; j < 4; j++)
            {
                k = j;    // row with largest pivot cadence
                for (int i = j + 1; i < 4; i++)
                    if (System.Math.Abs(original[i, j]) > System.Math.Abs(original[k, j])) k = i;

                original.SwapRows(k, j);
                identity.SwapRows(k, j);
                //Vector4.Swap(ref original[k], ref original[j]);
                //Vector4.Swap(ref identity[k], ref identity[j]);

                if (original[j, j] == 0.0f)
                    throw new Exception("Matrix4 was a singular matrix and cannot be inverted.");

                identity[j] /= original[j, j];
                original[j] /= original[j, j];

                for (int i = 0; i < 4; i++)
                {
                    if (i != j)
                    {
                        identity[i] -= original[i, j] * identity[j];
                        original[i] -= original[i, j] * original[j];
                    }
                }
            }
            return identity;
        }

        /// <summary>
        /// Swaps two rows in the Matrix4 object.
        /// </summary>
        /// <param name="i">First row to switch</param>
        /// <param name="j">Second row to switch</param>
        public void SwapRows(int i, int j)
        {
            if (i < 0 || i > 3)
                throw new ArgumentOutOfRangeException("i");
            if (j < 0 || j > 3)
                throw new ArgumentOutOfRangeException("j");
            if (i == j)
                return;

            for (int k = 0; k < 4; k++)
            {
                float temp = this[i, k];
                this[i, k] = this[j, k];
                this[j, k] = temp;
            }
        }

        /// <summary>
        /// Swaps two columns in the Matrix4 object.
        /// </summary>
        /// <param name="i">First column to switch</param>
        /// <param name="j">Second column to switch</param>
        public void SwapCols(int i, int j)
        {
            if (i < 0 || i > 3)
                throw new ArgumentOutOfRangeException("i");
            if (j < 0 || j > 3)
                throw new ArgumentOutOfRangeException("j");
            if (i == j)
                return;

            for (int k = 0; k < 4; k++)
            {
                float temp = this[k, i];
                this[k, i] = this[k, j];
                this[k, j] = temp;
            }
        }

        /// <summary>
        /// Returns a floating array that represents the Matrix4.
        /// </summary>
        /// <returns>Floating array that represents that Matrix4</returns>
        public float[] ToFloat()
        {
            return new float[]
            {
                M11, M12, M13, M14,
                M21, M22, M23, M24,
                M31, M32, M33, M34,
                M41, M42, M43, M44
            };
        }

        /// <summary>
        /// Build a rotation matrix from a quaternion
        /// </summary>
        /// <param name="rotation">A quaternion representation of the rotation.</param>
        /// <returns>A rotation matrix</returns>
        public static Matrix4 CreateFromQuaternion(Quaternion rotation)
        {
            Vector4 axisangle = rotation.ToAxisAngle();
            return CreateFromAxisAngle(axisangle.Xyz, rotation.W);
        }

        public void SetMatrix(Vector4 v0, Vector4 v1, Vector4 v2, Vector4 v3)
        {
            this.M11 = v0.X;
            this.M12 = v0.Y;
            this.M13 = v0.Z;
            this.M14 = v0.W;
            this.M21 = v1.X;
            this.M22 = v1.Y;
            this.M23 = v1.Z;
            this.M24 = v1.W;
            this.M31 = v2.X;
            this.M32 = v2.Y;
            this.M33 = v2.Z;
            this.M34 = v2.W;
            this.M41 = v3.X;
            this.M42 = v3.Y;
            this.M43 = v3.Z;
            this.M44 = v3.W;
        }

        #endregion
    }
}
