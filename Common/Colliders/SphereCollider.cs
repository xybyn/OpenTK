// unset

using GlmNet;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace Common.Colliders
{
    public class SphereCollider : Collider
    {
        public SphereCollider()
        {
            float r = 1;
            var sections = 20;
            var step = 2 * PI /( sections-1);
            float angle = 0;
            var vertices = new List<vec3>();
            for (int i = 0; i < sections; i++)
            {
                var x = r*Cos(angle);
                var z = r*Sin(angle);
                vertices.Add(new vec3(x, 0, z));
                angle += step;
            }
            var indices = new List<uint>();
            for (uint i = 0; i < vertices.Count; i++)
            {
                indices.Add(i);
                indices.Add((i+1)%((uint)vertices.Count));
            }
            angle = 0;
            for (int i = 0; i < sections; i++)
            {
                var x = r*Cos(angle);
                var y = r*Sin(angle);
                vertices.Add(new vec3(x, y, 0));
                angle += step;
            }

            for (uint i = (uint)vertices.Count/2; i < vertices.Count; i++)
            {
                indices.Add(i);
                if(i+1 == vertices.Count)
                    indices.Add((uint)vertices.Count/2);
                else 
                    indices.Add((i+1));
            }
            
            InitializeVAO_VBO_EBO(vertices.ToSingleArray(), indices.ToArray());
            Material.Color = new vec3(0, 1, 0);
        }
        
        public override bool IntersectsRay(vec3 rayDirection, vec3 rayOrigin, out float result)
        {
            var radius = 1;
            vec3 k = rayOrigin - Pos;
            var b = glm.dot(k, rayDirection);
            var c = glm.dot(k, k) - radius * radius;
            var d = b * b - c;
            if (d >= 0)
            {
                var sqrtfd = Sqrt(d);
                var t1 = -b + sqrtfd;
                var t2 = -b - sqrtfd;

                var mint = MathF.Min(t1, t2);
                var maxt = MathF.Min(t1, t2);

                float t = (mint >= 0) ? mint : maxt;
                result = t;
                return true;
            }
            result = 0;
            return false;
        }
    }
}