// unset

using GlmNet;
using OpenTKProject;
using System.Collections.Generic;

namespace Common
{
    public class Cube : SceneObject
    {
        private List<SceneObject> toDraw = new();
        
        public Cube(vec3 position)
        {
            TranslateGlobal(position);
            var topPlane    = new Plane3D(new vec3(0, 1, 0), new vec3(0, 0.5f, 0));
            var bottomPlane = new Plane3D(new vec3(0, -1, 0), new vec3(0, -0.5f, 0));
            var rightPlane  = new Plane3D(new vec3(1, 0, 0), new vec3(0.5f, 0, 0));
            var leftPlane   = new Plane3D(new vec3(-1, 0, 0), new vec3(-0.5f, 0, 0));
            var backPlane   = new Plane3D(new vec3(0, 0, 1), new vec3(0, 0, 0.5f));
            var frontPlane  = new Plane3D(new vec3(0, 0, -1), new vec3(0, 0, -0.5f));
            
            topPlane   .AttachTo(this);
            bottomPlane.AttachTo(this);
            rightPlane .AttachTo(this);
            leftPlane  .AttachTo(this);
            backPlane  .AttachTo(this);
            frontPlane .AttachTo(this);
            
            toDraw.Add(topPlane   );
            toDraw.Add(bottomPlane);
            toDraw.Add(rightPlane );
            toDraw.Add(leftPlane  );
            toDraw.Add(backPlane  );
            toDraw.Add(frontPlane );
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            foreach (SceneObject o in toDraw)
            {
                o.Draw(ref view, ref projection);
            }
        }
    }
}