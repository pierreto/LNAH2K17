//////////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduTest.h
/// @author Clint Phoeuk
/// @date 2016-11-05
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_ARBRERENDUTEST_H
#define _TESTS_ARBRERENDUTEST_H

#include "glm\glm.hpp"
#include <cppunit/extensions/HelperMacros.h>
#include "ArbreRenduINF2990.h"
#include <memory>

///////////////////////////////////////////////////////////////////////////
/// @class ArbreRenduTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        m�thodes de la classe ArbreRendu
///
/// @author Clint Phoeuk
/// @date 2016-11-05
///////////////////////////////////////////////////////////////////////////
class ArbreRenduTest : public CppUnit::TestFixture
{

	// =================================================================
	// D�claration de la suite de tests et des m�thodes de tests
	//
	// Important, vous devez d�finir chacun de vos cas de tests � l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas ex�cut� !
	// =================================================================
	CPPUNIT_TEST_SUITE(ArbreRenduTest);
	CPPUNIT_TEST(testCreerNoeud);
	CPPUNIT_TEST(testAjouterNoeud);
	CPPUNIT_TEST(testEffacerNoeud);
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

	/// Tests de la creation des noeuds
	void testCreerNoeud();

	/// Test de l'ajout d'un noeud a l'arbre
	void testAjouterNoeud();

	/// Tests de la fonction effacer()
	void testEffacerNoeud();

private:
	ArbreRendu arbre_;
	NoeudAbstrait* noeud1_;
	NoeudAbstrait* noeud2_;
	NoeudAbstrait* noeud3_;
};

#endif // _TESTS_ARBRERENDUTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

