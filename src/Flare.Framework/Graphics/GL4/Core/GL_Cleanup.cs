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

        }

        public static void AddVAO(VAO vao)
        {
            Queue.Add(new GL_Data() { handle = vao.vaoID, type = GL_Type.VAO });
        }

        public static void AddVBO<T>(VBO<T> vbo) where T : struct
        {
            Queue.Add(new GL_Data() { handle = vbo.vboID, type = GL_Type.VBO });
        }

        // Used to store cleanup type
        private struct GL_Data
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
