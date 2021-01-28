using Common;
using Common._3D_Objects;
using Common.Colliders;
using Common.Physics3D;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenTKTest
{
    internal class Program
    {
        private class TestWindow : Window3D
        {
            private Cube cube;

                AxisManipulator axisManipulator;
            public TestWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
            {

                axisManipulator = new AxisManipulator();
                axisManipulator.TranslateWorld(new vec3(1, 1, 0));
                toDraw.Add(axisManipulator);
                OnMouseMoving += axisManipulator.OnMouseMove;
                AddGrid();
                AddMainCoordinatesAxis();
            }

            protected override void OnUpdateFrame(FrameEventArgs args)
            {
                base.OnUpdateFrame(args);
                
               
            }
            public event Action<MouseMoveEventArgs, bool> OnMouseMoving;
            protected override void OnMouseMove(MouseMoveEventArgs e)
            {
                    base.OnMouseMove(e);
                    if (MouseState.IsButtonDown(MouseButton.Left))
                    {
                     
                            axisManipulator.CheckCollision(Physics3D.ScreenPointToWorldRay(
                                new vec2(MousePosition.X, MousePosition.Y), 
                                new vec2(Size.X, Size.Y), 
                                projection, view),position );
                        
                    }
                        OnMouseMoving?.Invoke(e, MouseState.IsButtonDown(MouseButton.Left));
            }

            protected override void OnMouseDown(MouseButtonEventArgs e)
            {
                base.OnMouseDown(e);

            }
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