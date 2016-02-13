#region License
/* Flare - A framework by developers, for developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Runtime.InteropServices;

namespace Flare.Graphics.GL4
{
    partial class GL
    {
        #region Preallocated Memory
        // pre-allocate the float[] for matrix data
        private static float[] matrixFloat = new float[16];
        private static uint[] int1 = new uint[1];
        private static bool[] bool1 = new bool[1];
        #endregion

        #region Private Fields
        private static int _version = 0;
        private static uint currentProgram = 0;
        #endregion

        #region Public Properties
        public static uint CurrentProgram 
        { 
            get { return currentProgram; } 
        }
        #endregion

        /// <summary>
        /// Returns the value or values of a selected parameter.
        /// </summary>
        /// <param name="pname">Supports Blend, CullFace, DepthTest, DepthWriteMask, </param>
        /// <returns></returns>
        public static bool GetBooleanv(GetPName pname)
        {
            GetBooleanv(pname, bool1);
            return bool1[0];
        }

        public static void TexParameteri(GL4.TextureTarget target, GL4.TextureParameterName pname, TextureParameter param)
        {
            Delegates.glTexParameteri(target, pname, (int)param);
        }

        /// <summary>
        /// Shortcut for quickly generating a single buffer id without creating an array to
        /// pass to the gl function.  Calls Gl.GenBuffers(1, id).
        /// </summary>
        /// <returns>The ID of the generated buffer.  0 on failure.</returns>
        public static uint GenBuffer()
        {
            int1[0] = 0;
            GL.GenBuffers(1, int1);
            return int1[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single texture id without creating an array to
        /// pass to the gl function.  Calls Gl.GenTexture(1, id).
        /// </summary>
        /// <returns>The ID of the generated texture.  0 on failure.</returns>
        public static uint GenTexture()
        {
            int1[0] = 0;
            GL.GenTextures(1, int1);
            return int1[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single vertex array id without creating an array to
        /// pass to the gl function.  Calls Gl.GenVertexArrays(1, id).
        /// </summary>
        /// <returns>The ID of the generated vertex array.  0 on failure.</returns>
        public static uint GenVertexArray()
        {
            int1[0] = 0;
            GL.GenVertexArrays(1, int1);
            return int1[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single framebuffer object without creating an array
        /// to pass to the gl function.  Calls Gl.GenFramebuffers(1, id).
        /// </summary>
        /// <returns>The ID of the generated framebuffer.  0 on failure.</returns>
        public static uint GenFramebuffer()
        {
            uint[] id = new uint[1];
            GL.GenFramebuffers(1, id);
            return id[0];
        }

        /// <summary>
        /// Shortcut for quickly generating a single renderbuffer object without creating an array
        /// to pass to the gl function.  Calls Gl.GenRenderbuffers(1, id).
        /// </summary>
        /// <returns>The ID of the generated framebuffer.  0 on failure.</returns>
        public static uint GenRenderbuffer()
        {
            int1[0] = 0;
            GL.GenRenderbuffers(1, int1);
            return int1[0];
        }

        /// <summary>
        /// Gets the program info from a shader program.
        /// </summary>
        /// <param name="program">The ID of the shader program.</param>
        public static string GetProgramInfoLog(UInt32 program)
        {
            int[] length = new int[1];
            GL.GetProgramiv(program, ProgramParameter.InfoLogLength, length);
            if (length[0] == 0) return String.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length[0]);
            GL.GetProgramInfoLog(program, sb.Capacity, length, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the program info from a shader program.
        /// </summary>
        /// <param name="program">The ID of the shader program.</param>
        public static string GetShaderInfoLog(UInt32 shader)
        {
            int[] length = new int[1];
            GL.GetShaderiv(shader, ShaderParameter.InfoLogLength, length);
            if (length[0] == 0) return String.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length[0]);
            GL.GetShaderInfoLog(shader, sb.Capacity, length, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Replaces the source code in a shader object.
        /// </summary>
        /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
        /// <param name="source">Specifies a string containing the source code to be loaded into the shader.</param>
        public static void ShaderSource(UInt32 shader, string source)
        {
            ShaderSource(shader, 1, new string[] { source }, new int[] { source.Length });
        }

        /// <summary>
        /// Creates and initializes a buffer object's data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">Specifies the target buffer object.</param>
        /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
        /// <param name="data">Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.</param>
        /// <param name="usage">Specifies expected usage pattern of the data store.</param>
        public static void BufferData<T>(BufferTarget target, Int32 size, [InAttribute, OutAttribute] T[] data, BufferUsageHint usage)
            where T : struct
        {
            GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                Delegates.glBufferData(target, new IntPtr(size), data_ptr.AddrOfPinnedObject(), usage);
            }
            finally
            {
                data_ptr.Free();
            }
        }

        /// <summary>
        /// Creates and initializes a buffer object's data store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">Specifies the target buffer object.</param>
        /// <param name="size">Specifies the size in bytes of the buffer object's new data store.</param>
        /// <param name="data">Specifies a pointer to data that will be copied into the data store for initialization, or NULL if no data is to be copied.</param>
        /// <param name="usage">Specifies expected usage pattern of the data store.</param>
        public static void BufferData<T>(BufferTarget target, Int32 position, Int32 size, [InAttribute, OutAttribute] T[] data, BufferUsageHint usage)
            where T : struct
        {
            GCHandle data_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                Delegates.glBufferData(target, new IntPtr(size), (IntPtr)((int)data_ptr.AddrOfPinnedObject() + position), usage);
            }
            finally
            {
                data_ptr.Free();
            }
        }

        /// <summary>
        /// Creates a standard VBO of type T.
        /// </summary>
        /// <typeparam name="T">The type of the data being stored in the VBO (make sure it's byte aligned).</typeparam>
        /// <param name="target">The VBO BufferTarget (usually ArrayBuffer or ElementArrayBuffer).</param>
        /// <param name="data">The data to store in the VBO.</param>
        /// <param name="hint">The buffer usage hint (usually StaticDraw).</param>
        /// <returns>The buffer ID of the VBO on success, 0 on failure.</returns>
        public static uint CreateVBO<T>(BufferTarget target, [InAttribute, OutAttribute] T[] data, BufferUsageHint hint)
            where T : struct
        {
            uint vboHandle = GL.GenBuffer();
            if (vboHandle == 0) return 0;

            GL.BindBuffer(target, vboHandle);
            GL.BufferData<T>(target, data.Length * Marshal.SizeOf(typeof(T)), data, hint);
            GL.BindBuffer(target, 0);
            return vboHandle;
        }

        /// <summary>
        /// Creates a standard VBO of type T where the length of the VBO is less than or equal to the length of the data.
        /// </summary>
        /// <typeparam name="T">The type of the data being stored in the VBO (make sure it's byte aligned).</typeparam>
        /// <param name="target">The VBO BufferTarget (usually ArrayBuffer or ElementArrayBuffer).</param>
        /// <param name="data">The data to store in the VBO.</param>
        /// <param name="hint">The buffer usage hint (usually StaticDraw).</param>
        /// <param name="length">The length of the VBO (will take the first 'length' elements from data).</param>
        /// <returns>The buffer ID of the VBO on success, 0 on failure.</returns>
        public static uint CreateVBO<T>(BufferTarget target, [InAttribute, OutAttribute] T[] data, BufferUsageHint hint, int length)
            where T : struct
        {
            uint vboHandle = GL.GenBuffer();
            if (vboHandle == 0) return 0;

            GL.BindBuffer(target, vboHandle);
            GL.BufferData<T>(target, length * Marshal.SizeOf(typeof(T)), data, hint);
            GL.BindBuffer(target, 0);
            return vboHandle;
        }

        /// <summary>
        /// Creates a standard VBO of type T where the length of the VBO is less than or equal to the length of the data.
        /// </summary>
        /// <typeparam name="T">The type of the data being stored in the VBO (make sure it's byte aligned).</typeparam>
        /// <param name="target">The VBO BufferTarget (usually ArrayBuffer or ElementArrayBuffer).</param>
        /// <param name="data">The data to store in the VBO.</param>
        /// <param name="hint">The buffer usage hint (usually StaticDraw).</param>
        /// <param name="length">The length of the VBO (will take the first 'length' elements from data).</param>
        /// <returns>The buffer ID of the VBO on success, 0 on failure.</returns>
        public static uint CreateVBO<T>(BufferTarget target, [InAttribute, OutAttribute] T[] data, BufferUsageHint hint, int position, int length)
            where T : struct
        {
            uint vboHandle = GL.GenBuffer();
            if (vboHandle == 0) return 0;

            GL.BindBuffer(target, vboHandle);
            GL.BufferData<T>(target, position * Marshal.SizeOf(typeof(T)), length * Marshal.SizeOf(typeof(T)), data, hint);
            GL.BindBuffer(target, 0);
            return vboHandle;
        }

        #region CreateInterleavedVBO
        public static uint CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 6];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }

        public static uint CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, Vector2[] data3, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length || data3.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 8];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;

                interleaved[j++] = data3[i].X;
                interleaved[j++] = data3[i].Y;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }

        public static uint CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, Vector3[] data3, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length || data3.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 9];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;

                interleaved[j++] = data3[i].X;
                interleaved[j++] = data3[i].Y;
                interleaved[j++] = data3[i].Z;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }

        public static uint CreateInterleavedVBO(BufferTarget target, Vector3[] data1, Vector3[] data2, Vector3[] data3, Vector2[] data4, BufferUsageHint hint)
        {
            if (data2.Length != data1.Length || data3.Length != data1.Length) throw new Exception("Data lengths must be identical to construct an interleaved VBO.");

            float[] interleaved = new float[data1.Length * 11];

            for (int i = 0, j = 0; i < data1.Length; i++)
            {
                interleaved[j++] = data1[i].X;
                interleaved[j++] = data1[i].Y;
                interleaved[j++] = data1[i].Z;

                interleaved[j++] = data2[i].X;
                interleaved[j++] = data2[i].Y;
                interleaved[j++] = data2[i].Z;

                interleaved[j++] = data3[i].X;
                interleaved[j++] = data3[i].Y;
                interleaved[j++] = data3[i].Z;

                interleaved[j++] = data4[i].X;
                interleaved[j++] = data4[i].Y;
            }

            return CreateVBO<float>(target, interleaved, hint);
        }
        #endregion

        /// <summary>
        /// Creates a vertex array object based on a series of attribute arrays and and attribute names.
        /// </summary>
        /// <param name="program">The shader program that contains the attributes to be bound to.</param>
        /// <param name="vbo">The VBO containing all of the attribute data.</param>
        /// <param name="sizes">An array of sizes which correspond to the size of each attribute.</param>
        /// <param name="types">An array of the attribute pointer types.</param>
        /// <param name="targets">An array of the buffer targets.</param>
        /// <param name="names">An array of the attribute names.</param>
        /// <param name="stride">The stride of the VBO.</param>
        /// <param name="eboHandle">The element buffer handle.</param>
        /// <returns>The vertex array object (VAO) ID.</returns>
        public static uint CreateVAO(ShaderProgram program, uint vbo, int[] sizes, VertexAttribPointerType[] types, BufferTarget[] targets, string[] names, int stride, uint eboHandle)
        {
            uint vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            int offset = 0;

            for (uint i = 0; i < names.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                GL.BindBuffer(targets[i], vbo);
                GL.VertexAttribPointer(i, sizes[i], types[i], true, stride, new IntPtr(offset));
                GL.BindAttribLocation(program.ProgramID, i, names[i]);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BindVertexArray(0);

            return vaoHandle;
        }
        
        /// <summary>
        /// Gets the current OpenGL version (returns a cached result on subsequent calls).
        /// </summary>
        /// <returns>The current OpenGL version, or 0 on an error.</returns>
        public static int Version()
        {
            if (_version != 0) return _version;	// cache the version information
            
            try
            {
                string version = GL.GetString(StringName.Version);
                return (_version = int.Parse(version.Substring(0, version.IndexOf('.'))));
            }
            catch (Exception)
            {
                Console.WriteLine("Error while retrieving the OpenGL version.");
                return 0;
            }
        }

        /// <summary>
        /// Installs a program object as part of current rendering state.
        /// </summary>
        /// <param name="Program">Specifies the handle of the program object whose executables are to be used as part of current rendering state.</param>
        public static void UseProgram(ShaderProgram Program)
        {
            GL.UseProgram(Program.ProgramID);
        }

        /// <summary>
        /// Bind a named texture to a texturing target
        /// </summary>
        /// <param name="Texture">Specifies the texture.</param>
        public static void BindTexture(Texture Texture)
        {
            GL.BindTexture(Texture.TextureTarget, Texture.TextureID);
        }

        private static int[] getInteger = new int[1];

        /// <summary>
        /// Return the value of the selected parameter.
        /// </summary>
        /// <param name="name">Specifies the parameter value to be returned.</param>
        public static int GetInteger(GetPName name)
        {
            GetIntegerv(name, getInteger);
            return getInteger[0];
        }

        /// <summary>
        /// Get the index of a uniform block in the provided shader program.
        /// Note:  This method will use the provided shader program, so make sure to
        /// store which program is currently active and reload it if required.
        /// </summary>
        /// <param name="program">The shader program that contains the uniform block.</param>
        /// <param name="uniformBlockName">The uniform block name.</param>
        /// <returns>The index of the uniform block.</returns>
        public static uint GetUniformBlockIndex(ShaderProgram program, string uniformBlockName)
        {
            program.Use();  // take care of a crash that can occur on NVIDIA drivers by using the program first
            return GetUniformBlockIndex(program.ProgramID, uniformBlockName);
        }

        /// <summary>
        /// Binds a VBO based on the buffer target.
        /// </summary>
        /// <param name="buffer">The VBO to bind.</param>
        public static void BindBuffer<T>(VBO<T> buffer) 
            where T : struct
        {
            GL.BindBuffer(buffer.BufferTarget, buffer.vboID);
        }

        /// <summary>
        /// Binds a VBO to a shader attribute.
        /// </summary>
        /// <param name="buffer">The VBO to bind to the shader attribute.</param>
        /// <param name="program">The shader program whose attribute will be bound to.</param>
        /// <param name="attributeName">The name of the shader attribute to be bound to.</param>
        public static void BindBufferToShaderAttribute<T>(VBO<T> buffer, ShaderProgram program, string attributeName) 
            where T : struct
        {
            uint location = (uint)GL.GetAttribLocation(program.ProgramID, attributeName);

            GL.EnableVertexAttribArray(location);
            GL.BindBuffer(buffer);
            GL.VertexAttribPointer(location, buffer.Size, buffer.PointerType, true, Marshal.SizeOf(typeof(T)), IntPtr.Zero);
        }

        /// <summary>
        /// Delete a single OpenGL buffer.
        /// </summary>
        /// <param name="buffer">The OpenGL buffer to delete.</param>
        public static void DeleteBuffer(uint buffer)
        {
            int1[0] = buffer;
            DeleteBuffers(1, int1);
            int1[0] = 0;
        }

        /// <summary>
        /// Set a uniform mat4 in the shader.
        /// Uses a cached float[] to reduce memory usage.
        /// </summary>
        /// <param name="location">The location of the uniform in the shader.</param>
        /// <param name="param">The Matrix4 to load into the shader uniform.</param>
        public static void UniformMatrix4fv(int location, Matrix4 param)
        {
            // use the statically allocated float[] for setting the uniform
            for (int i = 0; i < 16; i++)
                matrixFloat[i] = param[i];
            GL.UniformMatrix4fv(location, 1, false, matrixFloat);
        }
    }
}
