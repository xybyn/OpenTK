// unset

using Common.Drawers.Settings;
using Common.Extensions;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Utils;

namespace Common._3D_Objects
{
    public class Surface : SceneObject3D
    {
        private readonly SurfaceDrawer3DSettings _settings;

        public Surface(Func<float, float, float> f, SurfaceDrawer3DSettings settings)
            : this((x, z) => new vec3(x, f(x, z), z), settings)
        {
        }

        public Surface(Func<float, float, vec3> f, SurfaceDrawer3DSettings settings, bool invertNormals = false)
        {
            _settings = settings;
            BuildNewSurface(f, invertNormals);
        }

        //x = i*(sizeOfVector+numofparts) + j*sizeOfVector
        public void SetPositionForVertex(int i, int j, vec3 newPosition)
        {
            vbo.Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(((i * (3 + _settings.NumberOfPartitions)) + (j * 3)) * sizeof(float)),
                sizeof(float), ref newPosition.x);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(((i * (3 + _settings.NumberOfPartitions)) + (j * 3) + 1) * sizeof(float)),
                sizeof(float), ref newPosition.y);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(((i * (3 + _settings.NumberOfPartitions)) + (j * 3) + 2) * sizeof(float)),
                sizeof(float), ref newPosition.z);
        }

        private void BuildNewSurface(Func<float, float, vec3> f, bool invertNormals)
        {
            ClearBuffers();

            int steps = _settings.NumberOfPartitions;

            List<vec3> vertices = InitializeVertices(f);
            List<uint> indices = AppUtils.GetIndices(steps, steps);

            List<vec3> normals = CalculateNormals(steps, steps, vertices, invertNormals ? -1 : 1);
            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }

        public void SetNewVertices(Func<float, float, vec3> f, bool invertNormals)
        {
            List<vec3> vertices = InitializeVertices(f);
            List<vec3> normals = CalculateNormals(_settings.NumberOfPartitions, _settings.NumberOfPartitions, vertices, invertNormals ? -1 : 1);
            float[] singleArray = vertices.ToSingleArray();
            float[] normalsArray = normals.ToSingleArray();
            vbo.Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, sizeof(float) * 3 * vertices.Count, singleArray);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * 3 * vertices.Count), sizeof(float) * 3 * normals.Count,
                normalsArray);
        }

        public void SetNewVertices(Func<float, float, float> f, bool invertNormals)
        {
            SetNewVertices((x, z) => new vec3(x, f(x, z), z), invertNormals);
        }

        private List<vec3> InitializeVertices(Func<float, float, vec3> f)
        {
            List<vec3> vertices = new(_settings.NumberOfPartitions * _settings.NumberOfPartitions);
            int steps = _settings.NumberOfPartitions;
            float tStep = (_settings.MaxX - _settings.MinX) / (steps - 1);
            float uStep = (_settings.MaxZ - _settings.MinZ) / (steps - 1);
            float t = _settings.MinX;

            for (int i = 0; i < steps; ++i)
            {
                float u = _settings.MinZ;
                for (int j = 0; j < steps; ++j)
                {
                    vertices.Add(f(t, u));
                    u += uStep;
                }

                t += tStep;
            }
            return vertices;
        }

        private List<vec3> CalculateNormals(int height, int width, List<vec3> vertices, float inverted = 1)
        {
            List<vec3> normals = new(vertices.Capacity);

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (i == height - 1)
                    {
                        if (j == width - 1)
                        {
                            vec3 p1 = vertices[(i * height) + j];
                            vec3 p2 = vertices[(i * height) + j - 1];
                            vec3 p3 = vertices[((i - 1) * height) + j];
                            vec3 n = glm.normalize(inverted * glm.cross(p2 - p1, p3 - p1));
                            normals.Add(n);
                        }
                        else
                        {
                            vec3 p1 = vertices[(i * height) + j];
                            vec3 p2 = vertices[((i - 1) * height) + j + 1];
                            vec3 p3 = vertices[((i - 1) * height) + j];
                            vec3 n = glm.normalize(inverted * glm.cross(p3 - p1, p2 - p1));
                            normals.Add(n);
                        }
                    }
                    else if (j + 1 == width)
                    {
                        vec3 p1 = vertices[(i * height) + j];
                        vec3 p2 = vertices[((i + 1) * height) + j - 1];
                        vec3 p3 = vertices[(i * height) + j - 1];

                        vec3 n = glm.normalize(inverted * glm.cross(p2 - p1, p3 - p1));

                        normals.Add(n);
                    }
                    else
                    {
                        vec3 p1 = vertices[(i * height) + j];
                        vec3 p2 = vertices[(i * height) + j + 1];
                        vec3 p3 = vertices[((i + 1) * height) + j];

                        vec3 n = glm.normalize(inverted * glm.cross(p2 - p1, p3 - p1));

                        normals.Add(n);
                    }
                }
            }
            return normals;
        }
    }
}