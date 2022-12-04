using Common._3D_Objects;
using GlmNet;
using OpenTK.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class DynamicObstacleSimulating : Simulating
    {
        private SceneObject3D obstacle;
        private vec3 origin;
        public DynamicObstacleSimulating(SceneObject3D obstacle)
        {
            this.obstacle = obstacle;
            origin = obstacle.WorldPosition;
        }
        float Clamp(float a, float b, float c)
        {
            return a;
        }
        public override void UpdateSimulation(float time)
        {
            if (!isSimulating)
                return;
            float speed = 1;
            float power = 6;

            float t0 = MathF.Cos(time * speed) * power;

            float t = (Math.Max(Math.Min(t0, 1), -1) - 1)*0.5f;
            float y = t;
            obstacle.TranslateWorld(origin + new vec3(0, y, 0));
        }

        public override void EndSimulation()
        {
            base.EndSimulation();
            obstacle.TranslateWorld(origin);
        }
    }
}
