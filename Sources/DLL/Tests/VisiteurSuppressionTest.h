//////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSuppressionTest.h
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_VisiteurSuppressionTEST_H
#define _TESTS_VisiteurSuppressionTEST_H

#include "glm\glm.hpp"
#include "ArbreRenduINF2990.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurSuppressionTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        m�thodes de la classe VisiteurSupression
///
/// @author Clint Phoeuk
/// @date 2016-11-06
///////////////////////////////////////////////////////////////////////////
class VisiteurSuppressionTest : public CppUnit::TestFixture
{

	// =================================================================
	// D�claration de la suite de tests et des m�thodes de tests
	//
	// Important, vous devez d�finir chacun de vos cas de tests � l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas ex�cut� !
	// =================================================================
	CPPUNIT_TEST_SUITE(VisiteurSuppressionTest);
	CPPUNIT_TEST(testNoeudsAEffacer);
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

	/// Test la fonction obtenirNbSupression
	void testNoeudsAEffacer();

private:
	ArbreRendu arbre_;

	// Noeuds a effacer
	NoeudAbstrait* noeud1_;
	NoeudAbstrait* noeud2_;
	NoeudAbstrait* noeud3_;
	NoeudAbstrait* noeud4_;
};

#endif // _TESTS_VisiteurSupressionTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


