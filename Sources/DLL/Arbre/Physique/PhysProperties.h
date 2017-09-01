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
/// Cette classe contient certaines propri�t�s physiques. Elle impl�mente le singleton
/// dans le seul et unique but de ne pas polluer l'espace globale. Cette
/// classe est destin�e � fournir les propri�t�s qu'elle contient aux
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
	/// Lib�re l'instance unique de la classe.
	static void libererInstance();

	/// Coefficient de friction
	float coefficientFriction() const;
	/// Coefficient d'acc�l�ration
	float coefficientAcceleration() const;
	/// Coefficient de rebondissement
	float coefficientRebondissement() const;
	/// Changer la valeur du coefficient de friction
	void assignerFriction(float friction);
	/// Changer la valeur du coefficient d'acc�l�ration
	void assignerAcceleration(float acceleration);
	/// Changer la valeur du coefficient de rebondissement
	void assignerRebondissement(float rebondissement);
	
private:

	/// Constructeur.
	PhysProperties();
	/// Destructeur.
	virtual ~PhysProperties();
	/// Constructeur copie d�sactiv�.
	PhysProperties(const PhysProperties&) = delete;
	/// Op�rateur d'assignation d�sactiv�.
	PhysProperties& operator =(const PhysProperties&) = delete;

	/// Pointeur vers l'instance unique de la classe.
	static PhysProperties* instance_;

	
	// Proprietes physiques
	/// Coefficient de friction
	float coefficientFriction_;
	/// Coefficient d'acc�l�ration
	float coefficientAcceleration_;
	/// Coefficient d'acc�l�ration
	float coefficientRebondissement_;
	
};


#endif // __ARBRE_PHYSIQUE_PHYSPROPERTIES_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

