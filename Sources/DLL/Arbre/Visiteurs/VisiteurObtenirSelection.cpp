///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurObtenirSelection.cpp
/// @author Nam Lesage
/// @date 2016-09-27
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurObtenirSelection.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::VisiteurObtenirSelection()
///
/// Constructeur de VisiteurObtenirSelection
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurObtenirSelection::VisiteurObtenirSelection() {

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::~VisiteurObtenirSelection()
///
/// Destructeur de VisiteurObtenirSelection
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurObtenirSelection::~VisiteurObtenirSelection() {

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiteur un accélérateur pour la sélection. 
/// Si le noeud est sélectionné, on l'ajoute au vecteur.
///
///	@param[in] noeud : Un accélérateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurObtenirSelection::visiterAccelerateur(NoeudAccelerateur* noeud) {
	if (noeud->estSelectionne()) {
		noeuds_.push_back(noeud);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::visiterPointControl(NoeudPointControl * noeud)
///
/// Cette fonction permet de visiteur un point de contrôle pour la sélection. 
/// Si le noeud est sélectionné, on l'ajoute au vecteur.
///
///	@param[in] noeud : Un point de contrôle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurObtenirSelection::visiterPointControl(NoeudPointControl * noeud)
{
	if (noeud->estSelectionne()) {
		noeuds_.push_back(noeud);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiteur un mur pour la sélection. 
/// Si le noeud est sélectionné, on l'ajoute au vecteur.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurObtenirSelection::visiterMur(NoeudMur* noeud) {
	if (noeud->estSelectionne()) {
		noeuds_.push_back(noeud);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiteur un portail pour la sélection. 
/// Si le noeud est sélectionné, on l'ajoute au vecteur.
///
///	@param[in] noeud : Un portail
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurObtenirSelection::visiterPortail(NoeudPortail* noeud) {
	if (noeud->estSelectionne()) {
		noeuds_.push_back(noeud);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurObtenirSelection::obtenirNoeuds()
///
/// Cette fonction retourne le vecteur de noeuds.
///
/// @return noeuds_
///
////////////////////////////////////////////////////////////////////////
std::vector<NoeudAbstrait*> VisiteurObtenirSelection::obtenirNoeuds() {
	return noeuds_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 VisiteurObtenirSelection::obtenirCentreSelection()
///
/// Cette fonction calcule et retourne le centre de la sélection.
///
/// @return Le centre de la sélection
///
////////////////////////////////////////////////////////////////////////
glm::dvec3 VisiteurObtenirSelection::obtenirCentreSelection() {
	glm::dvec3 centre(0);

	for (auto noeud : noeuds_) {
		centre += noeud->obtenirPositionRelative();
	}

	if (noeuds_.size()) {
		centre /= noeuds_.size();
	}

	return centre;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////