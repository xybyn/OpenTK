#version 330 core

out vec4 color;
in vec3 Normal;
in vec3 FragPos;

void main()
{
    // Ambient
    vec3 lightPosition = vec3(6, 10, 0);
    vec3 resultColor= vec3(0.56,0,0.1 );

    vec3 directionToLight = lightPosition - FragPos;
    float distance = length(directionToLight);
    directionToLight = normalize(directionToLight);
    float strength = max(dot(directionToLight, Normal), 0.2) ;
    color = vec4(vec3(0.9, 0.9, 0)*strength, 1);
}
