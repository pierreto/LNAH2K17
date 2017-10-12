////////////////////////////////////////////////////////////////////////////////////
/// @file RigidBodyTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-05
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "RigidBodyTest.h"
#include "PhysProperties.h"
#include <iostream>

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(RigidBodyTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAbstraitTest::setUp()
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
void RigidBodyTest::setUp()
{

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBodyTest::tearDown()
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
void RigidBodyTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBodyTest::testAssignerVitesse()
///
/// Cas de test: Test de l'assignation d'une vitesse sur un RigidBody
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBodyTest::testAssignerVitesse()
{
	// Assignation d'une vitesse a l'objet
	glm::vec3 vitesse = glm::vec3(3, 0, 4);
	object_.assignerVitesse(vitesse);

	// Verifier si la vitesse est bonne
	CPPUNIT_ASSERT(object_.obtenirVitesse().x == 3);
	CPPUNIT_ASSERT(object_.obtenirVitesse().z == 4);

	// Tester la vitesse maximale
	glm::vec3 vitesseMax = glm::vec3(500, 0, 0);
	object_.assignerVitesse(vitesseMax);

	CPPUNIT_ASSERT(object_.obtenirVitesse().x == VITESSE_MAX);
	CPPUNIT_ASSERT(object_.obtenirVitesse().y == 0);
	CPPUNIT_ASSERT(object_.obtenirVitesse().z == 0);
	
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBodyTest::testAjouterVitesse()
///
/// Cas de test: Test de l'ajout d'une vitesse a un RigidBody qui a deja une vitesse
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBodyTest::testAjouterVitesse()
{
	// Assignation d'une vitesse a l'objet
	glm::vec3 velocity = glm::vec3(3, 0, 4);
	object_.assignerVitesse(velocity);

	// Ajout d'une vitesse
	float vitesse = 10.0;
	object_.ajouterVitesse(vitesse);

	CPPUNIT_ASSERT(object_.obtenirVitesse().x == 9.0);
	CPPUNIT_ASSERT(object_.obtenirVitesse().z == 12.0);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBodyTest::testChangerDirection()
///
/// Cas de test: Test du changement de direction sur un RigidBody
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBodyTest::testChangerDirection()
{
	// Assignation d'une vitesse a l'objet
	glm::vec3 velocity = glm::vec3(3, 0, 4);
	object_.assignerVitesse(velocity);

	// Changement de direction
	glm::vec3 direction = glm::vec3(4, 0, 3);
	object_.changerDirection(direction);

	CPPUNIT_ASSERT(object_.obtenirVitesse().x == 4.0);
	CPPUNIT_ASSERT(object_.obtenirVitesse().z == 3.0);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBodyTest::testAppliquerForce()
///
/// Cas de test: Test de l'application de la force
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBodyTest::testAppliquerForce()
{
	// Assignation d'une force de 10N
	object_.ajouterForce(glm::vec3(10, 0, 0));

	// Appliquer la force
	object_.appliquerForce(5);

	CPPUNIT_ASSERT(object_.obtenirAcceleration().x == 10 && object_.obtenirAcceleration().y == 0 && object_.obtenirAcceleration().z == 0);
	CPPUNIT_ASSERT(object_.obtenirVitesse().x == 50 && object_.obtenirAcceleration().y == 0 && object_.obtenirAcceleration().z == 0);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void RigidBodyTest::testCalculFriction()
///
/// Cas de test: Test du calcul de la friction
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void RigidBodyTest::testCalculFriction()
{
	// Assignation d'une vitesse nulle
	object_.assignerVitesse(glm::vec3(0));

	// Verifier que la force de friction est bien nulle
	CPPUNIT_ASSERT(object_.calculerFriction().x == 0);
	CPPUNIT_ASSERT(object_.calculerFriction().y == 0);
	CPPUNIT_ASSERT(object_.calculerFriction().z == 0);

	// Assignation d'une vitesse non nulle
	object_.assignerVitesse(glm::vec3(10, 0, 0));

	// Assigner le coefficient de friction
	PhysProperties::obtenirInstance()->assignerFriction(1);

	// Verifier que la force de friction agit dans le sens inverse de la vitesse
	CPPUNIT_ASSERT(object_.calculerFriction().x == -9.8f);
	CPPUNIT_ASSERT(object_.calculerFriction().y == 0);
	CPPUNIT_ASSERT(object_.calculerFriction().z == 0);
}
