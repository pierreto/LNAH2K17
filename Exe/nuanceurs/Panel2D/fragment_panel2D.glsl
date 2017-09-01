#version 420

in Attribs {
   vec4 Color;
} AttribsIn;


void main() {
    gl_FragColor = AttribsIn.Color;
}  