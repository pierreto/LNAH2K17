#version 420

uniform mat4 matrProj;

layout(location=0) in vec2 Vertex;
layout(location=3) in vec4 Color;

out Attribs {
   vec4 Color;
} AttribsOut;


void main() {
    gl_Position = matrProj * vec4(Vertex, 0.0, 1.0);
    AttribsOut.Color = Color;
}  