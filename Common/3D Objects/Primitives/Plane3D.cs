// unset

using Common._3D_Objects;
using Common.MathAbstractions;
using GlmNet;
using static System.MathF;

namespace Common
{
    public class Plane3D : SceneObject3D
    {
        public Plane Plane { get; }

        public Plane3D(vec3 normal, vec3 d) : this(new Plane
        {
            D = d,
            Normal = normal
        })
        {
        }

        public Plane3D(Plane plane)
        {
            Plane = plane;
            TranslateWorld(plane.D);
            var upVector = new vec3(0, 1, 0);
            var target = plane.Normal;
            float gamma = Acos(glm.dot(target, upVector) / (Sqrt(glm.dot(upVector, upVector)) * Sqrt(glm.dot(target, target))));
            if (gamma != 0)
            {
                var right = glm.cross(upVector, target);
                RotateLocal(gamma, right);
            }
            float[] vertices = new[]
            {
                -0.5f, 0, -0.5f,
                0.5f, 0, -0.5f,
                0.5f, 0, 0.5f,
                -0.5f, 0, 0.5f
            };
            uint[] indices = new uint[]
            {
                0, 1, 3, 1, 2, 3
            };

            float[] normals = new float[]
            {
                0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0
            };

            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }
    }
}