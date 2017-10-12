///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurCollisionRondelle.h
/// @author Nam Lesage
/// @date 2016-10-20
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __APPLICATION_VISITEURS_VISITEURCOLLISIONRONDELLE_H__
#define __APPLICATION_VISITEURS_VISITEURCOLLISIONRONDELLE_H__

#include "Visiteurs/VisiteurAbstrait.h"
#include "AideCollision.h"
#include <map>
#include <vector>
#include "PhysProperties.h"

#define ATTRACTION_PORTAIL 20.0f

using namespace aidecollision;

///////////////////////////////////////////////////////////////////////////
/// @struct ResultatsCollision
///
/// Une structure pour contenir les resultats d'une collision
///
/// @author Nam Lesage
/// @date 2016-11-03
///////////////////////////////////////////////////////////////////////////
struct ResultatsCollision
{
	NoeudAbstrait * noeud;
	DetailsCollision details;
	glm::vec3 force;
};


///////////////////////////////////////////////////////////////////////////
/// @struct Segment
///
/// Un segment compose de deux points
///
/// @author Nam Lesage
/// @date 2016-11-03
///////////////////////////////////////////////////////////////////////////
struct Segment
{
	glm::vec3 p1;
	glm::vec3 p2;
};

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurCollisionRondelle
/// @brief Permet de detecter les collisions de la rondelle avec les objets de l'arbre
///
/// Les collisions calculées sont purement élastiques. Le résultat est récupérable dans
/// la variable vitesseFinale_
///
/// @author Nam Lesage
/// @date 2016-10-20
///////////////////////////////////////////////////////////////////////////
class VisiteurCollisionRondelle : public VisiteurAbstrait
{
public:
	typedef std::map < std::string, std::vector<ResultatsCollision>> Collisions;

	/// Constructeur
	VisiteurCollisionRondelle(NoeudRondelle* rondelle);
	/// Destructeur
	virtual ~VisiteurCollisionRondelle();

	/// Visite un accélérateur pour vérifier la collision
	virtual void visiterAccelerateur(NoeudAccelerateur* noeud);
	/// Visite un maillet pour vérifier la collision
	virtual void visiterMaillet(NoeudMaillet* noeud);
	/// Visite une table pour vérifier la collision
	virtual void visiterTable(NoeudTable* noeud);
	/// Visite un point de contrôle pour vérifier la collision
	virtual void visiterPointControl(NoeudPointControl* noeud) {};
	/// Visite un mur pour vérifier la collision
	virtual void visiterMur(NoeudMur* noeud);
	/// Visite un portail pour vérifier la collision
	virtual void visiterPortail(NoeudPortail* noeud);
	/// Visite une rondelle pour vérifier la collision
	virtual void visiterRondelle(NoeudRondelle* noeud) {};
	/// Visite un but pour vérifier la collision
	virtual void visiterBut(NoeudBut* noeud);

	// Accesseurs
	/// Retourne la force de la collision
	inline glm::vec3 obtenirForce() const { return force_; };
	/// Obtient la vitesse finale de la rondelle
	inline glm::vec3 obtenirVitesseFinale() const { return vitesseFinale_; };
	/// Obtient le nombre de collisions
	inline int obtenirNombreCollisions() const { return nombreCollisions_; }
	/// Obtient tous les resultats de collision
	inline Collisions obtenirCollisions() { return collisions_; }

	/// Calcule la vitesse finales de la rondelle
	glm::vec3 calculerVitesseFinale();
	/// Calcule l'enfoncement resultant de multiple collisions
	glm::vec3 calculerEnfoncement(std::vector<ResultatsCollision> collisions);

	// Vérification de la validité de la position de la rondelle
	/// Vérifie si la rondelle est sur la patinoire et dans le but
	bool rondelleSurTable(const glm::vec3& sommet) const;
	/// Vérifie si la rondelle est présentement dans un mur
	bool estDansMur();

private :
	// Fonctions privées
	/// Test si deux vecteurs sont parallèles
	bool areParallel(glm::vec3 v1, glm::vec3 v2);
	/// Obtenir le contour de la table avec les buts
	std::vector<glm::vec3> obtenirBordureTable(NoeudTable* table) const;
	/// Ajoute un résultat de collision à la map
	void ajouterCollision(std::string type, ResultatsCollision resultat);
	/// Vérifie s'il y a tel type de collision
	bool enCollisionAvec(const std::string& type) const;
	///  Calcule les collisions avec un mur
	void resultatVisiterMur( NoeudMur* noeud, std::vector<ResultatsCollision>& resultats, std::vector<int>& segmentsEnCollision);

	// Attributs
	/// Booléen pour savoir s'il y a eu une collision
	unsigned int nombreCollisions_;
	/// Résultats des collisions
	Collisions collisions_;
	/// Si la rondelle est dans un mur
	bool dansMur_;
	/// Vitesse finale de l'objet après une collision élastique
	glm::vec3 vitesseFinale_;
	/// La force générée
	glm::vec3 force_;

	// Données de la rondelle
	/// La rondelle
	NoeudRondelle* rondelle_;
	double rayonR_;
	glm::dvec3 vitesseR_;
	double masseR_;
};




#endif // __APPLICATION_VISITEURS_VISITEURCOLLISIONRONDELLE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////