using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using Flare.Framework.Graphics.Fonts;
using System.Drawing;

namespace Flare.Framework.Graphics
{
    public class Text
    {
        public string String { get; set; }

        public Vector2 Position { get; set; }

        public BitmapFont Font { get; set; }

        public Vector4 Color = Vector4.One;

        public int VAO, VBO, UBO; // VAO, Verticies, UVs

        Vector2[] verticies, uvs;

        public Text(BitmapFont font, string str, Vector2 pos)
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            UBO = GL.GenBuffer();

            Font = font;
            String = str;
            Position = pos;

            GenerateInformation();
        }

        public Text(BitmapFont font, string str, Vector2 pos, Color color) : this(font, str, pos)
        {
            this.Color = new Vector4((float)color.R / 255, (float)color.G / 255, (float)color.B / 255, (float)color.A / 255);
        }

        public Text(BitmapFont font, string str, Vector2 pos, Vector4 color) : this(font, str, pos)
        {
            this.Color = color;
        }

        /// <summary>
        /// Regenerates vertex and UV information. (Call this after making changes to font, string, or position.)
        /// Note: This is called in the constructor.
        /// </summary>
        public void GenerateInformation()
        {
            BitmapFontRegion[] regions = new BitmapFontRegion[String.Length];

            verticies = new Vector2[String.Length * 6];
            uvs = new Vector2[String.Length * 6];

            int length = String.Length;
            int vi = 0, ui = 0; // Vertex index, UV index
            int dx = 0, dy = 0;

            for (int i = 0; i < length; i++)
            {
                regions[i] = Font.GetCharacterRegion(String[i]);
                if (regions[i] != null)
                {
                    // Calculate verticies
                    var VTopLeft = new Vector2(dx + regions[i].XOffset, dy + regions[i].YOffset);
                    var VTopRight = VTopLeft + new Vector2(regions[i].Rect.Width, 0);
                    var VBottomLeft = VTopLeft + new Vector2(0, regions[i].Rect.Height);
                    var VBottomRight = VTopLeft + new Vector2(regions[i].Rect.Width, regions[i].Rect.Height);

                    // Add verticies to array (TL,TR,BL,TR,BL,BR)
                    verticies[vi++] = VTopLeft;
                    verticies[vi++] = VTopRight;
                    verticies[vi++] = VBottomLeft;
                    verticies[vi++] = VTopRight;
                    verticies[vi++] = VBottomLeft;
                    verticies[vi++] = VBottomRight;

                    // Calculate and add UVs
                    Vector2 uvCoefficient = new Vector2(1 / Font.TextureSize.X, 1 / Font.TextureSize.Y);
                    var UTopLeft = new Vector2(regions[i].Rect.X * uvCoefficient.X, regions[i].Rect.Y * uvCoefficient.Y);
                    var UTopRight = UTopLeft + new Vector2(regions[i].Rect.Width * uvCoefficient.X, 0);
                    var UBottomLeft = UTopLeft + new Vector2(0, regions[i].Rect.Height * uvCoefficient.Y);
                    var UBottomRight = UTopLeft + new Vector2(regions[i].Rect.Width * uvCoefficient.X, regions[i].Rect.Height * uvCoefficient.Y);

                    // Add UVs to array (TL,TR,BL,TR,BL,BR)
                    uvs[ui++] = UTopLeft;
                    uvs[ui++] = UTopRight;
                    uvs[ui++] = UBottomLeft;
                    uvs[ui++] = UTopRight;
                    uvs[ui++] = UBottomLeft;
                    uvs[ui++] = UBottomRight;

                    dx += regions[i].XAdvance;
                }
                if (String[i] == '\n')
                {
                    dy += Font.LineHeight;
                    dx = 0;
                }
            }

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
