///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatSelection.cpp
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatSelection.h"

#include "FacadeModele.h"
#include "Vue.h"


/// Pointeur vers l'instance unique de la classe.
ModeleEtatSelection* ModeleEtatSelection::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatSelection* ModeleEtatSelection::obtenirInstance()
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
ModeleEtatSelection* ModeleEtatSelection::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatSelection();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatSelection::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatSelection::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatSelection::initialiser()
///
/// Cette fonction initialise l'état. Elle décide quels objects sont
/// sélectionnable.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatSelection::initialiser()
{
	// Rendre tous les objets non selectionnables
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_MUR, true));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_PORTAIL, true));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_ACCELERATEUR, true));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_POINT_CONTROL, false));
}



////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatSelection::ModeleEtatSelection()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatSelection::ModeleEtatSelection()
	: ModeleEtat(), rectangleVisible_(false), rectangleInit_(false), annuler_(false)
{
}



////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatSelection::~ModeleEtatSelection()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatSelection::~ModeleEtatSelection() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatSelection::mouseUpL()
///
/// Évènement appelé lorsque le bouton de la souris gauche est levé.
/// Plus précisément, il applique la sélection aux objets cliqués ou
/// aux objets dans une zone de sélection.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatSelection::mouseUpL() {

	if (mouseDownL_ && !annuler_) {
		if (isAClick()) {
			ArbreRendu* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();

			if (!ctrlDown_) 
				arbre->deselectionnerTout();

			Raycast ray(mousePosX_, mousePosY_);
			VisiteurSelection visiteur(ray.getRayStart(), ray.getRayEnd(), ctrlDown_);
			arbre->accepterVisiteur(&visiteur);		
		}
		else if (dimensionsSuffisantes()) {
			ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
			
			if (!ctrlDown_)
				arbre->deselectionnerTout();

			glm::dvec3 pointAncrage, pointFinal;
			FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(initMousePosX_, initMousePosY_, pointAncrage);
			FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, pointFinal);

			VisiteurSelection visiteur(pointAncrage, pointFinal, true, ctrlDown_);
			arbre->accepterVisiteur(&visiteur);
		}
	}

	if (rectangleInit_) {
		rectangleInit_ = rectangleVisible_ = false;
		aidegl::terminerRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(mousePosX_, mousePosY_));
		FacadeModele::obtenirInstance()->changerAffichageRectangle(false);
	}

	mouseDownL_ = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatSelection::mouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge. En autre, elle fait la mise
/// jour du rectangle élastique si le bouton gauche de la souris est
/// enfoncé
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatSelection::mouseMove(int x, int y) {
	ModeleEtat::mouseMove(x, y);

	if (mouseDownL_ && dimensionsSuffisantes()) {
		if (!rectangleInit_) {
			rectangleInit_ = true;
			aidegl::initialiserRectangleElastique(glm::ivec2(initMousePosX_, initMousePosX_), 0x3333, 1);
			FacadeModele::obtenirInstance()->changerAffichageRectangle(true);
		}
		else if (!rectangleVisible_) {
			rectangleVisible_ = true;
			aidegl::mettreAJourRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(mousePosX_, mousePosY_));
		}
		else
			aidegl::mettreAJourRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(lastMousePosX_, lastMousePosY_), glm::ivec2(mousePosX_, mousePosY_));
	}
	else if (rectangleInit_) {
		rectangleInit_ = rectangleVisible_ = false;
		aidegl::terminerRectangleElastique(glm::ivec2(initMousePosX_, initMousePosY_), glm::ivec2(lastMousePosX_, lastMousePosY_));
		FacadeModele::obtenirInstance()->changerAffichageRectangle(false);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatSelection::escape()
///
/// Évènement appelé lorsque la touche escape est enfoncée. Dans le cas
/// de cette implémentation, cette fonction annule l'action en cours.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatSelection::escape() {
	annuler_ = true;
	mouseUpL();
	annuler_ = false;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn bool ModeleEtatSelection::dimensionsSuffisantes()
///
/// Vérifie si les dimensions sont suffisantes pour un rectangle élastique
///
/// @return Vrai si les dimensions sont suffisantes pour un rectangle.
///
////////////////////////////////////////////////////////////////////////
bool ModeleEtatSelection::dimensionsSuffisantes() {
	return !isAClick() && (mousePosX_ - initMousePosX_ != 0) && (mousePosY_ - initMousePosY_ != 0);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


