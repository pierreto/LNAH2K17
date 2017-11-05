#ifndef __ARBRE_NOEUDS_OPTIONSDESSIN_H__
#define __ARBRE_NOEUDS_OPTIONSDESSIN_H__

#include "glm\glm.hpp"

///////////////////////////////////////////////////////////////////////////
/// @struct VBO
/// @brief // Options a envoye a la fonction de dessin du vbo
///
///
/// @author Nam Lesage
/// @date 2015-10-05
///////////////////////////////////////////////////////////////////////////
struct OptionsDessin
{
	bool effetFantome_;
	glm::vec4 localSelectionColor_;

	bool useOtherColor_;
	bool invisible_;
	glm::vec4 color_;
	

	OptionsDessin(const bool& effetFantome = false, const bool& useOtherColor = false, const bool& invisible = false, const glm::vec4& color = glm::vec4(1))
		: effetFantome_(effetFantome), useOtherColor_(useOtherColor), invisible_(invisible), color_(color)
	{
		localSelectionColor_ = glm::vec4(0, 0, 1,0.25f);
	}
};

struct MatricesPipeline
{
	glm::mat4 matrProj;
	glm::mat4 matrModel;
	glm::mat4 matrVisu;
	glm::vec3 positionCamera;
};


#endif //__ARBRE_NOEUDS_OPTIONSDESSIN_H__