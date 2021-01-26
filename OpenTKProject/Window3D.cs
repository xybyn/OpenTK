using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using static System.MathF;
using Vector2 = OpenTK.Mathematics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace Common
{
    public class Window3D : WindowBase
    {
        private float _phi = 0;
        private float _ksi = 0;
        protected Matrix4 _projection;
        protected Matrix4 _view;
        protected Vector3 _position;
        protected Vector2 _lastPos;
        private float _distanceToTarget = 10;

        public Window3D(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings,
                nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Matrix4.CreatePerspectiveFieldOfView(MathF.PI / 4f, 4f / 3f, 0.1f, 100f, out _projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            Vector2 current = MousePosition;
            if (MouseState.IsButtonDown(MouseButton.Right))
            {
                _phi -= (_lastPos.X - current.X) * 0.4f;
                _ksi -= (_lastPos.Y - current.Y) * 0.4f;
            }
            var rad = MathF.PI / 180f;
            _position.X = _distanceToTarget * Cos(rad * _ksi) * Cos(rad * _phi);
            _position.Y = _distanceToTarget * Sin(rad * _ksi);
            _position.Z = _distanceToTarget * Cos(rad * _ksi) * Sin(rad * _phi);
            _view = Matrix4.LookAt(_position, Vector3.Zero, new Vector3(0, 1, 0));

            _lastPos = current;
            base.OnUpdateFrame(args);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _distanceToTarget = -e.OffsetY / 4 + 10;
            base.OnMouseWheel(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected void ClearBuffers()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}