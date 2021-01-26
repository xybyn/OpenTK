using Common.Interfaces;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Common.Buffers
{
    public class VBO : IOpenGLUnit
    {
        public int ProgramID { get; }

        private readonly float[] _data;
        private readonly int _sizeInBytes;

        public VBO(float[] data, int sizeInBytes)
        {
            ProgramID = GL.GenBuffer();
            _sizeInBytes = sizeInBytes;
            _data = data;
        }

        private readonly IEnumerable<SubData> subdatas;

        public VBO(float[] data, int sizeInBytes, IEnumerable<SubData> subdatas)
        {
            ProgramID = GL.GenBuffer();
            _sizeInBytes = sizeInBytes;
            this.subdatas = subdatas;
            _data = data;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ProgramID);
            GL.BufferData(BufferTarget.ArrayBuffer, _sizeInBytes, _data,
                BufferUsageHint.StaticDraw);
            if (subdatas != null)
            {
                foreach (SubData subdata in subdatas)
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
            GL.DeleteBuffer(ProgramID);
        }
    }
}