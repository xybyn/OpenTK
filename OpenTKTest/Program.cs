using Common;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using static System.MathF;

namespace OpenTKTest
{
    internal class Program
    {
        private class TestWindow : Window3D
        {
                Cube cube;
            public TestWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
            {
                var sphere = new Sphere(0.3f);
                sphere.TranslateWorld(new vec3(0, 0, 2));
                cube = new Cube();
                cube.TranslateWorld(new vec3(0, 0, 0));
                cube.ScaleLocal(new vec3(1));
                cube.Material.Color = new vec3(0.5f, 0, 0);
                cube.AttachTo(sphere);
                toDraw.Add(cube);
                toDraw.Add(sphere);
                cube.TranslateWorld(new vec3(0));
                AddGrid();
                AddMainCoordinatesAxis();
            }

            protected override void OnUpdateFrame(FrameEventArgs args)
            {
                base.OnUpdateFrame(args);

                if (KeyboardState.IsKeyDown(Keys.W))
                {
                    cube.TranslateWorld(cube.WorldPosition + new vec3(0, 0,(float)args.Time ));
                }
                if (KeyboardState.IsKeyDown(Keys.S))
                {
                    cube.TranslateWorld(cube.WorldPosition - new vec3(0, 0,(float)args.Time ));
                }
                if (KeyboardState.IsKeyDown(Keys.A))
                {
                    cube.TranslateWorld(cube.WorldPosition + new vec3((float)args.Time , 0,0));
                }
                if (KeyboardState.IsKeyDown(Keys.D))
                {
                    cube.TranslateWorld(cube.WorldPosition - new vec3((float)args.Time , 0,0));
                }
                if (KeyboardState.IsKeyDown(Keys.Q))
                {
                    cube.TranslateWorld(new vec3(0));
                }
                if (KeyboardState.IsKeyDown(Keys.E))
                {
                    cube.TranslateLocal(new vec3(0));
                }
                
                if (KeyboardState.IsKeyDown(Keys.Up))
                {
                    cube.Parent.TranslateWorld(cube.Parent.WorldPosition + new vec3(0, 0,(float)args.Time ));
                }
                if (KeyboardState.IsKeyDown(Keys.Down))
                {
                    cube.Parent.TranslateWorld(cube.Parent.WorldPosition - new vec3(0, 0,(float)args.Time ));
                }
                if (KeyboardState.IsKeyDown(Keys.Left))
                {
                    cube.Parent.TranslateWorld(cube.Parent.WorldPosition + new vec3((float)args.Time , 0,0));
                }
                if (KeyboardState.IsKeyDown(Keys.Right))
                {
                    cube.Parent.TranslateWorld(cube.Parent.WorldPosition - new vec3((float)args.Time , 0,0));
                }
                angle += (float)(args.Time);
                cube.Parent.RotateLocal(angle, new vec3(0, 1, 0));
                cube.Parent.ScaleLocal(new vec3(1.2f - Sin((float)GLFW.GetTime())));
                cube.Parent.TranslateWorld(new vec3(Sin((float)GLFW.GetTime()), 0, Cos((float)GLFW.GetTime())));
                Console.WriteLine($"loc : {cube.LocalPosition}");
                Console.WriteLine($"wor : {cube.WorldPosition}");
            }

            private float angle = 0;
        }

        private static void Main(string[] args)
        {
            var settings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "Surface"
            };

            using (var window = new TestWindow(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}