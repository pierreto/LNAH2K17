//////////////////////////////////////////////////////////////////////////////
/// @file VisiteurObtenirSelectionTest.h
/// @author Clint Phoeuk
/// @date 2016-10-21
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////

#ifndef _TESTS_VISITEURObtenirSelectionTEST_H
#define _TESTS_VISITEURObtenirSelectionTEST_H

#include "glm\glm.hpp"
#include "ArbreRenduINF2990.h"
#include <cppunit/extensions/HelperMacros.h>
#include <memory>

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurObtenirSelectionTest
/// @brief Classe de test cppunit pour tester le bon fonctionnement des
///        méthodes de la classe VisiteurObtenirSelection
///
/// @author Clint Phoeuk
/// @date 2016-11-05
///////////////////////////////////////////////////////////////////////////
class VisiteurObtenirSelectionTest : public CppUnit::TestFixture
{

	// =================================================================
	// Déclaration de la suite de tests et des méthodes de tests
	//
	// Important, vous devez définir chacun de vos cas de tests à l'aide
	// de la macro CPPUNIT_TEST sinon ce dernier ne sera pas exécuté !
	// =================================================================
	CPPUNIT_TEST_SUITE(VisiteurObtenirSelectionTest);
	CPPUNIT_TEST(testObtenirSelection);
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

	/// Test de l'obtention de noeuds avec le visiteur
	void testObtenirSelection();

private:
	ArbreRendu arbre_;

	// Noeuds
	NoeudAbstrait* noeud1_;
	NoeudAbstrait* noeud2_;
	NoeudAbstrait* noeud3_;
	NoeudAbstrait* noeud4_;
	NoeudAbstrait* noeud5_;
	NoeudAbstrait* noeud6_;
	NoeudAbstrait* noeud7_;
	NoeudAbstrait* noeud8_;

	// Vecteur regroupant les noeuds spécifiques
	std::vector<NoeudAbstrait*> noeuds_;
};

#endif // _TESTS_VisiteurObtenirSelectionTEST_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


