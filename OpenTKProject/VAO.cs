// unset

using Common;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace OpenTKProject
{
    public sealed class VAO<TVbo> : BufferBase
        where TVbo : struct
    {
        public VAO(VBO<TVbo> vbo, IEnumerable<VertexAttribPointer> attribPointers)
        {
            Program = GL.GenVertexArray();
            Bind();
            vbo.Bind();
            foreach (VertexAttribPointer attribPointer in attribPointers)
            {
                attribPointer.Enable();
            }
        }

        public VAO(VBO<TVbo> vbo, EBO ebo, IEnumerable<VertexAttribPointer> attribPointers)
        {
            Program = GL.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo.Bind();
            foreach (VertexAttribPointer attribPointer in attribPointers)
            {
                attribPointer.Enable();
            }
        }

        public override void Bind()
        {
            GL.BindVertexArray(Program);
        }

        public override void Free()
        {
            GL.DeleteVertexArray(Program);
        }
    }
}