using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Common
{
    public class PlaneDrawer3D : Drawer3DBase
    {
        private readonly Shader shader;
        private mat4 model;

        private readonly Line3D line3d;
        private readonly float[] vertices;

        public PlaneDrawer3D(Drawer3DSettingsBase settings) : base(settings)
        {
            shader = new Shader(@"Shaders\Plane\plane.vert", @"Shaders\Plane\plane.frag");
            vertices = new float[]
            {
                -0.5f, 0,-0.5f,
                 0.5f, 0,-0.5f,
                 0.5f, 0, 0.5f,
                -0.5f, 0, 0.5f,
            };
            model = glm.translate(mat4.identity(), new vec3(0, -1, 0));
            var indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3,
            };
            VBO vbo = new VBO(vertices, vertices.Length * sizeof(float));
            EBO ebo = new EBO(indices);

            vao = new VAO(vbo, ebo, new[]
            {
                new VertexAttribPointer
                {
                    Size = 3,
                    Index = 0,
                    OffsetInBytes = 0,
                    StrideInBytes = 3 * sizeof(float),
                    Type = VertexAttribPointerType.Float,
                    Normalize = false
                }
            });
            Normal = new Line
            {
                point = new vec3(0, 0, 0),
                direction = new vec3(0, 1, 0),
            };
            line3d = new Line3D(Normal);
        }

        private readonly VAO vao;
        private float x = 0;

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            shader.Use();
            model = glm.translate(mat4.identity(), new vec3(x, 0, 0)) * glm.rotate((float)GLFW.GetTime() / 10f, new vec3(0, 1, 1));

            shader.SetMat4("projection", ref projection);
            shader.SetMat4("model", ref model);
            shader.SetMat4("view", ref view);
            vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            line3d.SetLine(GetNormal());
            line3d.Draw(ref view, ref projection);
            x += 0.0001f;
        }

        private Line GetNormal()
        {
            Line line = new Line();
            line.point = new vec3(model * new vec4(Normal.point, 1));
            line.direction = new vec3(model * new vec4(Normal.direction, 1));
            return line;
        }

        private readonly Line Normal;
    }
}