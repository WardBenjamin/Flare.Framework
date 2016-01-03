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
using Flare.Framework.Graphics.Cameras;

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

        private void DrawSprites(OrthographicCamera camera = OrthographicCamera.Default)
        {
            Matrix4 MVP;
            if (camera == null)
                MVP =
            Vector4 lastTint = Vector4.One;
            int lastTexID = -1;

            spriteShader.Use();
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            spriteShader.SetUniform("tint", lastTint);
            foreach (var spr in spritesToDraw)
            {
                GL.BindVertexArray(spr.VAO);
                if (spr.Tint != lastTint)
                {
                    lastTint = spr.Tint;
                    spriteShader.SetUniform("tint", lastTint);
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
            in vec3 vertPos;
            in vec2 vertUV;

            uniform mat4 MVP;

            out vec2 UV;

            void main() {
                gl_Position = MVP * vec4(vertPos, 1)
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