// unset

using Common.Buffers;
using Common.Extensions;
using Common.Misc;
using Common.Shaders;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Common._3D_Objects
{
    public abstract class SceneObject3D
    {
        private const string DEFAULT_SCENE_OBJECT_SHADER_VERTEX_PATH = @"Shaders\3D\default.vert";
        private const string DEFAULT_SCENE_OBJECT_SHADER_FRAGMENT_PATH = @"Shaders\3D\default.frag";
        private const string DEFAULT_NORMALS_SHADER_VERTEX_PATH = @"Shaders\normal.vert";
        private const string DEFAULT_NORMALS_SHADER_FRAGMENT_PATH = @"Shaders\normal.frag";
        private const string DEFAULT_NORMALS_SHADER_GEOM_PATH = @"Shaders\normal.geom";
        private readonly List<SceneObject3D> _children = new();
        private readonly Shader _normalsShader;

        private EBO ebo;

        protected int indicesCount;

        private mat4 _rotationMatrix = mat4.identity();
        private mat4 _scaling = mat4.identity();
        protected Shader shader;

        public mat4 translation = mat4.identity();
        protected VAO vao;

        private VBO vbo;

        public SceneObject3D()
        {
            if (Parent == null && this is not World)
            {
                Parent = new World();
            }

            shader ??= new Shader(DEFAULT_SCENE_OBJECT_SHADER_VERTEX_PATH,
                DEFAULT_SCENE_OBJECT_SHADER_FRAGMENT_PATH);

            Material ??= new Material
            {
                Color = new vec3(0.5f)
            };

            _normalsShader = new Shader(
                DEFAULT_NORMALS_SHADER_VERTEX_PATH,
                DEFAULT_NORMALS_SHADER_FRAGMENT_PATH,
                DEFAULT_NORMALS_SHADER_GEOM_PATH);
        }

        public virtual mat4 Model => Parent.Model * translation * _rotationMatrix * _scaling;

        public Material Material { get; set; }

        public bool ShowNormals { get; set; }

        public IReadOnlyCollection<SceneObject3D> Children => _children;

        public SceneObject3D Parent { get; protected set; }

        public vec3 WorldPosition => new(Parent.Model * new vec4(LocalPosition, 1));

        public vec3 LocalPosition { get; private set; }

        public vec3 LocalScaling { get; private set; } = new vec3(1);

        public void AttachTo(SceneObject3D parent)
        {
            Parent = parent;
            Parent.AddChild(this);
        }

        public void TranslateWorld(vec3 newPosition)
        {
            LocalPosition = new vec3(glm.inverse(Parent.Model) * new vec4(newPosition, 1));
            translation = glm.translate(mat4.identity(), LocalPosition);
        }

        public void TranslateLocal(vec3 newPosition)
        {
            LocalPosition = newPosition;
            translation = glm.translate(mat4.identity(), newPosition);
        }

        public void ScaleWorld(vec3 newScale)
        {
            LocalScaling = new vec3(glm.inverse(Parent.Model) * new vec4(newScale, 1));
            translation = glm.translate(mat4.identity(), LocalScaling);
        }

        public void ScaleLocal(vec3 newScale)
        {
            LocalScaling = newScale;
            _scaling = glm.scale(mat4.identity(), newScale);
        }

        //TODO: quaternion and euler rotations
        public void RotateLocal(float angle, vec3 pivot)
        {
            vec3 temp = LocalPosition;
            _rotationMatrix = glm.rotate(angle, pivot);
        }

        protected void AddChild(SceneObject3D newChild)
        {
            _children.Add(newChild);
        }

        public virtual void Draw(ref mat4 view, ref mat4 projection)
        {
            vao.Bind();
            UpdateDefaultShader(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            if (!ShowNormals)
            {
                return;
            }
            DrawNormals(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
        }

        protected void InitializeVAO_VBO_EBO(List<vec3> vertices, List<vec3> normals, List<uint> indices)
        {
            InitializeVAO_VBO_EBO(vertices.ToSingleArray(), normals.ToSingleArray(), indices.ToArray());
        }

        protected void ClearBuffers()
        {
            vao?.Free();
            vbo?.Free();
            ebo?.Free();
        }

        protected void InitializeVAO_VBO_EBO(float[] vertices, float[] normals, uint[] indices)
        {
            vbo = new VBO(
                null,
                (sizeof(float) * vertices.Length) + (sizeof(float) * normals.Length),
                new[]
                {
                    new SubData
                    {
                        Data = vertices, Index = 0, SizeInBytes = sizeof(float) * vertices.Length
                    },
                    new SubData
                    {
                        Data = normals, Index = sizeof(float) * vertices.Length, SizeInBytes = sizeof(float) * normals.Length
                    }
                });
            indicesCount = indices.Length;
            ebo = new EBO(indices);
            vao = new VAO(vbo, ebo,
                new[]
                {
                    new VertexAttribPointer
                    {
                        Index = 0,
                        Normalize = false,
                        OffsetInBytes = 0,
                        Size = 3,
                        StrideInBytes = 3 * sizeof(float),
                        Type = VertexAttribPointerType.Float
                    },
                    new VertexAttribPointer
                    {
                        Index = 1,
                        Normalize = false,
                        OffsetInBytes = vertices.Length * sizeof(float),
                        Size = 3,
                        StrideInBytes = 3 * sizeof(float),
                        Type = VertexAttribPointerType.Float
                    }
                });
        }

        protected void InitializeVAO_VBO_EBO(float[] vertices, uint[] indices)
        {
            VBO vbo = new(
                vertices,
                sizeof(float) * vertices.Length);
            indicesCount = indices.Length;
            EBO ebo = new(indices);
            vao = new VAO(vbo, ebo,
                new[]
                {
                    new VertexAttribPointer
                    {
                        Index = 0,
                        Normalize = false,
                        OffsetInBytes = 0,
                        Size = 3,
                        StrideInBytes = 3 * sizeof(float),
                        Type = VertexAttribPointerType.Float
                    }
                });
        }

        protected void InitializeVAO_VBO_EBO(List<vec3> vertices, List<uint> indices)
        {
            InitializeVAO_VBO_EBO(vertices.ToSingleArray(), indices.ToArray());
        }

        protected virtual void UpdateDefaultShader(ref mat4 view, ref mat4 projection)
        {
            var parentModel = Parent.Model;
            var color = Material.Color;
            
            shader.SetMat4("parentModel", ref parentModel);
            shader.SetVec3("color", ref color);
            shader.SetMat4("view", ref view);
            shader.SetMat4("translation", ref translation);
            shader.SetMat4("scaling", ref _scaling);
            shader.SetMat4("rotation", ref _rotationMatrix);
            shader.SetMat4("projection", ref projection);
        }

        private void DrawNormals(ref mat4 view, ref mat4 projection)
        {
            var model = Model;
            _normalsShader.Use();
            _normalsShader.SetMat4("projection", ref projection);
            _normalsShader.SetMat4("model", ref model);
            _normalsShader.SetMat4("view", ref view);
        }
    }
}