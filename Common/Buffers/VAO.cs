using Common.Interfaces;
using Common.Misc;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Common.Buffers
{
    public class VAO : IOpenGLUnit
    {
        public int ProgramID { get; }

        public VAO(VBO vbo, EBO ebo, IEnumerable<VertexAttribPointer> attribPointers)
        {
            ProgramID = GL.GenVertexArray();

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
            GL.BindVertexArray(ProgramID);
        }

        public void Free()
        {
            GL.DeleteVertexArray(ProgramID);
        }

        public VAO(VBO vbo, IEnumerable<VertexAttribPointer> attribPointers)
        {
            ProgramID = GL.GenVertexArray();

            Bind();
            vbo.Bind();
            foreach (VertexAttribPointer attribPointer in attribPointers)
            {
                attribPointer.Enable();
            }
        }
    }
}