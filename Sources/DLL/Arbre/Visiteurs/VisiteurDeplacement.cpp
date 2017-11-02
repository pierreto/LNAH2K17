///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDeplacement.cpp
/// @author Julien Charbonneau
/// @date 2016-09-17
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurDeplacement.h"
#include "ModeleEtatJeu.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::VisiteurDeplacement(glm::vec3 delta)
///
/// Constructeur de VisiteurDeplacement.
///
///	@param[in] delta: Vecteur trois dimensions pour le changement de position de l'objet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurDeplacement::VisiteurDeplacement(glm::vec3 delta, MoveEventCallback move) {
	delta_ = delta;
	moveEventCallback_ = move;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::VisiteurDeplacement(glm::vec3 delta)
///
/// Destructeur de VisiteurDeplacement.
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurDeplacement::~VisiteurDeplacement() {

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un accélérateur pour le déplacement.
///
///	@param[in] noeud: Noeud de l'objet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::visiterAccelerateur(NoeudAccelerateur* noeud) {
	if (noeud->estSelectionne())
	{
		noeud->deplacer(noeud->obtenirPositionRelative() + delta_);
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			moveEventCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));
		}

	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::visiterPointControl(NoeudPointControl * noeud)
///
/// Cette fonction permet de visiter un point de contrôle pour le déplacement.
///
///	@param[in] noeud: Noeud de l'objet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::visiterPointControl(NoeudPointControl * noeud)
{
	if (noeud->estSelectionne()) {
		glm::dvec3 pos(noeud->obtenirPositionRelative());
		pos += delta_;

		// Deplacer le premier noeud
		noeud->deplacer(pos);

		// Deplacer son opposé
		noeud->obtenirNoeudOppose()->deplacer(pos * glm::dvec3(noeud->obtenirSymmetrie()));
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			moveEventCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));

			moveEventCallback_(noeud->obtenirNoeudOppose()->getUUID(), glm::value_ptr(noeud->obtenirNoeudOppose()->obtenirPositionRelative()));
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiter un mur pour le déplacement.
///
///	@param[in] noeud: Noeud de l'objet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::visiterMur(NoeudMur* noeud) {
	if (noeud->estSelectionne())
	{
		noeud->deplacer(noeud->obtenirPositionRelative() + delta_);
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			moveEventCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));
		}

	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail pour le déplacement.
///
///	@param[in] noeud: Noeud de l'objet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::visiterPortail(NoeudPortail* noeud) {
	if (noeud->estSelectionne())
	{
		noeud->deplacer(noeud->obtenirPositionRelative() + delta_);
		if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
		{
			moveEventCallback_(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));
		}
	}
}

void VisiteurDeplacement::visiterRondelle(NoeudRondelle * noeud)
{
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////