// unset

using Common.Drawers;
using Common.Extensions;
using Common.MathAbstractions;
using Common.Misc;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.MathF;

namespace Common.Colliders
{//TODO: FIX local translation
    public class AxisAlignedBoxCollider : Collider
    {
        private readonly vec3 _leftNearBottomPoint;
        private readonly vec3 _rightFarTopPoint;

        public vec3 LeftNearBottomPoint => new vec3(ParentModelWithoutRotation*new vec4(_leftNearBottomPoint, 1));
        public vec3 RightFarTopPoint => new vec3(ParentModelWithoutRotation*new vec4(_rightFarTopPoint, 1));


        public AxisAlignedBoxCollider(vec3 leftNearBottomPoint, vec3 rightFarTopPoint)
        {
            _leftNearBottomPoint = leftNearBottomPoint;
            _rightFarTopPoint = rightFarTopPoint;

            var horizontalDistance = _rightFarTopPoint.x - _leftNearBottomPoint.x;
            var verticalDistance = _rightFarTopPoint.y - _leftNearBottomPoint.y;
            var depthDistance = _rightFarTopPoint.z - _leftNearBottomPoint.z;
            var v = new List<vec3>
            {
                _leftNearBottomPoint,
                _leftNearBottomPoint + new vec3(0, 0, depthDistance),
                _leftNearBottomPoint + new vec3(horizontalDistance, 0, depthDistance),
                _leftNearBottomPoint + new vec3(horizontalDistance, 0, 0),
                
                _leftNearBottomPoint + new vec3(0, verticalDistance, 0),
                _leftNearBottomPoint + new vec3(0, verticalDistance, depthDistance),
                _leftNearBottomPoint + new vec3(horizontalDistance, verticalDistance, depthDistance),
                _leftNearBottomPoint + new vec3(horizontalDistance, verticalDistance, 0),
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

            if (RightFarTopPoint.x <= otherBox.LeftNearBottomPoint.x || LeftNearBottomPoint.x >= otherBox.RightFarTopPoint.x)
            {
                result = null;
                return false;
            }
            if (RightFarTopPoint.y <= otherBox.LeftNearBottomPoint.y || LeftNearBottomPoint.y >= otherBox.RightFarTopPoint.y)
            {
                result = null;
                return false;
            }
            if (RightFarTopPoint.z <= otherBox.LeftNearBottomPoint.z || LeftNearBottomPoint.z >= otherBox.RightFarTopPoint.z)
            {
                result = null;
                return false;
            }
            
            var minX = Min( Abs(otherBox.LeftNearBottomPoint.x-RightFarTopPoint.x),Abs(LeftNearBottomPoint.x - otherBox.RightFarTopPoint.x));
            var minY = Min( Abs(otherBox.LeftNearBottomPoint.y-RightFarTopPoint.y),Abs(LeftNearBottomPoint.y - otherBox.RightFarTopPoint.y));
            var minZ = Min( Abs(otherBox.LeftNearBottomPoint.z-RightFarTopPoint.z),Abs(LeftNearBottomPoint.z - otherBox.RightFarTopPoint.z));
            int side;
            if (minY >= minX && minX <= minZ)
            {
                //determines colliding from left or right side
                side = -MathF.Sign(otherBox.LeftNearBottomPoint.x - LeftNearBottomPoint.x);
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
                side = -MathF.Sign(otherBox.LeftNearBottomPoint.y - LeftNearBottomPoint.y);
                result = new AxisAlignedBoxHit
                {
                    Offset = new vec3(0, side * minY, 0),
                    Normal = new vec3(0, side, 0)
                };
                return true;
            }

            //determines colliding from near or far side
            side = -MathF.Sign(otherBox.LeftNearBottomPoint.z - LeftNearBottomPoint.z);
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