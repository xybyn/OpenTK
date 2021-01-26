using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace Common
{
    public class Window2D : WindowBase
    {
        public Window2D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        private readonly List<Drawer2DBase> _drawers = new List<Drawer2DBase>();

        public void AddDrawer(Drawer2DBase newDrawer)
        {
            _drawers.Add(newDrawer);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            foreach (Drawer2DBase drawer in _drawers)
            {
                drawer.Initialize();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            mat4 model = mat4.identity();
            foreach (Drawer2DBase drawer in _drawers)
            {
                drawer.Draw();
            }
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        public event Action<Vector2> OnLeftButtonMouseClicked;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                OnLeftButtonMouseClicked?.Invoke(MousePosition);
            }

            base.OnMouseDown(e);
        }
    }
}