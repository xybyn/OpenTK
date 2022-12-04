using Common._3D_Objects;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class WindmillSimulating : Simulating
    {
        private SceneObject3D obstacle;
        private vec3 origin;
        public WindmillSimulating(SceneObject3D obstacle)
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
            float t = Clamp((1 + MathF.Sin(time * 0.5f)) * 0.5f, 0, 1);
            float y = -t;
            obstacle.RotateLocal(time * 1, new vec3(0, 1, 0));
        }

        public override void EndSimulation()
        {
            base.EndSimulation();
            obstacle.RotateLocal(0, new vec3(0, 1, 0));
        }
    }
}
