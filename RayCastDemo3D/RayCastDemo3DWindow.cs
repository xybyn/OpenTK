// unset

using Common;
using Common.Colliders;
using Common.Physics3D;
using GlmNet;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;

namespace RayCastDemo
{
    public class RayCastDemo3DWindow : Window3D
    {
        private List<Collider> colliders = new List<Collider>();
        public RayCastDemo3DWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            /*var sphere = new Sphere(new vec3(-4, 1, 3), 1);
            var cube = new Cube(new vec3(-2, 1, 3));
            var cylinder = new Cylinder(1, 3);
            cylinder.TranslateGlobal(new vec3(0, 1, 3));
            var plane = new Plane3D(new vec3(-1, 1, 0), new vec3(2, 1, 3));

            sphere.Material.Color = new vec3(0.5f, 0, 0);

            cylinder.Material.Color = new vec3(0.5f, 0, 0.5f);
            plane.Material.Color = new vec3(0.5f, 0, 0);
            ToDraw.Add(sphere);
            ToDraw.Add(cube);
            ToDraw.Add(cylinder);
            ToDraw.Add(plane);*/

            var sphere1 = new Sphere(new vec3(4, 1, 0), 0.5f);
            sphere1.Material.Color = new vec3(0.5f);
            ToDraw.Add(sphere1);
            var boxCollider = new BoxCollider();
            boxCollider.AttachTo(sphere1);
            ToDraw.Add(boxCollider);
            colliders.Add(boxCollider);
            
            var sphere2 = new Sphere(new vec3(2, 1, 0), 0.5f);
            sphere2.Material.Color = new vec3(0.5f);
            ToDraw.Add(sphere2);
            var sphereCollider2 = new SphereCollider();
            sphereCollider2.AttachTo(sphere2);
            ToDraw.Add(sphereCollider2);
            colliders.Add(sphereCollider2);
            
            var cylinder = new Cylinder(0.5f, 1f);
            cylinder.Rotate(30, new vec3(1, 1, 1));
            cylinder.TranslateGlobal(new vec3(0, 1, 0));
            cylinder.Scale(new vec3(1, 1, 1));
            cylinder.Material.Color = new vec3(0.5f);
            ToDraw.Add(cylinder);
            var sphereCollider3 = new SphereCollider();
            sphereCollider3.AttachTo(cylinder);
            ToDraw.Add(sphereCollider3);
            colliders.Add(sphereCollider3);
            
            var sphere4 = new Sphere(new vec3(-2, 1, 0), 0.5f);
            sphere4.Material.Color = new vec3(0.5f);
            ToDraw.Add(sphere4);
            var planeCollider4 = new PlaneCollider(new vec3(1, 1, 0), new vec3(0, 0, 0));
            planeCollider4.AttachTo(sphere4);
            ToDraw.Add(planeCollider4);
            colliders.Add(planeCollider4);
            
            var cylinder2 = new Cylinder(0.5f, 1f);
            cylinder2.Rotate(30, new vec3(1, 1, 1));
            cylinder2.TranslateGlobal(new vec3(-4, 1, 0));
            cylinder2.Scale(new vec3(1, 1, 1));
            cylinder2.Material.Color = new vec3(0.5f);
            ToDraw.Add(cylinder2);
            var boxcollider2 = new BoxCollider();
            boxcollider2.AttachTo(cylinder2);
            ToDraw.Add(boxcollider2);
            colliders.Add(boxcollider2);
            
            
            AddGrid();
            AddMainCoordinatesAxis();
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
                new vec2(Size.X, Size.Y), _projection, _view);

            /*foreach (IRayCasting rayCasting in _rayCastings)
            {
                rayCasting.CheckCollision(ray, _position);
            }*/
            foreach (var collider in colliders)
            {
                if(collider.IntersectsRay(ray, _position, out var result))
                {
                    collider.Parent.Material.Color = new vec3(0, 0.5f, 0);
                    
                    ToDraw.Add(new Sphere(ray*result + _position, 0.1f)
                    {
                        Material =
                        {
                            Color = new vec3(0, 0, 0.5f)
                        }
                    });
                }
                else
                {
                    collider.Parent.Material.Color = new vec3(0.5f);
                }
            }
        }
    }
}