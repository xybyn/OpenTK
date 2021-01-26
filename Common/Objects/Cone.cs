// unset

using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTKProject;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using static System.MathF;

namespace Common
{
    public class Cone : SceneObject
    {
        public Cone()
        {
            float h = 1;
            float r = 4;
            int segments = 100;
            var step = 2 * PI / (segments-1);
            var normals = new List<vec3>();
            var vertices = new List<vec3>();
            var indices = new List<uint>();

            float angle = 0;
            for (int i = 1; i <= segments; i++)
            {
                var x = r * Cos(angle);
                var z = r * Sin(angle);
                vertices.Add(new vec3(x, 0, z));
                if (i % 2 == 0)
                {
                    vertices.Add(new vec3(0, h, 0));
                }
                angle += step;
            }
            
            indices = AppUtils.GetIndices(segments, 2);

            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }
    }
}