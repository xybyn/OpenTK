// unset

using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTKProject;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.MathF;

namespace Common
{
    public class Pipe : SceneObject
    {
        private float[] vertices;
        private List<List<vec3>> contours = new();
        private List<vec3> path = new();

        private void CalculateVertexNormalsIndices()
        {
            generatecont();

            vertices = contours.ToSingleArray();
            var no = normals.ToSingleArray();
            var ind = new List<uint>();
            for (int i = 0; i < contours.Count - 1; i++)
            {
                var currentCont = contours[(int)i];
                var other = contours[(int)i + 1];

                for (int j = 0; j < currentCont.Count - 1; j++)
                {
                    var curr = (uint)(i * currentCont.Count + j);
                    var currR = (uint)(i * currentCont.Count + j + 1);
                    var o = (uint)((i + 1) * currentCont.Count + j);
                    var or = (uint)((i + 1) * currentCont.Count + j + 1);

                    ind.Add(curr);
                    ind.Add(currR);
                    ind.Add(o);

                    ind.Add(o);
                    ind.Add(currR);
                    ind.Add(or);
                }
            }
            InitializeVAO_VBO_EBO(vertices, no, ind.ToArray());
        }

        public Pipe(Func<List<vec3>> pathFunc, Func<List<vec3>> contourFunc = null)
        {
            if (contourFunc != null)
                contour = contourFunc();
            else
                contour = circle(0.1f, 40);
            path = pathFunc();

            CalculateVertexNormalsIndices();
        }

        public Pipe(IEnumerable<vec3> path, Func<List<vec3>> contourFunc = null)
        {
            if (contourFunc != null)
                contour = contourFunc();
            else
                contour = circle(0.1f, 40);

            foreach (var p in path)
            {
                this.path.Add(p);
            }
            CalculateVertexNormalsIndices();
        }

        private uint[] indices;

        private List<vec3> circle(float radius, int steps)
        {
            List<vec3> points = new List<vec3>();
            if (steps < 2) return points;

            const float PI2 = 2 * MathF.PI;
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

        private List<vec3> proj(int fromIndex, int toIndex)
        {
            vec3 v1, v2, normal, point;

            Line line = new Line();

            // find direction vectors; v1 and v2
            v1 = path[toIndex] - path[fromIndex];
            if (toIndex == (int)path.Count - 1)
                v2 = v1;
            else
                v2 = path[toIndex + 1] - path[toIndex];

            // normal vector of plane at toIndex
            normal = v1 + v2;

            // define plane equation at toIndex with normal and point
            Plane plane = new Plane()
            {
                Normal = normal,
                D = path[toIndex]
            };

            // project each vertex of contour to the plane
            List<vec3> fromContour = contours[fromIndex];
            List<vec3> toContour = new List<vec3>();
            int count = (int)fromContour.Count;
            for (int i = 0; i < count; ++i)
            {
                line.Direction = v1;
                line.Point = fromContour[i];// define line with direction and point
                point = plane.Intersect(line);  // find the intersection point
                toContour.Add(point);
            }
            return toContour;
        }

        private void transformFirstContour()
        {
            int pathCount = (int)path.Count;
            int vertexCount = (int)contour.Count;
            mat4 matrix;

            if (pathCount > 0)
            {
                // transform matrix
                if (pathCount > 1)
                /*matrix = glm.lookAt(new vec3(0, 0, 0), path[1]-path[0], new vec3(0, 1, 0));*/
                {
                    var normal = new vec3(0, 1, 0);
                    var target = path[1] - path[0];
                    var gamma = MathF.Acos(glm.dot(target, normal) / (Sqrt(glm.dot(normal, normal)) * Sqrt(glm.dot(target, target))));
                    var right = glm.cross(normal, target);
                    matrix = glm.rotate(gamma, right);
                }

                var model = glm.translate(mat4.identity(), path[0]);
                /*var model = mat4.identity();*/

                // multiply matrix to the contour
                // NOTE: the contour vertices are transformed here
                //       MUST resubmit contour data if the path is resset to 0
                for (int i = 0; i < vertexCount; ++i)
                {
                    contour[i] = new vec3(model * matrix * new vec4(contour[i], 1));
                }
            }
        }

        private List<vec3> contour = new List<vec3>();
        private List<List<vec3>> normals = new();

        private void generatecont()
        {
            contours.Clear();
            normals.Clear();

            // path must have at least a point
            if (path.Count < 1)
                return;

            // rotate and translate the contour to the first path point
            transformFirstContour();
            contours.Add(contour);
            normals.Add(computeContourNormal(0));

            // project contour to the plane at the next path point
            int count = (int)path.Count;
            for (int i = 1; i < count; ++i)
            {
                contours.Add(proj(i - 1, i));
                normals.Add(computeContourNormal(i));
            }
        }

        private List<vec3> computeContourNormal(int pathIndex)
        {
            // get current contour and center point
            var contour = contours[pathIndex];
            vec3 center = path[pathIndex];

            List<vec3> contourNormal = new List<vec3>();
            vec3 normal;
            for (int i = 0; i < (int)contour.Count; ++i)
            {
                normal = glm.normalize(contour[i] - center);
                contourNormal.Add(normal);
            }

            return contourNormal;
        }
    }
}