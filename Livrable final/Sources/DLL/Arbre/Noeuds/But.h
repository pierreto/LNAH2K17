///////////////////////////////////////////////////////////////////////////////
/// @file but.h
/// @author Nam Lesage
/// @date 2015-10-13
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __ARBRE_NOEUDS_BUT_H__
#define __ARBRE_NOEUDS_BUT_H__

#include "utilitaire.h"
#include <vector>
#include "GL/glew.h"
#include "Materiau.h"

namespace but
{
	/// Obtient les faces de la table
	std::vector<GLuint> obtenirFaces();

	modele::Materiau obtenirMateriau();
	
}


#endif // __ARBRE_NOEUDS_BUT_H__
///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
