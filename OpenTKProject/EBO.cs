using OpenTK.Graphics.OpenGL4;
using OpenTKProject;

namespace Common
{
    public class EBO : BufferBase
    {
        private readonly int[] _indices;
        private readonly int _typeSize;

        public EBO(int[] indices)
        {
            Program = GL.GenBuffer();
            _typeSize = sizeof(int);
            _indices = indices;
        }

        public override void Free()
        {
            GL.DeleteBuffer(Program);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Program);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _typeSize, _indices,
                BufferUsageHint.StaticDraw);
        }
    }
}