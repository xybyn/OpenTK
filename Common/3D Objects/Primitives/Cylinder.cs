using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTKProject;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.MathF;

namespace Common
{
    public class Cylinder : SceneObject
    {
        private List<float> GetCircle(float r, int sectorCount)
        {
            float sectorStep = 2 * MathF.PI / sectorCount;
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
        private vec3 _color;

        private List<float> vertices = new List<float>();
        private List<float> normals = new List<float>();

        List<vec3> calcnorm(int height, int width, List<vec3> vertices)
        {
            var norms = new List<vec3>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == height - 1)
                    {
                        if (j == width - 1)
                        {
                            var p1 = vertices[(i * height + j)];
                            var p2 = vertices[(i * height + j - 1)];
                            var p3 = vertices[((i - 1) * height + j)];
                            var n = glm.normalize(glm.cross(p3 - p1, p2 - p1));
                            norms.Add(n);
                        }
                        else
                        {
                            var p1 = vertices[(i * height + j)];
                            var p2 = vertices[((i - 1) * height + j + 1)];
                            var p3 = vertices[((i - 1) * height + j)];
                            var n = glm.normalize(glm.cross(p2 - p1, p3 - p1));
                            norms.Add(n);
                        }
                    }
                    else if ((j + 1) == width)
                    {
                        var p1 = vertices[(i * height + j)];
                        var p2 = vertices[((i + 1) * height + j - 1)];
                        var p3 = vertices[(i * height + j - 1)];

                        var n = glm.normalize(glm.cross(p3 - p1, p2 - p1));

                        norms.Add(n);
                    }
                    else
                    {
                        var p1 = vertices[(i * height + j)];
                        var p2 = vertices[(i * height + j + 1)];
                        var p3 = vertices[((i + 1) * height + j)];

                        var n = glm.normalize(glm.cross(p3 - p1, p2 - p1));

                        norms.Add(n);
                    }
                }
            }
            return norms;
        }


        public Cylinder(float radius, float height, int sectorCount = 20)
        {
            // clear memory of prev arrays
            var unitVertices = GetCircle(1, sectorCount);

            // put side vertices to arrays
            for (int i = 0; i < 2; ++i)
            {
                float h = -height / 2.0f + i * height; // z value; -h/2 to h/2
                float t = 1.0f - i; // vertical tex coord; 1 to 0

                for (int j = 0, k = 0; j <= sectorCount; ++j, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k + 2];
                    float uz = unitVertices[k + 1];
                    // position vector
                    vertices.Add(ux * radius); // vx
                    vertices.Add(h); // vz
                    vertices.Add(uy * radius); // vy
                    // normal vector
                    normals.Add(ux); // nx
                    normals.Add(uz); // nz// t
                    normals.Add(uy); // ny
                }
            }

            // the starting index for the base/top surface
            //NuOTE: it is used for generating indices later
            int baseCenterIndex = (int)vertices.Count() / 3;
            int topCenterIndex = baseCenterIndex + sectorCount + 1; // include center vertex

            // put base and top vertices to arrays
            for (int i = 0; i < 2; ++i)
            {
                float h = -height / 2.0f + i * height; // z value; -h/2 to h/2
                float nz = -1 + i * 2; // z value of normal; -1 to 1

                // center point
                vertices.Add(0);
                vertices.Add(h);
                vertices.Add(0);
                normals.Add(0);
                normals.Add(nz);
                normals.Add(0);

                for (int j = 0, k = 0; j < sectorCount; ++j, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k + 1];
                    // position vector
                    vertices.Add(ux * radius); // vx
                    vertices.Add(h); // vz
                    vertices.Add(uy * radius); // vy
                    // normal vector
                    normals.Add(0); // nx
                    normals.Add(nz); // nz// t
                    normals.Add(0); // ny
                }
            }

            indices = new();
            int k1 = 0; // 1st vertex index at base
            int k2 = sectorCount + 1; // 1st vertex index at top

            // indices for the side surface
            for (int i = 0; i < sectorCount; ++i, ++k1, ++k2)
            {
                // 2 triangles per sector
                // k1 => k1+1 => k2
                indices.Add((uint)k1);
                indices.Add((uint)k1 + 1);
                indices.Add((uint)k2);

                // k2 =>Addk2+1
                indices.Add((uint)k2);
                indices.Add((uint)k1 + 1);
                indices.Add((uint)k2 + 1);
            }

            // indices for the base surface
            //NOTE: baseCenterIndex and topCenterIndices are pre-computed during vertex generation
            //      please see the previous code snippet
            for (int i = 0, k = baseCenterIndex + 1; i < sectorCount; ++i, ++k)
            {
                if (i < sectorCount - 1)
                {
                    indices.Add((uint)baseCenterIndex);
                    indices.Add((uint)k + 1);
                    indices.Add((uint)k);
                }
                else // last triangle
                {
                    indices.Add((uint)baseCenterIndex);
                    indices.Add((uint)baseCenterIndex + 1);
                    indices.Add((uint)k);
                }
            }

            // indices for the top surface
            for (int i = 0, k = topCenterIndex + 1; i < sectorCount; ++i, ++k)
            {
                if (i < sectorCount - 1)
                {
                    indices.Add((uint)topCenterIndex);
                    indices.Add((uint)k);
                    indices.Add((uint)k + 1);
                }
                else // lastAdd
                {
                    indices.Add((uint)topCenterIndex);
                    indices.Add((uint)k);
                    indices.Add((uint)topCenterIndex + 1);
                }
            }

           InitializeVAO_VBO_EBO(vertices.ToArray(), normals.ToArray(), indices.ToArray());
        }

        private VAO vao;
        private List<uint> indices;
        
    }
}