///////////////////////////////////////////////////////////////////////////
/// @file NoeudMaillet.h
/// @author Nam Lesage
/// @date 2016-09-08
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDMAILLET_H__
#define __ARBRE_NOEUDS_NOEUDMAILLET_H__


#include "NoeudAbstrait.h"
#include "GL/glew.h"


///////////////////////////////////////////////////////////////////////////
/// @class NoeudMaillet
/// @brief Classe pour afficher un maillet
///
/// @author Nam Lesage
/// @date 2016-09-08
///////////////////////////////////////////////////////////////////////////
class NoeudMaillet : public NoeudAbstrait {
public:
	/// Constructeur à partir du type du noeud.
	NoeudMaillet(const std::string& typeNoeud, const char* uuid);
	/// Destructeur.
	~NoeudMaillet();

	/// Affiche le cube.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;

	/// Anime l'accélérateur
	virtual void animer(float temps);
	
	// Accesseurs 
	/// Obtient la vitesse
	glm::vec3 obtenirVitesse() const;
	/// Obtenir l'id
	int obtenirId() const;

	// Modificateurs
	/// Assigner un Id
	void assignerId(const int& id);

	/// Obtenir le collider
	utilitaire::CylindreEnglobant obtenirCollider();
	/// Calcule le collider du mur
	virtual void calculerCollider();

	// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur);


	void setCurrentTexture(std::string textureName);
	void freeCurrentTexture();

private:
	/// Le collider du maillet
	utilitaire::CylindreEnglobant collider_;
	/// La vitesse du maillet
	glm::vec3  vitesse_;
	/// La derni
	glm::vec3 lastPosition_;
	/// l'id du maillet
	unsigned int id_;
	
};


#endif // __ARBRE_NOEUDS_NOEUDMAILLET_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
