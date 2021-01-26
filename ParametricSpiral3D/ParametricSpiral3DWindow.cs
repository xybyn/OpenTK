// unset

using Common;
using Common._3D_Objects;
using Common.Windows;
using GlmNet;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
            List<vec3> SpiralFunction()
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
            }

            Pipe staticPipe = new Pipe((Func<List<vec3>>)SpiralFunction);
            staticPipe.TranslateWorld(new vec3(3, 0, 0));
            staticPipe.Material.Color = new vec3(0.6f, 0, 0);
            AddGrid();
            AddMainCoordinatesAxis();
            toDraw.Add(staticPipe);

            _dynamicPipe = new Pipe();
            toDraw.Add(_dynamicPipe);
        }
        readonly List<vec3> _list = new List<vec3>();
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            List<vec3> SpiralFunction()
            {
                
                float minT = 0f;
                float maxT = 4 * PI;
                float step = (maxT - minT) / (100 - 1);
                float t = minT;
                for (int i = 0; i < 100; i++)
                {
                    float x = (float)(t * Cos((float)(t + GLFW.GetTime() * 5)) / 5f);
                    float y = (t+ Sin( (float)GLFW.GetTime())) * Sin((float)(t + GLFW.GetTime() * 5)) / 5f;
                    _list.Add(new vec3(x, t / 5f, y));
                    t += step;
                }

                return _list;
            }

            _dynamicPipe.SetNewFunctions(SpiralFunction);
        }

        private Pipe _dynamicPipe;
    }
}