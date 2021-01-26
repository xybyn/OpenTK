// unset

using Common;
using GlmNet;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace ParametricSpiral
{
    public class ParametricSpiral3DWindow : Window3D
    {
        public ParametricSpiral3DWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            Func<List<vec3>> spiralFunction = () =>
            {
                var list = new List<vec3>();
                var minT = 0f;
                var maxT = 4 * PI;
                var step = (maxT - minT) / (100 - 1);
                var t = minT;
                for (int i = 0; i < 100; i++)
                {

                    var x = t*Cos(t)/5f;
                    var y = t*Sin(t)/5f;
                    list.Add(new vec3(x, t/5f, y));
                    t += step;
                }
                
                return list;
            };

            var pipe = new Pipe(spiralFunction);
            pipe.Material.Color = new vec3(0.6f, 0, 0);
            AddGrid();
            AddMainCoordinatesAxis();
            ToDraw.Add(pipe);
        }
    }
}