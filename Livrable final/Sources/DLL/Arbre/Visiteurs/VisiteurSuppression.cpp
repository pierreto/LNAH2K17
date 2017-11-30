///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSuppression.cpp
/// @author Julien Charbonneau
/// @date 2016-10-02
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "VisiteurSuppression.h"
#include "FacadeModele.h"
#include "ModeleEtatJeu.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSuppression::VisiteurSuppression()
///
/// Cette fonction permet de visiter un accélérateur pour la suppression.
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
VisiteurSuppression::VisiteurSuppression() 
	: nbSuppression_(0)
{

}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSuppression::visiterAccelerateur(NoeudAccelerateur* noeud)
///
/// Cette fonction permet de visiter un accélérateur pour la suppression.
///
///	@param[in] noeud : Un accélérateur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSuppression::visiterAccelerateur(NoeudAccelerateur* noeud) {
	if (noeud->estSelectionne()) {
		nodeToDelete_.push_back(noeud);
		nbSuppression_++;
	}	
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSuppression::visiterMur(NoeudMur* noeud)
///
/// Cette fonction permet de visiter un mur pour la suppression.
///
///	@param[in] noeud : Un mur
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSuppression::visiterMur(NoeudMur* noeud) {
	if (noeud->estSelectionne()) {
		nodeToDelete_.push_back(noeud);
		nbSuppression_++;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSuppression::visiterPortail(NoeudPortail* noeud)
///
/// Cette fonction permet de visiter un portail pour la suppression.
///
///	@param[in] noeud : Un portail
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSuppression::visiterPortail(NoeudPortail* noeud) {
	if (noeud->estSelectionne()) {
		NoeudPortail* linkedNode = noeud->obtenirOppose();

		nodeToDelete_.push_back(noeud);
		nodeToDelete_.push_back(linkedNode);
		nbSuppression_++;
	}
}

void VisiteurSuppression::visiterRondelle(NoeudRondelle * noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurSuppression::deleteAllSelectedNode()
///
/// Cette fonction permet de supprimer tous les noeuds sélectionnés pour la suppression.
///
///	@param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void VisiteurSuppression::deleteAllSelectedNode(bool sendToServer) {
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	for(NoeudAbstrait* node : nodeToDelete_) {
		if(node)
		{
			arbre->effacer(node, ModeleEtatJeu::obtenirInstance()->getDeleteEventCallback(), sendToServer);
		}
	}
	if(sendToServer)
	{
		ModeleEtatJeu::obtenirInstance()->getDeleteEventCallback()("END");
	}
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////