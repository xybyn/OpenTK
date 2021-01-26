// unset

using GlmNet;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace Common.Colliders
{
    public class PlaneCollider : Collider
    {
        private Plane _plane;

        private Plane Plane
        {
            get
            {
                var inverted = (glm.inverse(Parent.Model));
                float t;
                for(int i = 0; i < 4; ++i)
                {
                    for(int j = i; j < 4; ++j)
                    {
                        t = inverted[i,j];
                        inverted[i,j] = inverted[j,i];
                        inverted[j,i] = t;
                    }
                }
                _plane.D = Pos;
                
                return new Plane
                {
                    D = Pos,
                    Normal = new vec3(inverted * new vec4(_plane.Normal, 1))
                };
            }
        }
        public PlaneCollider(vec3 normal, vec3 pos)
        {
            _plane = new Plane
            {
                Normal = normal,
                D = pos
            };
            var vertices = new List<vec3>
            {
                new vec3(-0.5f, 0, -0.5f),
                new vec3(0.5f, 0, -0.5f),
               new vec3( 0.5f, 0, 0.5f), new vec3(-0.5f, 0, 0.5f)
            };
            var indices = new uint[]
            {
                0, 1, 1, 2, 2, 3, 3, 0
            };
            var target = normal;
            var upVec = new vec3(0, 1, 0);
            var lookAtMat = mat4.identity();
            var gamma = Acos(glm.dot(target, upVec) / (Sqrt(glm.dot(upVec, upVec)) * Sqrt(glm.dot(target, target))));
            if (gamma == MathF.PI)
            {
                Rotate(gamma, new vec3(1, 0, 0));
                //Plane.Normal *= -1;
            }
            else if (gamma != 0)
            {
                var right = glm.cross(upVec, target);
                Rotate(gamma, right);
                //Plane.Normal *= -1;
            }
            
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = new vec3(lookAtMat * new vec4(vertices[i], 1));
            }
            InitializeVAO_VBO_EBO(vertices.ToSingleArray(), indices);
            Material.Color = new vec3(0, 1, 0);
            TranslateGlobal(pos);
 
        }
        
        public override bool IntersectsRay(vec3 rayDirection, vec3 rayOrigin, out float result)
        {
            var V = rayDirection;
            var P = rayOrigin;
            var domin = glm.dot(Plane.Normal, V);
            if (domin == 0)
            {
                result = 0;
                return false;
            }
            //plane.D = new vec3(Model * new vec4(plane.D, 1));
  
            var t = (-1 * glm.dot(Plane.Normal, P) + glm.dot(Plane.Normal, Plane.D)) / domin;
            var intersectionPoint = rayOrigin + t * rayDirection;
            intersectionPoint = new vec3(glm.inverse(Model) * new vec4(intersectionPoint, 1));
            if (intersectionPoint.x > scale.x/2f || intersectionPoint.x < -scale.x/2f
                                           || intersectionPoint.z > scale.y/2f || intersectionPoint.z < -scale.y/2f)
            {
                result = 0;
                return false;
                
            }
            result = t;
            return true;
        }
    }
}