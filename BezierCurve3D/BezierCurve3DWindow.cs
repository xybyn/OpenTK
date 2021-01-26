// unset

using Common;
using Common._3D_Objects;
using GlmNet;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using Utils;
using static System.MathF;

namespace BezierCurve
{
    public class BezierCurve3DWindow : Window3D
    {
        public BezierCurve3DWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            var pipe = new Pipe(() =>
            {
                var result = new List<vec3>();
                var points = new List<vec3>();
                points.Add(new vec3(-4, 0, 1));
                points.Add(new vec3( 8, 8, 0));
                points.Add(new vec3(-8, 8, 0));
                points.Add(new vec3( 4, 0, -1));
   

                foreach (var point in points)
                {
                    var point3D = new Sphere(point, 0.2f);
                    point3D.Material.Color = new vec3(0.7f, 0, 0);
                    ToDraw.Add(point3D);
                }
                
                var divisions = 100;
                var step = 1 / (float)(divisions-1);
                float t = 0;
                for (int i = 0; i < divisions; i++)
                {
                    result.Add(AppUtils.Bezier(t, points));
                    t += step;
                }
                return result;
            });
            pipe.Material.Color = new vec3(0.7f);

            AddMainCoordinatesAxis();
            AddGrid();
            ToDraw.Add(pipe);
            
        }
    }
}