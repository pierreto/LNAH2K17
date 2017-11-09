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

	stamp_ = std::vector<NoeudAbstrait*>();
	premierNoeudPortailTrouve_ = false;
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
/// Cette fonction permet de visiter un acc�l�rateur pour la duplication.
///
///	@param[in] noeud : Noeud de l'objet 
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDuplication::visiterAccelerateur(NoeudAccelerateur* noeud) {
	
	if (noeud->estSelectionne()) {
		// Cr�ation du noeud
		NoeudAbstrait* noeudDouble = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

		// Assigner les m�mes propri�t�s
		copyProperties(noeud, noeudDouble);
		noeud->assignerSelection(false);

		// Ajout du noeud � l'arbre de rendu
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeudDouble);

		stamp_.push_back(noeudDouble);


		
		if (sendToServer_)
		{
			boostCreationCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
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
		// Cr�ation du noeud
		NoeudAbstrait* noeudDouble = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_MUR);

		// Assigner les m�mes propri�t�s
		copyProperties(noeud, noeudDouble);

		noeud->assignerSelection(false);

		stamp_.push_back(noeudDouble);
		if (sendToServer_)
		{
			wallCreationCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
		}

		// Ajout du noeud � l'arbre de rendu
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

		// Cr�ation du noeud
		NoeudPortail* noeudDouble = (NoeudPortail*)FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->creerNoeud(ArbreRenduINF2990::NOM_PORTAIL);

		// Assigner les m�mes propri�t�s
		copyProperties(noeud, noeudDouble);

		stamp_.push_back(noeudDouble);


		// �tat de l'op�ration	
		if (!premierNoeudPortailTrouve_) {
			premierNoeud_ = noeudDouble;

			premierNoeudAEnvoye_ = noeud;

			noeudDouble->assignerOppose(nullptr);
			premierNoeudPortailTrouve_ = true;

		}

		else {
			// Relier les deux portails
			noeudDouble->assignerOppose(premierNoeud_);
			premierNoeud_->assignerOppose(noeudDouble);


			premierNoeudAEnvoye_->assignerSelection(false);
			noeud->assignerSelection(false);


			if (sendToServer_)
			{
				portalCreationCallback_(premierNoeudAEnvoye_->getUUID(), glm::value_ptr(premierNoeudAEnvoye_->obtenirPositionRelative()), (premierNoeudAEnvoye_->obtenirRotation().y), glm::value_ptr(premierNoeudAEnvoye_->obtenirScale()),
					noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
			}
			premierNoeudPortailTrouve_ = false;
		}

		// Ajout du noeud � l'arbre de rendu
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
/// Cette fonction permet de copier les propri�t�s de l'objet dupliqu�.
///
///	@param[in] noeud : Noeud de l'objet � copier
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