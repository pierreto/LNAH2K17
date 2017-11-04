///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSelection.h
/// @author Julien Charbonneau
/// @date 2016-09-17
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURSELECTION_H__
#define __APPLICATION_VISITEURS_VISITEURSELECTION_H__

#include "Visiteurs/VisiteurAbstrait.h"
#include "FacadeInterfaceNative.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSelection
/// @brief Permet de selectionner les noeuds
///
///
/// @author Julien Charbonneau
/// @date 2016-09-17
///////////////////////////////////////////////////////////////////////////
class VisiteurSelection : public VisiteurAbstrait {
public:
	/// Constructeur pour s�lection avec clic
	VisiteurSelection(glm::dvec3 rayStart, glm::dvec3 rayEnd, bool ctrl);
	/// Constructeur avec s�lection par rectangle �lastique
	VisiteurSelection(glm::dvec3 pointAncrage, glm::dvec3 pointFinal, bool multiSelection, bool ctrl);
	/// Destructeur
	virtual ~VisiteurSelection() {};

	/// Visiter l'acc�l�rateur pour la s�lection
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter le maillet pour la s�lection
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter la table pour la s�lection
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter le point de contr�le pour la s�lection
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter le mur pour la s�lection
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter le portail pour la s�lection
	virtual void visiterPortail(NoeudPortail* noeud);
	/// Visiter la rondelle pour la s�lection
	virtual void visiterRondelle(NoeudRondelle* noeud) {};

	/// Retourne le nombre de noeuds s�lectionn�s
	int getNbSelections() const { return nbSelections_; };

private:
	/// Effectue un test par collision sph�rique � l'aide de raycast pour confirmer la s�lection
	void sphereCollisionTest(NoeudAbstrait* noeud);
	/// Effectue un test par collision cubique � l'aide de raycast pour confirmer la s�lection
	void cubeCollisionTest(NoeudAbstrait* noeud);
	/// Effectue un test par collision cylindrique � l'aide de raycast pour confirmer la s�lection
	void cylinderCollisionTest(NoeudAbstrait* noeud);
	void handleSelection(NoeudAbstrait* node);
	/// S�lectionne le centre du noeud
	void selectionCentreNoeud(NoeudAbstrait* noeud);

	/// Nombre de noeuds s�lectionn�s
	int nbSelections_;
	/// Bool pour une s�lection multiple ou non
	bool multiSelection_;
	/// Bool pour l'appuie de la touche CTRL
	bool ctrl_;
	/// Vecteur trois dimensions pour la position initiale de la souris lors d'une s�lection
	glm::dvec3 pointDebut_;
	/// Vecteur trois dimensions pour la position finale de la souris lors d'une s�lection
	glm::dvec3 pointFin_;

};


#endif // __APPLICATION_VISITEURS_VISITEURSELECTION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////