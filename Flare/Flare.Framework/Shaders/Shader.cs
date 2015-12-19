using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Flare.Framework.Shaders
{
    public class Shader : IDisposable
    {
        public string VertexSource { get; set; }
        public string FragmentSource { get; set; }

        private int program, vertexShader, fragmentShader;
        private Dictionary<string, int> uniformDictionary;
        private bool disposed = false;

        public Shader()
        {
            program = vertexShader = fragmentShader = 0;
        }

        public Shader(string vShaderSource, string fShaderSource)
        {
            VertexSource = vShaderSource;
            FragmentSource = fShaderSource;
        }

        public void Compile()
        {
            uniformDictionary = new Dictionary<string, int>();

            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, VertexSource);
            GL.CompileShader(vertexShader);

            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, FragmentSource);
            GL.CompileShader(fragmentShader);

            program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            int shaderParams = -1;
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out shaderParams);
            Console.WriteLine("Vertex Shader: " + shaderParams);
            if (shaderParams != 1)
                Console.WriteLine(GL.GetShaderInfoLog(vertexShader));

            shaderParams = -1;
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out shaderParams);
            Console.WriteLine("Fragment Shader: " + shaderParams);
            if (shaderParams != 1)
                Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

            shaderParams = -1;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out shaderParams);
            if (shaderParams != 1)
                Console.WriteLine("Could not link shader program " + program);
        }

        public void Use()
        {
            GL.UseProgram(program);
        }

        public void SetUniform(string name, int data)
        {
            GL.Uniform1(FindUniformLocation(name), data);
        }

        public void SetUniform(string name, float data)
        {
            GL.Uniform1(FindUniformLocation(name), data);
        }

        public void SetUniform(string name, Vector2 data)
        {
            GL.Uniform2(FindUniformLocation(name), data);
        }

        public void SetUniform(string name, Vector2[] data)
        {
            float[] dataFloats = new float[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                dataFloats[i * 2] = data[i].X;
                dataFloats[i * 2 + 1] = data[i].Y;
            }
            GL.Uniform2(FindUniformLocation(name), data.Length, dataFloats);
        }

        public void SetUniform(string name, Vector3 data)
        {
            GL.Uniform3(FindUniformLocation(name), data);
        }

        public void SetUniform(string name, Vector4 data)
        {
            GL.Uniform4(FindUniformLocation(name), data);
        }

        public void SetUniform(string name, Matrix4 data)
        {
            OpenTK.Matrix4 tkData = data;
            GL.UniformMatrix4(FindUniformLocation(name), false, ref tkData);
        }

        private int FindUniformLocation(string name)
        {
            int location;
            if (!uniformDictionary.TryGetValue(name, out location))
            {
                location = GL.GetUniformLocation(program, name);

                if (location < 0)
                    throw new KeyNotFoundException("Uniform doesn't exist.");

                uniformDictionary[name] = location;
            }

            return location;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    GL.DeleteShader(vertexShader);
                    GL.DeleteShader(fragmentShader);
                    GL.DeleteProgram(program);
                }

                disposed = true;
            }
        }

        ~Shader()
        {
            Dispose(false);
        }
    }

}
