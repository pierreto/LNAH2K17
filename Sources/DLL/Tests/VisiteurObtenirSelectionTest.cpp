////////////////////////////////////////////////////////////////////////////////////
/// @file VisiteurObtenirSelectionTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "VisiteurObtenirSelectionTest.h"
#include "NoeudAccelerateur.h"
#include "UsineNoeud.h"
#include "VisiteurObtenirSelection.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(VisiteurObtenirSelectionTest);

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
void VisiteurObtenirSelectionTest::setUp()
{
	// Ajout d'une usine 
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ ArbreRenduINF2990::NOM_ACCELERATEUR, std::string{ "noModel" } });
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_MUR, new UsineNoeud<NoeudMur>{ ArbreRenduINF2990::NOM_MUR, std::string{ "noModel" } });

	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud2_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud3_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeud4_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

	noeud5_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);
	noeud6_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);
	noeud7_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);
	noeud8_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_MUR);

	// Selectionner quelques noeuds
	noeud1_->assignerSelection(true);
	noeud2_->assignerSelection(true);
	noeud5_->assignerSelection(true);

	// Ajouter les noeuds a l'arbre
	arbre_.ajouter(noeud1_);
	arbre_.ajouter(noeud2_);
	arbre_.ajouter(noeud3_);
	arbre_.ajouter(noeud4_);
	arbre_.ajouter(noeud5_);
	arbre_.ajouter(noeud6_);
	arbre_.ajouter(noeud7_);
	arbre_.ajouter(noeud8_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurObtenirSelectionTest::tearDown()
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
void VisiteurObtenirSelectionTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurObtenirSelectionTest::testObtenirSelectionAccelerateur()
///
/// Cas de test: Test de l'obtention des noeuds selectionnes avec le visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurObtenirSelectionTest::testObtenirSelection()
{
	VisiteurObtenirSelection visiteur;

	// Visiter les noeuds
	arbre_.accepterVisiteur(&visiteur);

	// Obtention des noeuds
	noeuds_ = visiteur.obtenirNoeuds();

	// Verification du nombre de noeuds
	CPPUNIT_ASSERT(noeuds_.size() == 3);

	// Compteurs pour compter le nombre d'accelerateurs et de murs selectionnes
	int countAccelerateur = 0;
	int countMur = 0;

	// Compter
	for (int i = 0; i < noeuds_.size(); i++)
	{
		if (noeuds_[i]->obtenirType() == "accelerateur")
			countAccelerateur++;
		if (noeuds_[i]->obtenirType() == "mur")
			countMur++;
	}

	CPPUNIT_ASSERT(countAccelerateur == 2);
	CPPUNIT_ASSERT(countMur == 1);
	
}



