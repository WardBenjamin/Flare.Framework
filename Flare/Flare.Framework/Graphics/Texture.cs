using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Flare.Framework.Graphics
{
    public class Texture : IDisposable
    {
        #region Static Members

        public static readonly Texture WhitePixel = new Texture(1, 1, new Vector4[] { Vector4.One });

        static Texture()
        {
            Game.Unload += CleanupStaticTextures;
        }

        private static void CleanupStaticTextures(object sender, EventArgs e)
        {
            WhitePixel.Dispose();
        }

        #endregion

        /// <summary>
        /// Represents the width and height of the texture, in pixels.
        /// </summary>
        public virtual Vector2 Size { get { return new Vector2(Width, Height); } }
        /// <summary>
        /// The width of the texture, in pixels.
        /// </summary>
        public virtual int Width { get; protected set; }
        /// <summary>
        /// The height of the texture, in pixels.
        /// </summary>
        public virtual int Height { get; protected set; }
        /// <summary>
        /// The OpenGL texture handle.
        /// </summary>
        internal virtual int TexID { get; set; }

        // Protected ctor for subclasses to use if handling OpenGL Texture Handles by themselves.
        protected Texture()
        {
        }

        /// <summary>
        /// Buffer memory and generate a new OpenGL texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Texture(int width, int height)
        {
            Width = width;
            Height = height;

            TexID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TexID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);

            GC.AddMemoryPressure(width * height * 4);
        }

        /// <summary>
        /// Generates an OpenGL texture handle and uploads pixels to that texture.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixels"></param>
        public Texture(int width, int height, Vector4[] pixels)
            : this(width, height)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Float, pixels);
        }

        /// <summary>
        /// Generates an OpenGL texture handle and uploads the pixels of the input bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        public Texture(Bitmap bitmap)
            : this(bitmap.Width, bitmap.Height)
        {
            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            bitmap.UnlockBits(bmpData);
        }

        #region IDisposable Support

        private bool disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing) {/* User is calling dispose */}
                else {/* We are being disposed from the destructor */}

                GLCleanupQueue.Add(new GLCleanupTexture(TexID));
            }
        }

        ~Texture()
        {
            Dispose(false);
        }

        /// <summary>
        /// Cleans up unmanaged OpenGL resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}