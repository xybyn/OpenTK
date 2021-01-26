/*using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTKProject;
using System.Collections.Generic;

namespace Common
{
    public class BezierCurve3D : SceneObject
    {
        private VAO vao;
        private VBO vbo;
        private Shader shader;
        private float[] vertices;
        private readonly int steps = 30;

        private List<vec3> points = new();
        public BezierCurve3D(List<vec3> points)
        {
        }

        private readonly List<vec3> points = new List<vec3>();

        public void AddPoint(vec3 newPoint)
        {
            if (vao != null)
            {
                vao.Free();
                vbo.Free();
            }

            points.Add(newPoint);
            float step = 1 / (float)steps;
            float t = 0;
            vertices = new float[steps * 2 + 2];
            for (int i = 0; i < steps + 1; i++)
            {
                vec3 point = Utils.AppUtils.Bezier(t, points);
                vertices[i * 2 + 0] = point.x;
                vertices[i * 2 + 1] = point.y;
                t += step;
            }

            vbo = new VBO(vertices, (vertices.Length) * sizeof(float));
            vao = new VAO(vbo, new[]
            {
                new VertexAttribPointer
                {
                    Index = 0,
                    Size = 2,
                    Type = VertexAttribPointerType.Float,
                    StrideInBytes = 2 * sizeof(float),
                    OffsetInBytes = 0,
                    Normalize = false
                }
            });
        }

        public override void Draw()
        {
            if (points.Count > 1)
            {
                shader.Use();
                vao.Bind();
                GL.DrawArrays(PrimitiveType.LineStrip, 0, steps + 1);
            }
        }

        public override void Initialize()
        {
            shader = new Shader(@"Shaders\test.vert", @"Shaders\test.frag");
        }
    }
}*/