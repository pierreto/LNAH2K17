///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatScale.cpp
/// @author Nam Lesage
/// @date 2016-09-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatScale.h"

#include "FacadeModele.h"
#include "ArbreRenduINF2990.h"
#include "Vue.h"
#include "ModeleEtatJeu.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatScale* ModeleEtatScale::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatScale* ModeleEtatScale::obtenirInstance()
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
ModeleEtatScale* ModeleEtatScale::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatScale();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatScale::ModeleEtatScale()
///
/// Constructeur
///
/// @return Aucune (Construteur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatScale::ModeleEtatScale()
	: ModeleEtat()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatScale::~ModeleEtatScale()
///
/// Destructeur
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatScale::~ModeleEtatScale() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}



////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::mouseDownL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est descendu.
/// Plus précisément, le bouton sauve les valeurs de scale de noeuds
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::mouseDownL()
{
	ModeleEtat::mouseDownL();
	this->saveScale();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge.
/// Cette fonction effectue la rotation sur les noeuds sélectionnés
///
/// @param x[in] : position en x de la souris
/// @param y[in] : position en y de la souris
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::playerMouseMove(int x, int y) {
	ModeleEtat::playerMouseMove(x, y);

	if (mouseDownL_)
	{
		// Remove last scale
		this->revertScale();

		// Calcul du nouveau scale
		float ajoutScale = float( initMousePosY_ - mousePosY_) / 10;

		// Apply nouveau scale
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&VisiteurScale(glm::vec3(ajoutScale), true));
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::mouseUpL()
///
/// Évènement appelé lorsque le bouton de la souris gauche est levé
/// Plus précisément, la fonction vérifie que tous les noeuds sont sur
/// la table. L'action est annulée si les noeuds ne sont pas tous
/// dans les limites de la patinoire.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::mouseUpL()
{
	ModeleEtat::mouseUpL();
	if (!noeudsSurLaTable()){
		escape();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::escape()
///
/// Cette fonction annule l'action de scaling
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::escape()
{
	this->revertScale();
	mouseUpL();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::saveScale()
///
/// Cette fonction sauvegarde le scaling de chacun des noeuds
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::saveScale()
{
	// Save scale on matrice
	VisiteurObtenirSelection visiteur;
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	std::vector<NoeudAbstrait*> noeuds = visiteur.obtenirNoeuds();

	for (auto noeud : noeuds) {
		noeud->saveScale();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatScale::revertScale()
///
/// Cette fonction revert le scaling de chacun des noeuds
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatScale::revertScale()
{
	// Revert scale on matrice
	VisiteurObtenirSelection visiteur;
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	std::vector<NoeudAbstrait*> noeuds = visiteur.obtenirNoeuds();

	for (auto noeud : noeuds) {
		noeud->revertScale();
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			TransformEventCallback callback = ModeleEtatJeu::obtenirInstance()->getTransformEventCallback();
			if (callback)
			{
				callback(noeud->getUUID(), glm::value_ptr(noeud->obtenirMatriceTransformation()));
			}
		}
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////