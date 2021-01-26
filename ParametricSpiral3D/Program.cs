using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;

namespace ParametricSpiral
{
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

            using (var window = new ParametricSpiral3DWindow(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}