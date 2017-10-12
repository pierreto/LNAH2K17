///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurInformation.h
/// @author Julien Charbonneau
/// @date 2016-09-28
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEUINFORMATION_H__
#define __APPLICATION_VISITEURS_VISITEUINFORMATION_H__

#include "Visiteurs/VisiteurAbstrait.h"

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurInformation
/// @brief Permet de récupérer l'information d'un noeud sélectionné
///
///
/// @author Julien Charbonneau
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////
class VisiteurInformation : public VisiteurAbstrait {
public:
	/// Constructeur
	VisiteurInformation();
	/// Destructeur
	virtual ~VisiteurInformation() {};

	/// Visite un accélérateur pour récupérer son information
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visite un maillet pour récupérer son information
	virtual void visiterMaillet(NoeudMaillet* noeud) {};
	/// Visite une table pour récupérer son information
	virtual void visiterTable(NoeudTable* noeud) {};
	/// Visite un point de contrôle pour récupérer son information
	virtual void visiterPointControl(NoeudPointControl* noeud) {};
	/// Visite un mur pour récupérer son information
	virtual void visiterMur(NoeudMur* noeud);
	/// Visite un portail pour récupérer son information
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

	/// Retourne vrai si l'information a été lue
	bool lireInformations(float infos[]);
	/// Écrit l'information dans un tableau
	bool ecrireInformations(float infos[]);

private:
	/// Vecteurs trois dimensions regroupant les informations du noeud
	glm::vec3 position_;
	glm::vec3 rotation_;
	glm::vec3 scaling_;
	/// Nombre de noeuds sélectionnés
	int nbSelections_;
	/// Bool pour le mode écriture
	bool modeEcriture_;

	/// Traite le noeud envoyé en paramètre
	void traiterNoeud(NoeudAbstrait* noeud, bool scaleX);
};


#endif // __APPLICATION_VISITEURS_VISITEUINFORMATION_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
