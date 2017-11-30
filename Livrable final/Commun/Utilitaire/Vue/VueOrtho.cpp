////////////////////////////////////////////////////////////////////////////////////
/// @file VueOrtho.cpp
/// @author DGI
/// @date 2006-12-16
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "Utilitaire.h"
#include "VueOrtho.h"

#include <iostream>

namespace vue {


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn VueOrtho::VueOrtho(const Camera& camera, int xMinCloture, int xMaxCloture, int yMinCloture, int yMaxCloture, double zAvant, double zArriere, double zoomInMax, double zoomOutMax, double incrementZoom, double xMinFenetre, double xMaxFenetre, double yMinFenetre, double yMaxFenetre)
	///
	/// Constructeur d'une vue orthogonale.  Ne fait que créer les objets
	/// Projection et Camera correspondant.
	///
	/// @param[in] camera        : Caméra à utiliser au départ pour cette vue.
	/// @param[in] xMinCloture   : coordonnée minimale en @a x de la clôture.
	/// @param[in] xMaxCloture   : coordonnée maximale en @a x de la clôture.
	/// @param[in] yMinCloture   : coordonnée minimale en @a y de la clôture.
	/// @param[in] yMaxCloture   : coordonnée maximale en @a y de la clôture.
	/// @param[in] zAvant        : distance du plan avant (en @a z).
	/// @param[in] zArriere      : distance du plan arrière (en @a z).
	/// @param[in] zoomInMax     : facteur de zoom in maximal.
	/// @param[in] zoomOutMax    : facteur de zoom out maximal.
	/// @param[in] incrementZoom : distance du plan arrière (en @a z).
	/// @param[in] xMinFenetre   : coordonnée minimale en @a x de la fenêtre
	///                            virtuelle.
	/// @param[in] xMaxFenetre   : coordonnée maximale en @a x de la fenêtre
	///                            virtuelle.
	/// @param[in] yMinFenetre   : coordonnée minimale en @a y de la fenêtre
	///                            virtuelle.
	/// @param[in] yMaxFenetre   : coordonnée maximale en @a y de la fenêtre
	///                            virtuelle.
	/// 
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////
	VueOrtho::VueOrtho(Camera const& camera, ProjectionOrtho const& projection) :
		Vue{ camera },
		projection_{projection}
	{
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn const ProjectionOrtho& VueOrtho::obtenirProjection() const
	///
	/// Retourne la projection orthogonale associée à cette vue.
	///
	/// @return La projection orthogonale associée à cette vue.
	///
	////////////////////////////////////////////////////////////////////////
	const ProjectionOrtho& VueOrtho::obtenirProjection() const
	{
		return projection_;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::redimensionnerFenetre( int largeur, int hauteur )
	///
	/// Permet d'ajuster les coordonnées de la fenêtre virtuelle en fonction
	/// d'un redimensionnement de la fenêtre.
	///
	/// @param[in]  hauteur : Dimension en X de la nouvelle clôture.
	/// @param[in]  largeur : Dimension en Y de la nouvelle clôture.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::redimensionnerFenetre(int largeur, int hauteur)
	{
		projection_.redimensionnerFenetre(largeur, hauteur);
	}
	void VueOrtho::setLargeurFenetre(int largeur, int hauteur)
	{
		projection_.setLargeurFenetre(largeur, hauteur);
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::zoomerIn()
	///
	/// Permet de faire un zoom in selon l'incrément de zoom.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::zoomerIn()
	{
		projection_.zoomerIn();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::zoomerOut()
	///
	/// Permet de faire un zoom out selon l'incrément de zoom.
	/// 
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::zoomerOut()
	{
		projection_.zoomerOut();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::zoomerElastique(const glm::ivec2& coin1, const glm::ivec2& coin2, bool zoomIn)
	///
	/// Permet de faire un zoom in/out élastique.
	/// 
	/// @param[in]  coin1 : Coin contenant les coordonnées du premier coin du
	///                     rectangle.
	/// @param[in]  coin2 : Coin contenant les coordonnées du second coin du
	///                     rectangle.
	/// @param[in]  zoomIn: Permet de déterminer s'il s'agit d'un zoom in ou out.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::zoomerElastique(const glm::ivec2& coin1,
		const glm::ivec2& coin2, bool zoomIn)
	{
		glm::dvec2 pointCentre;
		pointCentre.y = (coin1.x + coin2.x) / 2;
		pointCentre.x = (coin1.y + coin2.y) / 2;

		glm::dvec2 posCamera;
		posCamera.y = obtenirCamera().obtenirPosition().x;
		posCamera.x = obtenirCamera().obtenirPosition().z;

		double largeurFenetre = obtenirProjection().obtenirDimensionFenetreVirtuelle().x;
		double hauteurFenetre = obtenirProjection().obtenirDimensionFenetreVirtuelle().y;
		
		double largeurRectangle = utilitaire::VALEUR_ABSOLUE(coin1.y - coin2.y);
		double hauteurRectangle = utilitaire::VALEUR_ABSOLUE(coin1.x - coin2.x);

		if (zoomIn) {
			obtenirCamera().deplacerXY(pointCentre.x - posCamera.x, pointCentre.y - posCamera.y);
			while (largeurFenetre > largeurRectangle && hauteurFenetre > hauteurRectangle) {
				projection_.zoomerIn();
				largeurFenetre = obtenirProjection().obtenirDimensionFenetreVirtuelle().x;
				hauteurFenetre = obtenirProjection().obtenirDimensionFenetreVirtuelle().y;
			}
		}
		else {
			obtenirCamera().deplacerXY((posCamera.x - pointCentre.x) * (largeurFenetre / largeurRectangle), (posCamera.y - pointCentre.y) * (hauteurFenetre / hauteurRectangle));
			int cutoff = (largeurFenetre + hauteurFenetre);
			while (((largeurFenetre + hauteurFenetre) / 2) < cutoff) {
				projection_.zoomerOut();
				if (largeurFenetre == obtenirProjection().obtenirDimensionFenetreVirtuelle().x && hauteurFenetre == obtenirProjection().obtenirDimensionFenetreVirtuelle().y) break;
				largeurFenetre = obtenirProjection().obtenirDimensionFenetreVirtuelle().x;
				hauteurFenetre = obtenirProjection().obtenirDimensionFenetreVirtuelle().y;
			}
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::deplacerXY(double deplacementX, double deplacementY)
	///
	/// @param[in]  deplacementX : Déplacement en pourcentage de la largeur.
	/// @param[in]  deplacementY : Déplacement en pourcentage de la hauteur.
	///
	/// Permet de faire un "pan" d'un certain pourcentage.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::deplacerXY(double deplacementX, double deplacementY)
	{
		camera_.deplacerXY(deplacementX * projection_.obtenirDimensionFenetreVirtuelle()[0],
			deplacementY * projection_.obtenirDimensionFenetreVirtuelle()[1]);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::deplacerXY(const glm::ivec2& deplacement)
	///
	/// Permet de faire un "pan" équivalent à la distance du déplacement
	/// spécifié en coordonnées de clôture.
	///
	/// @param[in]  deplacement : Déplacement en coordonnées de clôture
	///                           (pixels).
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::deplacerXY(const glm::ivec2& deplacement)
	{
		
		glm::dvec2 d(deplacement);
		glm::ivec2 dimensionCloture = projection_.obtenirDimensionCloture();
		glm::ivec2 dimensionFenetreVirtuelle = projection_.obtenirDimensionFenetreVirtuelle();

		glm::dvec2 direction(-d[0] / dimensionCloture[0] * dimensionFenetreVirtuelle[0],
						     d[1] / dimensionCloture[1] * dimensionFenetreVirtuelle[1]);
        
		camera_.deplacerXY(direction[0], direction[1]);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::deplacerZ(double deplacement)
	///
	/// Ne fait rien, car se déplacer dans l'axe de la profondeur n'a pas
	/// vraiment de signification avec une vue orthogonale.
	///
	/// @param[in]  deplacement : Déplacement selon l'axe Z.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	
	void VueOrtho::deplacerZ(double deplacement)
	{
	}
	

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::rotaterXY(double rotationX, double rotationY)
	///
	/// Permet de faire une rotation de la caméra autour du point vers
	/// lequel elle regarde en modifiant l'angle de rotation et l'angle
	/// d'élévation.
	///
	/// Une rotation de 100% correspondant à 360 degrés en X (angle de
	/// rotation) ou 180 degrés en Y (angle d'élévation).
	///
	/// @param[in]  rotationX : Rotation en pourcentage de la largeur.
	/// @param[in]  rotationY : Rotation en pourcentage de la hauteur.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::rotaterXY(double rotationX, double rotationY)
	{
		camera_.orbiterXY(rotationX * 360, rotationY * 180);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::rotaterXY(const glm::ivec2& rotation)
	///
	/// Permet de faire une rotation de la caméra autour du point vers
	/// lequel elle regarde en modifiant l'angle de rotation et l'angle
	/// d'élévation.
	///
	/// Un déplacement de la taille de la fenêtre correspond à 100%.
	///
	/// @param[in]  rotation : Rotation en coordonnées de clotûre (pixels).
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::rotaterXY(const glm::ivec2& rotation)
	{
		const glm::ivec2 dimensions{ projection_.obtenirDimensionCloture() };
		rotaterXY(rotation[0] / (double) dimensions[0],
			rotation[1] / (double) dimensions[1]);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void VueOrtho::rotaterZ(double rotation)
	///
	/// Ne fait rien, car tourner autour de l'axe de la profondeur
	/// correspondrait à un rouli et n'est pas souhaitable pour cette vue.
	///
	/// @param[in]  rotation : Rotation selon l'axe Z.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void VueOrtho::rotaterZ(double rotation)
	{
	}


}; // Fin du namespace vue.


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
