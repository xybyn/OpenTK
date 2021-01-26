// unset

using GlmNet;
using System.Collections.Generic;

namespace Common
{
    public static class ListVector3Extension
    {
        public static float[] ToSingleArray(this List<vec3> list)
        {
            float[] result = new float[list.Count * 3];
            for (int i = 0; i < list.Count; i++)
            {
                result[i * 3] = list[i].x;
                result[(i * 3) + 1] = list[i].y;
                result[(i * 3) + 2] = list[i].z;
            }
            return result;
        }

        public static float[] ToSingleArray(this List<List<vec3>> list)
        {
            var result = new float[list.Count * list[0].Count * 3];

            for (int i = 0; i < list.Count; i++)
            {
                var cont = list[i];
                for (int j = 0; j < cont.Count; j++)
                {
                    result[j * 3 + 0 + i * cont.Count * 3] = cont[j].x;
                    result[j * 3 + 1 + i * cont.Count * 3] = cont[j].y;
                    result[j * 3 + 2 + i * cont.Count * 3] = cont[j].z;
                }
            }
            return result;
        }
    }
}