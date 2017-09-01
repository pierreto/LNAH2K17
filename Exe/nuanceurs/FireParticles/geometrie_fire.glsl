#version 410

uniform mat4 ProjMat;
uniform mat4 ViewMat;

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

out Attribs {
   vec2 texCoord;
} AttribsOut;


void main() {
   // Initialiser les coins
   vec2 corners[4];
   corners[0] = vec2( -0.5,  0.5 );
   corners[1] = vec2( -0.5, -0.5 );
   corners[2] = vec2(  0.5,  0.5 );
   corners[3] = vec2(  0.5, -0.5 );
   
   // Initialiser les coordonées de la textures
   vec2 texCoord[4];
   texCoord[0] = vec2(  0.0,  1.0 );
   texCoord[1] = vec2(  0.0,  0.0 );
   texCoord[2] = vec2(  1.0,  1.0 );
   texCoord[3] = vec2(  1.0,  0.0 );
   
   // Créer un vertex pour chaque coin et envoyer la position de la texture      
   for ( int i = 0 ; i < 4 ; ++i )
   {
      vec2 dpixels = corners[i] * gl_in[0].gl_PointSize;
	  vec4 pos = vec4( gl_in[0].gl_Position.xy + dpixels, gl_in[0].gl_Position.zw );

      gl_Position = ProjMat * pos;   
      AttribsOut.texCoord = texCoord[i];
      EmitVertex();
   }	
}
