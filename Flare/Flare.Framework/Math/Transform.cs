using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Flare.Framework
{
    /// <summary>
    /// Represents a transform matrix for a model, sprite, or other object.
    /// </summary>
    public class Transform
    {
        public Transform Parent { get; set; }

        Vector3 position, scale;
        Quaternion rotation;
        Matrix4 matrix;

        public Transform()
        {
            rotation = Quaternion.Identity;
            matrix = Matrix4.Identity;
            position = Vector3.Zero;
            scale = Vector3.One;
        }

        /// <summary>
        /// Rotate the transform in local coordinates, in the x axis.
        /// </summary>
        /// <param name="rads"></param>
        public void RotateLocalX(float rads)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitX, rads);
        }

        /// <summary>
        /// Rotate the transform in world coordinates, in the y axis.
        /// </summary>
        /// <param name="rads"></param>
        public void RotateWorldY(float rads)
        {
            Rotation *= Quaternion.FromAxisAngle(Vector3.UnitY, rads);
        }

        public void TranslateLocal(Vector3 vector)
        {
            position = Matrix.ExtractTranslation();
            Position += Vector3.Transform(vector, Rotation);
        }

        void UpdateMatrix()
        {
            matrix = Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Position);
        }

        /// <summary>
        /// Position component of the transform.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Absolute position. This is the same as the position, translated by the transform matrix.
        /// </summary>
        public Vector3 AbsolutePosition
        {
            get
            {
                return Vector3.TransformPosition(position, Matrix);
            }
        }

        /// <summary>
        /// Rotation component of the transform.
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Scale component of the transform.
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Get the matrix of the transform, containing all elements.
        /// </summary>
        public Matrix4 Matrix
        {
            get
            {
                var ret = matrix;
                if (Parent != null)
                    ret *= Parent.Matrix;
                return ret;
            }
        }
    }

}
