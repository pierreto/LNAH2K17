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
VisiteurDuplication::VisiteurDuplication()
	: nbItems_(0), centreDuplication_(glm::vec3(0,0,0))
{
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

		// Ajout du noeud � l'arbre de rendu
		FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->ajouter(noeudDouble);

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

		// �tat de l'op�ration	
		if (premierNoeud_ == nullptr) {
			premierNoeud_ = noeudDouble;
			noeudDouble->assignerOppose(nullptr);
		}

		else {
			// Relier les deux portails
			noeudDouble->assignerOppose(premierNoeud_);
			premierNoeud_->assignerOppose(noeudDouble);
			premierNoeud_ = nullptr;
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