// unset

using Common.Colliders;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTKProject;
using System;
using System.Collections.Generic;

namespace Common
{
    public class AxisManipulator : SceneObject, IRayCasting
    {
        private SceneObject x;
        private SceneObject y;
        private SceneObject z;
        private Collider xCollider;
        private Collider yCollider;
        private Collider zCollider;
        private Sphere sphere;
        private List<SceneObject> toDraw = new();
        private List<Collider> _colliders = new();

        private bool locked = true;
        
        public void OnMouseMove(MouseMoveEventArgs e, bool isPressed)
        {
            if (isPressed)
            {
                Console.WriteLine(axis);
                if (axis.Equals("x"))
                    {
                        TranslateGlobal(Pos+new vec3(-e.DeltaX/70f, 0, 0));
                    }else if (axis.Equals("y"))
                    {
                        TranslateGlobal(Pos+new vec3(0, -e.DeltaY/70f, 0));
                    }
                    else if (axis.Equals("z"))
                    {
                        TranslateGlobal(Pos+new vec3(0, 0, -e.DeltaX/70f));
                    }
            }
            else
            {
                axis = string.Empty;
            }
            
        }
        string axis = String.Empty;
        public AxisManipulator()
        {
            float height = 1;
            float offset = height / 2;
            var radius = 0.03f;
            sphere = new Sphere(new vec3(0, 0, 0), radius + 0.02f);
            sphere.AttachTo(this);

            x = new Vector3D("x", new vec3(0.8f, 0, 0));
            x.AttachTo(this);
            x.Rotate(MathF.PI / 2, new vec3(0, 0, 1));
            x.Material.Color = new vec3(0.5f, 0, 0);
            x.TranslateGlobal(new vec3(offset, 0, 0));
            xCollider = new BoxCollider();
            xCollider.Scale(new vec3(0.1f, 1, 0.1f));
            xCollider.AttachTo(x);

            y = new Vector3D("y", new vec3(0, 0.8f, 0));
            y.TranslateGlobal(new vec3(0, offset, 0));
            y.Material.Color = new vec3(0, 0.5f, 0);
            y.AttachTo(this);
            yCollider = new BoxCollider();
            yCollider.Scale(new vec3(0.1f, 1, 0.1f));
            yCollider.AttachTo(y);

            z = new Vector3D("z", new vec3(0, 0, 0.8f));
            z.Rotate(MathF.PI / 2, new vec3(1, 0, 0));
            z.TranslateGlobal(new vec3(0, 0, offset));
            z.Material.Color = new vec3(0, 0, 0.5f);
            z.AttachTo(this);
            zCollider = new BoxCollider();
            zCollider.Scale(new vec3(0.1f, 1, 0.1f));
            zCollider.AttachTo(z);

            _colliders.Add(xCollider);
            _colliders.Add(yCollider);
            _colliders.Add(zCollider);

            toDraw.Add(sphere);
            toDraw.Add(x);
            toDraw.Add(y);
            toDraw.Add(z);
            /*toDraw.Add(xCollider);
            toDraw.Add(yCollider);
            toDraw.Add(zCollider);*/
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            foreach (SceneObject sceneObject in toDraw)
            {
                sceneObject.Draw(ref view, ref projection);
            }
        }

        public void CheckCollision(vec3 ray, vec3 cameraPosition)
        {

            foreach (var collider in _colliders)
            {
                if (collider.IntersectsRay(ray, cameraPosition, out var result))
                {

                    var axisname = ((Vector3D)collider.Parent).AxisName;
                    axis = axisname;

                    return;
                }
  
            }
            axis = String.Empty;
        }
    }
}