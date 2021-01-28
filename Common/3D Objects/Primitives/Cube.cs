// unset

using Common._3D_Objects;
using GlmNet;
using System.Collections.Generic;

namespace Common
{
    public class Cube : SceneObject3D
    {
        private readonly List<SceneObject3D> toDraw = new();

        public Cube()
        {
            Plane3D topPlane = new Plane3D(new vec3(0, 1, 0), new vec3(0, 0.5f, 0));
            Plane3D bottomPlane = new Plane3D(new vec3(0, -1, 0), new vec3(0, -0.5f, 0));
            Plane3D rightPlane = new Plane3D(new vec3(1, 0, 0), new vec3(0.5f, 0, 0));
            Plane3D leftPlane = new Plane3D(new vec3(-1, 0, 0), new vec3(-0.5f, 0, 0));
            Plane3D backPlane = new Plane3D(new vec3(0, 0, 1), new vec3(0, 0, 0.5f));
            Plane3D frontPlane = new Plane3D(new vec3(0, 0, -1), new vec3(0, 0, -0.5f));

            topPlane.Material = Material;
            bottomPlane.Material = Material;
            rightPlane.Material = Material;
            leftPlane.Material = Material;
            backPlane.Material = Material;
            frontPlane.Material = Material;

            topPlane.AttachTo(this);
            bottomPlane.AttachTo(this);
            rightPlane.AttachTo(this);
            leftPlane.AttachTo(this);
            backPlane.AttachTo(this);
            frontPlane.AttachTo(this);

            toDraw.Add(topPlane);
            toDraw.Add(bottomPlane);
            toDraw.Add(rightPlane);
            toDraw.Add(leftPlane);
            toDraw.Add(backPlane);
            toDraw.Add(frontPlane);
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            foreach (SceneObject3D o in toDraw)
            {
                o.Draw(ref view, ref projection);
            }
        }
    }
}