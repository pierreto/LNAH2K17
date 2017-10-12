///////////////////////////////////////////////////////////////////////////
/// @file NoeudPointControl.h
/// @author Nam Lesage
/// @date 2016-09-24
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDPOINTCONTROL_H__
#define __ARBRE_NOEUDS_NOEUDPOINTCONTROL_H__

#include "NoeudComposite.h"
#include "NoeudBut.h"
#include "NoeudBooster.h"
#include "GL/glew.h"
#include <vector>

///////////////////////////////////////////////////////////////////////////
/// @class NoeudPointControl
/// @brief Classe pour afficher un point de controle
///
/// @author Nam Lesage
/// @date 2016-09-08
///////////////////////////////////////////////////////////////////////////
class NoeudPointControl : public NoeudComposite {
public:
	/// Constructeur à partir du type du noeud.
	NoeudPointControl(const std::string& typeNoeud);
	/// Destructeur.
	~NoeudPointControl();

	/// Affiche l'accélarateur.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;
	/// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);

	// Modifificateurs
	/// Surcharge de la fonction assignerPositionRelative
	inline virtual void assignerPositionRelative(const glm::vec3& positionRelative);
	/// Permet d'assigner des points de la table au noeud
	void assignerPoints(std::vector<glm::vec3*> points);
	/// Permet d'assigner les voisins du point de controle
	void assignerVoisins(std::vector<NoeudPointControl*> voisins);
	/// Permet d'assigner ou d'obtenir le noeud oppose
	void assignerNoeudOppose(NoeudAbstrait* noeud, const glm::vec3& symmetrie);
	/// Permet d'assigner ou d'obtenir le but associe au noeud
	void assignerBut(NoeudBut* noeud);
	/// Permet d'assigner une zone de deplacement que le noeud doit respecter
	void assignerZoneDeplacement(const glm::vec4& zone);
	/// Permet d'assigner un booster 
	void assignerBooster(NoeudBooster* booster);

	// Accesseurs
	/// Obtient le point de contrôle opposé
	NoeudPointControl* obtenirNoeudOppose() const;
	/// Obtient le but associe
	NoeudBut* obtenirBut() const;
	/// Obtient les voisins
	std::vector<NoeudPointControl*> obtenirVoisins() const;
	/// Permet d'obtenir la symmetrie
	const glm::vec3 obtenirSymmetrie() const;
	/// Permet d'obtenir le rayon de la spere englobante du modele 3D
	double getRayonModele3D();
	///Retourne le point interne de la bordure de la table vvis a vis le point de controle
	glm::vec3 getInternalPosition() { return *(points_[0]); }

	/// Ajuste les points de la table selon la nouvelle position du noeud
	void ajusterPoints();

private:
	/// Pointeurs vers les points de la table
	std::vector<glm::vec3*> points_;
	/// Pointeurs vers les voisins du points de controle
	std::vector<NoeudPointControl*> voisins_;
	/// Pointeur vers le noeud oppose
	NoeudAbstrait* noeudOppose_{ nullptr };
	/// Pointeur vers un but
	NoeudBut* but_{nullptr};
	/// Vecteur appliquant la symmetrie au deplacement du noeud oppose
	glm::vec3 symmetrie_;

	NoeudBooster* booster_{ nullptr };

	/// Zone de deplacment (-x,x, -z, z)
	bool possedeZoneDeplacement_{false};
	glm::vec4 zoneDeplacement_;

	/// Rayon du modele 3D
	double rayonModele3D_;
};

#endif // __ARBRE_NOEUDS_NOEUDPOINTCONTROL_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
