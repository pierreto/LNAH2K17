#pragma once
#include "VisiteurAbstrait.h"

class VisitorByUUID : public VisiteurAbstrait
{
public:
	/// Constructeur pour sélection avec clic
	VisitorByUUID(const char* wantedUuid);
	/// Constructeur avec sélection par rectangle élastique
	/// Destructeur
	virtual ~VisitorByUUID() {};

	/// Visiter l'accélérateur pour la sélection
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter le maillet pour la sélection
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visiter la table pour la sélection
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visiter le point de contrôle pour la sélection
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter le mur pour la sélection
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter le portail pour la sélection
	virtual void visiterPortail(NoeudPortail* noeud);
	/// Visiter la rondelle pour la sélection
	virtual void visiterRondelle(NoeudRondelle* noeud);

	NoeudAbstrait* getNode() const { return wantedNode_; }
	bool hasFound;

private:
	NoeudAbstrait* wantedNode_;
	const char* wantedUuid_;
	void defaultImpl(NoeudAbstrait* noeud);
	bool isUUIDWanted(char* noeud);
};

