using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Common
{
    public class PointDrawer3D : Drawer3DBase
    {
        public PointDrawer3D(Drawer3DSettingsBase settings) : base(settings)
        {
            pointShader = new Shader(@"Shaders\Point\point3D.vert", @"Shaders\Point\point3D.frag");
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            if (points.Count > 0)
            {
                mat4 model = mat4.identity();
                pointShader.Use();
                pointShader.SetMat4("projection", ref projection);
                pointShader.SetMat4("model", ref model);
                pointShader.SetMat4("view", ref view);
                vao.Bind();
                GL.DrawArrays(PrimitiveType.Points, 0, points.Count);
            }
        }

        public List<vec3> points = new List<vec3>();
        private readonly Shader pointShader;
        private VAO vao;
        private VBO pointsVbo;

        public void AddPoint(vec3 newPoint)
        {
            points.Add(newPoint);
            if (vao != null)
            {
                vao.Free();
                pointsVbo.Free();
            }

            float[] vertices = new float[points.Count * 3];
            for (int i = 0; i < points.Count; i++)
            {
                vertices[i * 3] = points[i].x;
                vertices[i * 3 + 1] = points[i].y;
                vertices[i * 3 + 2] = points[i].z;
            }

            pointsVbo = new VBO(vertices, vertices.Length * sizeof(float));
            vao = new VAO(pointsVbo, new[]
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

        public void Initialize()
        {
        }
    }
}