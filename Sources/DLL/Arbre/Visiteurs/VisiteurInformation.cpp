///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurInformation.cpp
/// @author Julien Charbonneau
/// @date 2016-09-17
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurInformation.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::VisiteurInformation()
///
/// Constructeur de VisiteurInformation
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurInformation::VisiteurInformation() {
	nbSelections_ = 0;
	modeEcriture_ = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet d'obtenir l'information d'un noeud représentant
/// un accélérateur.
///
///	@param[in] noeud : Un accélérateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurInformation::visiterAccelerateur(NoeudAccelerateur* noeud) {
	traiterNoeud(noeud, true);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet d'obtenir l'information d'un noeud représentant
/// un mur.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurInformation::visiterMur(NoeudMur* noeud) {
	traiterNoeud(noeud, false);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet d'obtenir l'information d'un noeud représentant
/// un portail.
///
///	@param[in] noeud : Un portail
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurInformation::visiterPortail(NoeudPortail* noeud) {
	traiterNoeud(noeud, true);
}

void VisiteurInformation::visiterRondelle(NoeudRondelle * noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::traiterNoeud(NoeudAbstrait* noeud, bool scaleX))
///
/// Cette fonction permet de retenir les informations sur le noeud et 
///	l'actualise si on est en mode écriture
///
///	@param[in] noeud : Un noeud
///	@param[in] scaleX : Si doit scale en x
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurInformation::traiterNoeud(NoeudAbstrait* noeud, bool scaleX) {
	if (noeud->estSelectionne() && !modeEcriture_) {
		position_ = noeud->obtenirPositionRelative();
		rotation_ = noeud->obtenirRotation();
		scaling_ = noeud->obtenirScale();

		nbSelections_++;
	}
	if (noeud->estSelectionne() && modeEcriture_) {
		noeud->deplacer(position_);
		noeud->rotate(glm::radians(rotation_.y), glm::vec3(0, 1, 0));
		if (scaleX) noeud->scale(glm::dvec3(scaling_.z, scaling_.y, scaling_.z)); else noeud->scale(glm::dvec3(1.0f, scaling_.y, scaling_.z));

		modeEcriture_ = false;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::lireInformations(float infos[])
///
/// Cette fonction permet d'assigner les données obtenues dans un 
///	tableau.
///
///	@param[in] infos[] : Un tableau d'information
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
bool VisiteurInformation::lireInformations(float infos[]) {
	if (nbSelections_ != 1) {
		return false;
	}
	else if (infos != nullptr) {
		infos[0] = position_.x;
		infos[1] = position_.y;
		infos[2] = position_.z;

		infos[3] = glm::degrees(rotation_.x);
		infos[4] = glm::degrees(rotation_.y);
		infos[5] = glm::degrees(rotation_.z);

		infos[6] = scaling_.x;
		infos[7] = scaling_.y;
		infos[8] = scaling_.z;
	}

	nbSelections_ = 0;
	return true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurInformation::ecrireInformations(float infos[])
///
/// Cette fonction permet d'écrire les informations retenues dans le tableau.
///
///	@param[in] infos[] : Un tableau d'information
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
bool VisiteurInformation::ecrireInformations(float infos[]) {
	if (infos != nullptr) {
		position_ = glm::dvec3(infos[0], infos[1], infos[2]);
		rotation_ = glm::dvec3(infos[3], infos[4], infos[5]);
		scaling_ = glm::dvec3(infos[6], infos[7], infos[8]);

		modeEcriture_ = true;
		return true;
	}
	return false;
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

