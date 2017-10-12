////////////////////////////////////////////////////////////////////////////////////
/// @file ProjectionOrtho.cpp
/// @author DGI
/// @date 2006-12-15
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "GL/glew.h"
#include "ProjectionOrtho.h"
#include <glm/gtc/matrix_transform.inl>

#include "Utilitaire.h"
#include <iostream>

namespace vue {


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn ProjectionOrtho::ProjectionOrtho(int largeurCloture, int hauteurCloture, double zAvant, double zArriere, double zoomInMax, double zoomOutMax, double incrementZoom, double largeurFenetre, double hauteurFenetre)
	///
	/// Constructeur d'une projection orthogonale.  Ne fait qu'assigner les
	/// variables membres et ajuste ensuite le rapport d'aspect.
	///
	/// @param[in] largeurCloture   : dimension en @a X de la clôture.
	/// @param[in] hauteurCloture   : dimension en @a Y de la clôture.
	/// @param[in] zAvant			: distance du plan avant (en @a z).
	/// @param[in] zArriere			: distance du plan arrière (en @a z).
	/// @param[in] zoomInMax		: facteur de zoom in maximal.
	/// @param[in] zoomOutMax		: facteur de zoom out maximal.
	/// @param[in] incrementZoom	: distance du plan arrière (en @a z).
	/// @param[in] largeurFenetre	: dimension en @a X de la fenêtre
	///								  virtuelle.
	/// @param[in] hauteurFenetre	: dimension en @a Y de la fenêtre
	///								  virtuelle.
	/// 
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	ProjectionOrtho::ProjectionOrtho(int largeurCloture, int hauteurCloture,
		double zAvant, double zArriere,
		double zoomInMax, double zoomOutMax,
		double incrementZoom,
		double largeurFenetre, double hauteurFenetre) :
		Projection{ largeurCloture, hauteurCloture,
		zAvant, zArriere,
		zoomInMax, zoomOutMax, incrementZoom, false },
		largeurFenetre_{ largeurFenetre },
		hauteurFenetre_{ hauteurFenetre }
	{
		hauteurRatio = 2.5;
		largeurRatio = 2.5;

		ajusterRapportAspect();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionOrtho::zoomerIn()
	///
	/// Permet de faire un zoom in selon l'incrément de zoom.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionOrtho::zoomerIn()
	{
		if (hauteurFenetre_ > (2*zoomInMax_)) {
			double ratio = hauteurFenetre_ / largeurFenetre_;

			largeurFenetre_ -= incrementZoom_;
			hauteurFenetre_ -= incrementZoom_ * ratio;
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionOrtho::zoomerIn(double)
	///
	/// Permet de faire un zoom in avec une valeur spécifique.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionOrtho::zoomerIn(double valeur)
	{
		if (hauteurFenetre_ > (2*zoomInMax_)){
			double ratio = hauteurFenetre_ / largeurFenetre_;

			largeurFenetre_ -= valeur;
			hauteurFenetre_ -= valeur * ratio;
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionOrtho::zoomerOut()
	///
	/// Permet de faire un zoom out selon l'incrément de zoom.
	///
	/// @return Aucune.
	///
	//////////////////////////////////////////////////////////////////////// 
	void ProjectionOrtho::zoomerOut()
	{
		if (hauteurFenetre_ < zoomOutMax_/10) {
			double ratio = hauteurFenetre_ / largeurFenetre_;

			largeurFenetre_ += incrementZoom_;
			hauteurFenetre_ += incrementZoom_ * ratio;
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionOrtho::zoomerOut(double)
	///
	/// Permet de faire un zoom out avec une valeur spécifique.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionOrtho::zoomerOut(double valeur)
	{
		if (hauteurFenetre_ < zoomOutMax_/10) {
			double ratio = hauteurFenetre_ / largeurFenetre_;

			largeurFenetre_ += valeur;
			hauteurFenetre_ += valeur * ratio;
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionOrtho::redimensionnerFenetre( double largeur, double hauteur )
	///
	/// Permet d'ajuster les coordonnées de la fenêtre virtuelle en fonction
	/// d'un redimensionnement de la fenêtre.
	///
	/// L'agrandissement de la fenêtre virtuelle est proportionnel à
	/// l'agrandissement de la clotûre afin que les objets gardent la même
	/// grandeur apparente lorsque la fenêtre est redimensionnée.
	///
	/// @param[in]  largeur : largeur de la nouvelle clôture
	/// @param[in]  hauteur : hauteur de la nouvelle clôture
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionOrtho::redimensionnerFenetre(int largeur, int hauteur)
	{
		hauteurRatio = hauteurCloture_ / hauteurFenetre_;
		largeurRatio = largeurCloture_ / largeurFenetre_;

		largeurCloture_ = largeur;
		hauteurCloture_ = hauteur;
		glViewport(0,0,largeur, hauteur);

		ajusterRapportAspect();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn glm::mat4 ProjectionOrtho::obtenirMatrice() const
	///
	/// Cette fonction permet de retourner la fenêtre virtuelle en appelant les
	/// fonctions de glm selon le type de projection et les propriétés de la
	/// fenêtre.
	///
	/// @return Matrice de projection.
	///
	////////////////////////////////////////////////////////////////////////
	glm::mat4 ProjectionOrtho::obtenirMatrice() const
	{
		return glm::ortho(-largeurFenetre_ / 2, largeurFenetre_ / 2, 
			-hauteurFenetre_ / 2, hauteurFenetre_ / 2,	
			zAvant_, zArriere_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionOrtho::ajusterRapportAspect()
	///
	/// Permet d'ajuster les coordonnées de la fenêtre virtuelle en fonction
	/// de la clôture de façon à ce que le rapport d'aspect soit respecté.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionOrtho::ajusterRapportAspect()
	{
		largeurFenetre_ = largeurCloture_ / largeurRatio;
		hauteurFenetre_ = hauteurCloture_ / hauteurRatio;
	}

}; // Fin du namespace vue.


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
