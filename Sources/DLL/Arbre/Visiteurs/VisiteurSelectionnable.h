///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSelectionnable.h
/// @author Nam Lesage
/// @date 2016-09-29
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURSELECTIONNABLE_H__
#define __APPLICATION_VISITEURS_VISITEURSELECTIONNABLE_H__

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSelectionnable
/// @brief Permet de rendre selectionnable ou non un type de noeud
///
///
/// @author Nam Lesage
/// @date 2016-09-29
///////////////////////////////////////////////////////////////////////////
class VisiteurSelectionnable : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurSelectionnable(const std::string& type, const bool& selectionnable = false);
	/// Destructeur
	virtual ~VisiteurSelectionnable();

	/// Visiter un acc�l�rateur pour le rendre s�lectionnable
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour le rendre s�lectionnable
	virtual void visiterMaillet(NoeudMaillet* noeud);
	/// Visiter une table pour le rendre s�lectionnable
	virtual void visiterTable(NoeudTable* noeud);
	/// Visiter un point de contr�le pour le rendre s�lectionnable
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter un mur pour le rendre s�lectionnable
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour le rendre s�lectionnable
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

	/// Assigne un type au noeud
	void assignerType(const std::string& type);
	/// Rend un noeud s�lectionnable
	void assignerSelectionnable(const bool& selectionnable = false);

private:

	/// Impl�mente un noeud par d�faut
	void implementationDefaut(NoeudAbstrait* noeud);

	/// Type du noeud
	std::string type_;
	/// Bool pour la caract�ristique de s�lection du noeud
	bool selectionnable_;
};


#endif // __APPLICATION_VISITEURS_VISITEURSELECTIONNABLE_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////