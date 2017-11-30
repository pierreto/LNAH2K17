#version 410

layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

in Attribs {
   vec4 color;
   vec3 normale;
   vec3 obsvec;
   vec2 texCoord;
   vec3 vertex;
   vec3 pos;
} AttribsIn[];

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
    vec3 norm;
   
	vec3 v1 = AttribsIn[1].pos - AttribsIn[0].pos;
	vec3 v2 = AttribsIn[2].pos - AttribsIn[0].pos;
	norm = cross(normalize(v1), normalize(v2));

   // émettre les sommets
   for ( int i = 0 ; i < gl_in.length() ; ++i )
   {
      gl_Position = gl_in[i].gl_Position;
      AttribsOut.color = AttribsIn[i].color;
      AttribsOut.normale = norm;
      AttribsOut.obsvec = AttribsIn[i].obsvec;
      AttribsOut.texCoord = AttribsIn[i].texCoord;
	  AttribsOut.vertex = AttribsIn[i].vertex;
	  AttribsOut.pos = AttribsIn[i].pos;

      EmitVertex();
   }

    EndPrimitive();
}