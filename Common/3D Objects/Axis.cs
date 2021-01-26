// unset

using GlmNet;
using OpenTKProject;
using System;

namespace Common
{
    public class Axis : SceneObject
    {
        private SceneObject x;
        private SceneObject y;
        private SceneObject z;
        private Sphere sphere;
        public Axis()
        {
            float height = 1;
            float offset = height / 2;
            sphere = new Sphere(new vec3(0, 0, 0), 0.06f);
            sphere.Material.Color = new vec3(0.5f, 0.5f, 0.5f);
            
            x = new Vector3D("x",new vec3(0.5f, 0, 0));
            x.AttachTo(this);
            x.Rotate(MathF.PI/2, new vec3(0, 0, 1));
            x.TranslateGlobal(new vec3(offset, 0, 0));
            
            y = new Vector3D("y",new vec3(0, 0.5f, 0));
            y.AttachTo(this);
            y.TranslateGlobal(new vec3(0, offset, 0));
            y.Material.Color = new vec3(0, 0.5f, 0);
            
            z = new Vector3D("z",new vec3(0, 0, 0.5f));
            z.AttachTo(this);
            z.Rotate(MathF.PI/2, new vec3(1, 0, 0));
            z.TranslateGlobal(new vec3(0, 0, offset));
            z.Material.Color = new vec3(0, 0, 0.5f);
        }
        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            x.Draw(ref view, ref projection);
            y.Draw(ref view, ref projection);
            z.Draw(ref view, ref projection);
            sphere.Draw(ref view, ref projection);
        }
    }
}