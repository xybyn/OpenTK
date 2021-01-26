// unset

using GlmNet;
using OpenTKProject;

namespace Common
{
    public class World : SceneObject
    {
        public override mat4 Model=>mat4.identity();
        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            
        }
    }
}