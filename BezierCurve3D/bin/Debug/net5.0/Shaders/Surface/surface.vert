#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 normal;
uniform mat4 projection;
uniform mat4 model;
uniform mat4 view;
out vec3 Normal;
out vec3 FragPos;
void main()
{
    Normal = normal;
    gl_Position = projection*view*model*vec4(aPosition, 1.0);
    FragPos = vec3(model*vec4(aPosition, 1.0));
    gl_PointSize = 2.0;
}