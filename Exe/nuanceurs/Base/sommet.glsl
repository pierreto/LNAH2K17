#version 420

uniform mat4 modelViewProjection;
uniform mat4 matrModel;
uniform mat3 matrNormale;
uniform vec3 positionCamera;

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoordIn;
layout (location = 2) in vec3 normale;
layout (location = 3) in vec3 primary_color;

out Attribs {
   vec4 color;
   vec3 normale;
   vec3 obsvec;
   vec2 texCoord;
   vec3 vertex;
   vec3 pos;
} AttribsOut;

void main()
{
	gl_Position = modelViewProjection * vec4(position, 1.0);
	AttribsOut.texCoord = texCoordIn;
	AttribsOut.color = vec4(primary_color, 1.0);
	AttribsOut.vertex = position;

	// Calcule de la normale
	AttribsOut.normale = matrNormale * normale;

	// Position du sommet
	vec3 pos = vec3(matrModel * vec4(position, 1.0f));
	AttribsOut.pos = pos;

	// Vecteur de la direction vers l'observateur
	AttribsOut.obsvec = normalize(positionCamera - pos);
}