#version 410

uniform sampler2D Texture;

in Attribs {
   vec2 texCoord;
} AttribsIn;


void main( void ) {
    // Appliquer la texture au fragment
    gl_FragColor = texture( Texture, AttribsIn.texCoord );
}
