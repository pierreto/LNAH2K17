////////////////////////////////////////////////////////////////////////////////////
/// @file Projection.cpp
/// @author DGI
/// @date 2006-12-15
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "GL/glew.h"
#include "Projection.h"
#include "glm/gtc/type_ptr.hpp"


namespace vue {


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn Projection::Projection(int largeurCloture, int hauteurCloture, double zAvant,  double zArriere, double zoomInMax, double zoomOutMax, double incrementZoom, bool estPerspective)
	///
	/// Constructeur d'une projection.  Ne fait qu'assigner les variables
	/// membres.
	///
	/// @param[in] largeurCloture   : dimension en @a X de la clôture.
	/// @param[in] hauteurCloture   : dimension en @a Y de la clôture.
	/// @param[in] zAvant         : distance du plan avant (en @a z).
	/// @param[in] zArriere       : distance du plan arrière (en @a z).
	/// @param[in] zoomInMax      : facteur de zoom in maximal.
	/// @param[in] zoomOutMax     : facteur de zoom out maximal.
	/// @param[in] incrementZoom  : distance du plan arrière (en @a z).
	/// @param[in] estPerspective : vrai si la projection est perspective.
	/// 
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	Projection::Projection(int largeurCloture, int hauteurCloture,
		double zAvant, double zArriere,
		double zoomInMax, double zoomOutMax,
		double incrementZoom, bool estPerspective) :
		largeurCloture_{ largeurCloture },
		hauteurCloture_{ hauteurCloture },
		zAvant_{ zAvant },
		zArriere_{ zArriere },
		zoomInMax_{ zoomInMax },
		zoomOutMax_{ zoomOutMax },
		incrementZoom_{ incrementZoom },
		estPerspective_{ estPerspective }
	{
	}

}; // Fin du namespace vue.


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
