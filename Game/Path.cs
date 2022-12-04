using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Path
    {
        struct PathSegment
        {
            public PathSegment(vec3 p0, vec3 p1)
            {
                _p0 = p0;
                _p1 = p1;
            }
            public vec3 GetPosition(float t)
            {
                return _p0 * (1 - t) + _p1 * t;
            }
            public float GetLength()
            {
                vec3 v = _p1 - _p0;
                return MathF.Sqrt(glm.dot(v, v));
            }
            private vec3 _p0;
            private vec3 _p1;

            public override string ToString()
            {
                return $"from {_p0} to {_p1}";
            }
        }
        private Queue<PathSegment> _segments = new Queue<PathSegment>();
        private vec3 _lastPoint;
        private float _prev = 0;
        public Path(IList<vec3> pathPoints)
        {
            for (int i = 0; i < pathPoints.Count - 1; i++)
            {
                _segments.Enqueue(new PathSegment(pathPoints[i], pathPoints[i + 1]));
            }
            _lastPoint = pathPoints.Last();
        }

        public bool Ended()
        {
            return _segments.Count == 0;
        }

        public vec3 GetPosition(float t)
        {
            if (_segments.Count == 0)
                return _lastPoint;
            float tau = (t - _prev) / (_segments.Peek().GetLength());
            if (tau > 1.0f)
            {
                _prev = t;
                _segments.Dequeue();
                if (_segments.Count == 0)
                    return _lastPoint;
                tau = (t - _prev) / (_segments.Peek().GetLength());
            }
            return _segments.Peek().GetPosition(tau);
        }

        public override string ToString()
        {
            string s = "Path: ";
            foreach (var item in _segments)
            {
                s += item.ToString() + "\n";
            }
            return s;
        }
    }
}
