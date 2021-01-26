// unset

using Common.Colliders;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.MathF;

namespace Common
{
    public class BoxCollider : Collider
    {
        private List<PlaneCollider> _planeColliders = new List<PlaneCollider>();
        public BoxCollider()
        {
            _planeColliders.Add(new PlaneCollider(new vec3(0, 1, 0), new vec3(0, 0.5f, 0)));
            _planeColliders.Add(new PlaneCollider(new vec3(0, -1, 0), new vec3(0, -0.5f, 0)));
            _planeColliders.Add(new PlaneCollider(new vec3(1, 0, 0), new vec3(0.5f, 0, 0)));
            _planeColliders.Add(new PlaneCollider(new vec3(-1, 0, 0), new vec3(-0.5f, 0, 0)));
            _planeColliders.Add(new PlaneCollider(new vec3(0, 0, 1), new vec3(0, 0, 0.5f)));
            _planeColliders.Add(new PlaneCollider(new vec3(0, 0, -1), new vec3(0, 0, -0.5f)));

            foreach (var planeCollider in _planeColliders)
            {
                planeCollider.AttachTo(this);
            }
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            foreach (PlaneCollider planeCollider in _planeColliders)
            {
                planeCollider.Draw(ref view, ref projection);
            }
        }

        public override bool IntersectsRay(vec3 rayDirection, vec3 rayOrigin, out float result)
        {
            var results = new List<float>();
            foreach (var planeCollider in _planeColliders)
            {
                if (planeCollider.IntersectsRay(rayDirection, rayOrigin, out var res))
                {
                    results.Add(res);
                }
            }
            if (results.Count == 0)
            {
                result = 0;
                return false;
            }
            var min = results.Min();
            result = min;
            return true;
        }
    }
}