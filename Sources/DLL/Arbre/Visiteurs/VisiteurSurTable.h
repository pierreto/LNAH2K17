///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSurTable.h
/// @author Nam Lesage
/// @date 2016-09-28
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURSURTABLE_H__
#define __APPLICATION_VISITEURS_VISITEURSURTABLE_H__

#include "Visiteurs/VisiteurAbstrait.h"


///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSurTable
/// @brief Permet de savoir si les noeuds sont sur la table
///
///
/// @author Nam Lesage
/// @date 2016-09-28
///////////////////////////////////////////////////////////////////////////
class VisiteurSurTable : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurSurTable();
	/// Destructeur
	virtual ~VisiteurSurTable();

	/// Visiter un accélérateur pour savoir s'il est sur la table
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visiter un maillet pour savoir s'il est sur la table
	virtual void visiterMaillet(NoeudMaillet* noeud);
	/// Visiter une table pour savoir s'il est sur la table (Ne fait rien)
	virtual void visiterTable(NoeudTable* noeud);
	/// Visiter un point de contrôle pour savoir s'il est sur la table
	virtual void visiterPointControl(NoeudPointControl* noeud);
	/// Visiter un mur pour savoir s'il est sur la table
	virtual void visiterMur(NoeudMur* noeud);
	/// Visiter un portail pour savoir s'il est sur la table
	virtual void visiterPortail(NoeudPortail* noeud);

	virtual void visiterRondelle(NoeudRondelle* noeud);

	/// Retourne vrai si le noeud est sur la table
	bool sontSurTable() const;

	/// Verifie si le point est sur la table
	bool pointSurTable(const glm::vec3& sommet);

	/// 
	void reinitialiser();
private:
	
	/// Obtient tous les sommets d'un modele de maniere recursive
	std::vector<glm::vec3> obtenirSommets(NoeudAbstrait* noeud) const;
	std::vector<glm::vec3> obtenirSommets(const modele::Noeud& noeud) const;

	/****************** Test effetue sur les noeud **********************/
	/// Implementation general pour tous les noeuds
	void testPoints(NoeudAbstrait* noeud);
	/// Intersection du mur avec table
    void intersectCubeTable(NoeudAbstrait* noeud);
	/// Intersection d'un boost avec la table
	void intersectCercleTable(NoeudAbstrait* noeud, float rayon);

	/****************** Fonctions Utilitaires **********************/
	/// Eneleve les points 3d en double
	void enleverDoublons(std::vector<glm::vec3>& vec) const;

	/// Attributs du visiteur
	NoeudTable* table_{nullptr};
	bool sontSurTable_;
};


#endif // __APPLICATION_VISITEURS_VISITEURONTABLE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////