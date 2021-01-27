﻿// unset

using Common.Extensions;
using Common.Misc;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using static System.MathF;

namespace Common.Colliders
{
    public class SphereCollider : Collider
    {
        private readonly float _radius;
        public SphereCollider(float radius)
        {
            _radius = radius;
            var sections = 20;
            var step = 2 * PI / (sections - 1);
            float angle = 0;
            var vertices = new List<vec3>();
            for (int i = 0; i < sections; i++)
            {
                var x = radius * Cos(angle);
                var z = radius * Sin(angle);
                vertices.Add(new vec3(x, 0, z));
                angle += step;
            }
            var indices = new List<uint>();
            for (uint i = 0; i < vertices.Count; i++)
            {
                indices.Add(i);
                indices.Add((i + 1) % ((uint)vertices.Count));
            }
            angle = 0;
            for (int i = 0; i < sections; i++)
            {
                var x = radius * Cos(angle);
                var y = radius * Sin(angle);
                vertices.Add(new vec3(x, y, 0));
                angle += step;
            }

            for (uint i = (uint)vertices.Count / 2; i < vertices.Count; i++)
            {
                indices.Add(i);
                if (i + 1 == vertices.Count)
                    indices.Add((uint)vertices.Count / 2);
                else
                    indices.Add((i + 1));
            }

            InitializeVAO_VBO_EBO(vertices.ToSingleArray(), indices.ToArray());
            Material.Color = new vec3(0, 1, 0);
        }

        public override bool IsIntersectsRay(vec3 rayDirection, vec3 rayOrigin, out RaycastHit result)
        {
            vec3 k = rayOrigin - WorldPosition;
            var b = glm.dot(k, rayDirection);
            var c = glm.dot(k, k) - _radius * _radius;
            var d = b * b - c;
            if (d >= 0)
            {
                var sqrtfd = Sqrt(d);
                var t1 = -b + sqrtfd;
                var t2 = -b - sqrtfd;

                var mint = MathF.Min(t1, t2);
                var maxt = MathF.Min(t1, t2);

                float t = (mint >= 0) ? mint : maxt;
                var intersectedPoint = rayDirection * t + rayOrigin;
                result = new RaycastHit()
                {
                    Point =intersectedPoint,
                    Normal = glm.normalize(intersectedPoint-WorldPosition)
                };
                return true;
            }
            result = null;
            return false;
        }

        public bool IsIntersectsPlane(PlaneCollider planeCollider, vec3 movingDirection, out object o)
        {
            var P1 = WorldPosition;
            var V = glm.normalize(movingDirection);

            var D = -glm.dot(planeCollider.Plane.Normal, planeCollider.Plane.D);
            var L = planeCollider.Plane.Normal;

            var upper = -(glm.dot(L, P1) + D);
            var lower =  (glm.dot(L, V) + D);
            var t = upper / lower;
            if(!float.IsNaN(t))
            {
                var r = glm.dot(planeCollider.Plane.Normal, P1) + D;
                var C = P1 + t * V - r * planeCollider.Plane.Normal;
                Console.WriteLine(C);
            }
            o = null;
            return false;
        }
        
        public bool IsIntersectsSphere(SphereCollider planeCollider, out RaycastHit result)
        {
            var p = WorldPosition - planeCollider.WorldPosition;
            var dist = Sqrt(glm.dot(p, p));

            if (dist <= _radius + planeCollider._radius)
            {
                p = glm.normalize(p);
                var point = planeCollider.WorldPosition + planeCollider._radius * p;
                var normal = glm.normalize(point-planeCollider.WorldPosition);
                result = new RaycastHit()
                {
                    Point = point, Normal = normal
                };
                return true;
            }
            result = null;
            return false;
        }
    }
}