using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Flare.Framework.Graphics
{
    public class Sprite
    {
        private Vector2 position, scale;
        private Texture texture;

        public Texture Texture
        {
            get { return texture; }
            set { texture = value; GenerateInformation(); }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; GenerateInformation(); }
        }
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; GenerateInformation(); }
        }
        public Vector4 Tint { get; set; }

        public int VAO, VBO, UBO; // VAO, Verticies, UVs
        Vector2[] verticies, uvs;

        protected Sprite() { }

        public Sprite(Texture texture, Vector2 position)
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            UBO = GL.GenBuffer();

            this.texture = texture;
            this.position = position;
            this.scale = Vector2.One;
            Tint = Vector4.One;

            GenerateInformation();
        }

        public Sprite(Texture texture, Vector2 position, Vector2 scale, Vector4 tint)
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            UBO = GL.GenBuffer();

            this.texture = texture;
            this.position = position;
            this.scale = scale;
            Tint = tint;

            GenerateInformation();
        }

        public void GenerateInformation()
        {
            // TL, BL, TR, BR for glTriangleStrips
            Vector2 VTL = Vector2.Zero;
            Vector2 VTR = new Vector2(Texture.Width * scale.X, 0);
            Vector2 VBL = new Vector2(0, Texture.Height * scale.Y);
            Vector2 VBR = Texture.Size * scale;

            verticies = new Vector2[4] { VTL, VBL, VTR, VBR };

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
    }
}
