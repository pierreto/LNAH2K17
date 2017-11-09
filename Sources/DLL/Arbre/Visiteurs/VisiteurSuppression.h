///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSuppression.h
/// @author Julien Charbonneau
/// @date 2016-10-02
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURSUPPRESSION_H__
#define __APPLICATION_VISITEURS_VISITEURSUPPRESSION_H__

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSuppression
/// @brief Permet de supprimer les noeud sélectionnés
///
///
/// @author Julien Charbonneau
/// @date 2016-10-02
///////////////////////////////////////////////////////////////////////////
class VisiteurSuppression : public VisiteurAbstrait {
public:
	/// Constructeur
	VisiteurSuppression();
	/// Destructeur
	virtual ~VisiteurSuppression() {};

	/// Visiter un accélérateur pour la suppression
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour la suppression
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter une table pour la suppression
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter un point de contrôle pour la suppression
	virtual void visiterPointControl(NoeudPointControl* noeud) {};
	/// Visiter un mur pour la suppression
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour la suppression
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

	/// Retourne le nombre de suppressions
	int obtenirNbSupression() const { return nbSuppression_; };
	/// Supprime tous les noeuds sélectionnés
	void deleteAllSelectedNode(bool sendToServer = true);

private:
	/// Nombre de suppressions
	int nbSuppression_;
	/// Vecteur des noeuds à supprimer
	std::vector<NoeudAbstrait*> nodeToDelete_;
};


#endif // __APPLICATION_VISITEURS_VISITEURSUPPRESSION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////