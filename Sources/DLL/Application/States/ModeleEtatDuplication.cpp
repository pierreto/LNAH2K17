///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDuplication.cpp
/// @author Clint Phoeuk
/// @date 2016-09-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>
#include <vector>
#include <string>
#include "ModeleEtatDuplication.h"
#include "FacadeModele.h"
#include "Vue.h"
#include "ArbreRenduINF2990.h"

#include "VisiteurDuplication.h"
#include "VisiteurObtenirSelection.h"
#include "ModeleEtatJeu.h"
#include "ModeleEtatCreerMuret.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatDuplication* ModeleEtatDuplication::instance_{ nullptr };

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatDuplication* ModeleEtatDuplication::obtenirInstance()
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
ModeleEtatDuplication* ModeleEtatDuplication::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatDuplication();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDuplication::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDuplication::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatDuplication::ModeleEtatDuplication()
///
/// Constructeur
///
/// @return Aucune (Construteur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatDuplication::ModeleEtatDuplication()
	: ModeleEtat(), estCopie_(false)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatDuplication::~ModeleEtatDuplication()
///
/// Destructeur
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatDuplication::~ModeleEtatDuplication() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDuplication::mouseUpL()
///
/// Évènement appelé lorsque le bouton de la souris gauche est levé. 
/// La fonction duplique les objets sélectionnés s'ils sont tous
/// dans la zone de jeu.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDuplication::mouseUpL() {	
	if (mouseDownL_ && isAClick()) {
		// Dupliquer seulement si les objets sont dans la zone de jeu
		if (noeudsSurLaTable()) {

			bool sendToServer;
			if(ModeleEtatJeu::obtenirInstance()->currentOnlineClientType()==ModeleEtatJeu::ONLINE_EDITION)
			{
				sendToServer = true;
			}else
			{
				sendToServer = false;
			}

			VisiteurDuplication duplication = VisiteurDuplication(sendToServer, wallCreationCallback_);
			FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&duplication);
		}
	}

	ModeleEtat::mouseUpL();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDuplication::mouseUpR()
///
/// Évènement appelé lorsque le bouton de la souris droit est levé. 
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDuplication::mouseUpR() {
	ModeleEtat::mouseUpR();

	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();

	VisiteurObtenirSelection selection = VisiteurObtenirSelection();
	arbre->accepterVisiteur(&selection);

	glm::dvec3 mousePos;
	FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, mousePos);

	VisiteurDeplacement deplacementInit = VisiteurDeplacement(mousePos - selection.obtenirCentreSelection());
	arbre->accepterVisiteur(&deplacementInit);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDuplication::playerMouseMove(int x, int y) 
///
/// Évènement appelé lorsque la souris bouge. Entre autre,
/// cette fonction déplace un fantome de la sélection
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDuplication::playerMouseMove(int x, int y) {
	
	ModeleEtat::playerMouseMove(x, y);

	if (!mouseDownR_)
	{
		ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();

		glm::dvec3 mousePos, lastMousePos;
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, mousePos);
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(lastMousePosX_, lastMousePosY_, lastMousePos);

		if (!estCopie_) {
			VisiteurDuplication duplication = VisiteurDuplication(false);
			arbre->accepterVisiteur(&duplication);
			glm::dvec3 centreDuplication = duplication.obtenirCentreDuplication();

			VisiteurDeplacement deplacementInit = VisiteurDeplacement(mousePos - centreDuplication,false);
			arbre->accepterVisiteur(&deplacementInit);
			estCopie_ = true;
		}
		else {
			VisiteurDeplacement deplacement = VisiteurDeplacement(mousePos - lastMousePos, false);
			arbre->accepterVisiteur(&deplacement);
		}
	}
	
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDuplication::nettoyerEtat()
///
/// Cette fonction nettoie l'etat des changements apportes
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDuplication::nettoyerEtat() {
	VisiteurSuppression suppression = VisiteurSuppression();
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&suppression);
	suppression.deleteAllSelectedNode();
	estCopie_ = false;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////