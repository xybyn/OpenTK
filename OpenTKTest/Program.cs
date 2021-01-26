using Common;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace OpenTKTest
{
    internal class Program
    {
        private class TestWindow : Window3D
        {
            public TestWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
            {
                var cube = new Cube();
                cube.TranslateWorld(new vec3(0, 1, 0));
                cube.Material.Color = new vec3(1, 0, 0);
                toDraw.Add(cube);
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