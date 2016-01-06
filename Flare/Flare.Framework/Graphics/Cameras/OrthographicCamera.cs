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
        /// Enumeration of the different camera view modes. 
        /// See documentation on individual modes for more information.
        /// </summary>
        public enum ViewMode
        {
            /// <summary>
            /// Default view mode. Equivalent to ViewMode.TopLeft.
            /// </summary>
            Default = 0,
            /// <summary>
            /// 0, 0 is defined as the top-left of the screen when the position is at 0, 0.
            /// </summary>
            TopLeft = 0,
            /// <summary>
            /// 0, 0 is defined as the center of the projection when at position 0, 0.
            /// </summary>
            Center = 1
        }

        /// <summary>
        /// The view mode of the camera. 
        /// See documentation on individual view modes for more information.
        /// </summary>
        public ViewMode Mode
        {
            get { return viewMode; }
            set
            {
                if ((int)value == (int)ViewMode.Default)
                {
                    if (Screen_Resize_Hook_Used)
                    {
                        Game.Resize -= Screen_Resize_Hook;
                        Screen_Resize_Hook_Used = false;
                    }
                }
                else
                {
                    Game.Resize += Screen_Resize_Hook;
                    Screen_Resize_Hook_Used = true;
                }
                viewMode = value;
            }
        }

        private void Screen_Resize_Hook(object sender, EventArgs e)
        {
            ScreenSize = Game.ClientSize;
        }

        private ViewMode viewMode = ViewMode.Default;
        private bool Screen_Resize_Hook_Used = false;
        private Size ScreenSize = Size.Empty;

        /// <summary>
        /// View matrix used in drawing. The view matrix converts from world space to view (aka camera) space.
        /// </summary>
        public override Matrix4 ViewMatrix
        {
            get
            {
                if (Mode == ViewMode.TopLeft)
                {
                    Console.WriteLine("Screen Size" + ScreenSize);
                    Console.WriteLine("Top Left");
                    Console.WriteLine(Matrix4.CreateTranslation(new Vector3(ScreenSize.Width, ScreenSize.Height, 0))
                        * Matrix4.CreateTranslation(Position)
                        * Matrix4.CreateRotationX(Orientation.X)
                        * Matrix4.CreateRotationY(Orientation.Y));
                    Console.WriteLine("Center");
                    Console.WriteLine(Matrix4.CreateTranslation(Position)
                        * Matrix4.CreateRotationX(Orientation.X)
                        * Matrix4.CreateRotationY(Orientation.Y));
                    return Matrix4.CreateTranslation(new Vector3(ScreenSize.Width, ScreenSize.Height, 0))
                        * Matrix4.CreateTranslation(Position)
                        * Matrix4.CreateRotationX(Orientation.X)
                        * Matrix4.CreateRotationY(Orientation.Y);
                }
                else if (Mode == ViewMode.Center)
                {
                    return Matrix4.CreateTranslation(Position)
                        * Matrix4.CreateRotationX(Orientation.X)
                        * Matrix4.CreateRotationY(Orientation.Y);
                }
                else
                {
                    // TODO: Should this be different from ViewMode.Center?
                    return Matrix4.CreateTranslation(Position)
                        * Matrix4.CreateRotationX(Orientation.X)
                        * Matrix4.CreateRotationY(Orientation.Y);
                }
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

        internal OrthographicCamera(Matrix4 projectionMatrix, ViewMode mode)
        {
            ProjectionMatrix = projectionMatrix;
            Mode = mode;
        }
    }
}
