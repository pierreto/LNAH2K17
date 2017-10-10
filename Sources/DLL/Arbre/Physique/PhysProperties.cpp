#include "PhysProperties.h"
///////////////////////////////////////////////////////////////////////////////
/// @file PhysProperties.cpp
/// @author Nam Lesage
/// @date 2016-11-03
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <iostream>
#include "PhysProperties.h"

/// Pointeur vers l'instance unique de la classe.
PhysProperties* PhysProperties::instance_{ nullptr };

////////////////////////////////////////////////////////////////////////
///
/// @fn PhysProperties* PhysProperties::obtenirInstance()
///
/// Cette fonction retourne un pointeur vers l'instance unique de la
/// classe.  Si cette instance n'a pas �t� cr��e, elle la cr�e.  Cette
/// cr�ation n'est toutefois pas n�cessairement "thread-safe", car
/// aucun verrou n'est pris entre le test pour savoir si l'instance
/// existe et le moment de sa cr�ation.
///
/// @return Un pointeur vers l'instance unique de cette classe.
///
////////////////////////////////////////////////////////////////////////
PhysProperties * PhysProperties::obtenirInstance()
{
	if (instance_ == nullptr)
		instance_ = new PhysProperties();
	return instance_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PhysProperties::libererInstance()
///
/// Cette fonction lib�re l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PhysProperties::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn float PhysProperties::coefficientFriction() const
///
/// Cette fonction retourne le coefficient de friction
///
/// @return Le coefficient de friction.
///
////////////////////////////////////////////////////////////////////////
float PhysProperties::coefficientFriction() const
{
	return coefficientFriction_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn float PhysProperties::coefficientAcceleration() const
///
/// Cette fonction retourne le coefficient d'acc�l�ration
///
/// @return Le coefficient d'acc�l�ration.
///
////////////////////////////////////////////////////////////////////////
float PhysProperties::coefficientAcceleration() const
{
	return coefficientAcceleration_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn float PhysProperties::coefficientRebondissement() const
///
/// Cette fonction retourne le coefficient de rebondissement
///
/// @return Le coefficient de rebondissement.
///
////////////////////////////////////////////////////////////////////////
float PhysProperties::coefficientRebondissement() const
{
	return coefficientRebondissement_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PhysProperties::assignerFriction(float friction)
///
/// Cette fonction met � jour le coefficient de friction
///
/// @param[in] friction : Le coefficient de friction
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PhysProperties::assignerFriction(float friction)
{
	coefficientFriction_ = friction;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PhysProperties::assignerAcceleration(float acceleration)
///
/// Cette fonction met � jour le coefficient d'acc�l�ration
///
/// @param[in] acceleration : Le coefficient d'acc�l�ration
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PhysProperties::assignerAcceleration(float acceleration)
{
	coefficientAcceleration_ = acceleration;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PhysProperties::assignerRebondissement(float rebondissement)
///
/// Cette fonction met � jour le coefficient de rebondissement
///
/// @param[in] rebondissement : Le coefficient de rebondissement
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PhysProperties::assignerRebondissement(float rebondissement)
{
	coefficientRebondissement_ = rebondissement;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn PhysProperties::PhysProperties()
///
/// Constructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
PhysProperties::PhysProperties()
{
	coefficientFriction_ = 0.4;
	coefficientAcceleration_ = 40;
	coefficientRebondissement_ = 0;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn PhysProperties::~PhysProperties()
///
/// Destructeur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
PhysProperties::~PhysProperties() {
	if (instance_ != nullptr) {
		libererInstance();
	}
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////