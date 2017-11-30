////////////////////////////////////////////////////////////////////////////////////
/// @file ProjectionOrtho.h
/// @author DGI
/// @date 2006-12-15
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
////////////////////////////////////////////////////////////////////////////////////
#ifndef __UTILITAIRE_PROJECTIONORTHO_H__
#define __UTILITAIRE_PROJECTIONORTHO_H__


#include "Projection.h"


namespace vue {


	////////////////////////////////////////////////////////////////////////
	/// @class ProjectionOrtho
	/// @brief Classe implantant une projection orthogonale.
	///
	/// Cette classe implante l'interface de projection d�finie par la
	/// classe de base Projection et ajoute certaines fonctionnalit�es
	/// sp�cifiques � la projection orthogonale, comme le zoom autour d'un
	/// point en particulier et le zoom �lastique.
	///
	/// @author Martin Bisson
	/// @date 2006-12-15
	////////////////////////////////////////////////////////////////////////
	class ProjectionOrtho : public Projection
	{
	public:
		/// Constructeur.
		ProjectionOrtho(int largeurCloture, int hauteurCloture,
			double zAvant, double zArriere,
			double zoomInMax, double zoomOutMax,
			double incrementZoom,
			double largeurFenetre, double hauteurFenetre);


		/// Zoom in, c'est-�-dire un agrandissement.
		virtual void zoomerIn();
		/// Zoom in avec une valeur sp�cifique.
		virtual void zoomerIn(double valeur);
		/// Zoom out, c'est-�-dire un rapetissement.
		virtual void zoomerOut();
		/// Zoom out avec une valeur sp�cifique.
		virtual void zoomerOut(double valeur);
		/// Modification de la fen�tre virtuelle.
		virtual void redimensionnerFenetre(int largeur, int hauteur);
		void setLargeurFenetre(int largeur, int hauteur);
		virtual glm::ivec2 getLargeurFenetre() const { return glm::ivec2(largeurFenetre_, hauteurFenetre_); }
		/// Obtention de la matrice de projection.
		virtual glm::mat4 obtenirMatrice() const;

		/// Obtenir les coordonn�es de la fen�tre virtuelle.
		inline glm::dvec2 obtenirDimensionFenetreVirtuelle() const;

	private:
		/// Ajuste la fen�tre virtuelle pour respecter le rapport d'aspect.
		void ajusterRapportAspect();

		/// Largeur de la fen�tre virtuelle.
		double largeurFenetre_;
		/// Hauteur de la fen�tre virtuelle.
		double hauteurFenetre_;
	};




	////////////////////////////////////////////////////////////////////////
	///
	/// @fn inline void ProjectionOrtho::obtenirCoordonneesFenetreVirtuelle(double& xMin, double& xMax, double& yMin, double& yMax) const
	///
	/// Cette fonction retourne les coordonn�es de la fen�tre virtuelle
	/// associ�e � cette projection.
	///
	/// @param[out]  xMin : La variable qui contiendra l'abcsisse minimale.
	/// @param[out]  xMax : La variable qui contiendra l'abcsisse maximale.
	/// @param[out]  yMin : La variable qui contiendra l'ordonn�e minimale.
	/// @param[out]  yMax : La variable qui contiendra l'ordonn�e maximale.
	///
	/// @return Les coordonn�es de la fen�tre virtuelle.
	///
	////////////////////////////////////////////////////////////////////////
	inline glm::dvec2 ProjectionOrtho::obtenirDimensionFenetreVirtuelle() const
	{
		return glm::dvec2(largeurFenetre_, hauteurFenetre_);
	}


}; // Fin de l'espace de nom vue.


#endif // __UTILITAIRE_PROJECTIONORTHO_H__


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
