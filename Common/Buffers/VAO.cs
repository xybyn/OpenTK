using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Common
{
    public class VAO : IOpenGLUnit
    {
        public int ProgramID => _program;

        private readonly int _program;

        public VAO(VBO vbo, EBO ebo, IEnumerable<VertexAttribPointer> attribPointers)
        {
            _program = GL.GenVertexArray();

            Bind();
            vbo.Bind();
            ebo.Bind();
            foreach (VertexAttribPointer attribPointer in attribPointers)
            {
                attribPointer.Enable();
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(_program);
        }

        public void Free()
        {
            GL.DeleteVertexArray(_program);
        }

        public VAO(VBO vbo, IEnumerable<VertexAttribPointer> attribPointers)
        {
            _program = GL.GenVertexArray();

            Bind();
            vbo.Bind();
            foreach (VertexAttribPointer attribPointer in attribPointers)
            {
                attribPointer.Enable();
            }
        }
    }
}