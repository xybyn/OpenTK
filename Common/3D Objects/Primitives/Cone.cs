// unset

using Common._3D_Objects;
using GlmNet;
using System.Collections.Generic;
using Utils;
using static System.MathF;

namespace Common
{
    public class Cone : SceneObject3D
    {
        public Cone()
        {
            float h = 1;
            float r = 4;
            int segments = 100;
            float step = 2 * PI / (segments - 1);
            List<vec3> normals = new List<vec3>();
            List<vec3> vertices = new List<vec3>();
            List<uint> indices = new List<uint>();

            float angle = 0;
            for (int i = 1; i <= segments; i++)
            {
                float x = r * Cos(angle);
                float z = r * Sin(angle);
                vertices.Add(new vec3(x, 0, z));
                if (i % 2 == 0)
                {
                    vertices.Add(new vec3(0, h, 0));
                }
                angle += step;
            }

            indices = AppUtils.GetIndices(segments, 2);

            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }
    }
}