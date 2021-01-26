using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Common
{
    public class Subdata
    {
        public int Index { get; set; }
        public int SizeInBytes { get; set; }
        public float[] Data { get; set; }
    }

    public class VBO : IOpenGLUnit
    {
        public int ProgramID => _program;
        private readonly int _program;
        private readonly float[] _data;
        private readonly int _sizeInBytes;

        public VBO(float[] data, int sizeInBytes)
        {
            _program = GL.GenBuffer();
            _sizeInBytes = sizeInBytes;
            _data = data;
        }

        private readonly IEnumerable<Subdata> subdatas;

        public VBO(float[] data, int sizeInBytes, IEnumerable<Subdata> subdatas)
        {
            _program = GL.GenBuffer();
            _sizeInBytes = sizeInBytes;
            this.subdatas = subdatas;
            _data = data;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _program);
            GL.BufferData(BufferTarget.ArrayBuffer, _sizeInBytes, _data,
                BufferUsageHint.StaticDraw);
            if (subdatas != null)
            {
                foreach (Subdata subdata in subdatas)
                {
                    GL.BufferSubData(
                        BufferTarget.ArrayBuffer,
                        (IntPtr)(subdata.Index),
                        subdata.SizeInBytes,
                        subdata.Data);
                }
            }
        }

        public void Free()
        {
            GL.DeleteBuffer(_program);
        }
    }
}