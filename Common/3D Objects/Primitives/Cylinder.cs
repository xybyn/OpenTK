// unset

using Common._3D_Objects;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Common
{
    public class Cylinder : SceneObject3D
    {
        private readonly List<float> _normals = new();
        private readonly List<float> _vertices = new();

        public Cylinder(float radius, float height, int sectorCount = 20)
        {
            List<float> unitVertices = AppUtils.GetCircle(1, sectorCount);

            for (int i = 0; i < 2; ++i)
            {
                float h = (-height / 2.0f) + (i * height); // z value; -h/2 to h/2
                float t = 1.0f - i; // vertical tex coord; 1 to 0

                for (int j = 0, k = 0; j <= sectorCount; ++j, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k + 2];
                    float uz = unitVertices[k + 1];
                    // position vector
                    _vertices.Add(ux * radius); // vx
                    _vertices.Add(h); // vz
                    _vertices.Add(uy * radius); // vy
                    // normal vector
                    _normals.Add(ux); // nx
                    _normals.Add(uz); // nz// t
                    _normals.Add(uy); // ny
                }
            }

            // the starting index for the base/top surface
            //NuOTE: it is used for generating indices later
            int baseCenterIndex = _vertices.Count() / 3;
            int topCenterIndex = baseCenterIndex + sectorCount + 1; // include center vertex

            // put base and top vertices to arrays
            for (int i = 0; i < 2; ++i)
            {
                float h = (-height / 2.0f) + (i * height); // z value; -h/2 to h/2
                float nz = -1 + (i * 2); // z value of normal; -1 to 1

                // center point
                _vertices.Add(0);
                _vertices.Add(h);
                _vertices.Add(0);
                _normals.Add(0);
                _normals.Add(nz);
                _normals.Add(0);

                for (int j = 0, k = 0; j < sectorCount; ++j, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k + 1];
                    // position vector
                    _vertices.Add(ux * radius); // vx
                    _vertices.Add(h); // vz
                    _vertices.Add(uy * radius); // vy
                    // normal vector
                    _normals.Add(0); // nx
                    _normals.Add(nz); // nz// t
                    _normals.Add(0); // ny
                }
            }

            List<uint> indices = new List<uint>();
            int k1 = 0; // 1st vertex index at base
            int k2 = sectorCount + 1; // 1st vertex index at top

            // indices for the side surface
            for (int i = 0; i < sectorCount; ++i, ++k1, ++k2)
            {
                // 2 triangles per sector
                // k1 => k1+1 => k2
                indices.Add((uint)k1);
                indices.Add((uint)k1 + 1);
                indices.Add((uint)k2);

                // k2 =>Addk2+1
                indices.Add((uint)k2);
                indices.Add((uint)k1 + 1);
                indices.Add((uint)k2 + 1);
            }

            // indices for the base surface
            //NOTE: baseCenterIndex and topCenterIndices are pre-computed during vertex generation
            //      please see the previous code snippet
            for (int i = 0, k = baseCenterIndex + 1; i < sectorCount; ++i, ++k)
            {
                if (i < sectorCount - 1)
                {
                    indices.Add((uint)baseCenterIndex);
                    indices.Add((uint)k + 1);
                    indices.Add((uint)k);
                }
                else // last triangle
                {
                    indices.Add((uint)baseCenterIndex);
                    indices.Add((uint)baseCenterIndex + 1);
                    indices.Add((uint)k);
                }
            }

            // indices for the top surface
            for (int i = 0, k = topCenterIndex + 1; i < sectorCount; ++i, ++k)
            {
                if (i < sectorCount - 1)
                {
                    indices.Add((uint)topCenterIndex);
                    indices.Add((uint)k);
                    indices.Add((uint)k + 1);
                }
                else // lastAdd
                {
                    indices.Add((uint)topCenterIndex);
                    indices.Add((uint)k);
                    indices.Add((uint)topCenterIndex + 1);
                }
            }

            InitializeVAO_VBO_EBO(_vertices.ToArray(), _normals.ToArray(), indices.ToArray());
        }
    }
}