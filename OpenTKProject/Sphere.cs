// unset

using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class Sphere : SceneObject
    {
        public float Radius { get; protected set; }

        public Sphere(float radius, Vector3 position)
        {
            Radius = radius;
            Translate(position);
        }

        public Sphere(float radius)
        {
            Radius = radius;
            Translate(Vector3.Zero);
        }
    }
}