using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace SurfaceDemo
{
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

            using (var window = new SurfaceDemo3DWindow(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}