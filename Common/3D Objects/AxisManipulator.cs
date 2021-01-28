using Common.Colliders;
using Common.Interfaces;
using GlmNet;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;

//TODO: сделать полностью рабочим и сделать демонстрацию
namespace Common._3D_Objects
{
    public class AxisManipulator : SceneObject3D, IRayCasting
    {
        private readonly List<Collider> _colliders = new();

        private string _axis = String.Empty;

        private bool _locked = true;
        private readonly List<SceneObject3D> _toDraw = new();

        private const float HEIGHT = 1;

        public AxisManipulator()
        {
            float height = 1;
            float offset = height / 2;
            float radius = 0.03f;
            Sphere sphere = new Sphere(radius + 0.02f);
            sphere.TranslateWorld(new vec3(0, 0, 0));
            sphere.AttachTo(this);

            SceneObject3D x = new Vector3D("x");
            x.AttachTo(this);
            x.RotateLocal(MathF.PI / 2, new vec3(0, 0, 1));
            x.Material.Color = new vec3(0.5f, 0, 0);
            x.TranslateWorld(new vec3(offset, 0, 0));
            Collider xCollider = new BoxCollider();
            xCollider.ScaleLocal(new vec3(0.1f, 1, 0.1f));
            xCollider.AttachTo(x);

            SceneObject3D y = new Vector3D("y");
            y.TranslateWorld(new vec3(0, offset, 0));
            y.Material.Color = new vec3(0, 0.5f, 0);
            y.AttachTo(this);
            Collider yCollider = new BoxCollider();
            yCollider.ScaleLocal(new vec3(0.1f, 1, 0.1f));
            yCollider.AttachTo(y);

            SceneObject3D z = new Vector3D("z");
            z.RotateLocal(MathF.PI / 2, new vec3(1, 0, 0));
            z.TranslateWorld(new vec3(0, 0, offset));
            z.Material.Color = new vec3(0, 0, 0.5f);
            z.AttachTo(this);
            Collider zCollider = new BoxCollider();
            zCollider.ScaleLocal(new vec3(0.1f, 1, 0.1f));
            zCollider.AttachTo(z);

            _colliders.Add(xCollider);
            _colliders.Add(yCollider);
            _colliders.Add(zCollider);

            /*foreach (Collider collider in _colliders)
            {
                collider.ShowColliders = true;
                _toDraw.Add(collider);
            }*/

            _toDraw.Add(sphere);
            _toDraw.Add(x);
            _toDraw.Add(y);
            _toDraw.Add(z);
        }

        private vec3 lastHitPosition = new vec3(0);
        private vec3 hitPosition;
        public void CheckCollision(vec3 ray, vec3 cameraPosition)
        {
          
                foreach (Collider collider in _colliders)
                {
                    if (collider.IsIntersectsRay(ray, cameraPosition, out var result))
                    {
                        string axisname = ((Vector3D)collider.Parent).AxisName;
                        _axis = axisname;
                        hitPosition = result.Point;
                        return;
                    }
                }
                _axis = String.Empty;
            
        }
        private bool isFirstClicked = true;

        public void OnMouseMove(MouseMoveEventArgs e, bool isPressed)
        {
            if (isPressed)
            {
                var diff =  hitPosition-lastHitPosition;
                
                //Console.WriteLine(isFirstClicked);
                if (!isFirstClicked)
                {
                    if (_axis.Equals("x"))
                    {
                        TranslateWorld(WorldPosition + new vec3(diff.x, 0, 0));
                    }
                    else if (_axis.Equals("y"))
                    {
                        TranslateWorld(WorldPosition + new vec3(0, diff.y, 0));
                    }
                    else if (_axis.Equals("z"))
                    {
                        TranslateWorld(WorldPosition + new vec3(0, 0, diff.z));
                    }
                }
                
                isFirstClicked = false;

            }
            else
            {
                _axis = string.Empty;
                isFirstClicked = true;
                lastHitPosition = new vec3(0);
                hitPosition = new vec3(0);
            }
            lastHitPosition = hitPosition;
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            foreach (SceneObject3D sceneObject in _toDraw)
            {
                sceneObject.Draw(ref view, ref projection);
            }
        }
    }
}