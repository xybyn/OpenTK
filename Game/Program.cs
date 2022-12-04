using Common.Colliders;
using Common;
using Common.Windows;
using GlmNet;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Common.Drawers;
using Common.MathAbstractions;
using Common.Physics3D;
using System.Collections.Generic;
using Common._3D_Objects;
using OpenTK.Graphics.ES11;
using System.Numerics;
using System.IO;
using System;

namespace Game
{
    public class Game : Window3D
    {
        private int currentLevel = 0;
        List<Level> levels = new List<Level>();
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings,
            nativeWindowSettings)
        {
            levels.Add(new Level1());
            levels.Add(new Level2());
            levels.Add(new Level3());
            levels.Add(new Level4());
            levels[currentLevel].Initialize();
            levels[currentLevel].OnLevelDone += NextLevel;

        }
        public void NextLevel()
        {
            levels[currentLevel].End();
            levels[currentLevel].OnLevelDone -= NextLevel;
            currentLevel++;
            if (currentLevel >= levels.Count)
                currentLevel--;
            levels[currentLevel].Initialize();
            levels[currentLevel].OnLevelDone += NextLevel; 
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs obj)
        {
            if (obj.Key == Keys.Space)
            {
                levels[currentLevel].Start();
            }
            if (obj.Key == Keys.R)
            {
                levels[currentLevel].Reset();
            }
            if (obj.Key == Keys.Escape)
            {
                Close();
            }
            if (obj.Key == Keys.Left)
            {
                levels[currentLevel].End();
                currentLevel--;
                if (currentLevel < 0)
                    currentLevel = 0;
                levels[currentLevel].Initialize();
            }
            if (obj.Key == Keys.Right)
            {
                levels[currentLevel].End();
                currentLevel++;
                if (currentLevel >= levels.Count)
                    currentLevel--;
                levels[currentLevel].Initialize();
            }
        }

        public Action OnEnded;

        static double prev = 0;
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            double curr = GLFW.GetTime();
            double dt = curr - prev;
            prev = curr;

            toDraw = levels[currentLevel].ToDraw;

            levels[currentLevel].Update((float)dt);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Pick(MousePosition.X, MousePosition.Y);
            }
            base.OnMouseUp(e);
        }

        private void Pick(float mouseX, float mouseY)
        {
            var ray = Physics3D.ScreenPointToWorldRay(new vec2(mouseX, mouseY),
                new vec2(Size.X, Size.Y), projection, view);

            levels[currentLevel].RayCast(ray, position);
        }
    }
    class Empty : Window3D
    {
        public Empty(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1600, 1200),
                Title = "Game"
            };

            var window = new Game(settings, nativeWindowSettings);
            
  
            window.Run();
            window.Dispose();
        }
    }
}
