// unset
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKProject;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace Common
{
    public class Plane
    {
        public Vector3 normal { get; set; }
        public Vector3 d { get; set; }

        public Vector3 intersect(Line line)
        {
            var p = line.point;
            var v = line.direction;

            var t = -(Vector3.Dot(p, normal) - Vector3.Dot(normal, d)) / (Vector3.Dot(normal, v));
            return p + t * v;
        }
    }

    public class Pipe
    {
        private float[] vertices;
        private List<List<Vector3>> contours = new();
        private List<Vector3> path;
        private Shader normalShader;

        public Pipe()
        {
            normalShader = new Shader(@"Shaders\normal.vert", @"Shaders\normal.frag", @"Shaders\normal.geom");

            contour = circle(1, 40);

            path = new List<Vector3>();
            var stepCount = 40;
            var step = 2 * MathF.PI / stepCount;
            var ra = 8f;
            var an = 0f;
            var z = 0f;
            for (int i = 0; i <= stepCount; i++)
            {
                var ve = new Vector3(ra * Cos(an), z, ra * Sin(an));
                path.Add(ve);
                an += step;
                z += step;
                ra -= step;
            }

            generatecont();

            vertices = new float[contours.Count * contours[0].Count * 3];

            for (int i = 0; i < contours.Count; i++)
            {
                var cont = contours[i];
                for (int j = 0; j < cont.Count; j++)
                {
                    vertices[j * 3 + 0 + i * cont.Count * 3] = cont[j].X;
                    vertices[j * 3 + 1 + i * cont.Count * 3] = cont[j].Y;
                    vertices[j * 3 + 2 + i * cont.Count * 3] = cont[j].Z;
                }
            }
            var no = new float[normals.Count * normals[0].Count * 3];
            for (int i = 0; i < normals.Count; i++)
            {
                var norma = normals[i];
                for (int j = 0; j < norma.Count; j++)
                {
                    no[j * 3 + 0 + i * norma.Count * 3] = norma[j].X;
                    no[j * 3 + 1 + i * norma.Count * 3] = norma[j].Y;
                    no[j * 3 + 2 + i * norma.Count * 3] = norma[j].Z;
                }
            }
            var ind = new List<int>();
            for (int i = 0; i < contours.Count - 1; i++)
            {
                var currentCont = contours[(int)i];
                var other = contours[(int)i + 1];

                for (int j = 0; j < currentCont.Count - 1; j++)
                {
                    int curr = (i * currentCont.Count + j);
                    int currR = (i * currentCont.Count + j + 1);
                    int o = ((i + 1) * currentCont.Count + j);
                    int or = ((i + 1) * currentCont.Count + j + 1);

                    ind.Add(curr);
                    ind.Add(currR);
                    ind.Add(o);

                    ind.Add(o);
                    ind.Add(currR);
                    ind.Add(or);
                }
            }
            indices = ind.ToArray();
            var vbo = new VBO<float>(vertices, sizeof(float) * vertices.Length + sizeof(float) * no.Length, new[]
            {
                new Subdata<float>
                {
                    Data = vertices, Index = 0, SizeInBytes = sizeof(float) * vertices.Length
                },
                new Subdata<float>
                {
                    Data = no, Index = sizeof(float) * vertices.Length, SizeInBytes = sizeof(float) * no.Length
                }
            });
            var ebo = new EBO(indices);
            vao = new VAO<float>(vbo, ebo, new[]
            {
                new VertexAttribPointer
                {
                    SizeInBytes = 3,
                    Index = 0,
                    OffsetInBytes = 0,
                    Normalize = false,
                    StrideInBytes = 3 * sizeof(float),
                    Type = VertexAttribPointerType.Float
                },
                new VertexAttribPointer
                {
                    SizeInBytes = 3,
                    Index = 1,
                    OffsetInBytes = sizeof(float) * vertices.Length,
                    Normalize = false,
                    StrideInBytes = 3 * sizeof(float),
                    Type = VertexAttribPointerType.Float
                },
            });

            shader = new Shader(@"Shaders\Surface\surface.vert", @"Shaders\Surface\surface.frag");
        }

        private int[] indices;
        private VAO<float> vao;
        private Shader shader;

        private List<Vector3> circle(float radius, int steps)
        {
            List<Vector3> points = new List<Vector3>();
            if (steps < 2) return points;

            const float PI2 = 2 * MathF.PI;
            float x, y, a;
            for (int i = 0; i <= steps; ++i)
            {
                a = PI2 / steps * i;
                x = radius * Cos(a);
                y = radius * Sin(a);
                points.Add(new Vector3(x, 0, y));
            }
            return points;
        }

        private List<Vector3> proj(int fromIndex, int toIndex)
        {
            Vector3 v1, v2, normal, point;

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
                normal = normal,
                d = path[toIndex]
            };

            // project each vertex of contour to the plane
            List<Vector3> fromContour = contours[fromIndex];
            List<Vector3> toContour = new List<Vector3>();
            int count = (int)fromContour.Count;
            for (int i = 0; i < count; ++i)
            {
                line.direction = v1;
                line.point = fromContour[i];// define line with direction and point
                point = plane.intersect(line);  // find the intersection point
                toContour.Add(point);
            }
            return toContour;
        }

        private void transformFirstContour()
        {
            int pathCount = (int)path.Count;
            int vertexCount = (int)contour.Count;
            Matrix4 matrix = Matrix4.Identity;

            if (pathCount > 0)
            {
                // transform matrix
                if (pathCount > 1)
                /*matrix = glm.lookAt(new vec3(0, 0, 0), path[1]-path[0], new vec3(0, 1, 0));*/
                {
                    var normal = new Vector3(0, 1, 0);
                    var target = path[1] - path[0];
                    var gamma = MathF.Acos(Vector3.Dot(target, normal)
                                           / (Sqrt(Vector3.Dot(normal, normal)) *
                                              Sqrt(Vector3.Dot(target, target))));
                    var right = Vector3.Cross(normal, target);
                    matrix = Matrix4.CreateFromQuaternion(new Quaternion(right, gamma));
                }

                var model = Matrix4.CreateTranslation(path[0]);
                /*var model = mat4.identity();*/

                // multiply matrix to the contour
                // NOTE: the contour vertices are transformed here
                //       MUST resubmit contour data if the path is resset to 0
                for (int i = 0; i < vertexCount; ++i)
                {
                    contour[i] = new Vector3(model * matrix * new Vector4(contour[i]));
                }
            }
        }

        private List<Vector3> contour = new List<Vector3>();
        private List<List<Vector3>> normals = new();

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

        private List<Vector3> computeContourNormal(int pathIndex)
        {
            // get current contour and center point
            var contour = contours[pathIndex];
            Vector3 center = path[pathIndex];

            List<Vector3> contourNormal = new List<Vector3>();
            Vector3 normal;
            for (int i = 0; i < (int)contour.Count; ++i)
            {
                normal = (contour[i] - center).Normalized();
                contourNormal.Add(normal);
            }

            return contourNormal;
        }

        public void Draw(ref Matrix4 view, ref Matrix4 projection)
        {
            /*GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);*/
            /*pointdrawer.Draw(ref view, ref projection);*/
            Matrix4 model = Matrix4.Identity;
            shader.Use();
            shader.SetMat4("projection", ref projection);
            shader.SetMat4("model", ref model);
            shader.SetMat4("view", ref view);
            vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            normalShader.Use();
            normalShader.SetMat4("projection", ref projection);
            normalShader.SetMat4("model", ref model);
            normalShader.SetMat4("view", ref view);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}