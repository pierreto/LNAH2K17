//////////////////////////////////////////////////////////////////////////////
/// @file CollisionTest.h
/// @author Clint Phoeuk
/// @date 2016-10-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_COLLISIONTEST_H
#define _TESTS_COLLISIONTEST_H

#include "glm\glm.hpp"
#include "AideCollision.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>

class DetailsCollision;

///////////////////////////////////////////////////////////////////////////
/// @class CollisionTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        méthodes de la classe Collision
///
/// @author Clint Phoeuk
/// @date 2016-10-21
///////////////////////////////////////////////////////////////////////////
class CollisionTest : public CppUnit::TestFixture
{

	// =================================================================
	// Déclaration de la suite de tests et des méthodes de tests
	//
	// Important, vous devez définir chacun de vos cas de tests à l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas exécuté !
	// =================================================================
	CPPUNIT_TEST_SUITE(CollisionTest);
	CPPUNIT_TEST(testSegmentCercle);
	CPPUNIT_TEST(testCercleCercle);
	CPPUNIT_TEST(testPointDansTriangle);
	CPPUNIT_TEST_SUITE_END();

public:

	// =================================================================
	// Méthodes pour initialiser et 'finaliser' la suite de tests
	// =================================================================

	/// Traitement à effectuer pour initialiser cette suite de tests
	void setUp();

	/// Traitement à effectuer pour 'finaliser' cette suite de tests
	void tearDown();


	// =================================================================
	// Définissez ici les différents cas de tests...
	// =================================================================
	
	/// Test des collisions segment-cercle
	void testSegmentCercle();

	/// Test des collisions cercle-cercle
	void testCercleCercle();

	/// Test pour verifier si notre point est dans un triangle
	void testPointDansTriangle();

private:
	aidecollision::DetailsCollision details;
};

#endif // _TESTS_COLLISIONTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

