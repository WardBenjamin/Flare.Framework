﻿#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Drawing;

namespace Flare.Graphics.GL4
{
    public class FBO : IDisposable
    {
        #region Properties
        /// <summary>
        /// The ID for the entire framebuffer object.
        /// </summary>
        public uint BufferID { get; private set; }

        /// <summary>
        /// The IDs for each of the renderbuffer attachments.
        /// </summary>
        public uint[] TextureID { get; private set; }

        /// <summary>
        /// The ID for the single depth buffer attachment.
        /// </summary>
        public uint DepthID { get; private set; }

        /// <summary>
        /// The size (in pixels) of all renderbuffers associated with this framebuffer.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// The attachments used by this framebuffer.
        /// </summary>
        public FramebufferAttachment[] Attachments { get; private set; }

        /// <summary>
        /// The internal pixel format for each of the renderbuffers (depth buffer not included).
        /// </summary>
        public PixelInternalFormat Format { get; private set; }

        private bool mipmaps;
        #endregion

        #region Constructor and Destructor
        public FBO(int width, int height, FramebufferAttachment attachment = FramebufferAttachment.ColorAttachment0, PixelInternalFormat format = PixelInternalFormat.Rgba8, bool mipmaps = true)
            : this(new Size(width, height), new FramebufferAttachment[] { attachment }, format, mipmaps)
        {
        }

        /// <summary>
        /// Creates a framebuffer object and its associated resources (depth and pbuffers).
        /// </summary>
        /// <param name="Size">Specifies the size (in pixels) of the framebuffer and it's associated buffers.</param>
        /// <param name="Attachments">Specifies the attachment to use for the pbuffer.</param>
        /// <param name="Format">Specifies the internal pixel format for the pbuffer.</param>
        public FBO(Size Size, FramebufferAttachment Attachment = FramebufferAttachment.ColorAttachment0, PixelInternalFormat Format = PixelInternalFormat.Rgba8, bool Mipmaps = true)
            : this(Size, new FramebufferAttachment[] { Attachment }, Format, Mipmaps)
        {
        }

        /// <summary>
        /// Creates a framebuffer object and its associated resources (depth and pbuffers).
        /// </summary>
        /// <param name="Size">Specifies the size (in pixels) of the framebuffer and it's associated buffers.</param>
        /// <param name="Attachments">Specifies the attachments to use for the frame buffer.</param>
        /// <param name="Format">Specifies the internal pixel format for the frame buffer.</param>
        /// <param name="Mipmaps">Specified whether to build mipmaps after the frame buffer is unbound.</param>
        public FBO(Size Size, FramebufferAttachment[] Attachments, PixelInternalFormat Format, bool Mipmaps, TextureParameter filterType = TextureParameter.Linear)
        {
            this.Size = Size;
            this.Attachments = Attachments;
            this.Format = Format;
            this.mipmaps = Mipmaps;

            // First create the framebuffer
            BufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, BufferID);

            if (Attachments.Length == 1 && Attachments[0] == FramebufferAttachment.DepthAttachment)
            {
                // if this is a depth attachment only
                TextureID = new uint[] { GL.GenTexture() };
                GL.BindTexture(TextureTarget.Texture2D, TextureID[0]);

                GL.TexImage2D(TextureTarget.Texture2D, 0, Format, Size.Width, Size.Height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Nearest);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureParameter.Nearest);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureParameter.ClampToEdge);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureParameter.ClampToEdge);

                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureID[0], 0);
                GL.DrawBuffer(DrawBufferMode.None);
                GL.ReadBuffer(ReadBufferMode.None);
            }
            else
            {
                // Create n texture buffers (known by the number of attachments)
                TextureID = new uint[Attachments.Length];
                GL.GenTextures(Attachments.Length, TextureID);

                // Bind the n texture buffers to the framebuffer
                for (int i = 0; i < Attachments.Length; i++)
                {
                    GL.BindTexture(TextureTarget.Texture2D, TextureID[i]);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, Format, Size.Width, Size.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
                    if (Mipmaps)
                    {
                        GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Linear);
                        GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureParameter.LinearMipMapLinear);
                        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                    }
                    else
                    {
                        GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, filterType);
                        GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, filterType);
                    }
                    GL.FramebufferTexture(FramebufferTarget.Framebuffer, Attachments[i], TextureID[i], 0);
                }

                // Create and attach a 24-bit depth buffer to the framebuffer
                DepthID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, DepthID);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, Size.Width, Size.Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, TextureParameter.Nearest);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, TextureParameter.Nearest);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, TextureParameter.ClampToEdge);
                GL.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, TextureParameter.ClampToEdge);

                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, DepthID, 0);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, DepthID, 0);
            }

            // Build the framebuffer and check for errors
            FramebufferErrorCode status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Frame buffer did not compile correctly.  Returned {0}, glError: {1}", status.ToString(), GL.GetError().ToString());
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Check to ensure that the FBO was disposed of properly.
        /// </summary>
        ~FBO()
        {
            if (DepthID != 0 || BufferID != 0 || TextureID != null)
            {
                System.Diagnostics.Debug.Fail("FBO was not disposed of properly.");
            }
        }
        #endregion

        #region Enable and Disable
        /// <summary>
        /// Binds the framebuffer and all of the renderbuffers.
        /// Clears the buffer bits and sets viewport size.
        /// Perform all rendering after this call.
        /// </summary>
        /// <param name="clear">True to clear both the color and depth buffer bits of the FBO before enabling.</param>
        public void Enable(bool clear = true)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, BufferID);
            if (Attachments.Length == 1)
            {
                GL.BindTexture(TextureTarget.Texture2D, TextureID[0]);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, Attachments[0], TextureID[0], 0);
            }
            else
            {
                DrawBuffersEnum[] buffers = new DrawBuffersEnum[Attachments.Length];

                for (int i = 0; i < Attachments.Length; i++)
                {
                    GL.BindTexture(TextureTarget.Texture2D, TextureID[i]);
                    GL.FramebufferTexture(FramebufferTarget.Framebuffer, Attachments[i], TextureID[i], 0);
                    buffers[i] = (DrawBuffersEnum)Attachments[i];
                }

                GL.BindTexture(TextureTarget.Texture2D, DepthID);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, DepthID, 0);

                if (Attachments.Length > 1) GL.DrawBuffers(Attachments.Length, buffers);
            }

            GL.Viewport(0, 0, Size.Width, Size.Height);

            // configurably clear the buffer and color bits
            if (clear)
            {
                if (Attachments.Length == 1 && Attachments[0] == FramebufferAttachment.DepthAttachment)
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                else
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
        }

        /// <summary>
        /// Unbinds the framebuffer and then generates the mipmaps of each renderbuffer.
        /// </summary>
        public void Disable()
        {
            // unbind this framebuffer (does not guarantee the correct framebuffer is bound)
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // have to generate mipmaps here
            for (int i = 0; i < Attachments.Length && mipmaps; i++)
            {
                GL.BindTexture(TextureTarget.Texture2D, TextureID[i]);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (DepthID != 0 || BufferID != 0 || TextureID != null)
            {
                GL_Cleanup.AddTextures(TextureID.Length, TextureID);
                GL_Cleanup.AddFBO(DepthID, BufferID);

                BufferID = 0;
                DepthID = 0;
                TextureID = null;
            }
        }
        #endregion

        #region Sample Shader
        public static string vertexShaderSource = @"
#version 330

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;
uniform float animation_factor;

in vec3 in_position;
in vec3 in_normal;
in vec2 in_uv;

out vec2 uv;

void main(void)
{
  vec4 pos2 = projection_matrix * modelview_matrix * vec4(in_normal, 1);
  vec4 pos1 = projection_matrix * modelview_matrix * vec4(in_position, 1);

  uv = in_uv;
  
  gl_Position = mix(pos2, pos1, animation_factor);
}";

        public static string fragmentShaderSource = @"
#version 330

uniform sampler2D active_texture;

in vec2 uv;

out vec4 out_frag_color;

void main(void)
{
  out_frag_color = mix(texture2D(active_texture, uv), vec4(1, 1, 1, 1), 0.05);
}";
        #endregion
    }
}
