////////////////////////////////////////////////////////////////////////////////////
/// @file VisiteurSuppressionTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "VisiteurSuppressionTest.h"
#include "NoeudAccelerateur.h"
#include "NoeudMur.h"
#include "UsineNoeud.h"
#include "VisiteurSuppression.h"
#include "VisiteurObtenirSelection.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(VisiteurSuppressionTest);

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisisteurSuppressionTest::setUp()
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
void VisiteurSuppressionTest::setUp()
{
	// Ajout d'une usine 
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ ArbreRenduINF2990::NOM_ACCELERATEUR, std::string{ "noModel" } });
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_MUR, new UsineNoeud<NoeudMur>{ ArbreRenduINF2990::NOM_MUR, std::string{ "noModel" } });

	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud2_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud3_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);
	noeud4_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);

	// Selectionner deux noeuds
	noeud1_->assignerSelection(true);
	noeud3_->assignerSelection(true);

	// Ajouter les noeuds a l'arbre
	arbre_.ajouter(noeud1_);
	arbre_.ajouter(noeud2_);
	arbre_.ajouter(noeud3_);
	arbre_.ajouter(noeud4_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSuppressionTest::tearDown()
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
void VisiteurSuppressionTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurSuppressionTest::testNoeudsAEffacer()
///
/// Cas de test: Test la fonction obtenirNbSupression
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurSuppressionTest::testNoeudsAEffacer()
{
	// Creation du visiteur de supression
	VisiteurSuppression visiteurSupression;

	// Compter le nombre de noeuds a effacer
	arbre_.accepterVisiteur(&visiteurSupression);

	// Obtenir le nombre de noeuds a effacer
	int nbNoeuds = visiteurSupression.obtenirNbSupression();

	// Verifier qu'il y a bien deux noeuds a effacer (on selectionne deux noeuds dans le setup)
	CPPUNIT_ASSERT(nbNoeuds == 2);
}
