// unset

using Common;
using Common.Colliders;
using Common.Drawers;
using Common.MathAbstractions;
using Common.Physics3D;
using Common.Windows;
using GlmNet;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace RayCastDemo
{
    public class RayCastDemo3DWindow : Window3D
    {
        private List<Collider> colliders = new List<Collider>();
        private List<Line3D> lines = new();

        public RayCastDemo3DWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings,
            nativeWindowSettings)
        {
            var sphere1 = new Sphere(0.5f);
            sphere1.TranslateWorld(new vec3(4, 1, 0));
            sphere1.Material.Color = new vec3(0.5f);
            toDraw.Add(sphere1);
            var boxCollider = new BoxCollider();
            boxCollider.AttachTo(sphere1);
            toDraw.Add(boxCollider);
            colliders.Add(boxCollider);

            var sphere2 = new Sphere(0.5f);
            sphere2.TranslateWorld(new vec3(2, 1, 0));
            sphere2.Material.Color = new vec3(0.5f);
            toDraw.Add(sphere2);
            var sphereCollider2 = new SphereCollider(1);
            sphereCollider2.AttachTo(sphere2);
            toDraw.Add(sphereCollider2);
            colliders.Add(sphereCollider2);

            var cylinder = new Cylinder(0.5f, 1f);
            cylinder.RotateLocal(30, new vec3(1, 1, 1));
            cylinder.TranslateWorld(new vec3(0, 1, 0));
            cylinder.Material.Color = new vec3(0.5f);
            toDraw.Add(cylinder);
            var sphereCollider3 = new SphereCollider(1);
            sphereCollider3.AttachTo(cylinder);
            toDraw.Add(sphereCollider3);
            colliders.Add(sphereCollider3);

            var sphere4 = new Sphere(0.5f);
            sphere4.TranslateWorld(new vec3(-2, 1, 0));
            sphere4.Material.Color = new vec3(0.5f);
            toDraw.Add(sphere4);
            var planeCollider4 = new PlaneCollider(new vec3(1, 1, 0), new vec3(0, 0, 0));
            planeCollider4.AttachTo(sphere4);
            toDraw.Add(planeCollider4);
            colliders.Add(planeCollider4);

            var cylinder2 = new Cylinder(0.5f, 1f);
            cylinder2.RotateLocal(30, new vec3(1, 1, 1));
            cylinder2.TranslateWorld(new vec3(-4, 1, 0));
            cylinder2.Material.Color = new vec3(0.5f);
            toDraw.Add(cylinder2);
            var boxcollider2 = new BoxCollider();
            boxcollider2.AttachTo(cylinder2);
            toDraw.Add(boxcollider2);
            colliders.Add(boxcollider2);

            AddGrid();
            AddMainCoordinatesAxis();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            foreach (var VARIABLE in lines)
            {
                VARIABLE.Draw(ref view, ref projection);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Pick(MousePosition.X, MousePosition.Y);
            }
            base.OnMouseUp(e);
        }

        private void Pick(float mouseX, float mouseY)
        {
            var ray = Physics3D.ScreenPointToWorldRay(new vec2(mouseX, mouseY),
                new vec2(Size.X, Size.Y), projection, view);

            foreach (var collider in colliders)
            {
                if (collider.IsIntersectsRay(ray, position, out var result))
                {
                    collider.Parent.Material.Color = new vec3(0, 0.5f, 0);

                    var point = new Sphere(0.05f);
                    point.TranslateWorld(result.Point);
                    Console.WriteLine(result.Normal);
                    toDraw.Add(point);
                    toDraw.Add(new Line3D(new Line
                    {
                        Direction = result.Normal, Point = result.Point
                    })
                    {
                        Material =
                        {
                            Color = new vec3(1, 0, 0)
                        }
                    });
                    return;
                }

                collider.Parent.Material.Color = new vec3(0.5f);

            }
        }
    }
}