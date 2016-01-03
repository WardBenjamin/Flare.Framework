using Flare.Framework.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Flare.Framework.Graphics
{
    public static class TextExtensions
    {
        static Shader textShader;
        static List<Text> textToDraw = new List<Text>();

        static TextExtensions()
        {
            if (textShader == null)
            {
                textShader = new Shader(vshader, fshader);
                textShader.Compile();
            }
        }

        public static void AddString(this SpriteBatch spriteBatch, Text text)
        {
            textToDraw.Add(text);
        }

        public static void DrawStrings(this SpriteBatch spriteBatch)
        {
            Vector2 lastTarSize = Vector2.Zero, lastPos = -Vector2.One;
            Vector4 lastColor = Vector4.One;
            int lastTexID = -1;

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            textShader.Use();
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            textShader.SetUniform("targetSize", lastTarSize);
            textShader.SetUniform("color", lastColor);
            foreach (var text in textToDraw)
            {
                GL.BindVertexArray(text.VAO);
                if (text.Color != lastColor)
                {
                    lastColor = text.Color;
                    textShader.SetUniform("color", lastColor);
                }
                if (text.Position != lastPos)
                {
                    lastPos = text.Position;
                    textShader.SetUniform("pos", lastPos);
                }
                Vector2 clientSize;
                if ((clientSize = new Vector2(Game.ClientSize.Width, Game.ClientSize.Height)) != lastTarSize)
                {
                    Console.WriteLine(Game.ClientSize);
                    lastTarSize = clientSize;
                    textShader.SetUniform("targetSize", lastTarSize);
                }
                if (text.Font.Texture.TexID != lastTexID)
                {
                    lastTexID = text.Font.Texture.TexID;
                    GL.BindTexture(TextureTarget.Texture2D, lastTexID);
                }
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6 * text.String.Length); // Six verticies for each character
            }
            GL.BindVertexArray(0);
            textToDraw.Clear();

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

        // Takes UV, relative vertex, and pos in screen space, converts vert pos to homogenous.
        private static string vshader = @"#version 130
            in vec2 vertPosS;
            in vec2 vertUV;

            out vec2 UV;

            uniform vec2 pos, targetSize;

            void main() {
                vec2 halfTargetSize = vec2(targetSize.x / 2, targetSize.y / 2);
                vec2 vertPosH = (pos + vertPosS) - halfTargetSize;
                vertPosH /= halfTargetSize;
                gl_Position = vec4(vertPosH.x, -vertPosH.y, 0.0, 1.0);
                UV = vertUV;
            }";

        private static string fshader = @"#version 130
            in vec2 UV;

            uniform sampler2D tex;
            uniform vec4 color;

            out vec4 fragColor;

            void main() {
                fragColor = texture(tex, UV) * color;
            }";
    }
}