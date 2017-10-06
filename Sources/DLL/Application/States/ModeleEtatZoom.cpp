///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatZoom.cpp
/// @author Francis Dalpé
/// @date 2016-09-20
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatZoom.h"

#include "FacadeModele.h"


/// Pointeur vers l'instance unique de la classe.
ModeleEtatZoom* ModeleEtatZoom::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatZoom* ModeleEtatZoom::obtenirInstance()
///
/// Cette fonction retourne un pointeur vers l'instance unique de la
/// classe.  Si cette instance n'a pas été créée, elle la crée.  Cette
/// création n'est toutefois pas nécessairement "thread-safe", car
/// aucun verrou n'est pris entre le test pour savoir si l'instance
/// existe et le moment de sa création.
///
/// @return Un pointeur vers l'instance unique de cette classe.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatZoom* ModeleEtatZoom::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatZoom();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatZoom::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatZoom::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatZoom::libererInstance()
///
/// Constructeur.
///
/// @return Aucune (Constructeur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatZoom::ModeleEtatZoom()
	: ModeleEtat(), rectangleActif_(false)
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatZoom::~ModeleEtatZoom()
///
/// Destructeur.
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatZoom::~ModeleEtatZoom() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatZoom::mouseDownL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est descendu.
/// Plus précisément, elle active le rectangle si celui-ci n'est pas déjà
/// activé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatZoom::mouseDownL() {
	ModeleEtat::mouseDownL();
	
	if (!rectangleActif_) {
		rectangleActif_ = true;
		FacadeModele::obtenirInstance()->changerAffichageRectangle(true);
		aidegl::initialiserRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_));
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatZoom::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge. Entre autre,
/// cette fonction agrandi le rectangle élactique si la souris est pressée
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatZoom::playerMouseMove(int x, int y) {
	ModeleEtat::playerMouseMove(x, y);

	if (mouseDownL_ && rectangleActif_) {
		aidegl::mettreAJourRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(lastMousePosX_, lastMousePosY_), glm::ivec2(mousePosX_, mousePosY_));
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatZoom::mouseUpL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est levé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatZoom::mouseUpL() {
	if (mouseDownL_) {

		glm::dvec3 pointAncrage, pointFin;
		obtenirVue()->convertirClotureAVirtuelle(initMousePosX_, initMousePosY_, pointAncrage);
		obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, pointFin);

		if (utilitaire::VALEUR_ABSOLUE(pointAncrage.x - pointFin.x) > 10 && utilitaire::VALEUR_ABSOLUE(pointAncrage.z - pointFin.z) > 10) {
			if (altDown_) {
				obtenirVue()->zoomerElastique(glm::ivec2(pointAncrage.x, pointAncrage.z), glm::ivec2(pointFin.x, pointFin.z), false);
			}
			else {
				obtenirVue()->zoomerElastique(glm::ivec2(pointAncrage.x, pointAncrage.z), glm::ivec2(pointFin.x, pointFin.z), true);
			}
		}

		if (rectangleActif_) {
			aidegl::terminerRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(mousePosX_, mousePosY_));
			rectangleActif_ = false;
			FacadeModele::obtenirInstance()->changerAffichageRectangle(false);
		}
	}

	mouseDownL_ = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatZoom::escape()
///
/// Évènement appelé lorsque la touche escape est enfoncée. Dans le cas
/// de cette implémentation, cette fonction annule l'action en cours.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatZoom::escape() {
	initMousePosX_ = mousePosX_;
	initMousePosY_ = mousePosY_;
	mouseUpL();
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////