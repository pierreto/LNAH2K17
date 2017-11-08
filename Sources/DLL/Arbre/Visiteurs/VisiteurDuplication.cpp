///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDuplication.cpp
/// @author Clint Phoeuk
/// @date 2016-09-28
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurDuplication.h"
#include "FacadeModele.h"
#include "ModeleEtatJeu.h"
#include "ModeleEtatCreerBoost.h"
#include "ModeleEtatCreerMuret.h"
#include "ModeleEtatCreerPortail.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::VisiteurDuplication()
///
/// Constructeur pour VisiteurDuplication
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurDuplication::VisiteurDuplication(bool sendToServer, WallCreationCallback wallCallback,BoostCreationCallback boostCallback, PortalCreationCallback portalCallback)
	: nbItems_(0), centreDuplication_(glm::vec3(0,0,0)),sendToServer_(sendToServer)
{
	wallCreationCallback_ = wallCallback;
	portalCreationCallback_ = portalCallback;
	boostCreationCallback_ = boostCallback;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::VisiteurDuplication()
///
/// Destructeur pour VisiteurDuplication
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurDuplication::~VisiteurDuplication() {

}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un accélérateur pour la duplication.
///
///	@param[in] noeud : Noeud de l'objet 
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDuplication::visiterAccelerateur(NoeudAccelerateur* noeud) {
	
	if (noeud->estSelectionne()) {
		// Création du noeud
		NoeudAbstrait* noeudDouble = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

		// Assigner les mêmes propriétés
		copyProperties(noeud, noeudDouble);

		// Ajout du noeud à l'arbre de rendu
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeudDouble);

		if (sendToServer_)
		{
			boostCreationCallback_(noeudDouble->getUUID(), glm::value_ptr(noeudDouble->obtenirPositionRelative()), noeudDouble->obtenirRotation().y, glm::value_ptr(noeudDouble->obtenirScale()));
		}

	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiter un mur pour la duplication.
///
///	@param[in] noeud : Noeud de l'objet 
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDuplication::visiterMur(NoeudMur* noeud) {
	
	if (noeud->estSelectionne()) {
		// Création du noeud
		NoeudAbstrait* noeudDouble = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_MUR);

		// Assigner les mêmes propriétés
		copyProperties(noeud, noeudDouble);



		if (sendToServer_)
		{
			wallCreationCallback_(noeudDouble->getUUID(), glm::value_ptr(noeudDouble->obtenirPositionRelative()), noeudDouble->obtenirRotation().y, glm::value_ptr(noeudDouble->obtenirScale()));
		}

		// Ajout du noeud à l'arbre de rendu
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeudDouble);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail pour la duplication.
///
///	@param[in] noeud : Noeud de l'objet 
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDuplication::visiterPortail(NoeudPortail* noeud) {
	if (noeud->estSelectionne() && noeud->obtenirOppose()->estSelectionne()) {

		// Création du noeud
		NoeudPortail* noeudDouble = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL);

		// Assigner les mêmes propriétés
		copyProperties(noeud, noeudDouble);

		// État de l'opération	
		if (premierNoeud_== nullptr) {
			premierNoeud_ = noeudDouble;
			noeudDouble->assignerOppose(nullptr);
		}

		else {
			// Relier les deux portails
			noeudDouble->assignerOppose(premierNoeud_);
			premierNoeud_->assignerOppose(noeudDouble);
			if (sendToServer_)
			{
				portalCreationCallback_(premierNoeud_->getUUID(), glm::value_ptr(premierNoeud_->obtenirPositionRelative()), (premierNoeud_->obtenirRotation().y), glm::value_ptr(premierNoeud_->obtenirScale()),
					noeudDouble->getUUID(), glm::value_ptr(noeudDouble->obtenirPositionRelative()), noeudDouble->obtenirRotation().y, glm::value_ptr(noeudDouble->obtenirScale()));
			}
			premierNoeud_ = nullptr;
		}

		// Ajout du noeud à l'arbre de rendu
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeudDouble);
	}
	else
		noeud->assignerSelection(false);
}

void VisiteurDuplication::visiterRondelle(NoeudRondelle * noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::obtenirCentreDuplication()
///
/// Cette fonction permet de chercher le centre de la duplication.
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
glm::vec3 VisiteurDuplication::obtenirCentreDuplication() {
	centreDuplication_ /= nbItems_;
	return centreDuplication_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDuplication::copyProperties(NoeudAbstrait* node, NoeudAbstrait* nodeCopy)
///
/// Cette fonction permet de copier les propriétés de l'objet dupliqué.
///
///	@param[in] noeud : Noeud de l'objet à copier
///	@param[in] nodeCopy : Copie du noeud de l'objet 
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDuplication::copyProperties(NoeudAbstrait* node, NoeudAbstrait* nodeCopy) {
	nodeCopy->assignerPositionRelative(node->obtenirPositionRelative());
	nodeCopy->scale(node->obtenirScale());
	nodeCopy->rotate(node->obtenirRotation().y, glm::vec3(0, 1, 0));

	centreDuplication_ += nodeCopy->obtenirPositionRelative();
	nbItems_++;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////