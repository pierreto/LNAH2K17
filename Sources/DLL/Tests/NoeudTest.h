//////////////////////////////////////////////////////////////////////////////
/// @file NoeudTest.h
/// @author Clint Phoeuk
/// @date 2011-11-08
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_NoeudTest_H
#define _TESTS_NoeudTest_H

#include <cppunit/extensions/HelperMacros.h>
#include <memory>

class NoeudAbstrait;

///////////////////////////////////////////////////////////////////////////
/// @class NoeudTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        m�thodes de la classe NoeudAbstrait
///
/// @author Clint Phoeuk
/// @date 2016-11-08
///////////////////////////////////////////////////////////////////////////
class NoeudTest : public CppUnit::TestFixture
{

	// =================================================================
	// D�claration de la suite de tests et des m�thodes de tests
	//
	// Important, vous devez d�finir chacun de vos cas de tests � l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas ex�cut� !
	// =================================================================
	CPPUNIT_TEST_SUITE(NoeudTest);
	CPPUNIT_TEST(testRotate);
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

	/// Test de la rotation
	void testRotate();


private:
	/// Instance d'un noeud abstrait
	std::unique_ptr<NoeudAbstrait> noeud;
};

#endif // _TESTS_NoeudTest_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

