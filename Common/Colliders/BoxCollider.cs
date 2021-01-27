// unset

using Common.Misc;
using GlmNet;
using System.Collections.Generic;
using System.Linq;

namespace Common.Colliders
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

        public override bool IsIntersectsRay(vec3 rayDirection, vec3 rayOrigin, out RaycastHit result)
        {
            var results = new List<RaycastHit>();
            foreach (var planeCollider in _planeColliders)
            {
                if (planeCollider.IsIntersectsRay(rayDirection, rayOrigin, out var res))
                {
                    results.Add(res);
                }
            }
            if (results.Count == 0)
            {
                result = null;
                return false;
            }
            var min = 
                (from r in results 
                orderby glm.dot(r.Point - rayOrigin, r.Point - rayOrigin) 
                select r).First();
            result = min;
            return true;
        }
    }
}