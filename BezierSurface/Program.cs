using Common;
using Common._3D_Objects;
using Common.Drawers.Settings;
using Common.Physics3D;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using Utils;

namespace BezierSurface
{
    internal class BezierSurface : Window3D
    {
            Surface bezierSurface;
            private List<AxisManipulator> manipulators = new();
        public BezierSurface(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            var points = new vec3[16]
            {
                new vec3(-3, 0, -3), new vec3(-1, 0, -3),new vec3(1, 0, -3),new vec3(3, -1, -3),
                new vec3(-3, -1, -1), new vec3(-1, 3, -1),new vec3(1, 3, -1),new vec3(3, 0, -1),
                new vec3(-3, -1, 1), new vec3(-1, 3, 1),new vec3(1, 3, 1),new vec3(3, 0, 1),
                new vec3(-3, 0,  3), new vec3(-1, 0,  3),new vec3(1, 0,  3),new vec3(3, -1,  3)
            };
            foreach (var point in points)
            {
                var man = new AxisManipulator();
                man.TranslateWorld(point);
                OnMouseMoving += man.OnMouseMove;
                manipulators.Add(man);
                toDraw.Add(man);
            }
            bezierSurface = new Surface(
                (u, o) =>
                {
                    var n = (int)MathF.Sqrt(points.Length) - 1;
                    var m = n;
                    var result = new vec3(0);
                    for (int i = 0; i <= n; i++)
                    {
                        for (int j = 0; j <= m; j++)
                        {
                            result += points[i * 4 + j] * AppUtils.Bernshtein(n, i, u) * AppUtils.Bernshtein(m, j, o);
                        }
                    }
                    return result;
                },
                new SurfaceDrawer3DSettings()
                {
                    MaxX = 1,
                    MinX = 0,
                    MaxZ = 1,
                    MinZ = 0,
                    NumberOfPartitions = 60
                }, invertNormals: true
            );
            //bezierSurface.ShowNormals = true;
            AddGrid();
            AddMainCoordinatesAxis();
            toDraw.Add(bezierSurface);
        }
        private float y = 0;
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

                foreach (var axisManipulator in manipulators)
                {
                    axisManipulator.CheckCollision(Physics3D.ScreenPointToWorldRay(
                        new vec2(MousePosition.X, MousePosition.Y), 
                        new vec2(Size.X, Size.Y), 
                        projection, view),position );
                }
                bezierSurface.SetNewVertices((u, o) =>
                {
                    var n = (int)MathF.Sqrt(manipulators.Count) - 1;
                    var m = n;
                    var result = new vec3(0);
                    for (int i = 0; i <= n; i++)
                    {
                        for (int j = 0; j <= m; j++)
                        {
                            result += manipulators[i * 4 + j].WorldPosition * AppUtils.Bernshtein(n, i, u) * AppUtils.Bernshtein(m, j, o);
                        }
                    }
                    return result;
                }, true);
            }
            OnMouseMoving?.Invoke(e, MouseState.IsButtonDown(MouseButton.Left));
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "Surface"
            };

            using (var window = new BezierSurface(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}