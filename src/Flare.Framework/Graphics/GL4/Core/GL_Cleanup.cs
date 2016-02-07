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

namespace Flare.Graphics.GL4
{
    static class GL_Cleanup
    {
        private static List<GL_Data> Queue;

        static GL_Cleanup()
        {
            Queue = new List<GL_Data>();
            // TODO: Add Cleanup to Game.Update when implemented
        }

        public static void Cleanup()
        {
            for (int i = 0; i < Queue.Count; i++)
            {
                var item = Queue[i];
                CleanupItem(ref item);
            }
            Queue.Clear();
        }

        private static void CleanupItem(ref GL_Data data)
        {
            if (data.type == GL_Type.VAO)
                CleanupVAO(data.handle);
            else if (data.type == GL_Type.VBO)
                CleanupVBO(data.handle);
            else if (data.type == GL_Type.Texture)
                CleanupTexture(data.handle);
            else if (data.type == GL_Type.FBO_Depth)
                CleanupFBODepth(data.handle);
            else if (data.type == GL_Type.FBO_Buffer)
                CleanupFBOBuffer(data.handle);
            else
                throw new NotImplementedException("OpenGL data type does not have a cleanup method.");
            
            data = null;
        }

        #region Item cleanup

        private static void CleanupVAO(uint vao)
        {
            GL.DeleteVertexArrays(1, new uint[] { vao });
        }

        public static void CleanupVBO(uint vbo)
        {
            GL.DeleteBuffer(vbo);
        }

        public static void CleanupTexture(uint texture)
        {
            GL.DeleteTextures(1, new uint[] { texture });
        }

        public static void CleanupFBODepth(uint depth)
        {
            GL.DeleteFramebuffers(1, new uint[] { depth });
        }

        public static void CleanupFBOBuffer(uint buffer)
        {
            GL.DeleteFramebuffers(1, new uint[] { buffer });
        }

        #endregion

        #region Item addition

        public static void AddVAO(uint vao)
        {
            Queue.Add(new GL_Data() { handle = vao, type = GL_Type.VAO });
        }

        public static void AddVBO(uint vbo)
        {
            Queue.Add(new GL_Data() { handle = vbo, type = GL_Type.VBO });
        }

        public static void AddTexture(uint texture)
        {
            Queue.Add(new GL_Data() { handle = texture, type = GL_Type.Texture });
        }

        public static void AddTextures(int length, uint[] textures)
        {
            for (int i = 0; i < length; i++)
                AddTexture(textures[i]);
        }

        public static void AddFBO(uint depth, uint buffer)
        {
            Queue.Add(new GL_Data() { handle = depth, type = GL_Type.FBO_Depth });
            Queue.Add(new GL_Data() { handle = buffer, type = GL_Type.FBO_Buffer });
        }

        #endregion

        // Used to store cleanup type
        private class GL_Data
        {
            internal uint handle;
            internal GL_Type type;
        }

        private enum GL_Type
        {
            VAO,
            VBO,
            Texture,
            FBO_Depth,
            FBO_Buffer
        }
    }
}
