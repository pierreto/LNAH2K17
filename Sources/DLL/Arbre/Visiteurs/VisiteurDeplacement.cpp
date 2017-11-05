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
#include "ModeleEtatDeplacement.h"
#include "ModeleEtatPointControl.h"

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
VisiteurDeplacement::VisiteurDeplacement(glm::vec3 delta) {
	delta_ = delta;
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
	defaultVisit(noeud);
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
			ControlPointEventCallback callback = ModeleEtatPointControl::obtenirInstance()->getControlPointEventCallback();
			if(callback)
			{
				callback(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()));

				callback(noeud->obtenirNoeudOppose()->getUUID(), glm::value_ptr(noeud->obtenirNoeudOppose()->obtenirPositionRelative()));
			}
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
	defaultVisit(noeud);
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
	defaultVisit(noeud);
}

void VisiteurDeplacement::visiterRondelle(NoeudRondelle * noeud)
{
}

void VisiteurDeplacement::defaultVisit(NoeudAbstrait* noeud)
{
		if (noeud->estSelectionne())
	{
		noeud->deplacer(noeud->obtenirPositionRelative() + delta_);
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