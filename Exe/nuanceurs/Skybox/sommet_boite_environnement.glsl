#version 420

uniform mat4 projection;
uniform mat4 modelView;

layout (location = 0) in vec3 position;

smooth out vec3 directionOeil;

void main() {
    vec4 pos = vec4(position, 1.0);
    mat4 projectionInverse = inverse(projection);
    mat3 modelViewInverse = transpose(mat3(modelView));
    vec3 deprojete = (projectionInverse * pos).xyz;
    directionOeil = modelViewInverse * deprojete;

    gl_Position = pos;
} 