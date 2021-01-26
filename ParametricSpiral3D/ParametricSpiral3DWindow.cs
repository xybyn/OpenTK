// unset

using Common;
using Common._3D_Objects;
using Common.Windows;
using GlmNet;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace ParametricSpiral
{
    public class ParametricSpiral3DWindow : Window3D
    {
        public ParametricSpiral3DWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings,
            nativeWindowSettings)
        {
            Func<List<vec3>> spiralFunction = () =>
            {
                List<vec3> list = new List<vec3>();
                float minT = 0f;
                float maxT = 4 * PI;
                float step = (maxT - minT) / (100 - 1);
                float t = minT;
                for (int i = 0; i < 100; i++)
                {
                    float x = t * Cos(t) / 5f;
                    float y = t * Sin(t) / 5f;
                    list.Add(new vec3(x, t / 5f, y));
                    t += step;
                }

                return list;
            };

            Pipe pipe = new Pipe(spiralFunction);
            pipe.Material.Color = new vec3(0.6f, 0, 0);
            AddGrid();
            AddMainCoordinatesAxis();
            toDraw.Add(pipe);
        }
    }
}