using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTKProject;
using System;
using System.Collections.Generic;
using Utils;

namespace Common
{
    public class Surface : SceneObject
    {
        private readonly SurfaceDrawer3DSettings _settings;
        private List<vec3> InitializeVertices(Func<float, float, vec3> f)
        {
            List<vec3> vertices = new List<vec3>();
            int steps = _settings.NumberOfPartitions;
            float tStep = (_settings.MaxX-_settings.MinX)/ (steps - 1);
            float uStep = (_settings.MaxZ-_settings.MinZ)/ (steps - 1);
            float t = _settings.MinX;
            
            for (int i = 0; i < steps; i++)
            {
                float u = _settings.MinZ;
                for (int j = 0; j < steps; j++)
                {
                    vertices.Add(f(t, u));
                    u += uStep;
                }

                t += tStep;
            }
            return vertices;
        }

        List<vec3> CalculateNormals(int height, int width, List<vec3> vertices, float inverted = 1)
        {
            var normals = new List<vec3>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == height - 1)
                    {
                        if (j == width - 1)
                        {
                            var p1 = vertices[(i * height + j)];
                            var p2 = vertices[(i * height + j-1)];
                            var p3 = vertices[((i - 1) * height + j)];
                            var n = glm.normalize(inverted*glm.cross(p2 - p1, p3 - p1));
                            normals.Add(n);
                        }
                        else
                        {
                            var p1 = vertices[(i * height + j)];
                            var p2 = vertices[((i - 1) * height + j + 1)];
                            var p3 = vertices[((i - 1) * height + j)];
                            var n = glm.normalize(inverted*glm.cross(p3 - p1, p2 - p1));
                            normals.Add(n);
                        }
                    }
                    else if ((j + 1) == width)
                    {
                        var p1 = vertices[(i * height + j)];
                        var p2 = vertices[((i + 1) * height + j-1)];
                        var p3 = vertices[(i * height + j - 1)];
                    
                        var n = glm.normalize(inverted*glm.cross(p2-p1, p3-p1));
                    
                        normals.Add(n);
                    }
                    else
                    {
                        var p1 = vertices[(i * height + j)];
                        var p2 = vertices[(i * height + j + 1)];
                        var p3 = vertices[((i + 1) * height + j)];
                    
                        var n = glm.normalize(inverted*glm.cross(p2-p1, p3-p1));
                    
                        normals.Add(n);
                    }
                }
            }
            return normals;
        }
        
        public Surface(Func<float, float, float> f, SurfaceDrawer3DSettings settings)
        :this((x, z) => new vec3(x, f(x, z), z), settings)
        {}
        public Surface(Func<float, float, vec3> f, SurfaceDrawer3DSettings settings, bool invertNormals = false)
        {
            this._settings = settings;
            int steps = settings.NumberOfPartitions;

            var vertices = InitializeVertices(f);
            var indices = AppUtils.GetIndices(steps, steps);
            
            var normals = CalculateNormals(steps, steps, vertices, invertNormals?-1:1);
            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }
    }
}