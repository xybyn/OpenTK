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
    public class Window3D : WindowBase
    {
        private float _phi = -90;
        private float _ksi = 0;
        private mat4 _projection;
        private mat4 _view;
        protected vec3 _position;
        private Vector2 _lastPos;
        private float _distanceToTarget = 10;
        private readonly List<SceneObject> _drawers = new();
        private readonly List<IRayCasting> _rayCastings = new();

        private event Action<MouseMoveEventArgs, bool> OnMouseMoveEvent;
        private event Action<MouseButtonEventArgs> OnMousePressed; 
        public Window3D(
            GameWindowSettings gameWindowSettings,
            NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings,
                nativeWindowSettings)
        {
            var mainAxis = new Axis();
            var manipulatingAxis = new AxisManipulator();
            OnMouseMoveEvent += manipulatingAxis.OnMouseMove;
            manipulatingAxis.TranslateGlobal(new vec3(1, 0, 0));
            _rayCastings.Add(manipulatingAxis);
            _drawers.Add(mainAxis);
            _drawers.Add(manipulatingAxis);

            var surface = new Surface((x, z) => Sin(x*z/5), new SurfaceDrawer3DSettings()
            {
                MaterialFace = MaterialFace.FrontAndBack,
                MaxX =  10,
                MinX = -10,
                MaxZ =  10,
                MinZ = -10,
                NumberOfPartitions = 120,
                PolygonMode = PolygonMode.Fill
            });
            surface.Material.Color = new vec3(142/ 255f, 110 / 255f, 253 / 255f);
            surface.AttachTo(manipulatingAxis);
            _drawers.Add(surface);
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
            var Width = 800;
            var Height = 600;
            var x = (2f * mouseX) / Width - 1f;
            var y = 1f - (2f * mouseY) / Height;
            var z = 1f;
            var rayNormalizedDeviceCoordinates = new Vector3(x, y, z);
            var rayClip = new vec4(rayNormalizedDeviceCoordinates.X, rayNormalizedDeviceCoordinates.Y, -1f, 1f);

            var rayEye = glm.inverse(_projection) * rayClip;
            rayEye = new vec4(rayEye.x, rayEye.y, -1f, 0f);
            var rayWorldCoordinates = new vec3(glm.inverse(_view) * rayEye);
            rayWorldCoordinates = glm.normalize(rayWorldCoordinates);

            var p = _position + rayWorldCoordinates;

            foreach (IRayCasting rayCasting in _rayCastings)
            {
                rayCasting.CheckCollision(rayWorldCoordinates, _position);
            }
        }

        private float y = 0;
        private float ys = 0;
        private vec3 spherePos = new vec3(2, 2, 0);
        private float sphereRadius = 1;
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
            //.Rotate((float)GLFW.GetTime() / 5f, new vec3(0, 0, 1));
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                y += 0.001f;
                /*var v = new vec3(plane.Model * new vec4(ys, y, 0, 1));*/
                var v = new vec3(glm.translate(mat4.identity(), new vec3(ys, y, 0)) * new vec4(ys, y, 0, 1));
                //sphere.TranslateGlobal(new vec3(ys, y, 0));
            }
            else if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _drawers.Add(new Sphere(
                    new vec3(
                        glm.inverse(_view) * new vec4(new vec3(MousePosition.X / 800, -MousePosition.Y / 600, -5) * 4 / 3f,
                            1)), 1));
            }
            else if (KeyboardState.IsKeyDown(Keys.S))
            {
                y -= 0.001f;
                //var v = new vec3(plane.Model * new vec4(ys, y, 0, 1));
                //sphere.TranslateGlobal(new vec3(ys, y, 0));
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                ys += 0.001f;
                //sphere.TranslateGlobal(new vec3(ys, y, 0));
            }
            else if (KeyboardState.IsKeyDown(Keys.D))
            {
                ys -= 0.001f;
                //sphere.TranslateGlobal(new vec3(ys, y, 0));
            }
            else if (KeyboardState.IsKeyDown(Keys.Left))
            {
                //sphere.Rotate(-45, new vec3(0, 0, 1));
            }
            else if (KeyboardState.IsKeyDown(Keys.Right))
            {
                //sphere.Rotate(45, new vec3(0, 0, 1));
            }
            else if (KeyboardState.IsKeyDown(Keys.C))
            {
                //sphere.Material.Color = new vec3(0, 1, 0);
            }
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

            foreach (var d in _drawers)
            {
                d.Draw(ref _view, ref _projection);
            }
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}