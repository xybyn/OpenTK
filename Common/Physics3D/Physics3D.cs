// unset

using GlmNet;

namespace Common.Physics3D
{
    public static class Physics3D
    {
        public static vec3 ScreenPointToWorldRay(in vec2 screenPosition,in vec2 windowSize, in mat4 projection, in mat4 view)
        {
            var Width = windowSize.x;
            var Height = windowSize.y;
            var x = (2f * screenPosition.x) / Width - 1f;
            var y = 1f - (2f * screenPosition.y) / Height;
            var z = 1f;
            var rayNormalizedDeviceCoordinates = new vec3(x, y, z);
            var rayClip = new vec4(rayNormalizedDeviceCoordinates.x, rayNormalizedDeviceCoordinates.y, -1f, 1f);

            var rayEye = glm.inverse(projection) * rayClip;
            rayEye = new vec4(rayEye.x, rayEye.y, -1f, 0f);
            var rayWorldCoordinates = new vec3(glm.inverse(view) * rayEye);
            rayWorldCoordinates = glm.normalize(rayWorldCoordinates);
            return rayWorldCoordinates;
        }
    }
}