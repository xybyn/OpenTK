// unset

using GlmNet;
using OpenTKProject;

namespace Common
{
    public class Vector3D : SceneObject
    {
        private Cylinder _cylinder;
        public string AxisName { get; }
        public Vector3D(string axisName, vec3 color)
        {
            AxisName = axisName;
            _cylinder = new Cylinder(0.05f, 1);
            _cylinder.AttachTo(this);
            _cylinder.Material.Color = color;
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            _cylinder.Draw(ref view, ref projection);
        }
    }
}