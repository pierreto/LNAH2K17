////////////////////////////////////////////////////////////////////////////////////
/// @file VisiteurDuplicationTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "VisiteurDuplicationTest.h"
#include "NoeudAccelerateur.h"
#include "NoeudPortail.h"
#include "UsineNoeud.h"
#include "VisiteurDuplication.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(VisiteurDuplicationTest);

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
void VisiteurDuplicationTest::setUp()
{
	// Ajout d'une usine 
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ ArbreRenduINF2990::NOM_ACCELERATEUR, std::string{ "noModel" } });

	// Création des noeuds
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

	// Déplacements des noeuds
	glm::dvec3 point1 = glm::dvec3(5, 0, 2);

	// Assigner les positions aux noeuds
	noeud1_->deplacer(point1);

	// Ajuster le scaling
	noeud1_->scale(glm::vec3(10, 10, 10));

	// Ajuster la rotation
	noeud1_->rotate(15, glm::vec3(0, 1, 0));

	// Ajouter les noeuds a l'arbre
	arbre_.ajouter(noeud1_);


}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDuplicationTest::tearDown()
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
void VisiteurDuplicationTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDuplicationTest::testCopyProperties()
///
/// Cas de test: Test la fonction copyProperties dans 
///              VisiteurDuplication qui copie les proprietes d'un noeud a un autre
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDuplicationTest::testCopyProperties()
{
	// Creation du visiteur de scaling
	VisiteurDuplication visiteur;

	// Noeud de copie
	NoeudAbstrait* noeudCopie = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

	// Copy properties
	visiteur.copyProperties(noeud1_, noeudCopie);

	// Tests
	CPPUNIT_ASSERT(noeudCopie->obtenirPositionRelative().x == 5 && noeudCopie->obtenirPositionRelative().z == 2);
	CPPUNIT_ASSERT(noeudCopie->obtenirScale().x == 10 && noeudCopie->obtenirScale().y == 10 && noeudCopie->obtenirScale().z == 10);
	CPPUNIT_ASSERT(noeudCopie->obtenirRotation() == noeud1_->obtenirRotation());
}
