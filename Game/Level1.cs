using Common._3D_Objects;
using Common.Colliders;
using Common;
using GlmNet;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Common.Windows;
using OpenTK.Windowing.Desktop;
using System;

namespace Game
{
    public class Level1 : Level
    {
        private vec3 playerStartPosition = new vec3(4, 0.75f, 0);
        private vec3 playerEndPosition = new vec3(-4, 0.75f, 0);
        public override void Initialize()
        {
            BuildGround();
            BuildWalls();
            CreatePlayer(playerStartPosition);
            CreateFinishPoint(playerEndPosition);
        }


        public override void Reset()
        {
            base.Reset();
            player.TranslateWorld(playerStartPosition);
        }
    }
}
