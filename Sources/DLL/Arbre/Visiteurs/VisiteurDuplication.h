///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDuplication.h
/// @author Clint Phoeuk
/// @date 2016-09-29
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURDUPLICATION_H__
#define __APPLICATION_VISITEURS_VISITEURDUPLICATION_H__

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurDuplication
/// @brief Permet de dupliquer des noeuds
///
///
/// @author Clint Phoeuk
/// @date 2016-09-29
///////////////////////////////////////////////////////////////////////////
class VisiteurDuplication : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurDuplication();
	/// Destructeur
	virtual ~VisiteurDuplication();

	/// Visiter un accélérateur pour la duplication
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour la duplication
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter une table pour la duplication
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter un point de contrôle pour la duplication
	virtual void visiterPointControl(NoeudPointControl* noeud) {};
	/// Visiter un mur pour la duplication
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour la duplication
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

	/// Retourne le vecteur trois dimensions pour obtenir le point centre pour la duplication
	glm::vec3 obtenirCentreDuplication();

	/// Copie les propriétés des noeuds à dupliquer
	void copyProperties(NoeudAbstrait* node, NoeudAbstrait* nodeCopy);

private:
	/// Nombre d'items à dupliquer
	int nbItems_;
	/// Vecteur trois dimensions du point centre pour la duplication
	glm::vec3 centreDuplication_;
	/// Premier noeud sélectionné pour la duplication
	NoeudPortail* premierNoeud_{ nullptr };
};


#endif // __APPLICATION_VISITEURS_VISITEURDUPLICATION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////