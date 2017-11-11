//////////////////////////////////////////////////////////////////////////////
/// @file VuePerspective.h
/// @author DGI
/// @date 2016-11-25
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __UTILITAIRE_VUEPERSPECTIVE_H__
#define __UTILITAIRE_VUEPERSPECTIVE_H__


#include "Vue.h"
#include "Camera.h"
#include "ProjectionPerspective.h"

namespace vue {


	////////////////////////////////////////////////////////////////////////
	/// @class VuePerspective
	/// @brief Classe concrète de vue Perspective.
	///
	/// Cette classe implante le comportement attendu d'une vue Perspective.
	///
	/// @author Martin Bisson
	/// @date 2006-12-16
	////////////////////////////////////////////////////////////////////////
	class VuePerspective : public Vue
	{
	public:
		// Constructeur.
		VuePerspective(Camera const& camera, ProjectionPerspective const& projection);

		/// Obtention de la projection.
		virtual const ProjectionPerspective& obtenirProjection() const;

		/// Modification de la clotûre.
		virtual void redimensionnerFenetre(int largeur, int hauteur);
		virtual void setLargeurFenetre(int largeur, int hauteur) { };

		/// Zoom in, c'est-à-dire un agrandissement.
		virtual void zoomerIn();
		/// Zoom out, c'est-à-dire un rapetissement.
		virtual void zoomerOut();
		/// Zoom in/out élastique.
		virtual void zoomerElastique(const glm::ivec2& coin1,
			const glm::ivec2& coin2, bool zoomIn);
		/// Déplacement dans le plan XY par rapport à la vue.
		virtual void deplacerXY(double deplacementX, double deplacementY);
		/// Déplacement dans le plan XY par rapport à la vue.
		virtual void deplacerXY(const glm::ivec2& deplacement);
		/// Déplacement selon l'axe des Z par rapport à la vue.
		virtual void deplacerZ(double deplacement);
		/// Rotation selon les axes des X et des Y par rapport à la vue.
		virtual void rotaterXY(double rotationX, double rotationY);
		/// Rotation selon les axes des X et des Y par rapport à la vue.
		virtual void rotaterXY(const glm::ivec2& rotation);
		/// Rotation selon l'axe des Z par rapport à la vue.
		virtual void rotaterZ(double rotation);


	private:
		/// Projection utilisée pour cette vue.
		ProjectionPerspective projection_;

	};


}; // Fin de l'espace de nom vue.


#endif // __UTILITAIRE_VUEPERSPECTIVE_H__


   ///////////////////////////////////////////////////////////////////////////
   /// @}
   ///////////////////////////////////////////////////////////////////////////
