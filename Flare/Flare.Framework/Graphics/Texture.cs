using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Flare.Framework.Graphics
{
    public class Texture
    {
        public static readonly Texture WhitePixel = new Texture(1, 1, new Vector4[] { Vector4.One });

        public virtual Vector2 Size { get { return new Vector2(Width, Height); } }
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }
        internal virtual int TexID { get; set; }

        // Protected ctor for subclasses to use if handling OpenGL Texture Handles by themselves.
        protected Texture() { }

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

        public Texture(int width, int height, Vector4[] pixels)
            : this(width, height)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.Float, pixels);
        }

        public Texture(Bitmap bitmap)
            : this(bitmap.Width, bitmap.Height)
        {
            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            bitmap.UnlockBits(bmpData);
        }

        ~Texture()
        {
            GL.DeleteTexture(TexID);
        }

    }
}