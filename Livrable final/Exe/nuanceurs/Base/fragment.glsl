#version 420
#define MAX_LIGHT_SOURCES 10

struct Material {
    vec3 emission;
	vec3 ambiant;
	vec3 diffuse;
	vec3 speculaire;
	float shininess;
	float transparence;
};

uniform Material material;
uniform bool useDiffuseColor;
uniform bool disableSpeculaire;
uniform bool disableAmbiant;
uniform bool colorAppliedToTexture;

layout(binding=0) uniform sampler2D diffuseTex;
layout (std140, binding = 2) uniform LightSource
{									// Base alignment	// Aligned offset										
    int ouverte;					// 4				// 0
    int type;						// 4				// 4 
    vec4 ambiant;					// 16				// 16
    vec4 diffuse;					// 16				// 32
    vec4 speculaire;				// 16				// 48
    vec4 position;					// 16				// 64
    vec3 spotDirection;				// 12				// 80
    float spotExponent;				// 4			    // 92
    float spotCutoff;				// 4				// 96
    float constantAttenuation;		// 4				// 100
    float linearAttenuation;		// 4				// 104
    float quadraticAttenuation;		// 4				// 108
} Lights[MAX_LIGHT_SOURCES];

in Attribs {
    vec4 color;
    vec3 normale;
    vec3 obsvec;
    vec2 texCoord;
    vec3 vertex;
    vec3 pos;
} AttribsIn;

out vec4 color;

///////////////////////////////////////////////////////////////////////////////////////////////////
float calculerSpot(in int index,  in vec3 L)
{	
	if (Lights[index].type == 2) 
    {
        float LdotSD = dot(-L, normalize(Lights[index].spotDirection));

		if (LdotSD > cos(radians(Lights[index].spotCutoff))) 
        {
			float spotAttenuation = pow(LdotSD, Lights[index].spotExponent);

			// Attenuation de la distance
			float d = length(vec3(Lights[index].position) - AttribsIn.pos);
			float denominateur = Lights[index].constantAttenuation + Lights[index].linearAttenuation * d + Lights[index].quadraticAttenuation * pow(d, 2);
			float distanceAttenuation = min(1.0f, 1 / denominateur);

			return distanceAttenuation * spotAttenuation;
		}

		return 0.0f;
	}
	
	return 1.0f;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
vec4 calculerReflexion(in vec3 N, in vec3 O, in int index)
{
    if (Lights[index].ouverte == 0)
    {
        return vec4(0);
    }

    // Rayon incident
    vec3 L = -normalize(Lights[index].spotDirection);
    if (Lights[index].type != 1)
    {
        L = normalize(vec3(Lights[index].position) - AttribsIn.pos);
    }
    
    // Reflected ray
    vec3 R = reflect(-L, N);

    // Produits vectoriels utiles
    float NdotL = max (0.0, dot(N, L));
    float OdotR = max (0.0, dot(O, R));
    
    // Attenuation du spot
    float spotAttenuation = calculerSpot(index, L);
    
    // Lumiere ambiente
    vec4 ambiant = disableAmbiant ? vec4(0) : vec4(material.ambiant, 1.0f) * vec4(material.diffuse, 1.0f) * Lights[index].ambiant;
    vec4 diffuse = vec4(0);
    vec4 speculaire = vec4(0);
    
    if (Lights[index].type != 0)
    {
        // Lumiere diffuse 
        diffuse = vec4(material.diffuse, 1.0f) * Lights[index].diffuse * NdotL;
        // Lumiere Speculaire 
        speculaire = disableSpeculaire ? vec4(0) : Lights[index].speculaire * vec4(material.speculaire, 1.0f) * pow(OdotR, material.shininess);
    }
    
    // Retour de la lumiere
    return(spotAttenuation * (ambiant + diffuse + speculaire));
}

///////////////////////////////////////////////////////////////////////////////////////////////////
void main()
{
	vec3 N = normalize( AttribsIn.normale );
	vec3 O = normalize( AttribsIn.obsvec );
    
	// Calcul de la lumiere
	vec4 couleur = vec4(0);
	for (int i = 0; i < MAX_LIGHT_SOURCES; ++i)
	{
		couleur += calculerReflexion(N, O, i);
	}
	
	if(colorAppliedToTexture){
		color = (useDiffuseColor) ? couleur : couleur * texture2D(diffuseTex, AttribsIn.texCoord.st);
	}
	else{
		color = (useDiffuseColor) ? couleur : texture2D(diffuseTex, AttribsIn.texCoord.st);
	}
	color.a = material.transparence;  
	color = clamp(color, 0.0f, 1.0f);
}