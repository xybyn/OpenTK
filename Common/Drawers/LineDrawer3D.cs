using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Common
{
    public class LineDrawer3D : Drawer3DBase
    {
        public LineDrawer3D(Drawer3DSettingsBase settings) : base(settings)
        {
            pointShader = new Shader(@"Shaders\Point\point3D.vert", @"Shaders\Point\point3D.frag");
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            if (lines.Count > 0)
            {
                mat4 model = mat4.identity();
                pointShader.Use();
                pointShader.SetMat4("projection", ref projection);
                pointShader.SetMat4("model", ref model);
                pointShader.SetMat4("view", ref view);
                vao.Bind();
                GL.DrawArrays(PrimitiveType.Lines, 0, 2 * lines.Count);
            }
        }

        public List<Line> lines = new List<Line>();
        private readonly Shader pointShader;
        private VAO vao;
        private VBO vbo;
        public void Clear()
        {
            if (vao != null)
            {
                vao.Free();
                vbo.Free();
            }
            lines.Clear();
        }
        public void AddLine(Line newPoint)
        {
            lines.Add(newPoint);
            if (vao != null)
            {
                vao.Free();
                vbo.Free();
            }

            float[] vertices = new float[lines.Count * 6];
            for (int i = 0; i < lines.Count; i++)
            {
                vertices[i * 6] = lines[i].point.x;
                vertices[i * 6 + 1] = lines[i].point.y;
                vertices[i * 6 + 2] = lines[i].point.z;
                vertices[i * 6 + 3] = lines[i].direction.x;
                vertices[i * 6 + 4] = lines[i].direction.y;
                vertices[i * 6 + 5] = lines[i].direction.z;
            }

            vbo = new VBO(vertices, vertices.Length * sizeof(float));
            vao = new VAO(vbo, new[]
            {
                new VertexAttribPointer
                {
                    Index = 0,
                    Size = 3,
                    Type = VertexAttribPointerType.Float,
                    Normalize = false,
                    StrideInBytes =3* sizeof(float),
                    OffsetInBytes = 0
                }
            });
        }
    }
}