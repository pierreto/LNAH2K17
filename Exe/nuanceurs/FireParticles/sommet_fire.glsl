#version 410

uniform mat4 ModelMat;
uniform mat4 ViewMat;

layout(location=0) in vec4 Vertex;


void main( void ) {
   gl_Position = ViewMat * ModelMat * Vertex;
   gl_PointSize = 1.0f;
}
