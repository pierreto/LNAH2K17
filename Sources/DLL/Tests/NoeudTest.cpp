////////////////////////////////////////////////////////////////////////////////////
/// @file NoeudTest.cpp
/// @author Julien Gascon-Samson
/// @date 2011-07-16
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "NoeudTest.h"
#include "NoeudAccelerateur.h"
#include "ArbreRenduINF2990.h"
#include "Utilitaire.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(NoeudTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTest::setUp()
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
void NoeudTest::setUp()
{
	// Creation du noeud
	noeud = std::make_unique<NoeudAccelerateur>(ArbreRenduINF2990::NOM_ACCELERATEUR);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTest::tearDown()
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
void NoeudTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTest::testRotate()
///
/// Cas de test: Test de la rotation
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTest::testRotate()
{
	// Application de la mise a echelle
	noeud->scale(glm::vec3(1, 1, 1));

	// Deplacer le noeud
	noeud->deplacer(glm::vec3(1, 0, 1));

	// Rotation du noeud
	noeud->rotate(90, glm::vec3(0, 1, 0));

	// Tests
	CPPUNIT_ASSERT(noeud->obtenirScale().x == 1 && noeud->obtenirScale().y == 1 && noeud->obtenirScale().z == 1);
	CPPUNIT_ASSERT(noeud->obtenirPositionRelative().x == 1 && noeud->obtenirPositionRelative().y == 0 && noeud->obtenirPositionRelative().z == 1);
	CPPUNIT_ASSERT(noeud->obtenirRotation().x == 0 && noeud->obtenirRotation().y == 90 && noeud->obtenirRotation().z == 0);

}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
