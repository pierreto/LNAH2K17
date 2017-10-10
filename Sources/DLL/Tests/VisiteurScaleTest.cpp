////////////////////////////////////////////////////////////////////////////////////
/// @file VisiteurScaleTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "VisiteurScaleTest.h"
#include "NoeudAccelerateur.h"
#include "UsineNoeud.h"
#include "VisiteurScale.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(VisiteurScaleTest);

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
void VisiteurScaleTest::setUp()
{
	// Ajout d'une usine 
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ ArbreRenduINF2990::NOM_ACCELERATEUR, std::string{ "noModel" } });
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_MUR, new UsineNoeud<NoeudMur>{ ArbreRenduINF2990::NOM_MUR, std::string{ "noModel" } });

	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud2_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);

	// Déplacements des noeuds
	glm::dvec3 point1 = glm::dvec3(0, 0, 0);
	glm::dvec3 point2 = glm::dvec3(1, 0, 0);

	// Assigner les positions aux noeuds
	noeud1_->deplacer(point1);
	noeud2_->deplacer(point2);

	// Selectionner les noeuds
	noeud1_->assignerSelection(true);
	noeud2_->assignerSelection(true);

	// Ajouter les noeuds a l'arbre
	arbre_.ajouter(noeud1_);
	arbre_.ajouter(noeud2_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurScaleTest::tearDown()
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
void VisiteurScaleTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurScaleTest::testScaleAccelerateur()
///
/// Cas de test: Test de la mise a l'echelle d'un accelerateur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurScaleTest::testScaleAccelerateur()
{
	VisiteurScale visiteur(glm::vec3(10, 10, 10));

	// Visiter les noeuds
	visiteur.visiterAccelerateur((NoeudAccelerateur*)noeud1_);

	// Tests
	CPPUNIT_ASSERT(noeud1_->obtenirScale().x == 11 && noeud1_->obtenirScale().y == 11 && noeud1_->obtenirScale().z == 11);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurScaleTest::testScaleMur()
///
/// Cas de test: Test de la mise a l'echelle d'un mur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurScaleTest::testScaleMur()
{
	VisiteurScale visiteur(glm::vec3(10, 10, 10));

	// Visiter les noeuds
	visiteur.visiterMur((NoeudMur*)noeud2_);

	// Tests
	CPPUNIT_ASSERT(noeud2_->obtenirScale().x == 1 && noeud2_->obtenirScale().y == 1 && noeud2_->obtenirScale().z == 11);
}

