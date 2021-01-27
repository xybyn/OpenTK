// unset

using Common._3D_Objects;
using Common.Drawers.Settings;
using Common.Extensions;
using Common.Shaders;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Common.Drawers
{
    public class ParametricFunctionDrawer3D : SceneObject3D
    {
        private readonly ParametricFunctionDrawerSettings _settings;
        private readonly List<vec3> _vertices = new();
        private readonly List<uint> _indices = new();

        public ParametricFunctionDrawer3D(Func<float, vec3> func, ParametricFunctionDrawerSettings settings)
        {
            _settings = settings;
            shader = new Shader(@"Shaders\3D\parametricFunction.vert", @"Shaders\3D\parametricFunction.frag");
            SetFunction(func);
        }

        public void SetFunction(Func<float, vec3> func)
        {
            _vertices.Clear();
            _indices.Clear();
            ClearBuffers();

            var step = (_settings.MaxParameterValue - _settings.MinParameterValue) / (_settings.CountOfDivisions - 1);
            var t = _settings.MinParameterValue;
            for (int i = 0; i < _settings.CountOfDivisions; i++)
            {
                _vertices.Add(func(t));
                t += step;
            }

            for (uint i = 0; i < _vertices.Count - 1; i++)
            {
                _indices.Add(i);
                _indices.Add(i + 1);
            }
            InitializeVAO_VBO_EBO(_vertices.ToSingleArray(), _indices.ToArray());
        }

        public override void Draw(ref mat4 view, ref mat4 projection)
        {
            if (vao == null)
            {
                return;
            }
            vao.Bind();
            UpdateDefaultShader(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
    }
}