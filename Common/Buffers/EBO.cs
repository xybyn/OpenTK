using OpenTK.Graphics.OpenGL4;

namespace Common
{
    public class EBO : IOpenGLUnit
    {
        public int ProgramID => _program;
        private readonly int _program;
        private readonly uint[] _indices;

        public EBO(uint[] indices)
        {
            _program = GL.GenBuffer();
            _indices = indices;
        }

        public void Free()
        {
            GL.DeleteBuffer(_program);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _program);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
                BufferUsageHint.StaticDraw);
        }
    }
}