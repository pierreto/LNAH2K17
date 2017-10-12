#version 420

layout(binding=0) uniform samplerCube diffuseTex;

smooth in vec3 directionOeil;

out vec4 color;

void main() {
    color = texture(diffuseTex, directionOeil);
}