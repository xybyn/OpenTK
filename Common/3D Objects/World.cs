// unset

using Common._3D_Objects;
using GlmNet;

namespace Common
{
    public class World : SceneObject3D
    {
        public override mat4 Model => mat4.identity();

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
        }
    }
}