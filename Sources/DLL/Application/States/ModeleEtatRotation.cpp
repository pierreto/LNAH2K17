///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatRotation.cpp
/// @author Nam Lesage
/// @date 2016-09-27
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatRotation.h"

#include "FacadeModele.h"
#include "ArbreRenduINF2990.h"
#include "Vue.h"
#include "ModeleEtatJeu.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatRotation* ModeleEtatRotation::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatRotation* ModeleEtatRotation::obtenirInstance()
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
ModeleEtatRotation* ModeleEtatRotation::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatRotation();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatRotation::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatRotation::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatRotation::ModeleEtatRotation()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatRotation::ModeleEtatRotation()
	: ModeleEtat(), angle_(0)
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatRotation::~ModeleEtatRotation()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
ModeleEtatRotation::~ModeleEtatRotation() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatRotation::mouseDownL()
///
/// Évènement appelé lorsque le bouton de la souris gauche est descendu.
/// Cette fonction recherche le point central de rotation
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatRotation::mouseDownL()
{
	ModeleEtat::mouseDownL();

	// Creation du visiteur
	VisiteurObtenirSelection visiteur;
	// Appel du visiteur
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	// Recuperation des noeuds
	noeuds_ = visiteur.obtenirNoeuds();
	// Récupérer le centre de rotation
	centreRotation_ = visiteur.obtenirCentreSelection();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatRotation::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge.
/// Cette fonction effectue la rotation sur les noeuds sélectionnés
///
/// @param x : position en x de la souris
/// @param y : position en y de la souris
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatRotation::playerMouseMove(int x, int y) {
	ModeleEtat::playerMouseMove(x, y);

	if (mouseDownL_) 
	{
		glm::dvec3 end, start;
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(lastMousePosX_, lastMousePosY_, start);
		FacadeModele::obtenirInstance()->obtenirVue()->convertirClotureAVirtuelle(mousePosX_, mousePosY_, end);

		// Calcule de l'angle immediat
		float angle = float(utilitaire::DEG_TO_RAD(start.x - end.x));

		// Update de l'angle totale
		angle_ += angle;

		bool canSendToServer = false;
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			SYSTEMTIME st;
			GetSystemTime(&st);
			accTime_ += st.wMilliseconds;
			if (accTime_>1000) {
				canSendToServer = true;
				accTime_ = 0;
			}
		}
		applyRotation(angle, canSendToServer);

	}
}

void ModeleEtatRotation::applyRotation(float angle, bool canSendToServer)
{
	for (auto noeud : noeuds_)
	{
		// Effectue une translation du point central vers l'origine
		noeud->appliquerDeplacement(-centreRotation_);

		// Effectuer la rotation
		noeud->appliquerRotation(angle, glm::vec3(0, 1, 0));

		// Remettre le point central à sa position initiale
		noeud->appliquerDeplacement(centreRotation_);

		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			if (noeudsSurLaTable()) {
				if (canSendToServer)
				{
					TransformEventCallback callback = ModeleEtatJeu::obtenirInstance()->getTransformEventCallback();
					if (callback)
					{

						callback(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
					}
				}
			}
		}

	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatRotation::mouseUpL()
///
/// Cette fonction réinitialise les attributs de l'état
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatRotation::mouseUpL()
{
	ModeleEtat::mouseUpL();

	if (!noeudsSurLaTable()){
		escape();
	}
	if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
	{
		for (auto noeud : noeuds_)
		{
			TransformEventCallback callback = ModeleEtatJeu::obtenirInstance()->getTransformEventCallback();
			if (callback)
			{
				callback(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
			}
		}
	}{}

	// Clean state
	noeuds_.clear();
	centreRotation_ = glm::vec3(0, 0, 0);
	angle_ = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatRotation::escape()
///
/// Cette fonction annule l'action de rotation
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatRotation::escape()
{

	for (auto noeud : noeuds_)
	{
		// Effectue une translation du point central vers l'origine
		noeud->appliquerDeplacement(-centreRotation_);

		// Effectuer la rotation
		noeud->appliquerRotation(-angle_, glm::vec3(0, 1, 0));

		// Remettre le point central à sa position initiale
		noeud->appliquerDeplacement(centreRotation_);
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			TransformEventCallback callback = ModeleEtatJeu::obtenirInstance()->getTransformEventCallback();
			if (callback)
			{
				callback(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
			}
		}
	}


	this->mouseUpL();
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////