// unset

using GlmNet;
using OpenTK.Windowing.Common;

namespace Common.Cameras
{
    public class TargetCamera
    {
        private mat4 projection;
        private vec3 targetPosition;
        public TargetCamera(vec3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            
        }
    }
}