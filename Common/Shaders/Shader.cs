using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace Common.Shaders
{
    public class Shader
    {
        private readonly int _fragmentShader;
        private readonly int _handle;
        private readonly int _vertexShader;
        private bool _disposedValue;

        public Shader(string vertexPath, string fragmentPath, string geometryPath = null)
        {
            string vertexShaderSource;

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                vertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            _vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(_vertexShader, vertexShaderSource);

            _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(_fragmentShader, FragmentShaderSource);

            GL.CompileShader(_vertexShader);

            string infoLogVertex = GL.GetShaderInfoLog(_vertexShader);
            if (infoLogVertex != string.Empty)
            {
                Console.WriteLine(infoLogVertex);
            }

            GL.CompileShader(_fragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(_fragmentShader);

            if (infoLogFrag != string.Empty)
            {
                Console.WriteLine(infoLogFrag);
            }

            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, _vertexShader);
            GL.AttachShader(_handle, _fragmentShader);
            int geometryShader = 0;
            if (geometryPath != null)
            {
                using (StreamReader reader = new StreamReader(geometryPath, Encoding.UTF8))
                {
                    string geomCode = reader.ReadToEnd();
                    geometryShader = GL.CreateShader(ShaderType.GeometryShader);
                    GL.ShaderSource(geometryShader, geomCode);
                    GL.CompileShader(geometryShader);
                    infoLogVertex = GL.GetShaderInfoLog(geometryShader);
                    if (infoLogVertex != string.Empty)
                    {
                        Console.WriteLine(infoLogVertex);
                    }

                    GL.AttachShader(_handle, geometryShader);
                }
            }

            GL.LinkProgram(_handle);
            string lof = GL.GetProgramInfoLog(_handle);
            // Console.WriteLine(lof);
            GL.DetachShader(_handle, _vertexShader);
            GL.DetachShader(_handle, _fragmentShader);
            GL.DeleteShader(_fragmentShader);
            GL.DeleteShader(_vertexShader);
            if (geometryPath != null)
            {
                GL.DetachShader(_handle, geometryShader);
                GL.DeleteShader(geometryShader);
            }
        }

        public void SetMat4(string value, ref mat4 mat)
        {
            Use();
            int location = GL.GetUniformLocation(_handle, value);
            GL.UniformMatrix4(location, 1, false, mat.to_array());
        }

        public void SetVec3(string value, ref vec3 vec)
        {
            Use();
            int location = GL.GetUniformLocation(_handle, value);
            GL.Uniform3(location, 1, vec.to_array());
        }

        public int GetAttributeLocation(string attribName)
        {
            return GL.GetUniformLocation(_handle, attribName);
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            GL.DeleteProgram(_handle);

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}