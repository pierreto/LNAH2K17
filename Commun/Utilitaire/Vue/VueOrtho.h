//////////////////////////////////////////////////////////////////////////////
/// @file VueOrtho.h
/// @author DGI
/// @date 2006-12-16
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __UTILITAIRE_VUEORTHO_H__
#define __UTILITAIRE_VUEORTHO_H__


#include "Vue.h"
#include "Camera.h"
#include "ProjectionOrtho.h"

namespace vue {


	////////////////////////////////////////////////////////////////////////
	/// @class VueOrtho
	/// @brief Classe concrète de vue orthogonale.
	///
	/// Cette classe implante le comportement attendu d'une vue orthogonale.
	///
	/// @author Martin Bisson
	/// @date 2006-12-16
	////////////////////////////////////////////////////////////////////////
	class VueOrtho : public Vue
	{
	public:
		// Constructeur.
		VueOrtho(Camera const& camera, ProjectionOrtho const& projection);

		/// Obtention de la projection.
		virtual const ProjectionOrtho& obtenirProjection() const;

		/// Modification de la clotûre.
		virtual void redimensionnerFenetre(int largeur, int hauteur);
		void setLargeurFenetre(int largeur, int hauteur);

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
		ProjectionOrtho projection_;

	};


}; // Fin de l'espace de nom vue.


#endif // __UTILITAIRE_VUEORTHO_H__


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
