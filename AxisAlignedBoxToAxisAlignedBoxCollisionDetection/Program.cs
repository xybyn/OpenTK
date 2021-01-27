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
    class CollisionDetectionWindow3D : Window3D
    {
        readonly AxisAlignedBoxCollider aaboxCollider;
        readonly AxisAlignedBoxCollider aaboxCollider2;
        Cylinder cylinder;
      
        public CollisionDetectionWindow3D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            AddGrid();
            AddMainCoordinatesAxis();

            cylinder = new Cylinder(0.5f, 2);
            //cylinder.ScaleLocal(new vec3(2));
            //.TranslateWorld(new vec3(3, 0, 0));
            var sphere = new Sphere(1);
            var sphere2 = new Sphere(0.3f);
            sphere2.TranslateWorld(new vec3(-2, 0, 0));
            sphere.AttachTo(cylinder);
            aaboxCollider = new(new vec3(-0.5f), new vec3(1f));
            aaboxCollider.AttachTo(sphere);
            
            aaboxCollider2 = new(new vec3(-0.5f), new vec3(0.5f));
            aaboxCollider2.TranslateWorld(new vec3(-2, 0, 0));
            toDraw.Add(cylinder);
            toDraw.Add(aaboxCollider);
            toDraw.Add(aaboxCollider2);
            toDraw.Add(sphere2);
        }
        private float angle = 0;
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                cylinder.TranslateWorld(cylinder.WorldPosition + new vec3(0, (float)args.Time,0 ));
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                cylinder.TranslateWorld(cylinder.WorldPosition - new vec3(0, (float)args.Time,0 ));
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                cylinder.TranslateWorld(cylinder.WorldPosition + new vec3((float)args.Time , 0,0));
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                cylinder.TranslateWorld(cylinder.WorldPosition - new vec3((float)args.Time , 0,0));
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                cylinder.TranslateWorld(cylinder.WorldPosition + new vec3(0, 0,(float)args.Time ));
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                cylinder.TranslateWorld(cylinder.WorldPosition - new vec3(0, 0,(float)args.Time ));
            }
            var V = cylinder.WorldPosition - lastPosition;
             cylinder.RotateLocal(angle, new vec3(1));
             cylinder.ScaleLocal(new vec3(1 - MathF.Sin((float)GLFW.GetTime())/2));
            angle += (float)args.Time;
            lastPosition= cylinder.WorldPosition;
            if (aaboxCollider.IsIntersectsAxisAlignedBox(aaboxCollider2, out var result))
            {
                cylinder.Material.Color = new vec3(0, 1, 0);
                cylinder.TranslateWorld(cylinder.WorldPosition+result.Offset);
                Console.WriteLine(result.Normal);
                
            }else
                cylinder.Material.Color = new vec3(0.5f);
        }

        private vec3 lastPosition = new vec3(0);
    }
    class Program
    {
        static void Main(string[] args)
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