// unset

using Common._3D_Objects;
using GlmNet;

namespace Common
{
    public class Vector3D : SceneObject3D
    {
        private readonly Cylinder _cylinder;
        private const float CYLINDER_RADIUS = 0.05f;
        private const float CYLINDER_HEIGHT = 1;

        public Vector3D(string axisName)
        {
            AxisName = axisName;
            _cylinder = new Cylinder(CYLINDER_RADIUS, CYLINDER_HEIGHT);
            _cylinder.AttachTo(this);
            _cylinder.Material = Material;
        }

        public string AxisName { get; }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            _cylinder.Draw(ref view, ref projection);
        }
    }
}