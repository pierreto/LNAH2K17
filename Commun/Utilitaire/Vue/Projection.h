//////////////////////////////////////////////////////////////////////////////
/// @file Projection.h
/// @author DGI
/// @date 2006-12-15
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __UTILITAIRE_PROJECTION_H__
#define __UTILITAIRE_PROJECTION_H__


#include "glm/glm.hpp"

namespace vue {


	////////////////////////////////////////////////////////////////////////
	/// @class Projection
	/// @brief Classe contr�lant la projection du monde � la fen�tre.
	///
	/// Cette classe offre certaines fonctionnalit�s commune � chacun des types
	/// de projection (isom�trique, perspective, etc.).  Elle d�finit une
	/// interface que doivent implanter les types concrets de projection,
	/// c'est-�-dire permettre de modifier le facteur de zoom par exemple.
	///
	/// @author Martin Bisson
	/// @date 2006-12-15
	////////////////////////////////////////////////////////////////////////
	class Projection
	{
	public:
		/// Constructeur.
		Projection(int largeurCloture, int hauteurCloture,
			double zAvant, double zArriere,
			double zoomInMax, double zoomOutMax,
			double incrementZoom, bool estPerspective);

		/// Destructeur virtuel vide.
		virtual ~Projection() {}

		/// Zoom in, c'est-�-dire un agrandissement.
		virtual void zoomerIn() = 0;
		/// Zoom out, c'est-�-dire un rapetissement.
		virtual void zoomerOut() = 0;
		/// Modification de la cl�ture.
		virtual void redimensionnerFenetre(int largeur, int hauteur) = 0;
		/// Obtention de la matrice de projection.
		virtual glm::mat4 obtenirMatrice() const = 0;
		/// Obtention des z du near/far plane
		inline glm::dvec2 obtenirZMinMax() const;
		/// Obtention des dimensions de la fen�tre de clot�re.
		inline glm::ivec2 obtenirDimensionCloture() const;

		/// V�rification de si la projection est perspective.
		inline bool estPerspective() const;


	protected:
		/// Dimension de la cl�ture en X.
		int largeurCloture_;
		/// Dimension de la cl�ture en Y.
		int hauteurCloture_;

		/// Avant du volume de visualisation.
		double zAvant_;
		/// Arri�re du volume de visualisation.
		double zArriere_;

		/// Facteur maximal de zoom in.
		const double zoomInMax_;
		/// Facteur maximal de zoom out.
		const double zoomOutMax_;
		/// Incr�ment des zooms.
		const double incrementZoom_;
		/// Vrai si la projection est perspective.
		const bool   estPerspective_;
		/// Rapport entre la hauteur de la cloture et la hauteur de la fen�tre.
		double hauteurRatio;
		/// Rapport entre la largeur de la cloture et la largeur de la fen�tre.
		double largeurRatio;
	};




	////////////////////////////////////////////////////////////////////////
	///
	/// @fn inline glm::ivec2 Projection::obtenirDimensionCloture() const
	///
	/// Cette fonction retourne les dimensions de la fen�tre de cl�ture
	/// associ�e � cette projection.
	///
	/// @return Les dimensions de la fen�tre de cl�ture.
	///
	////////////////////////////////////////////////////////////////////////
	inline glm::ivec2 Projection::obtenirDimensionCloture() const
	{
		return glm::ivec2{ largeurCloture_, hauteurCloture_ };
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn inline glm::dvec2 Projection::obtenirZMinMax() const
	///
	/// Cette fonction retourne les z des near / far plane
	///
	/// @return Les z des (far plane, near plane)
	///
	////////////////////////////////////////////////////////////////////////
	inline glm::dvec2 Projection::obtenirZMinMax() const {
		return glm::dvec2(zArriere_, zAvant_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn inline bool Projection::estPerspective() const
	///
	/// Cette fonction retourne vrai si la projection est une projection
	/// perspective.
	///
	/// @return Vrai si la projection est une projection perspective.
	///
	////////////////////////////////////////////////////////////////////////
	inline bool Projection::estPerspective() const
	{
		return estPerspective_;
	}


}; // Fin de l'espace de nom vue.


#endif // __UTILITAIRE_PROJECTION_H__


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
