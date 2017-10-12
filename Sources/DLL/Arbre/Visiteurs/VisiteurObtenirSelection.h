///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurObtenirSelection.h
/// @author Nam Lesage
/// @date 2016-09-27
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEUROBTENIRSELECTION_H__
#define __APPLICATION_VISITEURS_VISITEUROBTENIRSELECTION_H__

#include "Visiteurs/VisiteurAbstrait.h"
#include "NoeudAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurObtenirSelection
/// @brief Permet d'obtenir tous les noeuds selectionn�s
///
///
/// @author Nam Lesage
/// @date 2016-09-27
///////////////////////////////////////////////////////////////////////////
class VisiteurObtenirSelection : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurObtenirSelection();
	/// Destructeur
	virtual ~VisiteurObtenirSelection();

	/// Visiter un acc�l�rateur pour obtenir la s�lection du noeud
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour obtenir la s�lection du noeud
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter une table pour obtenir la s�lection du noeud
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter un point de contr�le pour obtenir la s�lection du noeud
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter un mur pour obtenir la s�lection du noeud
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour obtenir la s�lection du noeud
	virtual void visiterPortail(NoeudPortail* noeud);
	/// Visiter la rondelle
	virtual void visiterRondelle(NoeudRondelle* noeud) {};

	/// Vecteur regroupant les noeuds dont on veut obtenir leur s�lection
	std::vector<NoeudAbstrait*> obtenirNoeuds();

	/// Retourne le vecteur trois dimensions pour obtenir le point centre pour la s�lection
	glm::dvec3 obtenirCentreSelection();

private:
	/// Vecteur regroupant les noeuds sp�cifiques
	std::vector<NoeudAbstrait*> noeuds_;
};


#endif // __APPLICATION_VISITEURS_VISITEUROBTENIRSELECTION_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////