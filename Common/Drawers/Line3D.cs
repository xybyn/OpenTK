using Common.Buffers;
using Common.MathAbstractions;
using Common.Misc;
using Common.Shaders;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Common.Drawers
{
    public class Line3D
    {
        private readonly float[] vertices = new float[6];
        private readonly Shader shader;
        private Line line;

        public void SetLine(Line newLine)
        {
            line = newLine;
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ProgramID);
            float v = line.Point.x;
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, sizeof(float), ref v);
            v = line.Point.y;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(1 * sizeof(float)), sizeof(float), ref v);
            v = line.Point.z;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(2 * sizeof(float)), sizeof(float), ref v);
            v = line.Direction.x;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(3 * sizeof(float)), sizeof(float), ref v);
            v = line.Direction.y;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(4 * sizeof(float)), sizeof(float), ref v);
            v = line.Direction.z;
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(5 * sizeof(float)), sizeof(float), ref v);
        }

        public void SetStartX(float newX)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ProgramID);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, sizeof(float), ref newX);
        }

        public void SetStartY(float v)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ProgramID);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)sizeof(float), sizeof(float), ref v);
        }

        public void SetStartZ(float v)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ProgramID);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(2 * sizeof(float)), sizeof(float), ref v);
        }

        private readonly VBO vbo;
        private readonly VAO vao;

        public Line3D(Line line)
        {
            shader = new Shader(@"Shaders\Point\point3D.vert", @"Shaders\Point\point3D.frag");
            vertices[0] = line.Point.x;
            vertices[1] = line.Point.y;
            vertices[2] = line.Point.z;
            vertices[3] = line.Direction.x;
            vertices[4] = line.Direction.y;
            vertices[5] = line.Direction.z;
            vbo = new VBO(vertices, sizeof(float) * vertices.Length);
            vao = new VAO(vbo, new[]
            {
                new VertexAttribPointer
                {
                    Size = 3,
                    Index = 0,
                    Normalize = false,
                    OffsetInBytes = 0,
                    StrideInBytes = 3 * sizeof(float),
                    Type = VertexAttribPointerType.Float
                }
            });
        }

        public void Draw(ref mat4 view, ref mat4 projection)
        {
            mat4 model = mat4.identity();

            shader.Use();
            shader.SetMat4("projection", ref projection);
            shader.SetMat4("model", ref model);
            shader.SetMat4("view", ref view);
            vao.Bind();
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        }
    }
}