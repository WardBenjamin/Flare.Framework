using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Flare.Framework
{
    internal class GLCleanupQueue
    {
        private static GLCleanupQueue Queue = new GLCleanupQueue();

        internal static void Add(GLCleanupItem Item)
        {
            Queue.Items.Add(Item);
        }

        internal List<GLCleanupItem> Items;

        private GLCleanupQueue()
        {
            Items = new List<GLCleanupItem>();
            Game.UpdateFrame += CleanupItems;
        }

        private void CleanupItems(object sender, OpenTK.FrameEventArgs e)
        {
            foreach (var item in Items)
            {
                item.Cleanup();
            }
        }
    }

    internal abstract class GLCleanupItem
    {
        internal abstract void Cleanup();
    }
    internal class GLCleanupVAO : GLCleanupItem
    {
        public int VAO;
        public GLCleanupVAO(int VAO)
        {
            this.VAO = VAO;
        }
        internal override void Cleanup()
        {
            GL.DeleteVertexArray(VAO);
        }
    }
    internal class GLCleanupVBO : GLCleanupItem
    {
        public int VBO;
        public GLCleanupVBO(int VBO)
        {
            this.VBO = VBO;
        }
        internal override void Cleanup()
        {
            GL.DeleteBuffer(VBO);
        }
    }
    internal class GLCleanupTexture : GLCleanupItem
    {
        public int TextureHandle;
        public GLCleanupTexture(int TextureHandle)
        {
            this.TextureHandle = TextureHandle;
        }
        internal override void Cleanup()
        {
            GL.DeleteTexture(TextureHandle);
        }
    }
}
