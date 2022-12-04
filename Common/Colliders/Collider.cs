﻿// unset

using Common._3D_Objects;
using Common.Misc;
using Common.Shaders;
using GlmNet;
using OpenTK.Graphics.OpenGL4;

namespace Common.Colliders
{
    public abstract class Collider : SceneObject3D
    {
        public bool ShowColliders { get; set; } = true;
        private const string DEFAULT_COLLIDER_VERT_SHADER = @"Shaders\3D\collider.vert";
        private const string DEFAULT_COLLIDER_FRAG_SHADER = @"Shaders\3D\collider.frag";
        public string Name { get; set; }
        protected Collider()
        {
            shader = new Shader(DEFAULT_COLLIDER_VERT_SHADER, DEFAULT_COLLIDER_FRAG_SHADER);
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            if (!ShowColliders)
                return;
            vao.Bind();
            UpdateDefaultShader(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
        }

        public abstract bool IsIntersectsRay(vec3 rayDirection, vec3 rayOrigin, out RaycastHit result);
    }
}