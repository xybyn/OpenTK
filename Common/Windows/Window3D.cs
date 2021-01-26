using Common._3D_Objects;
using Common.Colliders;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKProject;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using static System.MathF;

namespace Common
{
    public abstract class Window3D : WindowBase
    {
        private float _phi = -90;
        private float _ksi = 0;
        private mat4 _projection;
        private mat4 _view;
        protected vec3 _position;
        private Vector2 _lastPos;
        private float _distanceToTarget = 10;
        public readonly List<SceneObject> ToDraw = new();
        private readonly List<IRayCasting> _rayCastings = new();

        private event Action<MouseMoveEventArgs, bool> OnMouseMoveEvent;
        private event Action<MouseButtonEventArgs> OnMousePressed; 
        public Window3D(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings,
                nativeWindowSettings)
        {
            
          
        }

        public void AddMainCoordinatesAxis()
        {
            var mainAxis = new Axis();
            ToDraw.Add(mainAxis);
        }

        public void AddGrid(int horizontalDivisions=21, int verticalDivisions=21)
        {
            var grid = new Grid3D(horizontalDivisions, verticalDivisions)
            {
                Material =
                {
                    Color = new vec3(0.5f)
                }
            };

            ToDraw.Add(grid);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _projection = glm.perspective(45f, 4f / 3f, 0.1f, 100f);
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
                new vec2(Size.X, Size.Y), _projection, _view);

            foreach (IRayCasting rayCasting in _rayCastings)
            {
                rayCasting.CheckCollision(ray, _position);
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
            _position.x = _distanceToTarget * Cos(glm.radians(_ksi)) * Cos(glm.radians(_phi));
            _position.z = _distanceToTarget * Cos(glm.radians(_ksi)) * Sin(glm.radians(_phi));
            _position.y = _distanceToTarget * Sin(glm.radians(_ksi));
            _view = glm.lookAt(_position, new vec3(0, 0, 0), new vec3(0, 1, 0));


            var vec = new vec3(
                glm.inverse(_view) * new vec4(new vec3(MousePosition.X / 800, MousePosition.Y / 600, -5),
                    1));
            _lastPos = current;
            var sp = new vec3(_projection * _view * new vec4(new vec3(4, 0, 0), 1));
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

            foreach (var d in ToDraw)
            {
                d.Draw(ref _view, ref _projection);
            }
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}