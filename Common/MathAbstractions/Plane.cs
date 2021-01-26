// unset

using GlmNet;

namespace Common
{
    public class Plane
    {
        private vec3 _normal;
        public vec3 Normal { get; set; }
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