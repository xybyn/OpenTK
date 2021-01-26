// unset

using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenTKProject
{
    public abstract class SceneObject
    {
        public Matrix4 Model
        {
            get
            {
                return Parent == null ? rotation * translation * scale : Parent.Model * rotation * translation * scale;
            }
        }

        public IReadOnlyCollection<SceneObject> Children => _children;

        public SceneObject Parent { get; protected set; }

        public Vector3 Position { get; protected set; }
        public Vector3 Rotation { get; protected set; }
        public float Scaling { get; protected set; }

        public void AttachTo(SceneObject parent)
        {
            Parent = parent;
        }

        protected Matrix4 translation = Matrix4.Identity;
        protected Matrix4 rotation = Matrix4.Identity;
        protected Matrix4 scale = Matrix4.Identity;
        private List<SceneObject> _children = new List<SceneObject>();

        public void Translate(Vector3 newPosition)
        {
            Position = newPosition;
            Matrix4.CreateTranslation(newPosition, out translation);
        }

        public void Scale(float newScale)
        {
            Matrix4.CreateScale(newScale, out scale);
        }

        public void RotateX(float angle)
        {
            Matrix4.CreateRotationX(angle, out rotation);
        }

        public void RotateY(float angle)
        {
            Matrix4.CreateRotationY(angle, out rotation);
        }

        public void RotateZ(float angle)
        {
            Matrix4.CreateRotationZ(angle, out rotation);
        }

        public void AddChild(SceneObject newChild)
        {
            _children.Add(newChild);
        }
    }
}