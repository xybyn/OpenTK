using GlmNet;
using System.Collections.Generic;
using static System.MathF;

namespace Utils
{
    public static class AppUtils
    {
        public static int Factorial(int n)
        {
            var factorial = 1;
            for (int i = 1; i <= n; i++)
                factorial *= i;

            return factorial;
        }

        public static List<float> GetCircle(float r, int sectorCount)
        {
            float sectorStep = 2 * PI / sectorCount;
            float sectorAngle; // radian

            List<float> unitCircleVertices = new();
            for (int i = 0; i <= sectorCount; ++i)
            {
                sectorAngle = i * sectorStep;
                unitCircleVertices.Add(r * Cos(sectorAngle)); // x
                unitCircleVertices.Add(0); // z
                unitCircleVertices.Add(r * Sin(sectorAngle)); // y
            }
            return unitCircleVertices;
        }

        public static vec3 Bezier(float t, List<vec3> points)
        {
            int n = points.Count - 1;
            vec3 result = Bernshtein(n, 0, t) * points[0];
            for (int k = 1; k <= n; k++)
            {
                result += Bernshtein(n, k, t) * points[k];
            }

            return result;
        }

        public static vec3 BezierSurface(float s, float t, List<vec3> points)
        {
            return Bezier(t, points) * Bezier(s, points);
        }

        public static float Bernshtein(int n, int k, float t)
        {
            /*var lower = (Factorial(k) * Factorial(n - k));

            var c = Factorial(n) / (Factorial(k) * Factorial(n - k));*/
            return C(n, k) * Pow(t, k) * Pow(1 - t, n - k);
        }

        public static float[] CalculateNormals(List<vec3> vertices, int numberOfPartitions)
        {
            List<vec3> normals = new();
            vec3 v1;
            vec3 v2;
            vec3 v3;
            var steps = (int)numberOfPartitions;
            for (int i = 0; i < steps - 1; i++)
                for (int j = 0; j < steps - 1; j++)
                {
                    v1 = vertices[i * steps + j];
                    v2 = vertices[i * steps + j + 1];
                    v3 = vertices[(i + 1) * steps + j];

                    vec3 normal = glm.cross(v2 - v1, v3 - v1);
                    normals.Add(glm.normalize(normal));

                    v1 = vertices[i * steps + j + 1];
                    v2 = vertices[(i + 1) * steps + j + 1];
                    v3 = vertices[(i + 1) * steps + j];

                    normal = glm.cross(v2 - v1, v3 - v1);
                    normals.Add(glm.normalize(normal));
                }
            var normalsArray = new float[normals.Count * 3];
            for (var i = 0; i < normals.Count; i++)
            {
                normalsArray[i * 3] = normals[i].x;
                normalsArray[i * 3 + 1] = normals[i].y;
                normalsArray[i * 3 + 2] = normals[i].z;

                /*
                normalsArray[i * 9 + 3] = normals[i].x;
                normalsArray[i * 9 + 4] = normals[i].y;
                normalsArray[i * 9 + 5] = normals[i].z;

                normalsArray[i * 9 + 6] = normals[i].x;
                normalsArray[i * 9 + 7] = normals[i].y;
                normalsArray[i * 9 + 8] = normals[i].z;*/
            }

            return normalsArray;
        }

        public static List<uint> GetIndices(int horizontalDivisions, int verticalDivisions)
        {
            List<uint> indices = new();
            for (int i = 0; i < verticalDivisions - 1; i++)
                for (int j = 0; j < horizontalDivisions - 1; j++)
                {
                    indices.Add((uint)(i * horizontalDivisions + j));
                    indices.Add((uint)(i * horizontalDivisions + j + 1));
                    indices.Add((uint)((i + 1) * horizontalDivisions + j));

                    indices.Add((uint)(i * horizontalDivisions + j + 1));
                    indices.Add((uint)((i + 1) * horizontalDivisions + j + 1));
                    indices.Add((uint)((i + 1) * horizontalDivisions + j));
                }

            return indices;
        }

        public static int C(int n, int k)
        {
            var pascal = new List<List<int>>();
            for (int i = 0; i < n + 1; i++)
            {
                pascal.Add(new List<int>());
                pascal[i].Add(1);
                for (int j = 1; j < i; j++)
                {
                    pascal[i].Add(pascal[i - 1][j - 1] + pascal[i - 1][j]);
                }
                if (i != 0)
                {
                    pascal[i].Add(1);
                }
            }
            return pascal[n][k];
        }
    }
}