// unset

using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace OpenTKProject
{
    public class Subdata<T> where T : struct
    {
        public int Index { get; set; }
        public int SizeInBytes { get; set; }
        public T[] Data { get; set; }
    }

    public class VBO<T> : BufferBase where T : struct
    {
        public int ProgramID => _program;
        private readonly int _program;
        private readonly T[] _data;
        private readonly int _sizeInBytes;

        public VBO(T[] data, int sizeInBytes)
        {
            _program = GL.GenBuffer();
            _sizeInBytes = sizeInBytes;
            _data = data;
        }

        private readonly IEnumerable<Subdata<T>> subdatas;

        public VBO(T[] data, int sizeInBytes, IEnumerable<Subdata<T>> subdatas)
        {
            _program = GL.GenBuffer();
            _sizeInBytes = sizeInBytes;
            this.subdatas = subdatas;
            _data = data;
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _program);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, _data.Length * _sizeInBytes, _data,
                BufferUsageHint.StaticDraw);
            if (subdatas != null)
            {
                foreach (Subdata<T> subdata in subdatas)
                {
                    GL.BufferSubData(
                        BufferTarget.ArrayBuffer,
                        (IntPtr)(subdata.Index),
                        subdata.SizeInBytes,
                        subdata.Data);
                }
            }
        }

        public override void Free()
        {
            GL.DeleteBuffer(_program);
        }
    }
}