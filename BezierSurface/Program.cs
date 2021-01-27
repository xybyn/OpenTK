using Common;
using Common._3D_Objects;
using Common.Drawers.Settings;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using Utils;

namespace BezierSurface
{
    class BezierSurface : Window3D
    {
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
                var sphere = new Sphere(0.1f);
                sphere.Material.Color = new vec3(0.5f, 0, 0);
                sphere.TranslateWorld(point);
                toDraw.Add(sphere);
            }
            var bezierSurface = new Surface(
                (u, o) =>
                {
                    var n =(int) MathF.Sqrt(points.Length)-1;
                    var m = n;
                    var result = new vec3(0);
                    for (int i = 0; i <= n; i++)
                    { 
                        for (int j = 0; j <=m; j++)
                        {
                            result += points[i * 4 + j] * AppUtils.Bernshtein(n, i, u) * AppUtils.Bernshtein(m, j, o);
                        }
                    }
                    return result;
                },
                new SurfaceDrawer3DSettings()
                {
                    MaxX = 1, MinX = 0, MaxZ = 1, MinZ = 0,NumberOfPartitions = 60
                }, invertNormals:true
            );
            //bezierSurface.ShowNormals = true;
            AddGrid();
            AddMainCoordinatesAxis();
            toDraw.Add(bezierSurface);
        }
    }
    class Program
    {
        static void Main(string[] args)
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