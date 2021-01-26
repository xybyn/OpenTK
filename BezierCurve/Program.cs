using Common;
using GlmNet;
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
                Title = "Bezier Curve"
            };
            var pointDrawer2D = new PointDrawer2D(null);
            var bezierCurveDrawer = new BezierCurveDrawer(null);
            using (var window = new Window2D(settings, nativeWindowSettings))
            {
                window.OnLeftButtonMouseClicked += (e) =>
                 {
                     var p = e;
                     var Width = 800;
                     var Height = 600;
                     float x = (p.X - Width / 2f) / (Width / 2f);
                     float y = -(p.Y - Height / 2f) / (Height / 2f);
                     var p2 = new vec2(x, y);

                     pointDrawer2D.AddPoint(p2);
                     bezierCurveDrawer.AddPoint(new vec3(p2.x, p2.y, 0));
                 };
                window.AddDrawer(pointDrawer2D);
                window.AddDrawer(bezierCurveDrawer);
                window.Run();
            }
        }
    }
}