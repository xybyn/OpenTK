// unset

using Common._3D_Objects;
using GlmNet;
using System.Collections.Generic;
using static System.MathF;

namespace Common
{
    public sealed class Sphere : SceneObject3D
    {
        private readonly List<uint> _indices = new();
        private readonly List<vec3> _normals = new();
        private readonly List<vec3> _vertices = new();

        public Sphere(float radius, int sectorCount = 20, int stackCount = 20)
        {
            CalculateVerticesAndNormals(sectorCount, stackCount, radius);
            CalculateIndices(sectorCount, stackCount);
            InitializeVAO_VBO_EBO(_vertices, _normals, _indices);
        }

        private void CalculateVerticesAndNormals(int sectorCount, int stackCount, float r)
        {
            var sectorStep = 2 * PI / sectorCount;

            var stackStep = PI / stackCount;
            var phi = 0;
            float xy;
            float z;
            float x, y;
            float nx, ny, nz;
            float stackAngle, sectorAngle;
            var lengthInv = 1.0f / r;
            for (var i = 0; i <= sectorCount; i++)
            {
                stackAngle = (PI / 2) - (i * stackStep); // starting from pi/2 to -pi/2
                xy = r * Cos(stackAngle); // r * cos(u)
                z = r * Sin(stackAngle); // r * sin(u)

                for (var j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep; // starting from 0 to 2pi

                    x = xy * Cos(sectorAngle); // r * cos(u) * cos(v)
                    y = xy * Sin(sectorAngle); // r * cos(u) * sin(v)
                    _vertices.Add(new vec3(x, y, z));
                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;
                    _normals.Add(new vec3(nx, ny, nz));
                }
            }
        }

        private void CalculateIndices(int sectorCount, int stackCount)
        {
            for (var i = 0; i < stackCount; ++i)
            {
                var k1 = i * (sectorCount + 1);
                var k2 = k1 + sectorCount + 1;

                for (var j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        _indices.Add((uint)k1);
                        _indices.Add((uint)k2);
                        _indices.Add((uint)k1 + 1);
                    }

                    // k1+1 => k2 => k2+1
                    if (i == stackCount - 1)
                    {
                        continue;
                    }
                    _indices.Add((uint)k1 + 1);
                    _indices.Add((uint)k2);
                    _indices.Add((uint)k2 + 1);
                }
            }
        }
    }
}