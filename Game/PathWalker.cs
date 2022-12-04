using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using Common._3D_Objects;

namespace Game
{
    public class PathWalker : Simulating
    {
        private PathBuilder _pathBuilder;

        private Path _path;

        private SceneObject3D obj;

        public Action OnFinished;

        float speed = 5;
        public PathWalker(SceneObject3D obj, PathBuilder pathBuilder)
        {
            this.obj = obj;
            _pathBuilder = pathBuilder;
        }

        public override void StartSimulation()
        {
            if (isSimulating)
                return;
            isSimulating = true;

            _path = _pathBuilder.CreatePath();
        }
        public override void EndSimulation()
        {
            base.EndSimulation();
            _pathBuilder.Clear();
        }
        public override void UpdateSimulation(float time)
        {
            if (isSimulating)
            {
                if(_path.Ended())
                {
                    OnFinished?.Invoke();
                    return;
                }
                obj.TranslateWorld(_path.GetPosition(time * speed));
            }
        }
    }
}
