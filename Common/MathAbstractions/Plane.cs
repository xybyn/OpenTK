// unset

using GlmNet;

namespace Common.MathAbstractions
{
    public class Plane
    {
        private readonly vec3 _normal;

        public vec3 Normal
        {
            get => glm.normalize(_normal);
            init => _normal = value;
        }
        public vec3 D { get; set; }

        public vec3 Intersect(Line line)
        {
            var p = line.Point;
            var v = line.Direction;

            var t = -(glm.dot(p, Normal) - glm.dot(Normal, D)) / (glm.dot(Normal, v));
            return p + t * v;
        }
    }
}