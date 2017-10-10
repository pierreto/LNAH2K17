//////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDuplicationTest.h
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_VISITEURDUPLICATIONTEST_H
#define _TESTS_VISITEURDUPLICATIONTEST_H

#include "glm\glm.hpp"
#include "ArbreRenduINF2990.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurDuplicationTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        m�thodes de la classe VisiteurDuplication
///
/// @author Clint Phoeuk
/// @date 2016-11-06
///////////////////////////////////////////////////////////////////////////
class VisiteurDuplicationTest : public CppUnit::TestFixture
{

	// =================================================================
	// D�claration de la suite de tests et des m�thodes de tests
	//
	// Important, vous devez d�finir chacun de vos cas de tests � l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas ex�cut� !
	// =================================================================
	CPPUNIT_TEST_SUITE(VisiteurDuplicationTest);
	CPPUNIT_TEST(testCopyProperties);
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

	/// Test la fonction copyProperties dans VisiteurDuplication qui copie les proprietes d'un noeud a un autre
	void testCopyProperties();

private:
	ArbreRendu arbre_;
	NoeudAbstrait* noeud1_;
};

#endif // _TESTS_VisiteurDuplicationTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


