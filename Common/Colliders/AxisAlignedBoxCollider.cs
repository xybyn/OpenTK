// unset

using Common.Extensions;
using Common.Misc;
using GlmNet;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace Common.Colliders
{//TODO: FIX local translation
    public class AxisAlignedBoxCollider : Collider
    {
        private readonly vec3 _minPoint;
        private readonly vec3 _maxPoint;

        public vec3 MinPoint => new(ParentModelWithoutRotation * new vec4(_minPoint, 1));
        public vec3 MaxPoint => new(ParentModelWithoutRotation * new vec4(_maxPoint, 1));

        public AxisAlignedBoxCollider(vec3 minPoint, vec3 maxPoint)
        {
            _minPoint = minPoint;
            _maxPoint = maxPoint;

            var horizontalDistance = _maxPoint.x - _minPoint.x;
            var verticalDistance = _maxPoint.y - _minPoint.y;
            var depthDistance = _maxPoint.z - _minPoint.z;
            var v = new List<vec3>
            {
                _minPoint,
                _minPoint + new vec3(0, 0, depthDistance),
                _minPoint + new vec3(horizontalDistance, 0, depthDistance),
                _minPoint + new vec3(horizontalDistance, 0, 0),

                _minPoint + new vec3(0, verticalDistance, 0),
                _minPoint + new vec3(0, verticalDistance, depthDistance),
                _minPoint + new vec3(horizontalDistance, verticalDistance, depthDistance),
                _minPoint + new vec3(horizontalDistance, verticalDistance, 0),
            };

            var indices = new uint[]
            {
                0, 1, 1, 2, 2, 3, 3, 0,

                4, 5, 5, 6, 6, 7, 7, 4,

                0, 4, 5, 1, 6, 2, 7, 3
            };
            Material.Color = new vec3(0, 1, 0);
            InitializeVAO_VBO_EBO(v.ToSingleArray(), indices);
        }

        public override mat4 ParentModel
        {
            get
            {
                return ParentModelWithoutRotation;
            }
        }

        public bool IsIntersectsAxisAlignedBox(AxisAlignedBoxCollider otherBox, out AxisAlignedBoxHit result)
        {
            //x checking

            if (MaxPoint.x <= otherBox.MinPoint.x || MinPoint.x >= otherBox.MaxPoint.x)
            {
                result = null;
                return false;
            }
            if (MaxPoint.y <= otherBox.MinPoint.y || MinPoint.y >= otherBox.MaxPoint.y)
            {
                result = null;
                return false;
            }
            if (MaxPoint.z <= otherBox.MinPoint.z || MinPoint.z >= otherBox.MaxPoint.z)
            {
                result = null;
                return false;
            }

            var minX = Min(Abs(otherBox.MinPoint.x - MaxPoint.x), Abs(MinPoint.x - otherBox.MaxPoint.x));
            var minY = Min(Abs(otherBox.MinPoint.y - MaxPoint.y), Abs(MinPoint.y - otherBox.MaxPoint.y));
            var minZ = Min(Abs(otherBox.MinPoint.z - MaxPoint.z), Abs(MinPoint.z - otherBox.MaxPoint.z));
            int side;
            if (minY >= minX && minX <= minZ)
            {
                //determines colliding from left or right side
                side = -MathF.Sign(otherBox.MinPoint.x - MinPoint.x);
                result = new AxisAlignedBoxHit
                {
                    Offset = new vec3(side * minX, 0, 0),
                    Normal = new vec3(side, 0, 0)
                };
                return true;
            }
            if (minX >= minY && minY <= minZ)
            {
                //determines colliding from top or bottom side
                side = -MathF.Sign(otherBox.MinPoint.y - MinPoint.y);
                result = new AxisAlignedBoxHit
                {
                    Offset = new vec3(0, side * minY, 0),
                    Normal = new vec3(0, side, 0)
                };
                return true;
            }

            //determines colliding from near or far side
            side = -MathF.Sign(otherBox.MinPoint.z - MinPoint.z);
            result = new AxisAlignedBoxHit
            {
                Offset = new vec3(0, 0, side * minZ),
                Normal = new vec3(0, 0, side)
            };
            return true;
        }

        public override bool IsIntersectsRay(vec3 rayDirection, vec3 rayOrigin, out RaycastHit result)
        {
            result = null;
            return true;
        }
    }
}