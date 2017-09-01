///////////////////////////////////////////////////////////////////////////////
/// @file but.cpp
/// @author Nam Lesage
/// @date 2015-10-13
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "utilitaire.h"
#include "But.h"
#include "GL/glew.h"


namespace but
{

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn std::vector<GLuint> obtenirFaces()
	///
	/// Cette fonction obtient toutes les faces de la table
	///
	/// @return Les faces.
	///
	////////////////////////////////////////////////////////////////////////
	std::vector<GLuint> obtenirFaces()
	{
		GLuint faces[] = {
			0,3,1,
			0,2,3,
			1,5,7,
			1,3,5,
			6,7,5,
			6,5,4,
			0,6,4,
			0,4,2,
			2,5,3,
			2,4,5,
			10,1,11,
			10,0,1,
			7,11,1,
			7,9,11,
			6,9,7,
			6,8,9,
			8,6,0,
			8,0,10,
			8,11,9,
			8,10,11
		};

		return std::vector<GLuint>(std::begin(faces), std::end(faces));;
	}

	modele::Materiau obtenirMateriau()
	{
		modele::Materiau materiau;
		materiau.ambiant_ = glm::vec3(0.1f, 0.1f, 0.1f);
		materiau.diffuse_ = glm::vec3(1.0f, 1.0f, 1.0f);
		materiau.speculaire_ = glm::vec3(0);
		materiau.shininess_ = 0;
		materiau.shininessStrength_ = 0;
		materiau.opacite_ = 1;
		materiau.emission_ = glm::vec3(0.0f, 0.0f, 0.0f);
		return materiau;
	}


} // namespace but
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
