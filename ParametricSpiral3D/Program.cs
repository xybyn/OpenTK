// unset

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace ParametricSpiral
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GameWindowSettings settings = GameWindowSettings.Default;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(800, 600),
                Title = "Surface"
            };

            using (ParametricSpiral3DWindow window = new ParametricSpiral3DWindow(settings, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}