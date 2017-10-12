///////////////////////////////////////////////////////////////////////////////
/// @file Table.h
/// @author Nam Lesage
/// @date 2015-10-13
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "utilitaire.h"
#include <vector>
#include "GL/glew.h"
#include "Materiau.h"
/// Nombre de points par coin
#define VERTEX_PER_CORNER 5
/// Largeur de la bordure
#define TABLE_BORDER_WIDTH 11.5f
/// Hauteur 
#define TABLE_BORDER_HEIGHT 3.5f
/// Hauteur de la base
#define TABLE_BASE_HEIGHT 15.0f

namespace table
{
	typedef std::vector<glm::vec3> Sommets;
	typedef Sommets Couleurs;


	/// Obtient les sommets de la table
	Sommets obtenirSommets();
	/// Obtient les faces de la table
	std::vector<GLuint> obtenirFaces();
	/// Obtient le matériau de la table
	modele::Materiau obtenirMateriau();
	/// Obtient les coordonnées de texture
	std::vector<glm::vec2> obtenirTexCoords();
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
