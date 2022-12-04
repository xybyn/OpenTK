// unset

using Common._3D_Objects;
using GlmNet;
using System.Collections.Generic;

namespace Common
{
    public class Cube : SceneObject3D
    {
        private readonly List<SceneObject3D> toDraw = new();

        public Cube()
        {
            float s = 0.5f;
            List<vec3> vertices = new List<vec3>()
            {
                new vec3(-s, -s, s), //0
                new vec3( s, -s, s),//1
                new vec3( s,  s, s),//2
                new vec3(-s,  s, s),//3

                new vec3(-s, -s, -s),//4
                new vec3( s, -s, -s),//5
                new vec3( s,  s, -s),//6
                new vec3(-s,  s, -s),//7

                new vec3( s,  -s, -s),//8
                new vec3( s,  s, -s),//9
                new vec3( s,  s, s),//10
                new vec3( s,  -s, s),//11

                new vec3( -s,  -s, -s),//8
                new vec3( -s,  s, -s),//9
                new vec3( -s,  s, s),//10
                new vec3( -s,  -s, s),//11

                new vec3( -s,  s, -s),//8
                new vec3(  s,  s, -s),//9
                new vec3(  s,  s, s),//10
                new vec3( -s,  s, s),//11

                new vec3( -s,  -s, -s),//8
                new vec3(  s,  -s, -s),//9
                new vec3(  s,  -s, s),//10
                new vec3( -s,  -s, s),//11
            };
            List<uint> indices = new List<uint>();
            indices.Add(0);
            indices.Add(1);
            indices.Add(2);
            indices.Add(2);
            indices.Add(3);
            indices.Add(0);

            indices.Add(4);
            indices.Add(5);
            indices.Add(6);
            indices.Add(6);
            indices.Add(7);
            indices.Add(4);

            indices.Add(8);
            indices.Add(9);
            indices.Add(10);
            indices.Add(10);
            indices.Add(11);
            indices.Add(8);

            indices.Add(12);
            indices.Add(13);
            indices.Add(14);
            indices.Add(14);
            indices.Add(15);
            indices.Add(12);

            indices.Add(16);
            indices.Add(17);
            indices.Add(18);
            indices.Add(18);
            indices.Add(19);
            indices.Add(16);

            indices.Add(20);
            indices.Add(21);
            indices.Add(22);
            indices.Add(22);
            indices.Add(23);
            indices.Add(20);

            List<vec3> normals = new List<vec3>()
            {
                new vec3(0, 0, 1),
                new vec3(0, 0, 1),
                new vec3(0, 0, 1),
                new vec3(0, 0, 1),

                new vec3(0, 0, -1),
                new vec3(0, 0, -1),
                new vec3(0, 0, -1),
                new vec3(0, 0, -1),

                new vec3(1, 0, 0),
                new vec3(1, 0, 0),
                new vec3(1, 0, 0),
                new vec3(1, 0, 0),

                 new vec3(-1, 0, 0),
                new vec3(-1, 0, 0),
                new vec3(-1, 0, 0),
                new vec3(-1, 0, 0),

                  new vec3(0, 1, 0),
                new vec3(0, 1, 0),
                new vec3(0, 1, 0),
                new vec3(0, 1, 0),

                  new vec3(0, -1, 0),
                new vec3(0, -1, 0),
                new vec3(0, -1, 0),
                new vec3(0, -1, 0),
            };
            InitializeVAO_VBO_EBO(vertices, normals, indices);
        }
    }
}