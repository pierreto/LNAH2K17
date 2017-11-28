///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatDeplacement.cpp
/// @author Julien Charbonneau
/// @date 2016-09-23
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatDeplacement.h"

#include "FacadeModele.h"
#include "Vue.h"
#include "ModeleEtatJeu.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatDeplacement* ModeleEtatDeplacement::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatDeplacement* ModeleEtatDeplacement::obtenirInstance()
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
ModeleEtatDeplacement* ModeleEtatDeplacement::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatDeplacement();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDeplacement::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDeplacement::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDeplacement::initialiser()
///
/// Fonction qui initialise l'état de déplacement
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDeplacement::initialiser()
{
	deplacementTotal_ = glm::vec3(0);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatDeplacement::ModeleEtatDeplacement()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatDeplacement::ModeleEtatDeplacement()
	: ModeleEtat(), deplacementTotal_(0), visiteurDeplacement_(glm::vec3(),false)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatDeplacement::~ModeleEtatDeplacement()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatDeplacement::~ModeleEtatDeplacement() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDeplacement::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge.
/// Cette fonction déplace les objets selectionnés quand la souris glise
///
/// @param x[in] : Position en x de la souris
/// @param y[in] : Position en y de la souris
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDeplacement::playerMouseMove(int x, int y) {
	ModeleEtat::playerMouseMove(x, y);

	if (mouseDownL_) {
		glm::dvec3 end, start;
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(lastMousePosX_, lastMousePosY_, start);
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, end);

		visiteurDeplacement_ = VisiteurDeplacement(end - start);
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteurDeplacement_);
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			SYSTEMTIME st;
			GetSystemTime(&st);
			accTime += st.wMilliseconds;
			if (accTime > 1000) {
				sendToServer();
				accTime = 0;
			}
		}

		deplacementTotal_  += (end - start);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDeplacement::mouseUpL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est levé.
/// Cette fonction vérifie si l'objet est sur la table
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDeplacement::mouseUpL()
{
	ModeleEtat::mouseUpL();

	if (!noeudsSurLaTable()) {
		escape();
	}

	if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
	{
		sendToServer();
	}


	initialiser();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatDeplacement::escape()
///
/// Cette fonction annule l'action en cours
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatDeplacement::escape()
{
	// Revert Changes
	visiteurDeplacement_ = VisiteurDeplacement(-deplacementTotal_);
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteurDeplacement_);
	// Reinitialiser l'etat
	initialiser();

	// Relacher la souris
	ModeleEtat::mouseUpL();
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

void ModeleEtatDeplacement::sendToServer()
{
	//if(noeudsSurLaTable())
	//{
		TransformEventCallback callback = ModeleEtatJeu::obtenirInstance()->getTransformEventCallback();

		for (NoeudAbstrait* node : visiteurDeplacement_.getSelectedNodes())
		{
			if (callback)
			{
				VisiteurSurTable visiteur;
				node->accepterVisiteur(&visiteur);
				if(visiteur.sontSurTable())
				{
					callback(node->getUUID(), glm::value_ptr(node->obtenirPositionRelative()), node->obtenirRotation().y, glm::value_ptr(node->obtenirScale()));
				}
			}
		}
	//}
}