///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDeplacement.h
/// @author Julien Charbonneau
/// @date 2016-09-17
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURDEPLACEMENT_H__
#define __APPLICATION_VISITEURS_VISITEURDEPLACEMENT_H__

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurDeplacement
/// @brief Permet de deplacer les noeuds
///
///
/// @author Julien Charbonneau
/// @date 2016-09-17
///////////////////////////////////////////////////////////////////////////
class VisiteurDeplacement : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurDeplacement(glm::vec3 delta);
	/// Destructeur
	virtual ~VisiteurDeplacement();

	/// Visiter un acc�l�rateur pour le d�placement
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour le d�placement
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter une table pour le d�placement
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter un point de contr�le pour le d�placement
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter un mur pour le d�placement
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour le d�placement
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

private:
	/// Vecteur trois dimensions pour le changement de position de l'objet
	glm::vec3 delta_;
};


#endif // __APPLICATION_VISITEURS_VISITEURDEPLACEMENT_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////