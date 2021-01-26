using GlmNet;
using OpenTKProject;
using static System.MathF;

namespace Common
{
    public class Plane3D : SceneObject
    {
        private Plane _plane;

        public Plane Plane
        {
            get
            {
                _plane.D = localpos;
                return _plane;
            }
        }

        public Plane3D(vec3 normal, vec3 d) : this(new Plane
        {
            D = d, Normal = normal
        })
        {
        }

        public Plane3D(Plane plane)
        {
            _plane = plane;
            TranslateGlobal(plane.D);
            var normal = new vec3(0, 1, 0);
            var target = plane.Normal;
            var gamma = Acos(glm.dot(target, normal) / (Sqrt(glm.dot(normal, normal)) * Sqrt(glm.dot(target, target))));
            if (gamma != 0)
            {
                var right = glm.cross(normal, target);
                Rotate(gamma, right);
            }
            var vertices = new float[]
            {
                -0.5f, 0, -0.5f, 0.5f, 0, -0.5f, 0.5f, 0, 0.5f, -0.5f, 0, 0.5f
            };
            var indices = new uint[]
            {
                0, 1, 3, 1, 2, 3
            };

            var normals = new float[]
            {
                0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0
            };

            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }
       
    }
}