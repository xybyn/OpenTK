// unset

using Common.Extensions;
using Common.Misc;
using GlmNet;
using System;
using System.Collections.Generic;
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
                    Point = intersectedPoint,
                    Normal = glm.normalize(intersectedPoint - WorldPosition)
                };
                return true;
            }
            result = null;
            return false;
        }

        public bool IsIntersectsWithInfinitePlane(InfinitePlaneCollider infinitePlaneCollider, vec3 movingDirection, out RaycastHit result)
        {
            var P1 = WorldPosition;
            var V = movingDirection;

            var D = -glm.dot(infinitePlaneCollider.Plane.Normal, infinitePlaneCollider.Plane.D);
            var L = infinitePlaneCollider.Plane.Normal;

            var distanceBetweenSphereAndPlane = glm.dot(infinitePlaneCollider.Plane.Normal, P1) + D;
            if (distanceBetweenSphereAndPlane <= _radius)
            {
                var dir = -1 * infinitePlaneCollider.Plane.Normal * distanceBetweenSphereAndPlane;
                var point = WorldPosition + dir;

                result = new RaycastHit()
                {
                    Point = point,
                    Normal = infinitePlaneCollider.Plane.Normal
                };
                return true;
            }
            result = null;
            return false;
        }
        
         public bool IsIntersectsAxisAlignedBox(AxisAlignedBoxCollider otherBox, out AxisAlignedBoxHit result)
        {

            var x =Max(otherBox.MinPoint.x, Min(WorldPosition.x, otherBox.MaxPoint.x));
            var y = Max(otherBox.MinPoint.y, Min(WorldPosition.y, otherBox.MaxPoint.y));
            var z =Max(otherBox.MinPoint.z, Min(WorldPosition.z, otherBox.MaxPoint.z));

            //Console.WriteLine(new vec3(x, y, z));
            var distance = Sqrt((x - WorldPosition.x) * (x - WorldPosition.x) +
                                      (y - WorldPosition.y) * (y - WorldPosition.y) +
                                      (z - WorldPosition.z) * (z - WorldPosition.z));

            if (distance >= _radius)
            {
                result = null;
                return false;
            }
            int side;

            
            x *= -MathF.Sign(otherBox.MaxPoint.x - WorldPosition.x);
            y *= -MathF.Sign(otherBox.MaxPoint.y - WorldPosition.y);
            z *= -MathF.Sign(otherBox.MaxPoint.z - WorldPosition.z);
            
            
            bool isXMax = false;
            bool isYMax = false;
            bool isZMax = false;
            
            var max = x;
            if (y > max)
            {
                max = y;
            }
            if (z > y)
            {
                max = z;
                
            }

            if (x >= max)
                isXMax = true;

            if (y >= max)
                isYMax = true;
            if (z >= max)
                isZMax = true;
            
            if (isYMax)
            {
                //y
                side = -MathF.Sign(otherBox.MaxPoint.y - WorldPosition.y);
                result = new AxisAlignedBoxHit
                {
                    Offset = new vec3(0, side*y, 0),
                    Normal = new vec3(0, side, 0)
                };
                return true;
            }
            if (isZMax)
            {
                //z
                side = -MathF.Sign(otherBox.MaxPoint.z - WorldPosition.z);
                result = new AxisAlignedBoxHit
                {
                    Offset = new vec3(0, 0, side*z),
                    Normal = new vec3(0, 0, side)
                };
                return true;
            }
            
            //x

                //determines colliding from left or right side
                side = -MathF.Sign(otherBox.MaxPoint.x - WorldPosition.x);
                result = new AxisAlignedBoxHit
                {
                    Offset = new vec3(side * x, 0, 0),
                    Normal = new vec3(side, 0, 0)
                };
                return true;
                
        }
        public bool IsIntersectsWithFinitePlane(FinitePlaneCollider infinitePlaneCollider, vec3 movingDirection, out RaycastHit result)
        {
            var P1 = WorldPosition;
            var V = movingDirection;

            var D = -glm.dot(infinitePlaneCollider.Plane.Normal, infinitePlaneCollider.Plane.D);
            var L = infinitePlaneCollider.Plane.Normal;

            var distanceBetweenSphereAndPlane = glm.dot(infinitePlaneCollider.Plane.Normal, P1) + D;
            if (distanceBetweenSphereAndPlane <= _radius)
            {
                var dir = -1 * infinitePlaneCollider.Plane.Normal * distanceBetweenSphereAndPlane;
                var point = WorldPosition + dir;

                result = new RaycastHit()
                {
                    Point = point,
                    Normal = infinitePlaneCollider.Plane.Normal
                };
                return true;
            }
            result = null;
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
                var normal = glm.normalize(point - planeCollider.WorldPosition);
                result = new RaycastHit()
                {
                    Point = point,
                    Normal = normal
                };
                return true;
            }
            result = null;
            return false;
        }
    }
}