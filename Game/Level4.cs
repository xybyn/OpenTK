using Common.Colliders;
using Common;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common._3D_Objects;

namespace Game
{
    public class Level4 : Level
    {
        private vec3 playerStartPosition = new vec3(4, 0.75f, 0);
        private vec3 playerEndPosition = new vec3(-4, 0.75f, 1);

        public override void Initialize()
        {
            BuildBase(playerStartPosition, playerEndPosition);

            float size = 0.5f;
            BuildWall(new vec3(size, size, 4), new vec3(3, 1, 0));
            BuildWall(new vec3(4, size, size), new vec3(-3, 1, 0));
            BuildWall(new vec3(4, size, size), new vec3(3, 1, 2));
            BuildWall(new vec3(size, size, 4), new vec3(0, 1, 3));
            BuildWall(new vec3(size, size, 2), new vec3(-3, 1, 1));

            BuildWall(new vec3(size, size, 4), new vec3(1, 1, -3));

            var ob1 = BuildObstacle(new vec3(size, size, 2f), new vec3(-3, 1, 3.25f));
            BuildDynamicObstacle(ob1);
            toDraw.Add(ob1);

            var ob2 = BuildObstacle(new vec3(size, size, 2f), new vec3( 3, 1, -3.25f));
            BuildDynamicObstacle(ob2);
            toDraw.Add(ob2);
        }

        public override void Reset()
        {
            base.Reset();
            player.TranslateWorld(playerStartPosition);
        }
    }
}
