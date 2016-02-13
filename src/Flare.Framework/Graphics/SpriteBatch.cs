#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flare.Graphics.GL4;

namespace Flare.Graphics
{
    class SpriteBatch
    {
        private List<Sprite> _sprites;
        private ShaderProgram shader;

        #region Constructors

        public SpriteBatch()
        {
            _sprites = new List<Sprite>();
            CompileShaders();
        }

        public SpriteBatch(string vertexShader, string fragmentShader)
        {
            vertex_shader = vertexShader;
            fragment_shader = fragmentShader;
            CompileShaders();
        }

        #endregion

        #region Private methods

        private void CompileShaders()
        {
            shader = new ShaderProgram(new Shader(vertex_shader, ShaderType.VertexShader),
                new Shader(fragment_shader, ShaderType.FragmentShader));
        }

        #endregion

        #region Shader sources

        protected readonly string vertex_shader = @"
            #version 130
            in vec3 vertPos;
            in vec2 vertUV;
            out vec2 UV;

            uniform mat4 MVP;

            void main() {
                gl_Position = MVP * vec4(vertPos, 1);
                UV = vertUV;
            }";

        protected readonly string fragment_shader = @"
            #version 130
            in vec2 UV;

            uniform sampler2D tex;
            uniform vec4 tint;

            out vec4 fragColor;

            void main() {
                fragColor = texture(tex, UV) * tint;
            }";

        #endregion
    }
}

/*using System;
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
    /// <summary>
    /// Class used to draw and batch sprites for faster GPU rendering.
    /// </summary>
    public class SpriteBatch
    {
        #region Default Camera

        internal static OrthographicCamera DefaultCamera { get; private set; }

        private static void Default_Camera_Resize_Hook(object sender, EventArgs e)
        {
            DefaultCamera = Camera.CreateOrthographicTopLeftOrigin(Game.ClientSize.Width, Game.ClientSize.Height, -1, 1);
        }

        #endregion

        List<Sprite> spritesToDraw = new List<Sprite>();
        static Shader spriteShader;

        static SpriteBatch()
        {
            if (spriteShader == null)
            {
                spriteShader = new Shader(vshader, fshader);
                spriteShader.Compile();
            }
            // Create the default camera for the first time
            Default_Camera_Resize_Hook(null, EventArgs.Empty);
            Game.Resize += Default_Camera_Resize_Hook;
        }

        /// <summary>
        /// Draw all batched sprites.
        /// </summary>
        /// <param name="camera">Optional camera.</param>
        public void Draw(OrthographicCamera camera = null)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            DrawSprites(camera);

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

        /// <summary>
        /// Add a sprite to the current batch.
        /// </summary>
        /// <param name="sprite">Sprite to be added.</param>
        public void Add(Sprite sprite)
        {
            spritesToDraw.Add(sprite);
        }

        private void DrawSprites(OrthographicCamera camera = null)
        {
            Matrix4 MVP, lastMVP = Matrix4.Identity;
            camera = camera ?? DefaultCamera;

            Vector4 lastTint = Vector4.One;
            int lastTexID = -1;

            spriteShader.Use();
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            spriteShader.SetUniform("tint", lastTint);

            foreach (var spr in spritesToDraw)
            {
                GL.BindVertexArray(spr.VAO);
                MVP = spr.Transform.Matrix * camera.ViewMatrix * camera.ProjectionMatrix;

                if (MVP != lastMVP)
                {
                    lastMVP = MVP;
                    spriteShader.SetUniform("MVP", lastMVP);
                }
                if (spr.Tint != lastTint)
                {
                    lastTint = spr.Tint;
                    spriteShader.SetUniform("tint", lastTint);
                }
                if (spr.Texture.TexID != lastTexID)
                {
                    lastTexID = spr.Texture.TexID;
                    GL.BindTexture(TextureTarget.Texture2D, lastTexID);
                    GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new int[] { (int)TextureMinFilter.Linear });
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


    }
}*/