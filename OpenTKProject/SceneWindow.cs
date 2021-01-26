// unset

using Common;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKProject
{
    public class SceneWindow : Window3D
    {
        public SceneWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            pipe = new Pipe();
            base.OnLoad();
        }

        private Pipe pipe;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            ClearBuffers();

            //draw here...
            pipe.Draw(ref _view, ref _projection);
            base.OnRenderFrame(args);
        }
    }
}