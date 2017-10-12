//////////////////////////////////////////////////////////////////////////////
/// @file LoggerTest.h
/// @author Clint Phoeuk
/// @date 2016-11-08
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_LOGGERTEST_H
#define _TESTS_LOGGERTEST_H

#include "glm\glm.hpp"
#include "Logger.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>


///////////////////////////////////////////////////////////////////////////
/// @class LoggerTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        méthodes de la classe Logger
///
/// @author Clint Phoeuk
/// @date 2016-11-08
///////////////////////////////////////////////////////////////////////////
class LoggerTest : public CppUnit::TestFixture
{

	// =================================================================
	// Déclaration de la suite de tests et des méthodes de tests
	//
	// Important, vous devez définir chacun de vos cas de tests à l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas exécuté !
	// =================================================================
	CPPUNIT_TEST_SUITE(LoggerTest);
	CPPUNIT_TEST(testAfficherTemps);
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

	/// Test qui verifie si le temps est bien affiche
	void testAfficherTemps();

private:
	Logger* log_;
};

#endif // _TESTS_LOGGERTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
