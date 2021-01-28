using Common.Colliders;
using Common.Extensions;
using Common.MathAbstractions;
using Common.Misc;
using GlmNet;
using System;
using System.Collections.Generic;

namespace Common.Drawers
{
    public class Line3D : Collider
    {
        public Line3D(Line line)
        {
            var vertices = new List<vec3>
            {
                line.Point,line.Point+ line.Direction
            };
            var indices = new uint[]
            {
                0, 1
            };
            InitializeVAO_VBO_EBO(vertices.ToSingleArray(), indices);
        }

        public override bool IsIntersectsRay(vec3 rayDirection, vec3 rayOrigin, out RaycastHit result)
        {
            throw new NotImplementedException();
        }
    }
}