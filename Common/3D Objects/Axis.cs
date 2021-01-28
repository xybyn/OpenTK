// unset

using GlmNet;
using System;

namespace Common._3D_Objects
{
    public class Axis : SceneObject3D
    {
        private readonly Sphere _sphere;
        private readonly SceneObject3D _xAxis;
        private readonly SceneObject3D _yAxis;
        private readonly SceneObject3D _zAxis;

        private const float HEIGHT = 1;
        private const float SPHERE_RADIUS = 0.06f;
        private const float COLOR_INTENSITY = 0.6f;

        public Axis()
        {
            float height = HEIGHT;
            float offset = height / 2;
            _sphere = new Sphere(SPHERE_RADIUS);
            _sphere.TranslateWorld(new vec3(0, 0, 0));
            _sphere.Material.Color = new vec3(COLOR_INTENSITY);

            _xAxis = new Vector3D("x");
            _xAxis.AttachTo(this);
            _xAxis.RotateLocal(MathF.PI / 2, new vec3(0, 0, 1));
            _xAxis.TranslateWorld(new vec3(offset, 0, 0));
            _xAxis.Material.Color = new vec3(COLOR_INTENSITY, 0, 0);

            _yAxis = new Vector3D("y");
            _yAxis.AttachTo(this);
            _yAxis.TranslateWorld(new vec3(0, offset, 0));
            _yAxis.Material.Color = new vec3(0, COLOR_INTENSITY, 0);

            _zAxis = new Vector3D("z");
            _zAxis.AttachTo(this);
            _zAxis.RotateLocal(MathF.PI / 2, new vec3(1, 0, 0));
            _zAxis.TranslateWorld(new vec3(0, 0, offset));
            _zAxis.Material.Color = new vec3(0, 0, COLOR_INTENSITY);
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            _xAxis.Draw(ref view, ref projection);
            _yAxis.Draw(ref view, ref projection);
            _zAxis.Draw(ref view, ref projection);
            _sphere.Draw(ref view, ref projection);
        }
    }
}