////////////////////////////////////////////////////////////////////////////////////
/// @file CollisionTest.cpp
/// @author Clint Phoeuk
/// @date 2016-10-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "CollisionTest.h"
#include <iostream>

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(CollisionTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void CollisionTest::setUp()
///
/// Effectue l'initialisation préalable à l'exécution de l'ensemble des
/// cas de tests de cette suite de tests (si nécessaire).
/// 
/// Si certains objets doivent être construits, il est conseillé de le
/// faire ici.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void CollisionTest::setUp()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void CollisionTest::tearDown()
///
/// Effectue les opérations de finalisation nécessaires suite à l'exécution
/// de l'ensemble des cas de tests de cette suite de tests (si nécessaire).
/// 
/// Si certains objets ont été alloués à l'initialisation, ils doivent être
/// désalloués, et il est conseillé de le faire ici.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void CollisionTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void CollisionTest::testSegmentCercle()
///
/// Cas de test: Tester les collisions segment-cercle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void CollisionTest::testSegmentCercle()
{
	// Les deux extremites du segment
	glm::dvec3 point1(0,0,0);
	glm::dvec3 point2(2,0,0);

	// La position du cercle
	glm::dvec3 position(1, 0, -1);

	// Le rayon du cercle
	double rayon = 2.0;

	// Calcul de la collision
	details = aidecollision::calculerCollisionSegment(point1, point2, position, rayon);

	// Tests 
	CPPUNIT_ASSERT(details.type == aidecollision::COLLISION_SEGMENT);
	CPPUNIT_ASSERT(details.enfoncement == 1);
	CPPUNIT_ASSERT(details.direction.z == -1);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void CollisionTest::testCercleCercle()
///
/// Cas de test: Tester les collisions cercle-cercle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void CollisionTest::testCercleCercle()
{
	// Centre du cercle
	glm::dvec2 centre(0, 0);

	// Rayon du cercle
	double rayonCercle = 1;

	// Position de l'objet circulaire
	glm::dvec2 position(1, 0);

	// Rayon de l'objet
	double rayonObjet = 2;

	// Calcul de la collision
	details = aidecollision::calculerCollisionCercle(centre, rayonCercle, position, rayonObjet);

	// Tests
	CPPUNIT_ASSERT(details.type == aidecollision::COLLISION_SPHERE);
	CPPUNIT_ASSERT(details.enfoncement == 2);
	CPPUNIT_ASSERT(details.direction.x == 1);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void CollisionTest::testPointDansTriangle()
///
/// Cas de test: Verifier si le point se retrouve dans le triangle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void CollisionTest::testPointDansTriangle()
{
	// Creation des points du triangle
	glm::vec3 point1 = glm::vec3(0, 0, 0);
	glm::vec3 point2 = glm::vec3(0, 0, 3);
	glm::vec3 point3 = glm::vec3(3, 0, 0);

	// Points a tester
	glm::vec3 pointTest1 = glm::vec3(1, 0, 1);
	glm::vec3 pointTest2 = glm::vec3(4, 0, 4);

	// Tests
	CPPUNIT_ASSERT(aidecollision::pointDansTriangle(pointTest1, point1, point2, point3) == true);
	CPPUNIT_ASSERT(aidecollision::pointDansTriangle(pointTest2, point1, point2, point3) == false);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
