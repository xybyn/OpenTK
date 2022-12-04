using Common.Colliders;
using Common;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public class Level3 : Level
    {
        private vec3 playerStartPosition = new vec3(4, 0.75f, 1);
        private vec3 playerEndPosition = new vec3(-4, 0.75f, 1);
        private Cube obstacle;

        public override void Initialize()
        {
            BuildBase(playerStartPosition, playerEndPosition);

            float size = 0.5f;
            var ob1 = BuildObstacle(new vec3(size, size, 10f), new vec3(0, 1, 0));
            BuildDynamicObstacle(ob1);
            toDraw.Add(ob1);
        }

        public override void Reset()
        {
            base.Reset();
            player.TranslateWorld(playerStartPosition);
        }
    }
}
