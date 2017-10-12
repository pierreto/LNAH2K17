///////////////////////////////////////////////////////////////////////////////
/// @file RigidBody.cpp
/// @author Francis Dalpé
/// @date 2016-10-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "RigidBody.h"


bool RigidBody::disablePhysic_ = false;

////////////////////////////////////////////////////////////////////////
///
/// @fn RigidBody::RigidBody(glm::vec3 vitesse, glm::vec3 acceleration)
///
/// Constructeur avec paramètres du RigidBody
///
///	@param[in] vitesse : La vitesse initiale de l'objet
/// @param[in] acceleration : L'accélération initiale de l'objet
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
RigidBody::RigidBody(const glm::vec3& vitesse, const glm::vec3& acceleration)
	: vitesse_(vitesse), acceleration_(acceleration), masse_(1)
{	
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::appliquerForce(float temps)
///
/// Applique les forces sur l'objet. L'application des forces permet
/// de faire la mise à jour de la vitesse et de l'accélération de l'objet
///
///	@param[in] temps : Le temps entre deux rafraichissements d'écran
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void RigidBody::appliquerForce(float temps) {
	
	glm::vec3 forces(0);

	// Additionner les forces
	for (auto& force : forces_) {
		forces += force;
	}
	forces_.clear();

	// Mise à jour des attributs
	acceleration_ =  forces / masse_;
	vitesse_ += acceleration_ * temps;

	// Limitation de la vitesse
	limiterVitesse();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::ajouterForce(glm::vec3 force)
///
/// Ajoute une force sur l'objet
///
///	@param[in] force : La force
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void RigidBody::ajouterForce(const glm::vec3& force) {
	forces_.push_back(force);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 RigidBody::obtenirVitesse() const
///
/// Obtient la vitesse de l'objet
///
/// @return La vitesse
///
////////////////////////////////////////////////////////////////////////
glm::vec3 RigidBody::obtenirVitesse() const {
	return vitesse_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 RigidBody::obtenirAcceleration() const
///
/// Obtient l'accélération de l'objet
///
/// @return L'accélération
///
////////////////////////////////////////////////////////////////////////
glm::vec3 RigidBody::obtenirAcceleration() const {
	return acceleration_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn float RigidBody::obtenirFrottement() const
///
/// Obtient la constante d'amortissement
///
/// @return La constante d'amortissement
///
////////////////////////////////////////////////////////////////////////
float RigidBody::obtenirFrottement() const {
	return PhysProperties::obtenirInstance()->coefficientFriction();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn float RigidBody::obtenirMasse() const
///
/// Obtient la masse de l'objet
///
/// @return La masse
///
////////////////////////////////////////////////////////////////////////
float RigidBody::obtenirMasse() const {
	return masse_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::changerDirection(glm::vec3 direction)
///
/// Change la direction de la vitesse
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void RigidBody::changerDirection(glm::vec3 direction) {
	if (!utilitaire::EGAL_ZERO(glm::length(vitesse_)))
		vitesse_ = glm::length(vitesse_) * glm::normalize(direction);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::assignerVitesse(glm::vec3 vitesse)
///
/// Assigne une nouvelle vitesse
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void RigidBody::assignerVitesse(glm::vec3 vitesse)
{
	vitesse_ = vitesse;
	limiterVitesse();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::ajouterVitesse(float ajout)
///
/// Ajoute une vitesse à la vitesse courante
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void RigidBody::ajouterVitesse(float ajout)
{
	vitesse_ = (glm::length(vitesse_) + ajout) * glm::normalize(vitesse_);
	limiterVitesse();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 RigidBody::calculerFriction()
///
/// Calcul la friction
///
/// @return La friction
///
////////////////////////////////////////////////////////////////////////
glm::vec3 RigidBody::calculerFriction()
{
	float friction = PhysProperties::obtenirInstance()->coefficientFriction();
	if (glm::length(vitesse_) > 0.1) {
		return masse_ * ACCELERATION_GRAVITATIONNELLE * PhysProperties::obtenirInstance()->coefficientFriction() * glm::normalize(-vitesse_);
	}

	vitesse_ = glm::vec3(0);
	return vitesse_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::enablePhysic()
///
/// Active la physique
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBody::enablePhysic()
{
	disablePhysic_ = false;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::disablePhysic()
///
/// Désactive la physique
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBody::disablePhysic()
{
	disablePhysic_ = true;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBody::limiterVitesse()
///
/// Limite la vitesse de la vitesse courante
///
/// @return La friction
///
////////////////////////////////////////////////////////////////////////
void RigidBody::limiterVitesse()
{
	if (glm::length(vitesse_) > VITESSE_MAX) {
		vitesse_ = VITESSE_MAX * glm::normalize(vitesse_);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn glm::vec3 RigidBody::calculerDeplacement(float temps)
///
/// Calcul du déplacement de l'objet
///
/// @return Le déplacement
///
////////////////////////////////////////////////////////////////////////
glm::vec3 RigidBody::calculerDeplacement(float temps) {
	if (!disablePhysic_)
	{
		ajouterForce(calculerFriction());
		appliquerForce(temps);
		return vitesse_*temps + acceleration_ * glm::pow(temps, 2) / 2.0f;
	}
	else
	{
		return glm::vec3(0);
	}
	
}


