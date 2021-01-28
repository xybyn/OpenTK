// unset

using Common.Extensions;
using Common.MathAbstractions;
using GlmNet;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace Common._3D_Objects
{
    public class Pipe : SceneObject3D
    {
        private List<vec3> _contour = new();
        private readonly List<List<vec3>> _contours = new();

        private readonly List<List<vec3>> _normals = new();
        private List<vec3> _path = new();
        private float[] _vertices;

        public Pipe()
        {
        }

        public Pipe(Func<List<vec3>> pathFunc, Func<List<vec3>> contourFunc = null)
        {
            SetNewFunctions(pathFunc, contourFunc);
        }

        public void SetNewFunctions(Func<List<vec3>> pathFunc, Func<List<vec3>> contourFunc = null)
        {
            ClearBuffers();
            _contour.Clear();
            _contours.Clear();
            _normals.Clear();
            _path.Clear();

            _contour = contourFunc != null ? contourFunc() : GetCirclePoints(0.1f, 40);
            _path = pathFunc();

            CalculateVertexNormalsIndices();
        }

        public Pipe(IEnumerable<vec3> path, Func<List<vec3>> contourFunc = null)
        {
            _contour = contourFunc != null ? contourFunc() : GetCirclePoints(0.1f, 40);

            foreach (vec3 p in path)
            {
                this._path.Add(p);
            }
            CalculateVertexNormalsIndices();
        }

        private void CalculateVertexNormalsIndices()
        {
            GenerateContour();

            _vertices = _contours.ToSingleArray();
            float[] no = _normals.ToSingleArray();
            List<uint> ind = new();
            for (int i = 0; i < _contours.Count - 1; i++)
            {
                List<vec3> currentCont = _contours[i];
                List<vec3> other = _contours[i + 1];

                for (int j = 0; j < currentCont.Count - 1; j++)
                {
                    uint curr = (uint)((i * currentCont.Count) + j);
                    uint currR = (uint)((i * currentCont.Count) + j + 1);
                    uint o = (uint)(((i + 1) * currentCont.Count) + j);
                    uint or = (uint)(((i + 1) * currentCont.Count) + j + 1);

                    ind.Add(curr);
                    ind.Add(currR);
                    ind.Add(o);

                    ind.Add(o);
                    ind.Add(currR);
                    ind.Add(or);
                }
            }
            InitializeVAO_VBO_EBO(_vertices, no, ind.ToArray());
        }

        private List<vec3> GetCirclePoints(float radius, int steps)
        {
            List<vec3> points = new();
            if (steps < 2)
            {
                return points;
            }

            const float PI2 = 2 * PI;
            float x, y, a;
            for (int i = 0; i <= steps; ++i)
            {
                a = PI2 / steps * i;
                x = radius * Cos(a);
                y = radius * Sin(a);
                points.Add(new vec3(x, 0, y));
            }
            return points;
        }

        private List<vec3> ContourProjectionOnPlane(int fromIndex, int toIndex)
        {
            vec3 v1, v2, normal, point;

            Line line = new();

            // find direction vectors; v1 and v2
            v1 = _path[toIndex] - _path[fromIndex];
            if (toIndex == _path.Count - 1)
            {
                v2 = v1;
            }
            else
            {
                v2 = _path[toIndex + 1] - _path[toIndex];
            }

            // normal vector of plane at toIndex
            normal = v1 + v2;

            // define plane equation at toIndex with normal and point
            Plane plane = new()
            {
                Normal = normal,
                D = _path[toIndex]
            };

            // project each vertex of contour to the plane
            List<vec3> fromContour = _contours[fromIndex];
            List<vec3> toContour = new();
            int count = fromContour.Count;
            for (int i = 0; i < count; ++i)
            {
                line.Direction = v1;
                line.Point = fromContour[i]; // define line with direction and point
                point = plane.Intersect(line); // find the intersection point
                toContour.Add(point);
            }
            return toContour;
        }

        private void TransformFirstContour()
        {
            int pathCount = _path.Count;
            int vertexCount = _contour.Count;
            mat4 matrix;

            if (pathCount > 0)
            {
                // transform matrix
                if (pathCount > 1)
                /*matrix = glm.lookAt(new vec3(0, 0, 0), path[1]-path[0], new vec3(0, 1, 0));*/
                {
                    vec3 normal = new(0, 1, 0);
                    vec3 target = _path[1] - _path[0];
                    float gamma = Acos(glm.dot(target, normal) / (Sqrt(glm.dot(normal, normal)) * Sqrt(glm.dot(target, target))));
                    vec3 right = glm.cross(normal, target);
                    matrix = glm.rotate(gamma, right);
                }

                mat4 model = glm.translate(mat4.identity(), _path[0]);
                /*var model = mat4.identity();*/

                // multiply matrix to the contour
                // NOTE: the contour vertices are transformed here
                //       MUST resubmit contour data if the path is resset to 0
                for (int i = 0; i < vertexCount; ++i)
                {
                    _contour[i] = new vec3(model * matrix * new vec4(_contour[i], 1));
                }
            }
        }

        private void GenerateContour()
        {
            _contours.Clear();
            _normals.Clear();

            // path must have at least a point
            if (_path.Count < 1)
            {
                return;
            }

            // rotate and translate the contour to the first path point
            TransformFirstContour();
            _contours.Add(_contour);
            _normals.Add(ComputeContourNormal(0));

            // project contour to the plane at the next path point
            int count = _path.Count;
            for (int i = 1; i < count; ++i)
            {
                _contours.Add(ContourProjectionOnPlane(i - 1, i));
                _normals.Add(ComputeContourNormal(i));
            }
        }

        private List<vec3> ComputeContourNormal(int pathIndex)
        {
            // get current contour and center point
            List<vec3> contour = _contours[pathIndex];
            vec3 center = _path[pathIndex];

            List<vec3> contourNormal = new();
            vec3 normal;
            for (int i = 0; i < contour.Count; ++i)
            {
                normal = glm.normalize(contour[i] - center);
                contourNormal.Add(normal);
            }

            return contourNormal;
        }
    }
}