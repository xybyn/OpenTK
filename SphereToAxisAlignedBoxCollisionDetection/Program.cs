using Common;
using Common.Colliders;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace SphereToAxisAlignedBoxCollisionDetection
{
    internal class CollisionDetectionWindow3D : Window3D
    {
        private readonly AxisAlignedBoxCollider _aaboxCollider2;
        private readonly Sphere _sphere;
            SphereCollider sphereCollider;

        public CollisionDetectionWindow3D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            AddGrid();
            AddMainCoordinatesAxis();

            _sphere = new Sphere(0.5f);
            _sphere.TranslateWorld(new vec3(-2, 0, 0));
            sphereCollider = new SphereCollider(0.5f);
            sphereCollider.AttachTo(_sphere);
            toDraw.Add(sphereCollider);
            _aaboxCollider2 = new AxisAlignedBoxCollider(new vec3(-0.5f), new vec3(0.5f));
            toDraw.Add(_sphere);
            toDraw.Add(_aaboxCollider2);
        }

        private float angle = 0;

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
                _sphere.TranslateWorld(_sphere.WorldPosition + new vec3(0, 0, (float)args.Time));
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                _sphere.TranslateWorld(_sphere.WorldPosition - new vec3(0, 0, (float)args.Time));
            }
            var V = _sphere.WorldPosition - lastPosition;
            //_sphere.RotateLocal(angle, new vec3(1));
            //_sphere.ScaleLocal(new vec3(1 - MathF.Sin((float)GLFW.GetTime()) / 2));
            angle += (float)args.Time;
            lastPosition = _sphere.WorldPosition;
            if (sphereCollider.IsIntersectsAxisAlignedBox(_aaboxCollider2, out var result))
            {
                var reflected = 2 * result.Normal * (glm.dot(result.Normal, V));
                _sphere.Material.Color = new vec3(0, 1, 0);
                Console.WriteLine(result.Normal);
                _sphere.TranslateWorld(_sphere.WorldPosition - reflected);
                /*_sphere.TranslateWorld(_sphere.WorldPosition + result.Offset);
                Console.WriteLine(result.Normal);*/
            }
            else
                _sphere.Material.Color = new vec3(0.5f);
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