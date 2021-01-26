using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace BezierCurve
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "Bezier Curve 3D"
            };

            using (var window = new BezierCurve3DWindow(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}