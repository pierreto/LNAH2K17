////////////////////////////////////////////////////////////////////////////////////
/// @file VisiteurInformationTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-06
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "VisiteurInformationTest.h"
#include "NoeudAccelerateur.h"
#include "UsineNoeud.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(VisiteurInformationTest);

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
void VisiteurInformationTest::setUp()
{
	// Ajout d'une usine 
	arbre_.ajouterUsine(ArbreRenduINF2990::NOM_ACCELERATEUR, new UsineNoeud<NoeudAccelerateur>{ ArbreRenduINF2990::NOM_ACCELERATEUR, std::string{ "noModel" } });

	// Création d'un noeud
	noeud1_ = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);

	// Assigner la position au noeud
	noeud1_->assignerPositionRelative(glm::vec3(0, 0, 0));

	// Ajuster le scaling
	noeud1_->scale(glm::vec3(10, 10, 10));

	// Ajuster la rotation
	noeud1_->rotate(15, glm::vec3(0, 1, 0));

	// Selectionner le noeud
	noeud1_->assignerSelection(true);

	// Ajouter le noeud a l'arbre
	arbre_.ajouter(noeud1_);

	// Ajout de l'information du noeud dans le tableau infos pour le test lireInfos
	// Position
	infos[0] = noeud1_->obtenirPositionRelative().x;
	infos[1] = noeud1_->obtenirPositionRelative().y;
	infos[2] = noeud1_->obtenirPositionRelative().z;

	// Rotation en degre
	infos[3] = glm::degrees(noeud1_->obtenirRotation().x);
	infos[4] = glm::degrees(noeud1_->obtenirRotation().y);
	infos[5] = glm::degrees(noeud1_->obtenirRotation().z);

	// Mise a l'echelle
	infos[6] = noeud1_->obtenirScale().x;
	infos[7] = noeud1_->obtenirScale().y;
	infos[8] = noeud1_->obtenirScale().z;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurInformationTest::tearDown()
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
void VisiteurInformationTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurInformationTest::testEcrireInfos()
///
/// Cas de test: Test de l'ecriture de l'information dans le visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurInformationTest::testEcrireInfos()
{
	VisiteurInformation visiteur;

	// Ecriture dans visiteur
	CPPUNIT_ASSERT(visiteur.ecrireInformations(infos));

	// Creation d'un nouveau noeud
	NoeudAbstrait* noeudDouble = arbre_.creerNoeud(ArbreRenduINF2990::NOM_ACCELERATEUR);
	noeudDouble->assignerSelection(true);

	// Passer le noeud dans le visiteur pour que l'information entre dans le noeud
	visiteur.visiterAccelerateur((NoeudAccelerateur*)noeudDouble);

	// Test pour verifier que l'information que l'on a mis dans le visiteur se retrouve presentement dans le noeud ce qui confirmerait
	// que l'ecriture de l'information dans le visiteur marche.
	CPPUNIT_ASSERT(noeudDouble->obtenirPositionRelative().x == 0 && noeudDouble->obtenirPositionRelative().y == 0 && noeudDouble->obtenirPositionRelative().z == 0);
	CPPUNIT_ASSERT(noeudDouble->obtenirRotation().x == 0 && noeudDouble->obtenirRotation().y == 15 && noeudDouble->obtenirRotation().z == 0);
	CPPUNIT_ASSERT(noeudDouble->obtenirScale().x == 10 && noeudDouble->obtenirScale().y == 10 && noeudDouble->obtenirScale().z == 10);

}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurInformationTest::testLireInfos()
///
/// Cas de test: Test de la lecture de l'information dans le visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurInformationTest::testLireInfos()
{
	VisiteurInformation visiteur;

	// Passer le noeud qu'on avait cree dans le set up dans le visiteur
	visiteur.visiterAccelerateur((NoeudAccelerateur*)noeud1_);
	
	// Construction du tableau qui recoit l'information du visiteur
	float receiveInfos[9];

	// Lecture de l'information qui se retrouve dans le visiteur qui met les informations dans receiveInfos
	CPPUNIT_ASSERT(visiteur.lireInformations(receiveInfos));

	// Test qui verifie le tableau receiveInfos correspond correctement au tableau infos precedemment cree dans le setup
	for (int i = 0; i < 9; i++) {
		CPPUNIT_ASSERT(receiveInfos[i] == infos[i]);
	}
}

