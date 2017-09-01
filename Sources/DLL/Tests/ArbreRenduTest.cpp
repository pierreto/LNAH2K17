////////////////////////////////////////////////////////////////////////////////////
/// @file ArbreRenduTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-05
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "ArbreRenduTest.h"
#include "NoeudTypes.h"
#include "UsineNoeud.h"
#include "ArbreRendu.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(ArbreRenduTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::setUp()
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
void ArbreRenduTest::setUp()
{
	// Ajout des usines 
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ ArbreRenduINF2990::NOM_ACCELERATEUR, std::string{ "noModel" } });
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_MUR, new UsineNoeud<NoeudMur>{ ArbreRenduINF2990::NOM_MUR, std::string{ "noModel" } });
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::tearDown()
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
void ArbreRenduTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testCreerNoeud()
///
/// Cas de test: Test de la creation des noeuds
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ArbreRenduTest::testCreerNoeud() 
{
	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud2_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);

	CPPUNIT_ASSERT(noeud1_->obtenirType() == "accelerateur");
	CPPUNIT_ASSERT(noeud2_->obtenirType() == "mur");
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testAjouterNoeud()
///
/// Cas de test: Test de la fonction ajouter() 
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ArbreRenduTest::testAjouterNoeud()
{
	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud2_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);

	// Ajout des noeuds dans l'arbre
	CPPUNIT_ASSERT(arbre_.ajouter(noeud1_));
	CPPUNIT_ASSERT(arbre_.ajouter(noeud2_));
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void ArbreRenduTest::testEffacerNoeud()
///
/// Cas de test: Test de la fonction effacer()
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void ArbreRenduTest::testEffacerNoeud()
{
	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud2_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);

	// Ajout des noeuds dans l'arbre
	arbre_.ajouter(noeud1_);
	arbre_.ajouter(noeud2_);

	CPPUNIT_ASSERT(arbre_.obtenirNombreEnfants() == 2);

	// Effacement des noeuds avec tests

	arbre_.effacer(noeud1_);
	CPPUNIT_ASSERT(arbre_.obtenirNombreEnfants() == 1);

	// Creer et verifier que le noeud est nulle
	NoeudAbstrait* noeud = nullptr;
	CPPUNIT_ASSERT(noeud == nullptr);

	// Chercher le noeud restant et verifier qu'il reste bien le noeud mur
	noeud = arbre_.chercher(ArbreRenduINF2990::NOM_MUR);
	CPPUNIT_ASSERT(noeud->obtenirType() == "mur");

	// Effacer le dernier noeud
	arbre_.effacer(noeud2_);
	CPPUNIT_ASSERT(arbre_.obtenirNombreEnfants() == 0);
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
