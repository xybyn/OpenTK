using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common._3D_Objects.Primitives
{
    public class Lines : SceneObject3D
    {
        private readonly List<vec3> points;

        public Lines(List<vec3> points)
        {
            this.points = points;

            List<uint> indices = new List<uint>();
            for (uint i = 0;i < points.Count-1; i++)
            {
                indices.Add(i);
                indices.Add(i+1);
            }


            InitializeVAO_VBO_EBO(points, indices);
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            vao.Bind();
            UpdateDefaultShader(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}
