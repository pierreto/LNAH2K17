#pragma once
#include "VisiteurAbstrait.h"

class VisitorByUUID : public VisiteurAbstrait
{
public:
	/// Constructeur pour s�lection avec clic
	VisitorByUUID(const char* wantedUuid);
	/// Constructeur avec s�lection par rectangle �lastique
	/// Destructeur
	virtual ~VisitorByUUID() {};

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
	virtual void visiterRondelle(NoeudRondelle* noeud);

	NoeudAbstrait* getNode() const { return wantedNode_; }
	bool hasFound;

private:
	NoeudAbstrait* wantedNode_;
	const char* wantedUuid_;
	void defaultImpl(NoeudAbstrait* noeud);
	bool isUUIDWanted(char* noeud);
};

