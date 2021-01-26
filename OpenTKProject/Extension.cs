// unset

using OpenTK.Mathematics;

namespace OpenTKProject
{
    public static class Extension
    {
        public static float[] ToArray(this Matrix4 matrix)
        {
            var array = new float[16];
            array[0] = matrix.Row0[0];
            array[1] = matrix.Row0[1];
            array[2] = matrix.Row0[2];
            array[3] = matrix.Row0[3];

            array[4] = matrix.Row1[0];
            array[5] = matrix.Row1[1];
            array[6] = matrix.Row1[2];
            array[7] = matrix.Row1[3];

            array[8] = matrix.Row2[0];
            array[9] = matrix.Row2[1];
            array[10] = matrix.Row2[2];
            array[11] = matrix.Row2[3];

            array[12] = matrix.Row2[0];
            array[13] = matrix.Row2[1];
            array[14] = matrix.Row2[2];
            array[15] = matrix.Row2[3];
            return array;
        }
    }
}