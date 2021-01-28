using Common;
using Common.Colliders;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SphereToFinitePlaneCollisionDetection
{
    internal class CollisionDetectionWindow3D : Window3D
    {
        private readonly Sphere _sphere;
        private readonly SphereCollider _sphereCollider;
        private readonly FinitePlaneCollider _finitePlaneCollider;

        public CollisionDetectionWindow3D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            AddGrid();
            AddMainCoordinatesAxis();

            _sphere = new Sphere(0.45f);
            _sphereCollider = new SphereCollider(0.5f);
            _sphereCollider.AttachTo(_sphere);
            _sphere.TranslateWorld(new vec3(0, 1, 1));

            var plane = new Plane3D(new vec3(0, 1, 0), new vec3(0));
            plane.TranslateWorld(new vec3(-4, 0, 0));
            plane.RotateLocal(30, new vec3(1));
            plane.ScaleLocal(new vec3(6, 1, 6));
            _finitePlaneCollider = new FinitePlaneCollider(new vec3(0, 1, 0), new vec3(0));
            _finitePlaneCollider.AttachTo(plane);
            toDraw.Add(_sphere);
            toDraw.Add(_sphereCollider);
            toDraw.Add(plane);
            toDraw.Add(_finitePlaneCollider);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _sphere.TranslateWorld(_sphere.WorldPosition + new vec3(0, (float)args.Time, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _sphere.TranslateWorld(_sphere.WorldPosition - new vec3(0, (float)args.Time, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _sphere.TranslateWorld(_sphere.WorldPosition + new vec3((float)args.Time, 0, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _sphere.TranslateWorld(_sphere.WorldPosition - new vec3((float)args.Time, 0, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                _sphere.TranslateWorld(new vec3(0));
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                _sphere.TranslateLocal(new vec3(0));
            }
            var V = _sphere.WorldPosition - lastPosition;
            if (_sphereCollider.IsIntersectsWithFinitePlane(_finitePlaneCollider, V, out var result))
            {
                var reflected = 2 * result.Normal * (glm.dot(result.Normal, V));

                var p = new Sphere(0.1f);
                p.TranslateWorld(result.Point);
                _sphere.TranslateWorld(_sphere.WorldPosition - reflected);
                toDraw.Add(p);
                _sphere.Material.Color = new vec3(0, 1, 0);
            }
            else
                _sphereCollider.Parent.Material.Color = new vec3(0.5f);
            lastPosition = _sphere.WorldPosition;
        }

        private vec3 lastPosition = new vec3(0);
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "Collision Detection"
            };

            using (var window = new CollisionDetectionWindow3D(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}