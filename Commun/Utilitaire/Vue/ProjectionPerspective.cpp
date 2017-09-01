////////////////////////////////////////////////////////////////////////////////////
/// @file ProjectionPerspective.cpp
/// @author DGI
/// @date 2016-11-25
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "GL/glew.h"
#include "ProjectionPerspective.h"
#include <glm/gtc/matrix_transform.inl>

#include "Utilitaire.h"
#include <iostream>

namespace vue {


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn ProjectionPerspective::ProjectionPerspective(int largeurCloture, int hauteurCloture, double zAvant, double zArriere, double zoomInMax, double zoomOutMax, double incrementZoom, double fov, double aspect)
	///
	/// Constructeur d'une projection perspective.  Ne fait qu'assigner les
	/// variables membres et ajuste ensuite le rapport d'aspect.
	///
	/// @param[in] largeurCloture   : dimension en @a X de la cl�ture.
	/// @param[in] hauteurCloture   : dimension en @a Y de la cl�ture.
	/// @param[in] zAvant			: distance du plan avant (en @a z).
	/// @param[in] zArriere			: distance du plan arri�re (en @a z).
	/// @param[in] zoomInMax		: facteur de zoom in maximal.
	/// @param[in] zoomOutMax		: facteur de zoom out maximal.
	/// @param[in] incrementZoom	: distance du plan arri�re (en @a z).
	/// @param[in] fov				: Field of view vertical de la projection, en degres.
	/// @param[in] aspect			: Ratio du field of view.
	/// 
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	ProjectionPerspective::ProjectionPerspective(int largeurCloture, int hauteurCloture,
		double zAvant, double zArriere,
		double zoomInMax, double zoomOutMax,
		double incrementZoom,
		double largeurFenetre, double hauteurFenetre, double fov) :
		Projection{ largeurCloture, hauteurCloture,
		zAvant, zArriere,
		zoomInMax, zoomOutMax, incrementZoom, false },
		largeurFenetre_{ largeurFenetre },
		hauteurFenetre_{ hauteurFenetre }, 
		fov_{ fov }
	{
		hauteurRatio = 2.5;
		largeurRatio = 2.5;
		//redimensionnerFenetre(largeurFenetre, hauteurFenetre);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionPerspective::zoomerIn()
	///
	/// Permet de faire un zoom in selon l'incr�ment de zoom.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionPerspective::zoomerIn()
	{
		if (fov_ > 44) {
			fov_ -= incrementZoom_;
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionPerspective::zoomerIn(double)
	///
	/// Permet de faire un zoom in avec une valeur sp�cifique.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionPerspective::zoomerIn(double valeur)
	{
		if (fov_ > 44) {
			fov_ -= valeur;
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionPerspective::zoomerOut()
	///
	/// Permet de faire un zoom out selon l'incr�ment de zoom.
	///
	/// @return Aucune.
	///
	//////////////////////////////////////////////////////////////////////// 
	void ProjectionPerspective::zoomerOut()
	{
		if (fov_ < 46) {
			fov_ += incrementZoom_;
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionPerspective::zoomerOut(double)
	///
	/// Permet de faire un zoom out avec une valeur sp�cifique.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionPerspective::zoomerOut(double valeur)
	{
		if (fov_ < 46) {
			fov_ += valeur;
		}
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionPerspective::redimensionnerFenetre( double largeur, double hauteur )
	///
	/// Permet d'ajuster les coordonn�es de la fen�tre virtuelle en fonction
	/// d'un redimensionnement de la fen�tre.
	///
	/// L'agrandissement de la fen�tre virtuelle est proportionnel �
	/// l'agrandissement de la clot�re afin que les objets gardent la m�me
	/// grandeur apparente lorsque la fen�tre est redimensionn�e.
	///
	/// @param[in]  largeur : largeur de la nouvelle cl�ture
	/// @param[in]  hauteur : hauteur de la nouvelle cl�ture
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionPerspective::redimensionnerFenetre(int largeur, int hauteur)
	{
		hauteurRatio = hauteurCloture_ / hauteurFenetre_;
		largeurRatio = largeurCloture_ / largeurFenetre_;

		largeurCloture_ = largeur;
		hauteurCloture_ = hauteur;
		glViewport(0, 0, largeur, hauteur);

		ajusterRapportAspect();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn glm::mat4 ProjectionPerspective::obtenirMatrice() const
	///
	/// Cette fonction permet de retourner la fen�tre virtuelle en appelant les
	/// fonctions de glm selon le type de projection et les propri�t�s de la
	/// fen�tre.
	///
	/// @return Matrice de projection.
	///
	////////////////////////////////////////////////////////////////////////
	glm::mat4 ProjectionPerspective::obtenirMatrice() const
	{
		return glm::perspective(fov_, largeurFenetre_ / hauteurFenetre_, zAvant_, zArriere_);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void ProjectionPerspective::ajusterRapportAspect()
	///
	/// Permet d'ajuster les coordonn�es de la fen�tre virtuelle en fonction
	/// de la cl�ture de fa�on � ce que le rapport d'aspect soit respect�.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void ProjectionPerspective::ajusterRapportAspect()
	{
		largeurFenetre_ = largeurCloture_ / largeurRatio;
		hauteurFenetre_ = hauteurCloture_ / hauteurRatio;
	}

}; // Fin du namespace vue.


   ///////////////////////////////////////////////////////////////////////////
   /// @}
   ///////////////////////////////////////////////////////////////////////////
