#version 330 core

out vec4 outColor;
in vec3 vertout_Normal;
in vec3 vertout_FragPos;
uniform vec3 color;
void main()
{
    vec3 lightPosition = vec3(2, 4, 0);
    vec3 lightColor = vec3(1, 1, 1);
    vec3 resultColor= vec3(0.56,0,0.1 );
    vec3 objectColor=color;
    // Ambient
    float ambientStrength = 0.5f;
    vec3 ambient = ambientStrength * lightColor;
    //diffuse
    vec3 directionToLight = lightPosition - vertout_FragPos;
    float distance = length(directionToLight);
    directionToLight = normalize(directionToLight);
    float strength = max(dot(directionToLight, vertout_Normal), 0.15) ;
    vec3 diffuse = lightColor *strength;

    vec3 result = (ambient+diffuse)*objectColor;
    outColor = vec4(result, 1);
}
