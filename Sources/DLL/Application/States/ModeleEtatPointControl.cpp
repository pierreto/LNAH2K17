///////////////////////////////////////////////////////////////////////////////
/// @file ModeleEtatPointControl.cpp
/// @author Nam Lesage
/// @date 2016-09-29
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>

#include "ModeleEtatPointControl.h"

#include "FacadeModele.h"
#include "Vue.h"
#include "Plan3D.h"

#include "visiteurSelectionnable.h"
#include "ArbreRenduINF2990.h"
#include "ModeleEtatDeplacement.h"
#include "ModeleEtatJeu.h"
#include "ModeleEtatSelection.h"

/// Pointeur vers l'instance unique de la classe.
ModeleEtatPointControl* ModeleEtatPointControl::instance_{ nullptr };


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatPointControl* ModeleEtatPointControl::obtenirInstance()
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
ModeleEtatPointControl* ModeleEtatPointControl::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new ModeleEtatPointControl();

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::initialiser()
///
/// Cette fonction initialise l'etat. Elle ajuste la couleur des points
/// de controle et rend ces derniers selectionnables
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::initialiser()
{
	// Rendre seulement les points de control selectionnable
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_MUR, false));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_ACCELERATEUR, false));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_PORTAIL, false));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_POINT_CONTROL, true));

	// Afficher les points en rose saumon
	NoeudComposite* table = (NoeudComposite*)arbre->chercher(ArbreRenduINF2990::NOM_TABLE);
	for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); ++it)
	{
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL)
		{
			if (!(*it)->isSelectedByAnotherUser())
			{
				(*it)->useOtherColor(true, glm::vec4(1, 85.0f / 255, 82.0f / 255, 1));
			}
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatPointControl::ModeleEtatPointControl()
///
/// Constructeur
///
/// @return Aucune (Construteur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatPointControl::ModeleEtatPointControl()
	: ModeleEtat(), visiteurDeplacement_(glm::vec3(), false)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn ModeleEtatPointControl::~ModeleEtatPointControl()
///
/// Destructeur
///
/// @return Aucune (Destructeur).
///
////////////////////////////////////////////////////////////////////////
ModeleEtatPointControl::~ModeleEtatPointControl() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::mouseDownL()
///
/// Évènement appelé lorsque le bouton gauche de la souris est descendu.
/// Entre autre, cette fonction selectionne un point de controle par raycast
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::mouseDownL() {

	ModeleEtat::mouseDownL();

	Raycast ray(mousePosX_, mousePosY_);
	VisiteurSelection visiteur(ray.getRayStart(), ray.getRayEnd(), ctrlDown_);

	// Envoi du visiteur
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);

	// Sauver la position du noeud selectionne
	savePosition();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::playerMouseMove(int x, int y)
///
/// Évènement appelé lorsque la souris bouge. Entre autre,
/// cette fonction déplace un noeud sélectionneé
///
/// @param x[in] : position de la souris en x
/// @param y[in] : position de la souris en y
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::playerMouseMove(int x, int y) {
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
			if (accTime>1000) {
				if (noeudsSurLaTable())
				{
					sendToServer();
				}
				accTime = 0;
			}
		}

	}
}
void ModeleEtatPointControl::sendToServer()
{
		for (NoeudAbstrait* node : visiteurDeplacement_.getSelectedNodes())
		{
			NoeudPointControl* noeudPointControl = dynamic_cast<NoeudPointControl*>(node);
			if (controlPointEventCallback_)
			{
				controlPointEventCallback_(noeudPointControl->getUUID(), glm::value_ptr(node->obtenirPositionRelative()));
				controlPointEventCallback_(noeudPointControl->obtenirNoeudOppose()->getUUID(), glm::value_ptr(noeudPointControl->obtenirNoeudOppose()->obtenirPositionRelative()));
			}
		}
}
////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::mouseUpL()
///
/// Évènement appelé lorsque le bouton de la souris gauche est levé. Entre
/// autre, cette fonction désélectionne les noeuds
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::mouseUpL() {
	ModeleEtat::mouseUpL();
	
	// On annule l'action si les objets ne sont plus tous sur la table
	if (!noeudsSurLaTable())
	{
		escape();

	}

	if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
	{
		sendToServer();
	}
	// Reinitialiser l'etat
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->deselectionnerTout();
	if(ModeleEtatJeu::obtenirInstance()->currentOnlineClientType()==ModeleEtatJeu::ONLINE_EDITION)
	{
		selectionCallback_("", false, true);

	}

	initialiser();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::escape()
///
/// Évènement appelé lorsque la touche escape est enfoncée. Dans le cas
/// de cette implémentation, cette fonction annule l'action en cours.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::escape()
{
	// Revert Changes
	revertPosition();

	// Reinitialiser l'etat
	initialiser();


	// Relacher la souris
	ModeleEtat::mouseUpL();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::nettoyerEtat()
///
/// Cette fonction nettoie l'état des changements apportes
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::nettoyerEtat()
{
	// Retablir les objets selectionnables
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_MUR, true));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_ACCELERATEUR, true));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_PORTAIL, true));
	arbre->accepterVisiteur(&VisiteurSelectionnable(ArbreRenduINF2990::NOM_POINT_CONTROL, false));

	// Annuler la couleur rose saumon
	NoeudComposite* table = (NoeudComposite*)arbre->chercher(ArbreRenduINF2990::NOM_TABLE);
	for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); ++it)
	{
		if( (*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL && !(*it)->isSelectedByAnotherUser() )
		{
			(*it)->useOtherColor(false);
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::savePosition()
///
/// Cette fonction sauve la position du noeud selectionne
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::savePosition()
{
	// Save position of matrice
	VisiteurObtenirSelection visiteur;
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	std::vector<NoeudAbstrait*> noeuds = visiteur.obtenirNoeuds();

	for (NoeudAbstrait* noeud : noeuds) {
		noeud->savePosition();
		static_cast<NoeudPointControl*>(noeud)->obtenirNoeudOppose()->savePosition();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ModeleEtatPointControl::revertPosition()
///
/// Cette fonction applique la position sauvée au noeud sélectionné
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ModeleEtatPointControl::revertPosition()
{
	// Revert position
	VisiteurObtenirSelection visiteur;
	FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->accepterVisiteur(&visiteur);
	std::vector<NoeudAbstrait*> noeuds = visiteur.obtenirNoeuds();

	for (auto noeud : noeuds) {
		noeud->revertPosition();
		static_cast<NoeudPointControl*>(noeud)->obtenirNoeudOppose()->revertPosition();
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			if (controlPointEventCallback_)
			{
				controlPointEventCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));
				controlPointEventCallback_(static_cast<NoeudPointControl*>(noeud)->obtenirNoeudOppose()->getUUID(), glm::value_ptr(static_cast<NoeudPointControl*>(noeud)->obtenirNoeudOppose()->obtenirPositionRelative()));

				selectionCallback_(noeud->getUUID(), false, false);
			}
		}

	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////