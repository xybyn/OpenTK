using Common._3D_Objects;
using Common.Colliders;
using Common;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Level
    {
        public Action OnLevelDone;
        protected Collider groundCollider;
        protected SceneObject3D groundBox = null;
        protected List<AxisAlignedBoxCollider> colliders = new List<AxisAlignedBoxCollider>();
        protected List<SceneObject3D> toDraw = new List<SceneObject3D>();
        protected PipePath pipePath;
        protected List<SphereCollider> sphereColliders = new List<SphereCollider>();
        protected void BuildGround()
        {
            groundBox = new Cube();
            groundBox.ScaleLocal(new vec3(11.2f, 1f, 11.2f));
            groundBox.Material.Color = new vec3(167, 243, 216) / 255;
            toDraw.Add(groundBox);
            pipePath = new PipePath();
            groundCollider = new BoxCollider();
            groundCollider.AttachTo(groundBox);
        }

        protected void BuildBase(vec3 start, vec3 end)
        {
            BuildGround();
            BuildWalls();
            CreatePlayer(start);
            CreateFinishPoint(end);
        }

        protected void BuildWindmillObstacle(SceneObject3D obstacle)
        {
            manager.AddSimulating(new WindmillSimulating(obstacle));
        }
        protected void BuildDynamicObstacle(SceneObject3D obstacle)
        {
            manager.AddSimulating(new DynamicObstacleSimulating(obstacle));
        }

        protected void BuildWall(vec3 size, vec3 position)
        {
            var wall1 = new Cube();
            wall1.ScaleLocal(size);
            wall1.TranslateWorld(position);
            wall1.Material.Color = new vec3(0.9f);
            toDraw.Add(wall1);
            var wall1Collider = new AxisAlignedBoxCollider(-1 * new vec3(0.5f), new vec3(0.5f));
            wall1Collider.AttachTo(wall1);
            colliders.Add(wall1Collider);
        }
        public SceneObject3D BuildObstacle(vec3 size, vec3 position)
        {
            var obstacle = new Cube();
            obstacle.Material.Color = new vec3(226f, 122f, 250f) / 255f;
            obstacle.TranslateWorld(position);
            obstacle.ScaleLocal(size);
            var collider = new AxisAlignedBoxCollider(-1 * new vec3(0.5f), new vec3(0.5f));
            collider.AttachTo(obstacle);
            colliders.Add(collider);

            return obstacle;
        }
        protected void BuildWalls()
        {
            BuildWall(new vec3(1, 1f, 11), new vec3(-5, 1, 0));
            BuildWall(new vec3(1, 1f, 11), new vec3(5, 1, 0));
            BuildWall(new vec3(9.0f, 1f, 1), new vec3(0, 1, -5));
            BuildWall(new vec3(11.0f, 1f, 1), new vec3(0, 1, 5));
        }
        protected Cube player;
        protected AxisAlignedBoxCollider playerCollider;
        protected Sphere finishPoint;
        protected AxisAlignedBoxCollider finishPointCollider;
        protected PathBuilder _builder;

        protected PathWalker walker;
        vec3 offset = new vec3(0, 0.25f, 0);
        vec3 playerColor = new vec3(242, 42, 47) / 255;
        protected void CreatePlayer(vec3 playerStartPosition)
        {
            player = new Cube();
            player.ScaleLocal(new vec3(0.5f));
            player.Material.Color = playerColor;
            player.TranslateWorld(playerStartPosition);
            playerCollider = new AxisAlignedBoxCollider(-1 * new vec3(0.5f), new vec3(0.5f));
            playerCollider.AttachTo(player);
            toDraw.Add(player);

            _builder = new PathBuilder(playerStartPosition, offset);

            walker = new PathWalker(player, _builder);
            walker.OnFinished += PlayerNotFinished;
            manager = new SimulationManager();
            manager.AddSimulating(walker);
            toDraw.Add(pipePath);
        }


        protected void CreateFinishPoint(vec3 endPoint)
        {
            finishPoint = new Sphere(0.25f);
            finishPoint.Material.Color = playerColor;
            finishPoint.TranslateWorld(endPoint);
            finishPointCollider = new AxisAlignedBoxCollider(-1 * new vec3(0.05f), new vec3(0.05f));
            finishPointCollider.AttachTo(finishPoint);
            toDraw.Add(finishPoint);

        }
        public List<SceneObject3D> ToDraw { get { return toDraw; } }

        protected SimulationManager manager;
        double timer = 0.0f;
        public virtual void Reset()
        {
            manager.EndSimulation();
            pipePath.Clear();
            timer = 0.0f;
        }

        public void End()
        {
            manager.EndSimulation();
            toDraw.Clear();
            walker.OnFinished -= PlayerNotFinished;
        }

        public void Start()
        {
            if (!manager.IsSimulating)
                manager.StartSimulation();
        }

        public abstract void Initialize();

        public void Stop()
        {
            manager.EndSimulation();
        }

        public void Update(float dt)
        {
            if (manager.IsSimulating)
            {
                timer += dt;
                manager.UpdateSimulation((float)timer);
            }

            bool intersected = false;
            bool finished = false;
            if (finishPointCollider.IsIntersectsAxisAlignedBox(playerCollider, out var result))
            {
                finished = true;
                PlayerFinished();
            }
            if (finished)
                return;
            foreach (var collider in colliders)
            {

                if (collider.IsIntersectsAxisAlignedBox(playerCollider, out var result2))
                {
                    intersected = true;
                    PlayerHitsObstacle();
                    break;
                }
            }

        }

        protected virtual void PlayerFinished()
        {
            Stop();
            OnLevelDone?.Invoke();
        }

        protected virtual void PlayerNotFinished()
        {
            Reset();
        }

        protected virtual void PlayerHitsObstacle()
        {
            Reset();
        }

        public void RayCast(vec3 ray, vec3 position)
        {
            if (groundCollider.IsIntersectsRay(ray, position, out var result))
            {
                foreach (var item in _builder.Points)
                {
                    vec3 diff = item - result.Point;
                    if (glm.dot(diff, diff) < 0.1)
                        return;
                }

                _builder.AddPoint(result.Point);
        
                if (_builder.Points.Count > 1)
                {
                    for (int i = 0; i < _builder.Points.Count - 1; i++)
                    {
                        pipePath.Add(_builder.Points[i], _builder.Points[i + 1]);
                    }

                }
                return;
            }
        }
    }
}
