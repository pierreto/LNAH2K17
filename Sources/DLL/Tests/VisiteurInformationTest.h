//////////////////////////////////////////////////////////////////////////////
/// @file VisiteurInformationTest.h
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_VISITEURINFORMATIONTEST_H
#define _TESTS_VISITEURINFORMATIONTEST_H

#include "glm\glm.hpp"
#include "ArbreRenduINF2990.h"
#include "VisiteurInformation.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurInformationTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        méthodes de la classe VisiteurInformation
///
/// @author Clint Phoeuk
/// @date 2016-11-06
///////////////////////////////////////////////////////////////////////////
class VisiteurInformationTest : public CppUnit::TestFixture
{

	// =================================================================
	// Déclaration de la suite de tests et des méthodes de tests
	//
	// Important, vous devez définir chacun de vos cas de tests à l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas exécuté !
	// =================================================================
	CPPUNIT_TEST_SUITE(VisiteurInformationTest);
	CPPUNIT_TEST(testLireInfos);
	CPPUNIT_TEST(testEcrireInfos);
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

	/// Test de l'ecriture de l'information dans le visiteur
	void testEcrireInfos();

	/// Test de la lecture de l'information contenue dans le visiteur
	void testLireInfos();

private:
	ArbreRendu arbre_;
	NoeudAbstrait* noeud1_;
	float infos[9];
};

#endif // _TESTS_VisiteurInformationTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

