using OpenTK.Graphics.OpenGL4;
using System;
using GL = OpenTK.Graphics.ES11.GL;
using PrimitiveType = OpenTK.Graphics.ES11.PrimitiveType;

namespace Common
{
    public class FunctionDrawer2D : Drawer2DBase
    {
        private float[] vertices;
        private VAO vao;
        private Shader shader;
        private readonly Func<float, float> _f;

        public FunctionDrawer2D(Func<float, float> f, Drawer2DSettingsBase settings) : base(settings)
        {
            _f = f;
        }

        public override void Draw()
        {
            shader.Use();
            vao.Bind();
            GL.DrawArrays(PrimitiveType.LineStrip, 0, (int)settings.NumberOfPartitions + 1);
        }

        public override void Initialize()
        {
            int steps = settings.NumberOfPartitions;
            float minX = settings.MinX;
            float maxX = settings.MaxX;
            shader = new Shader("test.vert", "test.frag");
            float step = (maxX - minX) / steps;
            float x = minX;
            vertices = new float[steps * 2 + 2];
            float scale = 0.01f;
            for (int i = 0; i < steps + 1; i++)
            {
                float res = _f(x);
                vertices[i * 2] = x * scale;
                vertices[i * 2 + 1] = res * scale;
                x += step;
            }

            VBO vbo = new VBO(vertices, vertices.Length * sizeof(float));
            vao = new VAO(vbo, new[]
            {
                            new VertexAttribPointer
                            {
                                Index = 0,
                                Normalize = false,
                                OffsetInBytes = 0,
                                Size = 2,
                                StrideInBytes = 2*sizeof(float),
                                Type = VertexAttribPointerType.Float
                            }
                        });
        }
    }
}