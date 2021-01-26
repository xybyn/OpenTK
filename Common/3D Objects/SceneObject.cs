// unset

using Common;
using GlmNet;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace OpenTKProject
{
    public abstract class SceneObject
    {
        public virtual mat4 Model
        {
            get
            {
                return  Parent.Model* translation * rotation * scaling;
            }
        }
        
        public virtual Material Material { get; set; }

        public bool IsShowingNormals { get; set; }

        public IReadOnlyCollection<SceneObject> Children => _children;

        public SceneObject Parent { get; protected set; }
        private vec3 _pos = new vec3(0);

        public vec3 localpos;
        public vec3 Rotation { get; protected set; }
        public float Scaling { get; protected set; }

        public void AttachTo(SceneObject parent)
        {
            Parent = parent;
            Parent.AddChild(this);
        }

        public mat4 translation = mat4.identity();
        public mat4 rotation = mat4.identity();
        public mat4 scaling = mat4.identity();
        protected VAO vao;
        protected Shader shader;
        private List<SceneObject> _children = new List<SceneObject>();
        private readonly Shader _normalsShader;
        public vec3 scale = new vec3(1);

        public SceneObject()
        {
            if (Parent == null && this is not World)
                Parent = new World();
            
            shader ??= new Shader(@"Shaders\3D\default.vert", @"Shaders\3D\default.frag");
            _normalsShader = new Shader(@"Shaders\normal.vert", @"Shaders\normal.frag", @"Shaders\normal.geom");
            Material = new Material();
            if (Material.Color == null)
            {
                Material.Color = new vec3(0.5f, 0.5f, 0.5f);
            }
            _color = Material.Color;
        }
        
        public void TranslateGlobal(vec3 newPosition)
        {
            localpos = new vec3(Parent.Model * new vec4(newPosition, 1));
            translation = glm.translate(mat4.identity(), newPosition);
        }

        public vec3 Pos
        {
            get
            {
                return new vec3(Parent.Model * new vec4(localpos, 1));
            }
        }

        private vec3 _color;
        public void EnableHighlighting()
        {
            _color = Material.Color;
            Material.Color += new vec3(1) * 0.3f;
        }
        public void DisableHighlighting()
        {
            Material.Color = _color;
        }

        public void Scale(vec3 newScale)
        {
            scale = newScale;
            scaling = glm.scale(mat4.identity(), newScale);
        }

        public void Rotate(float angle, vec3 pivot)
        {
            var temp = localpos;
            rotation = glm.rotate(angle, pivot);
            
            //TranslateGlobal(temp);
        }

        protected void AddChild(SceneObject newChild)
        {
            _children.Add(newChild);
        }
        protected int indicesCount = 0;
        public virtual void Draw(ref mat4 view, ref mat4 projection)
        {
            vao.Bind();
            UpdateDefaultShader(ref view, ref projection);
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            if (IsShowingNormals)
            {
                var model = Model;
                DrawNormals(ref model, ref view, ref projection);
                GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            }
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
        private VBO vbo;
        private EBO ebo;
        protected void InitializeVAO_VBO_EBO(float[] vertices, float[] normals, uint[] indices)
        {
            vbo = new(
                null,
                (sizeof(float) * vertices.Length) + (sizeof(float) * normals.Length),
                new[]
                {
                    new Subdata
                    {
                        Data = vertices,
                        Index = 0,
                        SizeInBytes = sizeof(float) * vertices.Length
                    },
                    new Subdata
                    {
                        Data = normals,
                        Index = sizeof(float) * vertices.Length,
                        SizeInBytes = sizeof(float) * normals.Length
                    }
                });
            indicesCount = indices.Length;
            ebo = new(indices);
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
                (sizeof(float) * vertices.Length));
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

        protected  virtual void UpdateDefaultShader(ref mat4 view, ref mat4 projection)
        {
            var parentModel = Parent.Model;
            shader.SetMat4("parentModel", ref parentModel);
            shader.Use();
            var color = Material.Color;
            shader.SetVec3("color", ref color);
            shader.SetMat4("view", ref view);
            shader.SetMat4("translation", ref translation);
            shader.SetMat4("scaling", ref scaling);
            shader.SetMat4("rotation", ref rotation);
            shader.SetMat4("projection", ref projection);
        }

        protected void DrawNormals(ref mat4 model, ref mat4 view, ref mat4 projection)
        {
            _normalsShader.Use();
            _normalsShader.SetMat4("projection", ref projection);
            _normalsShader.SetMat4("model", ref model);
            _normalsShader.SetMat4("view", ref view);
        }
    }
}