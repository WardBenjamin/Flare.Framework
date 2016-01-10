using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Flare.Framework.Graphics
{
    /// <summary>
    /// Represents a simple 2D texture and position to draw.
    /// </summary>
    public class Sprite : IDisposable
    {
        public Transform Transform;
        private Texture texture;

        /// <summary>
        /// The texture to draw.
        /// </summary>
        public Texture Texture
        {
            get { return texture; }
            set { texture = value; GenerateVerts(); }
        }

        public Vector4 Tint { get; set; }

        public int VAO, VBO, UBO; // VAO, Verticies, UVs
        public Vector3[] verticies;
        Vector2[] uvs;

        protected Sprite() { }

        public Sprite(Texture texture)
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            UBO = GL.GenBuffer();

            this.texture = texture;
            this.Transform = new Transform();
            Tint = Vector4.One;

            GenerateVerts();
        }

        public Sprite(Texture texture, Transform transform)
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            UBO = GL.GenBuffer();

            this.texture = texture;
            this.Transform = transform;
            Tint = Vector4.One;

            GenerateVerts();
        }

        public Sprite(Texture texture, Color tint) : this(texture)
        {
            Tint = new Vector4((float)tint.R / 255, (float)tint.G / 255, (float)tint.B / 255, (float)tint.A / 255);
        }

        public Sprite(Texture texture, Transform transform, Vector4 tint) : this(texture, transform)
        {
            Tint = tint;
        }

        private void GenerateVerts()
        {
            // TL, BL, TR, BR for glTriangleStrips
            // Scaling is done in the shader, as part of the model matrix
            Vector3 VTL = Vector3.Zero;
            Vector3 VTR = new Vector3(Texture.Width, 0, 0);
            Vector3 VBL = new Vector3(0, Texture.Height, 0);
            Vector3 VBR = new Vector3(Texture.Size);

            verticies = new Vector3[4] { VTL, VBL, VTR, VBR };

            Vector2 UTL = Vector2.Zero;
            Vector2 UTR = Vector2.UnitX;
            Vector2 UBL = Vector2.UnitY;
            Vector2 UBR = Vector2.One;

            uvs = new Vector2[4] { UTL, UBL, UTR, UBR };

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BlittableValueType.StrideOf(verticies) * verticies.Length), verticies, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, OpenTK.BlittableValueType.StrideOf(verticies), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, UBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BlittableValueType.StrideOf(uvs) * uvs.Length), uvs, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, OpenTK.BlittableValueType.StrideOf(uvs), 0);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)   
                {
                    // TODO: dispose managed state (managed objects).
                    GL.DeleteVertexArray(VAO);
                    GL.DeleteBuffers(1, ref VBO);
                    GL.DeleteBuffers(1, ref UBO);
                }
                else
                {
                    // We are being disposed from the destructor
                    // Figure out a way to clean up the vertex buffers/array.
                    // Oh wait, let's just complain!
                    Console.WriteLine("Warning: An instance of Type: Sprite was not disposed before garbage collection.");
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Sprite()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Cleans up unmanaged OpenGL resources
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
