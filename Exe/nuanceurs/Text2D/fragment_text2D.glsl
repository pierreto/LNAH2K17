#version 420

in vec2 TexCoords;

uniform sampler2D text;
uniform vec4 textColor;

void main() {    
    gl_FragColor = vec4(textColor.rgb, texture2D(text, TexCoords).r * textColor.a);
}  