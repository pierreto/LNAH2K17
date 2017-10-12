///////////////////////////////////////////////////////////////////////////////
/// @file PhysProperties.h
/// @author Nam Lesage
/// @date 2016-11-03
/// @version 0.2
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_PHYSIQUE_PHYSPROPERTIES_H__
#define __ARBRE_PHYSIQUE_PHYSPROPERTIES_H__

///////////////////////////////////////////////////////////////////////////
/// @class PhysProperties
///
/// Cette classe contient certaines propriétés physiques. Elle implémente le singleton
/// dans le seul et unique but de ne pas polluer l'espace globale. Cette
/// classe est destinée à fournir les propriétés qu'elle contient aux
/// instances de la classe RigidBody qui s'occupe de la physique des objets.
///
/// @author Nam Lesage
/// @date 2016-11-03
///////////////////////////////////////////////////////////////////////////

#define VITESSE_MAX 275.0f

class PhysProperties
{
public:
	/// Obtient l'instance unique de la classe.
	static PhysProperties* obtenirInstance();
	/// Libère l'instance unique de la classe.
	static void libererInstance();

	/// Coefficient de friction
	float coefficientFriction() const;
	/// Coefficient d'accélération
	float coefficientAcceleration() const;
	/// Coefficient de rebondissement
	float coefficientRebondissement() const;
	/// Changer la valeur du coefficient de friction
	void assignerFriction(float friction);
	/// Changer la valeur du coefficient d'accélération
	void assignerAcceleration(float acceleration);
	/// Changer la valeur du coefficient de rebondissement
	void assignerRebondissement(float rebondissement);
	
private:

	/// Constructeur.
	PhysProperties();
	/// Destructeur.
	virtual ~PhysProperties();
	/// Constructeur copie désactivé.
	PhysProperties(const PhysProperties&) = delete;
	/// Opérateur d'assignation désactivé.
	PhysProperties& operator =(const PhysProperties&) = delete;

	/// Pointeur vers l'instance unique de la classe.
	static PhysProperties* instance_;

	
	// Proprietes physiques
	/// Coefficient de friction
	float coefficientFriction_;
	/// Coefficient d'accélération
	float coefficientAcceleration_;
	/// Coefficient d'accélération
	float coefficientRebondissement_;
	
};


#endif // __ARBRE_PHYSIQUE_PHYSPROPERTIES_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

