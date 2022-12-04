using Common;
using GlmNet;
using System.Collections.Generic;
using Common._3D_Objects;

namespace Game
{
    public class PathBuilder
    {

        vec3 offset;
        private List<vec3> _points = new List<vec3>();

        public Path CreatePath()
        {
            return new Path(_points);
        }
        vec3 startPoint;
        public IReadOnlyList<vec3> Points => _points;
        public PathBuilder(vec3 startPoint, vec3 offset)
        {
            this.startPoint = startPoint;
            _points.Add(startPoint);
            this.offset = offset;
        }

        public void Clear()
        {
            _points.Clear();
            _points.Add(startPoint);
        }

        public void AddPoint(vec3 point)
        {
            _points.Add(point + offset);
        }
    }
}
