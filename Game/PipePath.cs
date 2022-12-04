using Common;
using Common._3D_Objects;
using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PipePath : SceneObject3D
    {
        List<Pipe> pipes = new List<Pipe>();
        List<Sphere> points = new List<Sphere>();
        private vec3 color = new vec3(232, 250, 132) / 255 ;
        public PipePath()
        {
        }
        public void Add(vec3 p0, vec3 p1)
        {
            var pipe = new Pipe(new List<vec3>
            {
                p0, p1
            });
            pipe.Material.Color = color;
            pipes.Add(pipe);
            if (points.Count == 0)
            {
                Sphere s0 = new Sphere(0.12f);
                s0.TranslateWorld(p0);
                s0.Material.Color = color; 
                points.Add(s0);
            }

            Sphere s1 = new Sphere(0.12f);
            s1.TranslateWorld(p1);
            s1.Material.Color = color;
            points.Add(s1);
        }
        public void Clear()
        {
            pipes.Clear();
            points.Clear();
        }
        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            foreach (var item in pipes)
            {
                item.Draw(ref view, ref projection);
            }

            foreach (var item in points)
            {
                item.Draw(ref view, ref projection);
            }
        }
    }
}
