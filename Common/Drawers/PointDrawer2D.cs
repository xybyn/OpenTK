using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Common
{
    public class PointDrawer2D : Drawer2DBase
    {
        public List<vec2> points = new List<vec2>();
        private Shader pointShader;
        private VAO vao;
        private VBO pointsVbo;

        public void AddPoint(vec2 newPoint)
        {
            points.Add(newPoint);
            if (vao != null)
            {
                vao.Free();
                pointsVbo.Free();
            }

            float[] vertices = new float[points.Count * 2];
            for (int i = 0; i < points.Count; i++)
            {
                vertices[i * 2] = points[i].x;
                vertices[i * 2 + 1] = points[i].y;
            }

            pointsVbo = new VBO(vertices, vertices.Length * sizeof(float));
            vao = new VAO(pointsVbo, new[]
            {
                new VertexAttribPointer
                {
                    Index = 0,
                    Size = 2,
                    Type = VertexAttribPointerType.Float,
                    Normalize = false,
                    StrideInBytes =2* sizeof(float),
                    OffsetInBytes = 0
                }
            });
        }

        public PointDrawer2D(Drawer3DSettingsBase settings) : base(settings)
        {
        }

        public override void Draw()
        {
            if (points.Count > 0)
            {
                pointShader.Use();
                GL.BindVertexArray(vao.ProgramID);
                GL.DrawArrays(PrimitiveType.Points, 0, points.Count);
            }
        }

        public override void Initialize()
        {
            pointShader = new Shader(@"Shaders\Point\point.vert", @"Shaders\Point\point.frag");
        }
    }
}