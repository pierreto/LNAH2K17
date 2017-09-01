///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatCreerBoost.cpp
/// @author Nam Lesage
/// @date 2016-09-13
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatCreerBoost.h"

#include "FacadeModele.h"
#include "Vue.h"
#include "ArbreRenduINF2990.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatCreerBoost* ModeleEtatCreerBoost::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerBoost* ModeleEtatCreerBoost::obtenirInstance()
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
ModeleEtatCreerBoost* ModeleEtatCreerBoost::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatCreerBoost();

	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->deselectionnerTout();
	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerBoost::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerBoost::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerBoost::ModeleEtatCreerBoost()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerBoost::ModeleEtatCreerBoost()
	: ModeleEtat()
{
	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatCreerBoost::~ModeleEtatCreerBoost()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatCreerBoost::~ModeleEtatCreerBoost() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatCreerBoost::mouseUpL() 
///
/// Évènement appelé lorsque le bouton gauche de la souris est levé.
/// Cette implémentation s'occupe de créer les accélérateurs.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatCreerBoost::mouseUpL() {
	if (mouseDownL_ && isAClick() && mouseOverTable()) {
		// Création du noeud
		NoeudAbstrait* noeud = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

		// Déplacement du noeud
		// Transformation du point dans l'esapce virtuelle
		glm::dvec3 point;
		obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, point);

		noeud->assignerPositionRelative(point);
		
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeud);

		// On annule si le noeud n'est pas sur la table
		if (!noeudsSurLaTable()) {
			FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->effacer(noeud);
		}
	}
	mouseDownL_ = false;
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////