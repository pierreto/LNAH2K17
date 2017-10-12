//////////////////////////////////////////////////////////////////////////////
/// @file RigidBodyTest.h
/// @author Clint Phoeuk
/// @date 2016-10-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_RIGIDBODYTEST_H
#define _TESTS_RIGIDBODYTEST_H

#include "glm\glm.hpp"
#include "RigidBody.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>


///////////////////////////////////////////////////////////////////////////
/// @class RigidBodyTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        m�thodes de la classe RigidBody
///
/// @author Clint Phoeuk
/// @date 2016-11-05
///////////////////////////////////////////////////////////////////////////
class RigidBodyTest : public CppUnit::TestFixture
{

	// =================================================================
	// D�claration de la suite de tests et des m�thodes de tests
	//
	// Important, vous devez d�finir chacun de vos cas de tests � l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas ex�cut� !
	// =================================================================
	CPPUNIT_TEST_SUITE(RigidBodyTest);
	CPPUNIT_TEST(testAssignerVitesse);
	CPPUNIT_TEST(testAjouterVitesse);
	CPPUNIT_TEST(testChangerDirection);
	CPPUNIT_TEST(testAppliquerForce);
	CPPUNIT_TEST(testCalculFriction);
	CPPUNIT_TEST_SUITE_END();

public:

	// =================================================================
	// M�thodes pour initialiser et 'finaliser' la suite de tests
	// =================================================================

	/// Traitement � effectuer pour initialiser cette suite de tests
	void setUp();

	/// Traitement � effectuer pour 'finaliser' cette suite de tests
	void tearDown();


	// =================================================================
	// D�finissez ici les diff�rents cas de tests...
	// =================================================================

	/// Test de l'ajout d'une vitesse a un RigidBody qui a deja une vitesse
	void testAjouterVitesse();

	/// Test de l'assignation de la vitesse a un RigidBody
	void testAssignerVitesse();

	/// Test du changement de direction d'un RigidBody
	void testChangerDirection();

	/// Test de l'application de la force
	void testAppliquerForce();

	/// Test du calcul de la friction
	void testCalculFriction();

private:
	RigidBody object_;
};

#endif // _TESTS_RIGIDBODYTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
