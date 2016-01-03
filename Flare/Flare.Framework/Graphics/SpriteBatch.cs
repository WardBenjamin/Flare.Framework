using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Flare.Framework.Shaders;
using System.Runtime.InteropServices;

namespace Flare.Framework.Graphics
{
    public class SpriteBatch
    {
        List<Sprite> spritesToDraw = new List<Sprite>();
        static Shader spriteShader;

        static SpriteBatch()
        {
            if (spriteShader == null)
            {
                spriteShader = new Shader(vshader, fshader);
                spriteShader.Compile();
            }
        }

        public void Draw()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            DrawSprites();

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

        public void Add(Sprite sprite)
        {
            spritesToDraw.Add(sprite);
        }

        private void DrawSprites()
        {
            Vector2 lastTarSize = Vector2.Zero, lastPos = -Vector2.One;
            Vector4 lastTint = Vector4.One;
            int lastTexID = -1;

            spriteShader.Use();
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            spriteShader.SetUniform("targetSize", lastTarSize);
            spriteShader.SetUniform("tint", lastTint);
            foreach (var spr in spritesToDraw)
            {
                GL.BindVertexArray(spr.VAO);
                if (spr.Tint != lastTint)
                {
                    lastTint = spr.Tint;
                    spriteShader.SetUniform("tint", lastTint);
                }
                if (spr.Position != lastPos)
                {
                    lastPos = spr.Position;
                    spriteShader.SetUniform("pos", lastPos);
                }
                Vector2 clientSize;
                if ((clientSize = new Vector2(Game.ClientSize.Width, Game.ClientSize.Height)) != lastTarSize)
                {
                    Console.WriteLine(Game.ClientSize);
                    lastTarSize = clientSize;
                    spriteShader.SetUniform("targetSize", lastTarSize);
                }
                if (spr.Texture.TexID != lastTexID)
                {
                    lastTexID = spr.Texture.TexID;
                    GL.BindTexture(TextureTarget.Texture2D, lastTexID);
                }

                GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            }

            GL.BindVertexArray(0);
            spritesToDraw.Clear();
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SpriteElement
        {
            public Vector2 Vertex;
            public Vector2 TexCoord;
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
            uniform vec4 tint;

            out vec4 fragColor;

            void main() {
                fragColor = texture(tex, UV) * tint;
            }";
    }
}