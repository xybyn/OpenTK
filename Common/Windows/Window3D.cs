using Common._3D_Objects;
using Common.Interfaces;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace Common.Windows
{
    public abstract class Window3D : WindowBase
    {
        private float _phi = -90;
        private float _ksi = 0;
        protected mat4 projection;
        protected mat4 view;
        protected vec3 position;
        private Vector2 _lastPos;
        private float _distanceToTarget = 10;
        protected readonly List<SceneObject3D> toDraw = new();
        private readonly List<IRayCasting> _rayCastings = new();

        private event Action<MouseMoveEventArgs, bool> OnMouseMoveEvent;

        private event Action<MouseButtonEventArgs> OnMousePressed;

        protected Window3D(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings,
                nativeWindowSettings)
        {
        }

        protected void AddMainCoordinatesAxis()
        {
            var mainAxis = new Axis();
            toDraw.Add(mainAxis);
        }

        protected void AddGrid(int horizontalDivisions = 20, int verticalDivisions = 20)
        {
            var grid = new Grid3D(horizontalDivisions, verticalDivisions)
            {
                Material =
                {
                    Color = new vec3(0.2f)
                }
            };

            toDraw.Add(grid);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            projection = glm.perspective(45f, 4f / 3f, 0.1f, 100f);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Pick(MousePosition.X, MousePosition.Y);
            }
            OnMousePressed?.Invoke(e);
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (MouseState.IsButtonDown(MouseButton.Left))
            {
                Pick(MousePosition.X, MousePosition.Y);
            }
            OnMouseMoveEvent?.Invoke(e, MouseState.IsButtonDown(MouseButton.Left));
        }

        private void Pick(float mouseX, float mouseY)
        {
            var ray = Physics3D.Physics3D.ScreenPointToWorldRay(new vec2(mouseX, mouseY),
                new vec2(Size.X, Size.Y), projection, view);

            foreach (IRayCasting rayCasting in _rayCastings)
            {
                rayCasting.CheckCollision(ray, position);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            Vector2 current = MousePosition;
            if (MouseState.IsButtonDown(MouseButton.Right))
            {
                _phi -= (_lastPos.X - current.X) * 0.1f;
                _ksi -= (_lastPos.Y - current.Y) * 0.1f;
            }
            position.x = _distanceToTarget * Cos(glm.radians(_ksi)) * Cos(glm.radians(_phi));
            position.z = _distanceToTarget * Cos(glm.radians(_ksi)) * Sin(glm.radians(_phi));
            position.y = _distanceToTarget * Sin(glm.radians(_ksi));
            view = glm.lookAt(position, new vec3(0, 0, 0), new vec3(0, 1, 0));

            var vec = new vec3(
                glm.inverse(view) * new vec4(new vec3(MousePosition.X / 800, MousePosition.Y / 600, -5),
                    1));
            _lastPos = current;
            var sp = new vec3(projection * view * new vec4(new vec3(4, 0, 0), 1));
            var right = new vec3(1, 0, 0);

            base.OnUpdateFrame(args);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _distanceToTarget = -e.OffsetY / 4 + 10;
            base.OnMouseWheel(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var d in toDraw)
            {
                d.Draw(ref view, ref projection);
            }
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}