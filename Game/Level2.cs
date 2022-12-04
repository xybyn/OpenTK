using Common;
using Common.Colliders;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Level2 : Level
    {
        private vec3 playerStartPosition = new vec3(4, 0.75f, 0);
        private vec3 playerEndPosition = new vec3(-4, 0.75f, 1);
        private Cube obstacle;

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
        }

        public override void Reset()
        {
            base.Reset();
            player.TranslateWorld(playerStartPosition);
        }
    }
}
