using OpenTK;
using System;
using System.Drawing;

namespace Flare.Framework.Graphics.Cameras
{
    /// <summary>
    /// Base Camera class providing basic functionality. See OrthoCamera and PerspectiveCamera for implementations.
    /// </summary>
    public abstract class Camera
    {
        /// <summary>
        /// Projection matrix used in drawing. The projection matrix converts from view (aka camera) space to screen space.
        /// </summary>
        public Matrix4 ProjectionMatrix { get; protected set; }

        /// <summary>
        /// View matrix used in drawing. The view matrix converts from world space to view (aka camera) space.
        /// </summary>
        public virtual Matrix4 ViewMatrix
        {
            get
            {
                return Matrix4.CreateTranslation(Position) 
                    * Matrix4.CreateRotationX(Orientation.X) 
                    * Matrix4.CreateRotationY(Orientation.Y);

                #region LookAt
                // TODO: Replace this with something other than a LookAt call (user choice)
                // Aka, ViewMatrix should be set with a LookAt call?
                // Different in perspective vs ortho?
                /*
                Vector3 lookat = new Vector3();
                lookat.X = (float)(Math.Sin(Orientation.X) * Math.Cos(Orientation.Y));
                lookat.Y = (float)Math.Sin(Orientation.Y);
                lookat.Z = (float)(Math.Cos(Orientation.X) * Math.Cos(Orientation.Y));
                return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);*/
                #endregion
            }
        }

        /// <summary>
        /// Camera position in world space. This represents the absolute OpenGL position where the origin is zero.
        /// See http://www.cocos2d-x.org/attachments/download/1563 for axis details.
        /// </summary>
        public Vector3 Position = Vector3.Zero;

        /// <summary>
        /// Rotation in world space. Requires radians.
        /// </summary>
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);

        /// <summary>
        /// Moves the camera in world space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            // Compute forward and right vectors for use in calculating offset.
            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;
            offset.NormalizeFast();

            Position += offset;
        }

        /// <summary>
        /// Adds rotation in world space.
        /// </summary>
        /// <param name="x">X Rotation</param>
        /// <param name="y">Y Rotation</param>
        public void AddRotation(float x, float y)
        {
            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }

        /// <summary>
        /// Check if the camera can "see" the input bounds
        /// </summary>
        /// <param name="rect">Bounds of object to check</param>
        /// <returns></returns>
        public virtual bool Contains(Rectangle rect)
        {
            // TODO: Write the Contains method.
            // Or make abstract?

            return false;
        }

        // Documentation for ortho and perspective fov matricies: 
        // https://unspecified.wordpress.com/2012/06/21/calculating-the-gluperspective-matrix-and-other-opengl-matrix-maths/

        /// <summary>
        /// Set projection matrix to be orthonographic, representing the field of view,
        /// aspect ratio, and clipping distance params.
        /// </summary>
        /// <param name="fovy">Field of view in the Y axis</param>
        /// <param name="aspect">Aspect ratio (width/height)</param>
        /// <param name="zNear">Near clipping distance</param>
        /// <param name="zFar">Far clipping distance</param>
        public static PerspectiveCamera CreatePerspectiveFov(float fovy, float aspect, float zNear, float zFar)
        {
            Matrix4 ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar);
            return new PerspectiveCamera(ProjectionMatrix);
        }

        /// <summary>
        /// Set projection matrix to be orthonographic, representing the field of view,
        /// aspect ratio, and clipping distance params. Calculates aspect ratio from width and height.
        /// </summary>
        /// <param name="fovy">Field of view in the Y axis</param>
        /// <param name="width">Width of viewport</param>
        /// <param name="height">Height of viewport</param>
        /// <param name="zNear">Near clipping distance</param>
        /// <param name="zFar">Far clipping distance</param>
        public static PerspectiveCamera CreatePerspectiveFov(float fovy, float width, float height, float zNear, float zFar)
        {
            return CreatePerspectiveFov(fovy, width / height, zNear, zFar);
        }

        /// <summary>
        /// Create a camera with an orthographic projection matrix. Note that this by default uses OrthographicCamera.ViewMode.Default.
        /// See OrthographicCamera.ViewMode for mode options and descriptions.
        /// </summary>
        /// <param name="width">Width of viewport</param>
        /// <param name="height">Height of viewport</param>
        /// <param name="zNear">Near clipping distance</param>
        /// <param name="zFar">Far clipping distance</param>
        /// <param name="mode">Camera view mode</param>
        /// <returns></returns>
        public static OrthographicCamera CreateOrthographic(float width, float height, float zNear, float zFar, OrthographicCamera.ViewMode mode = OrthographicCamera.ViewMode.Default)
        {
            return new OrthographicCamera(Matrix4.CreateOrthographic(width, height, zNear, zFar), mode);   
        }
    }
}
