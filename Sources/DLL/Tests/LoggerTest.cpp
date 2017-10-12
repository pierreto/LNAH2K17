////////////////////////////////////////////////////////////////////////////////////
/// @file LoggerTest.cpp
/// @author Clint Phoeuk
/// @date 2016-11-08
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////
#include <Windows.h>
#include <cctype>
#include <ctime>
#include <regex>

#include "LoggerTest.h"
#include "NoeudAccelerateur.h"
#include "UsineNoeud.h"
#include "Logger.h"

// Enregistrement de la suite de tests au sein du registre
CPPUNIT_TEST_SUITE_REGISTRATION(LoggerTest);

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
void LoggerTest::setUp()
{
	log_ = Logger::obtenirInstance();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void LoggerTest::tearDown()
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
void LoggerTest::tearDown()
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void LoggerTest::testAfficherTemps()
///
/// Cas de test: Test qui verifie si le temps est bien affiche
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void LoggerTest::testAfficherTemps()
{
	// Obtention du temps
	SYSTEMTIME realTime;
	GetLocalTime(&realTime);

	// Obtention de la sortie de la fonction
	std::string time;
	time = log_->afficherTemps();

	// On utilise une expression reguliere pour trouver une correspondance avec le pattern hh:mm:ss:mmm
	std::regex expression("^(\\d{2}):(\\d{2}):(\\d{2}):(\\d{3})$");

	// Le resultat de la recherche
	std::smatch result;

	// Verification du format
	CPPUNIT_ASSERT(std::regex_match(time, result, expression));

	// On verifie l'heure, les minutes et les secondes
	if (result.size() >= 4) {
		CPPUNIT_ASSERT(std::stoi(result[1].str()) == int(realTime.wHour));
		CPPUNIT_ASSERT(std::stoi(result[2].str()) == int(realTime.wMinute));
		CPPUNIT_ASSERT(std::stoi(result[3].str()) == int(realTime.wSecond));
	}
	
}



