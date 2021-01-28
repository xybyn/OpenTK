using Common;
using Common.Colliders;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace AxisAlignedBoxToAxisAlignedBoxCollisionDetection
{
    internal class CollisionDetectionWindow3D : Window3D
    {
        private readonly AxisAlignedBoxCollider _aaboxCollider;
        private readonly AxisAlignedBoxCollider _aaboxCollider2;
        private readonly Cylinder _cylinder;

        public CollisionDetectionWindow3D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            AddGrid();
            AddMainCoordinatesAxis();

            _cylinder = new Cylinder(0.5f, 2);
            var sphere = new Sphere(1);
            var sphere2 = new Sphere(0.3f);
            sphere2.TranslateWorld(new vec3(-2, 0, 0));
            sphere.AttachTo(_cylinder);
            _aaboxCollider = new AxisAlignedBoxCollider(new vec3(-0.5f), new vec3(1f));
            _aaboxCollider.AttachTo(sphere);

            _aaboxCollider2 = new AxisAlignedBoxCollider(new vec3(-0.5f), new vec3(0.5f));
            toDraw.Add(_cylinder);
            toDraw.Add(_aaboxCollider);
            toDraw.Add(_aaboxCollider2);
            toDraw.Add(sphere2);
        }

        private float angle = 0;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _cylinder.TranslateWorld(_cylinder.WorldPosition + new vec3(0, (float)args.Time, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _cylinder.TranslateWorld(_cylinder.WorldPosition - new vec3(0, (float)args.Time, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _cylinder.TranslateWorld(_cylinder.WorldPosition + new vec3((float)args.Time, 0, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _cylinder.TranslateWorld(_cylinder.WorldPosition - new vec3((float)args.Time, 0, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                _cylinder.TranslateWorld(_cylinder.WorldPosition + new vec3(0, 0, (float)args.Time));
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                _cylinder.TranslateWorld(_cylinder.WorldPosition - new vec3(0, 0, (float)args.Time));
            }
            var V = _cylinder.WorldPosition - lastPosition;
            _cylinder.RotateLocal(angle, new vec3(1));
            _cylinder.ScaleLocal(new vec3(1 - MathF.Sin((float)GLFW.GetTime()) / 2));
            angle += (float)args.Time;
            lastPosition = _cylinder.WorldPosition;
            if (_aaboxCollider.IsIntersectsAxisAlignedBox(_aaboxCollider2, out var result))
            {
                _cylinder.Material.Color = new vec3(0, 1, 0);
                _cylinder.TranslateWorld(_cylinder.WorldPosition + result.Offset);
                Console.WriteLine(result.Normal);
            }
            else
                _cylinder.Material.Color = new vec3(0.5f);
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