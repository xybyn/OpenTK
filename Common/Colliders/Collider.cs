// unset

using GlmNet;
using OpenTK.Graphics.OpenGL4;
using OpenTKProject;

namespace Common
{
    public abstract class Collider : SceneObject
    {
        public Collider()
        {
            shader = new Shader(@"Shaders\3D\collider.vert", @"Shaders\3D\collider.frag");
        }
        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            vao.Bind();
            UpdateDefaultShader(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
        }

        public abstract bool IntersectsRay(vec3 rayDirection, vec3 rayOrigin, out float result);
    }
}