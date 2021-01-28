#version 330 core
layout (location = 0) in vec3 a_Position;

uniform mat4 projection;
uniform mat4 translation;
uniform mat4 rotation;
uniform mat4 scaling;
uniform mat4 view;
uniform mat4 parentModel;
void main()
{

    mat4 resultModel = parentModel*translation*rotation*scaling;
    vec3 position = a_Position;
    gl_Position =
    projection*view*
    resultModel*
    vec4(position, 1.0);
}