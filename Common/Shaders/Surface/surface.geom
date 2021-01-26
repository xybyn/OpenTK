#version 330 core
layout (triangles) in;
layout (points, max_vertices = 1) out;

void main() {
    vec3 v1 = gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz;
    vec3 v2 = gl_in[2].gl_Position.xyz - gl_in[0].gl_Position.xyz;
    vec3 v3 = cross(v1.xyz, v2.xyz);
    vec3 center = (gl_in[0].gl_Position.xyz + gl_in[1].gl_Position.xyz + gl_in[2].gl_Position.xyz)/3;
    gl_Position = vec4(center, 1);
    EmitVertex();
    gl_Position =  vec4(center+v3, 1);
    EmitVertex();
    EndPrimitive();
}  