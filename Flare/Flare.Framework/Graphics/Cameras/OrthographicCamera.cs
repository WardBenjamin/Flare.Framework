using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare.Framework.Graphics.Cameras
{
    /// <summary>
    /// Orthographic camera. See Camera base class for more information.
    /// </summary>
    public class OrthographicCamera : Camera
    {
        /// <summary>
        /// View matrix used in drawing. The view matrix converts from world space to view (aka camera) space.
        /// </summary>
        public override Matrix4 ViewMatrix
        {
            get
            {
                //Console.WriteLine("Transform Pos: " + Transform.Position);  
                //Console.WriteLine("Transform AbsolutePos: " + Transform.AbsolutePosition);
                Matrix4 mat /*= Matrix4.CreateTranslation(-Transform.AbsolutePosition)
                    * Matrix4.CreateRotationX(Transform.Rotation.X)
                    * Matrix4.CreateRotationY(Transform.Rotation.Y);*/
                = Transform.Matrix;
                //Console.WriteLine(mat);
                return mat;
                #region LookAt
                // TODO: Replace this with something other than a LookAt call (user choice)
                // Aka, ViewMatrix should be set with a LookAt call?
                // Different in perspective vs ortho?
                
                //Vector3 lookat = new Vector3();
                //lookat.X = (float)(Math.Sin(Orientation.X) * Math.Cos(Orientation.Y));
                //lookat.Y = (float)Math.Sin(Orientation.Y);
                //lookat.Z = (float)(Math.Cos(Orientation.X) * Math.Cos(Orientation.Y));
                //return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
                #endregion
            }
        }

        internal OrthographicCamera(Matrix4 projectionMatrix)
        {
            ProjectionMatrix = projectionMatrix;

        }
    }
}
