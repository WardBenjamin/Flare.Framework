#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;

namespace Flare
{
    /// <summary>
    /// A viewing frustum, brought in from Orchard Sun.
    /// </summary>
    public class Frustum
    {
        #region Variables
        private Plane[] planes;

        /// <summary>
        /// Get the planes that make up the Frustum.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Plane this[int a]
        {
            get { return (a >= 0 && a < 6) ? planes[a] : null; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Builds a new Frustum and initializes six new planes
        /// </summary>
        public Frustum()
        {
            planes = new Plane[6];
            for (int i = 0; i < 6; i++) planes[i] = new Plane(0, 0, 0, 0);
        }

        /// <summary>
        /// Builds the Planes so that they make up the left, right, up, down, front and back of the Frustum.
        /// </summary>
        /// <param name="clipMatrix">The combined projection and view matrix (usually from the camera).</param>
        public void UpdateFrustum(Matrix4 clipMatrix)
        {
            planes[0].Set(clipMatrix.M44 - clipMatrix.M41, new Vector3(clipMatrix.M14 - clipMatrix.M11, clipMatrix.M24 - clipMatrix.M21, clipMatrix.M34 - clipMatrix.M31));
            planes[1].Set(clipMatrix.M44 + clipMatrix.M41, new Vector3(clipMatrix.M14 + clipMatrix.M11, clipMatrix.M24 + clipMatrix.M21, clipMatrix.M34 + clipMatrix.M31));
            planes[2].Set(clipMatrix.M44 + clipMatrix.M42, new Vector3(clipMatrix.M14 + clipMatrix.M12, clipMatrix.M24 + clipMatrix.M22, clipMatrix.M34 + clipMatrix.M32));
            planes[3].Set(clipMatrix.M44 - clipMatrix.M42, new Vector3(clipMatrix.M14 - clipMatrix.M12, clipMatrix.M24 - clipMatrix.M22, clipMatrix.M34 - clipMatrix.M32));
            planes[4].Set(clipMatrix.M44 - clipMatrix.M43, new Vector3(clipMatrix.M14 - clipMatrix.M13, clipMatrix.M24 - clipMatrix.M23, clipMatrix.M34 - clipMatrix.M33));
            planes[5].Set(clipMatrix.M44 + clipMatrix.M43, new Vector3(clipMatrix.M14 + clipMatrix.M13, clipMatrix.M24 + clipMatrix.M23, clipMatrix.M34 + clipMatrix.M33));

            for (int i = 0; i < 6; i++)
            {
                float t_mag = planes[i].Normal.Length;
                planes[i].Scalar /= t_mag;
                planes[i].Normal /= t_mag;
            }
        }

        /// <summary>
        /// Builds the Planes so that they make up the left, right, up, down, front and back of the Frustum.
        /// </summary>
        public void UpdateFrustum(Matrix4 projectionMatrix, Matrix4 modelviewMatrix)
        {
            UpdateFrustum(modelviewMatrix * projectionMatrix);
        }

        /// <summary>
        /// True if the AxisAlignedBoundingBox is in (or partially in) the Frustum
        /// </summary>
        /// <param name="b">AxixAlignedBoundingBox to check</param>
        /// <returns>True if an intersection exists</returns>
        public bool Intersects(AxisAlignedBoundingBox box)
        {
            for (int i = 0; i < 6; i++)
            {
                Plane p = planes[i];

                float d = box.Center.Dot(p.Normal);
                float r = box.Size.X * System.Math.Abs(p.Normal.X) + box.Size.Y * System.Math.Abs(p.Normal.Y) + box.Size.Z * System.Math.Abs(p.Normal.Z);
                float dpr = d + r;
                float dmr = d - r;

                if (dpr < -p.Scalar) return false;
            }
            return true;
        }
        #endregion
    }
}
