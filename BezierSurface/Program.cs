using Common;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;

namespace BezierSurface
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "Bezier Surface"
            };
            using (var window = new Window3D(settings, nativeWindowSettings))
            {
                var points = new PointDrawer3D(null);
                points.AddPoint(new vec3(-9, 0, -9));
                points.AddPoint(new vec3(-3, 0, -9));
                points.AddPoint(new vec3(3, 0, -9));
                points.AddPoint(new vec3(9, 0, -9));

                points.AddPoint(new vec3(-9, 0, 9));
                points.AddPoint(new vec3(-3, 0, 9));
                points.AddPoint(new vec3(3, 0, 9));
                points.AddPoint(new vec3(9, 0, 9));

                points.AddPoint(new vec3(-9, 0, -9));
                points.AddPoint(new vec3(-9, 0, -3));
                points.AddPoint(new vec3(-9, 0, 3));
                points.AddPoint(new vec3(-9, 0, 9));

                points.AddPoint(new vec3(9, 0, -9));
                points.AddPoint(new vec3(9, 0, -3));
                points.AddPoint(new vec3(9, 0, 3));
                points.AddPoint(new vec3(9, 0, 9));

                points.AddPoint(new vec3(-3, 14, -3));
                points.AddPoint(new vec3(3, 14, -3));
                points.AddPoint(new vec3(3, 14, 3));
                points.AddPoint(new vec3(-3, 14, 3));

                var initialpoints = new List<vec3>();
                foreach (var point in points.points)
                {
                    initialpoints.Add(point);
                }
                var steps = 60;
                var step = 1f / steps;
                float s = 0;
                for (int k = 0; k < steps; k++)
                {
                    float t = 0;
                    for (int l = 0; l < steps; l++)
                    {
                        points.AddPoint(Utils.AppUtils.BezierSurface(s, t, initialpoints));
                        t += step;
                    }

                    s += step;
                }
                window.Run();
            }
        }
    }
}