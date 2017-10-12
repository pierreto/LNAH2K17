///////////////////////////////////////////////////////////////////////////////
/// @file RigidBody.h
/// @author Francis Dalp�
/// @date 2016-10-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "Utilitaire.h"
#include "GL/glew.h"
#include "glm\glm.hpp"
#include <vector>
#include "PhysProperties.h"

#define ACCELERATION_GRAVITATIONNELLE 9.8f

class RigidBody 
{
public:
	/// Constructeur avec parametres
	RigidBody(const glm::vec3& vitesse = glm::vec3(0), const glm::vec3& acceleration = glm::vec3(0));

	/// Applique les forces a l'objet
	void appliquerForce(float temps);

	/// Ajoute une force sur l'objet
	void ajouterForce(const glm::vec3& force);

	/// Calculer d�placement � effectuer
	glm::vec3 calculerDeplacement(float temps);

	// Modificateur et accesseurs
	/// Obtient la vitesse du noeud
	glm::vec3 obtenirVitesse() const;

	/// Obtient l'acc�l�ration du noeud
	glm::vec3 obtenirAcceleration() const;

	/// Obtient le coefficient de frottement
	float obtenirFrottement() const;

	/// Obtient la masse
	float obtenirMasse() const;

	/// Change la direction de la vitesse de l'objet
	void changerDirection(glm::vec3 direction);

	/// Assigne une vitesse � l'objet
	void assignerVitesse(glm::vec3 vitesse);

	/// Ajouter vitesse
	void ajouterVitesse(float ajout);

	/// Calcule la force de friction
	glm::vec3 calculerFriction();

	/// Active la physique
	static void enablePhysic();

	/// D�sactive la physique
	static void disablePhysic();

private:

	// Fonctions priv�es
	/// Limite la vitesse � la vitesse maximale
	void limiterVitesse();

	// Attributs
	/// Conteneur des forces � appliquer sur le noeud
	std::vector<glm::vec3> forces_;

	///Vecteur de direction normalis� m/s^2
	glm::vec3 acceleration_;

	///Vitesse de d�placement en m/s
	glm::vec3 vitesse_;

	/// Masse en kg
	float masse_;

	/// Physique d�sactiv�
	static bool disablePhysic_;
};
