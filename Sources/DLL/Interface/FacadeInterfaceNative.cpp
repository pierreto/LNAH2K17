////////////////////////////////////////////////
/// @file   FacadeInterfaceNative.cpp
/// @author INF2990
/// @date   2014-08-16
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////
#include "FacadeInterfaceNative.h"
#include "FacadeModele.h"

#include "glm\glm.hpp"
#include "FacadeModele.h"
#include "PhysProperties.h"
#include "States/ModeleEtat.h"
#include "States/ModeleEtatJeu.h"
#include "AideGL.h"
#include "Vue.h"
#include "ArbreRenduINF2990.h"
#include "CompteurAffichage.h"
#include "Logger.h"
#include "LightManager.h"
#include "Audio.h"

#include "BancTests.h"

#include <iostream>
#include <iomanip>
#include "glm/gtc/type_ptr.hpp"
#include "../ModeleEtatJeuOnline.h"
#include "../NodeCreator.h"
#include "ModeleEtatCreerPortail.h"
#include "ModeleEtatCreerMuret.h"
#include "ModeleEtatCreerBoost.h"
#include "ModeleEtatDeplacement.h"
#include "ModeleEtatSelection.h"
#include "ModeleEtatPointControl.h"

extern "C"
{
	////////////////////////////////////////////////////////////////////////
	///
	/// __declspec(dllexport) void __cdecl initialiserOpenGL(int* handle)
	///
	/// Cette fonction initialise un contexte OpenGL dans la fen�tre
	/// identifi�e par le handle pass� en param�tre.  Cette fonction doit
	/// �tre la premi�re � �tre appel�e, car la cr�ation de l'objet du mod�le
	/// C++ s'attend � avoir un contexte OpenGL valide.
	///
	/// @param[in] handle : Le handle.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl initialiserOpenGL(int* handle)
	{
		if (handle == nullptr)
			return;

		FacadeModele::obtenirInstance()->initialiserOpenGL((HWND)handle);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl libererOpenGL()
	///
	/// Cette fonction lib�re le contexte OpenGL. Cette fonction doit �tre la
	/// derni�re � �tre appel�e, car elle lib�re �galement l'objet du mod�le
	/// C++.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl libererOpenGL()
	{
		FacadeModele::obtenirInstance()->libererOpenGL();

		// D�sinitialisation de la fa�ade.  Le fait de le faire apr�s la
		// d�sinitialisation du contexte OpenGL aura pour cons�quence que la
		// lib�ration des listes d'affichages, par exemple, sera faite une fois que
		// le contexte n'existera plus, et sera donc sans effet.
		FacadeModele::libererInstance();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl dessinerOpenGL()
	///
	/// Cette fonction affiche la sc�ne.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl dessinerOpenGL()
	{
		// Affiche la sc�ne.
		FacadeModele::obtenirInstance()->afficher();

		// Temporaire: pour d�tecter les erreurs OpenGL
		aidegl::verifierErreurOpenGL();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// __declspec(dllexport) void __cdecl redimensionnerFenetre(int largeur, int hauteur)
	///
	/// Cette fonction doit �tre appel�e lorsque la fen�tre est
	/// redimensionn�e afin d'ajuster les param�tres de la machine � �tats
	/// d'OpenGL pour correspondre aux nouvelles dimensions de la fen�tre.
	///
	/// @param[in] largeur : La nouvelle largeur de la fen�tre.
	/// @param[in] hauteur : La nouvelle hauteur de la fen�tre.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl redimensionnerFenetre(int largeur, int hauteur)
	{
		FacadeModele::obtenirInstance()->obtenirVue()->redimensionnerFenetre(largeur, hauteur);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl animer(double temps)
	///
	/// Cette fonction effectue les diff�rents calculs d'animations
	/// n�cessaires pour le mode jeu, tel que les diff�rents calculs de
	/// physique du jeu.
	///
	/// @param[in] temps : Intervalle de temps sur lequel effectuer le calcul.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl animer(double temps)
	{
		FacadeModele::obtenirInstance()->animer((float)temps);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl zoomIn()
	///
	/// Cette fonction applique un zoom avant sur le pr�sent volume de vision.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl zoomIn()
	{
		FacadeModele::obtenirInstance()->obtenirVue()->zoomerIn();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl zoomOut()
	///
	/// Cette fonction applique un zoom arri�re sur le pr�sent volume de vision.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl zoomOut()
	{
		FacadeModele::obtenirInstance()->obtenirVue()->zoomerOut();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) int __cdecl obtenirAffichagesParSeconde()
	///
	/// Cette fonction permet d'obtenir le nombre d'affichages par seconde.
	///
	/// @return Le nombre d'affichage par seconde.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) int __cdecl obtenirAffichagesParSeconde()
	{
		return utilitaire::CompteurAffichage::obtenirInstance()->obtenirAffichagesSeconde();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) bool __cdecl executerTests()
	///
	/// Cette fonction permet d'ex�cuter l'ensemble des tests unitaires
	///
	/// @return 0 si tous les tests ont r�ussi, 1 si au moins un test a �chou�
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) bool __cdecl executerTests()
	{
		bool reussite = BancTests::obtenirInstance()->executer();
		return reussite ? 0 : 1;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl fleches(double x, double y)
	///
	/// Cette fonction permet de g�rer les touches fl�ch�s du clavier
	///
	/// @param[in] x : D�placement en x
	/// @param[in] y : D�placement en y
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl fleches(double x, double y)
	{
		FacadeModele::obtenirInstance()->fleches(x, y);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl escape()
	///
	/// Cette fonction enclenche l'action pour la touche escape
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl escape()
	{
		FacadeModele::obtenirInstance()->escape();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl space()
	///
	/// Cette fonction enclenche l'action pour la touche d'espacement
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl space()
	{
		FacadeModele::obtenirInstance()->space();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void __cdecl changerModeleEtat(MODELE_ETAT etat)
	///
	/// Cette fonction change l'�tat du mode �ditiom
	///
	/// @param[in] etat : Type de l'�tat
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void __cdecl changerModeleEtat(int etat)
	{
		FacadeModele::obtenirInstance()->changerModeleEtat((MODELE_ETAT)etat);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void deleteSelection()
	///
	/// Cette fonction permet de supprimer les noeuds s�lectionn�s
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void deleteSelection()
	{
		FacadeModele::obtenirInstance()->deleteSelection();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void verifierSelection()
	///
	/// Cette fonction permet de v�rifier si un object est s�lectionn�
	///
	/// @return vrai un au moins un objet est s�lectionn�
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) bool verifierSelection()
	{
		return FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990()->selectionExiste();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void mouseDownL()
	///
	/// Cette fonction active l'�v�nement de mouseDownL
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void mouseDownL()
	{
		FacadeModele::obtenirInstance()->mouseDownL();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void mouseDownR()
	///
	/// Cette fonction active l'�v�nement de mouseDownR
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void mouseDownR()
	{
		FacadeModele::obtenirInstance()->mouseDownR();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void mouseUpL()
	///
	/// Cette fonction desactive l'�v�nement de mouseDownL
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void mouseUpL()
	{
		FacadeModele::obtenirInstance()->mouseUpL();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void mouseUpR()
	///
	/// Cette fonction active l'�v�nement de mouseDownR
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void mouseUpR()
	{
		FacadeModele::obtenirInstance()->mouseUpR();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void playerMouseMove(int x, int y)
	///
	/// Cette fonction g�re les �v�nements de mouvement de souris
	///
	/// @param[in] x : Position de la souris en x
	/// @param[in] y : Position de la souris en y
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void playerMouseMove(int x, int y)
	{
		FacadeModele::obtenirInstance()->playerMouseMove(x, y);
	}

	///////////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void opponentMouseMove(int x, int y)
	///
	/// Cette fonction g�re les �v�nements de mouvement de souris
	///
	/// @param[in] x : Position de la souris en x
	/// @param[in] y : Position de la souris en y
	///
	/// @return Aucune
	///
	///////////////////////////////////////////////////////////////////////////////
	void opponentMouseMove(int x, int y)
	{
		//TODO: IMPLEMENT
		ModeleEtatJeu::obtenirInstance()->opponentMouseMove(x, y);
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void modifierKeys(bool alt, bool ctrl)
	///
	/// Cette fonction active l'�v�nement de ALT et CTRL
	///
	/// @param[in] alt : Bool sur l'�tat de la touche ALT
	/// @param[in] ctrl : Bool sur l'�tat de la touche CTRL
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void modifierKeys(bool alt, bool ctrl)
	{
		FacadeModele::obtenirInstance()->modifierKeys(alt, ctrl);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) bool selectedNodeInfos(float infos[])
	///
	/// Cette fonction r�cup�re l'information des propri�t�s d'un noeud
	///
	/// @param[in,out] infos : Tableau initialement vide qui est retourn� 
	///						   avec les informations de position, rotation, 
	///						   scaling (x,y,z)
	///
	/// @return bool sur la r�ussite de l'op�ration (1 seul noeud trouv�)
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) bool selectedNodeInfos(float infos[])
	{
		return FacadeModele::obtenirInstance()->selectedNodeInfos(infos);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void applyNodeInfos(float infos[])
	///
	/// Cette fonction applique l'information des propri�t�s d'un noeud.
	///
	/// @param[in] infos : Tableau de position, rotation, scaling (x,y,z)
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void applyNodeInfos(float infos[])
	{
		FacadeModele::obtenirInstance()->applyNodeInfos(infos);
	}

	__declspec(dllexport) void getMapJson(float coefficients[], char* map)
	{
		FacadeModele::obtenirInstance()->getMapJson(coefficients, map);
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void enregistrerSous(char* filePath, float coefficients[])
	///
	/// Cette fonction permet l'enregistrement de l'arbre en cours dans 
	/// un fichier choisi
	///
	/// @param[in] filePath : Nom du filepath choisi
	/// @param[in] coefficients[] : Coefficients de la carte
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void enregistrerSous(char* filePath, float coefficients[])
	{
		FacadeModele::obtenirInstance()->enregistrerSous(filePath, coefficients);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void ouvrir(char* filePath, float coefficients[])
	///
	/// Cette fonction permet l'ouverture d'un fichier de sauvegarde pour 
	/// charger un arbre enregistr�
	///
	/// @param[in] filePath : Nom du filepath du fichier d'ouverture
	/// @param[in] coefficients[] : Coefficients de la carte
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void ouvrir(char* filePath, float coefficients[])
	{
		FacadeModele::obtenirInstance()->ouvrir(filePath, coefficients);
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void chargerCarte(const char* json, float coefficients[])
	///
	/// Cette fonction permet l'ouverture d'un fichier de sauvegarde pour 
	/// charger un arbre enregistr�
	///
	/// @param[in] json : JSON repr�sentant l'arbre enregistr� (= la carte)
	/// @param[in] coefficients[] : Coefficients de la carte
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void chargerCarte(const char* json, float coefficients[])
	{
		FacadeModele::obtenirInstance()->chargerCarte(json, coefficients);
	}

	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) bool mouseOverTable()
	///
	/// Cette fonction v�rifie si le curseur est au dessus de la table
	///
	/// @return Bool�en sur le r�sultat
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) bool mouseOverTable()
	{
		return FacadeModele::obtenirInstance()->mouseOverTable();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) bool mouseOverControlPoint()
	///
	/// Cette fonction v�rifie si le curseur est au dessus d'un 
	/// point de contr�le
	///
	/// @return Bool�en sur le r�sultat
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) bool mouseOverControlPoint()
	{
		return FacadeModele::obtenirInstance()->mouseOverControlPoint();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void resetNodeTree()
	///
	/// Cette fonction permet de remettre la table � son �tat initial
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void resetNodeTree()
	{
		FacadeModele::obtenirInstance()->reinitialiser();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void changeGridVisibility(bool visibility)
	///
	/// Cette fonction change la visibilit� de la grid derri�re la table.
	///
	/// @param[in] visibility : Visibilit� de la grid
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void changeGridVisibility(bool visibility)
	{
		FacadeModele::obtenirInstance()->changeGridVisibility(visibility);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void resetCameraPosition()
	///
	/// Cette fonction reinitialise la position de la cam�ra.
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void resetCameraPosition()
	{
		FacadeModele::obtenirInstance()->resetCameraPosition();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void gererRondelleMaillets(bool toggle)
	///
	/// Cette fonction gere les creations et suppressions de la rondelle et les maillets.
	///
	/// @param[in] toggle : Interrupteur pour creer ou detruire
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void gererRondelleMaillets(bool toggle)
	{
		FacadeModele::obtenirInstance()->gererRondelleMaillets(toggle);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void toggleControlPointsVisibility(bool visible)
	///
	/// Cette fonction change l'�tat d'affichage des points de contr�les;
	///
	/// @param[in] visible : Visibilit� des points (vrai si visible)
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void toggleControlPointsVisibility(bool visible)
	{
		FacadeModele::obtenirInstance()->toggleControlPointsVisibility(visible);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void moveMaillet() 
	///
	/// Cette fonction d�place le maillet du joueur 2
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void moveMaillet()
	{
		ModeleEtatJeu::obtenirInstance()->moveMaillet();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void setSpeedXMaillet(float speedX)
	///
	/// Cette fonction set la vitesse en X du maillet
	///
	/// @param[in] speedMailletX : Vitesse en X
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setSpeedXMaillet(float speedX)
	{
		ModeleEtatJeu::obtenirInstance()->setSpeedXMaillet(speedX);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void setSpeedYMaillet(float speedY)
	///
	/// Cette fonction set la vitesse en Y du maillet
	///
	/// @param[in] speedMailletX : Vitesse en Y
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setSpeedYMaillet(float speedY)
	{
		ModeleEtatJeu::obtenirInstance()->setSpeedYMaillet(speedY);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void setCoefficients(float friction, float acceleration, float rebondissement)
	///
	/// Cette fonction set les coefficients de la carte
	///
	/// @param[in] friction : Coefficient de friction
	/// @param[in] acceleration : Coefficient d'acc�l�ration
	/// @param[in] rebondissement : Coefficient de rebondissement
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setCoefficients(float friction, float acceleration, float rebondissement)
	{
		PhysProperties::obtenirInstance()->assignerFriction(friction);
		PhysProperties::obtenirInstance()->assignerAcceleration(acceleration);
		PhysProperties::obtenirInstance()->assignerRebondissement(rebondissement);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) bool isGameOver(int neededGoals)
	///
	/// Cette v�rifie si la partie en cours est termin� selon le nombre
	/// de buts requis pour une partie.
	///
	/// @param[in] neededGoals : Nombre de but requis pour la partie
	///
	/// @return Vrai si la partie est termin�e
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) int isGameOver(int neededGoals)
	{
		return ModeleEtatJeu::obtenirInstance()->isGameOver(neededGoals) ? 1 : 0;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void getGameScore(int score[])
	///
	/// R�cup�re le score de la partie en cours.
	///
	/// @param[in,out] score : Score de la partie en cours
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void getGameScore(int score[])
	{
		ModeleEtatJeu::obtenirInstance()->getGameScore(score);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void resetGame()
	///
	/// R�initilise la partie (buts, positions).
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void resetGame()
	{
		ModeleEtatJeu::obtenirInstance()->resetGame();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void getDebugStatus(bool enableCollision, bool enableSpeed, bool enableLight, bool enablePortal)
	///
	/// Cette fonction connecte les checkbox d'affichage de debogages avec les fonctions d'affichage.
	///
	/// @param[in] enableCollision : Active ou desactive l'affichage des collisions.
	/// @param[in] enableSpeed : Active ou desactive l'affichage de la vitesse.
	/// @param[in] enableLight : Active ou desactive l'affichage de l'eclairage.
	/// @param[in] enablePortal : Active ou desactive l'affichage des portails.
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void getDebugStatus(bool enableCollision, bool enableSpeed, bool enableLight, bool enablePortal)
	{
		Logger::obtenirInstance()->setLogger(enableCollision, enableSpeed, enableLight, enablePortal);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void toggleLights(int lumType)
	///
	/// Cette fonction connecte s'occupe de l'affichage d'activation ou de 
	/// la desactivation de l'eclairage.
	///
	/// @param[in] lumType : Type de lumiere.
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void toggleLights(int lumType)
	{
		light::LightManager::obtenirInstance()->toggleLight(light::LightType(lumType));
		Logger::obtenirInstance()->afficherLumiere(lumType);

		if (lumType == 0)
			light::LightManager::obtenirInstance()->toggleAmbiant();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void setLights(int lumType, bool ouverte)
	///
	/// Cette fonction connecte s'occupe de l'affichage d'activation ou de 
	/// la desactivation de l'eclairage.
	///
	/// @param[in] lumType : Type de lumiere.
	/// @param[in] ouverte : Etat de la lumiere.
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setLights(int lumType, bool ouverte)
	{
		light::LightManager::obtenirInstance()->setLight(light::LightType(lumType), ouverte);
		Logger::obtenirInstance()->afficherLumiere(lumType, ouverte);

		if (lumType == 0)
			light::LightManager::obtenirInstance()->setAmbiant(ouverte);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void aiStatus(bool enabled)
	///
	/// Cette fonction d�termine si le maillet 2 est control� par un joueur r�el ou virtuel.
	///
	/// @param[in] enabled : Active ou desactive l'intelligence artificielle du 2e maillet.
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setCurrentOpponentType(int opponentType)
	{
		ModeleEtatJeu::obtenirInstance()->setCurrentOpponentType(static_cast<ModeleEtatJeu::OpponentType>(opponentType));
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void aiActiveProfile(int speed, int passivity)
	///
	/// Cette fonction associe les param�tres d'un PlayerProfile � l'intelligence artificielle.
	///
	/// @param[in] speed : Index de la vitesse du maillet (en mode AI)
	/// @param[in] passivity : Index de la passivit� du maillet (en mode AI)
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void aiActiveProfile(int speed, int passivity)
	{
		ModeleEtatJeu::obtenirInstance()->aiActiveProfile(speed, passivity);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void setPlayerNames(char* player1, char* player2)
	///
	/// Change les noms des joueurs, afin qu'ils puissent �tre affich�es 
	/// par la suite.
	///
	/// @param[in] player1 : Nom du joueur 1
	/// @param[in] player2 : Nom du joueur 2
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setPlayerNames(char* player1, char* player2)
	{
		ModeleEtatJeu::obtenirInstance()->setPlayerNames(player1, player2);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void setPlayerColors(float player1[], float player2[])
	///
	/// Change la couleur des joueurs, afin qu'elles puissent �tre affich�es 
	/// par la suite.
	///
	/// @param[in] player1 : Couleur du joueur 1
	/// @param[in] player2 : couleur du joueur 2
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void setPlayerColors(float player1[], float player2[])
	{
		ModeleEtatJeu::obtenirInstance()->setPlayerColors(glm::vec4(player1[0], player1[1], player1[2], player1[3]),
		                                                  glm::vec4(player2[0], player2[1], player2[2], player2[3]));
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void toggleTestMode(bool isActive)
	///
	/// Change entre quickplay et mode test pour le modeleEtatJeu.
	///
	/// @param[in] isActive : Etat du mode test
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void toggleTestMode(bool isActive)
	{
		ModeleEtatJeu::obtenirInstance()->toggleTestMode(isActive);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) bool isGameStarted()
	///
	/// V�rifie si la partie est commenc�e.
	///
	/// @return Vrai si la partie est commenc�e.
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) bool isGameStarted()
	{
		return ModeleEtatJeu::obtenirInstance()->isGameStarted();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void loadSounds()
	///
	/// Charge tous les sons du jeu.
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void loadSounds()
	{
		return Audio::obtenirInstance()->loadSounds();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void playMusic(bool quickPlay)
	///
	/// Active ou deactive la musique de fond
	///
	/// @param[in] isActive : Etat du mode Partie Rapide
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void playMusic(bool quickPlay)
	{
		return Audio::obtenirInstance()->playMusic(quickPlay);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn __declspec(dllexport) void toggleOrbit(bool orbit)
	///
	/// Active ou desactive le mode orbite de la cam�ra
	///
	/// @param[in] orbit : Etat du mode orbite
	///
	/// @return Aucune
	///
	////////////////////////////////////////////////////////////////////////
	__declspec(dllexport) void toggleOrbit(bool orbit)
	{
		return FacadeModele::obtenirInstance()->toggleOrbit(orbit);
	}
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
__declspec(dllexport) void getGameElementPositions(float* slavePosition, float* masterPosition, float* puckPosition)
{
	glm::vec3 slaveVec = ModeleEtatJeu::obtenirInstance()->getOpponentPosition();
	glm::vec3 masterVec = ModeleEtatJeu::obtenirInstance()->getPlayerPosition();
	glm::vec3 puckVec = ModeleEtatJeu::obtenirInstance()->getPuckPosition();


	for (int i = 0; i < 3; i++)
	{
		slavePosition[i] = slaveVec[i];
		masterPosition[i] = masterVec[i];
		puckPosition[i] = puckVec[i];
	}
}

__declspec(dllexport) void setSlaveGameElementPositions(float* slavePosition, float* masterPosition,
                                                        float* puckPosition)
{
	// ModeleEtatJeu::obtenirInstance()->setOpponentPosition(slavePosition);
	ModeleEtatJeu::obtenirInstance()->setPlayerPosition(masterPosition);
	ModeleEtatJeu::obtenirInstance()->setPuckPosition(puckPosition);
}

__declspec(dllexport) void setMasterGameElementPositions(float* slavePosition)
{
	ModeleEtatJeu::obtenirInstance()->setOpponentPosition(slavePosition);
}

__declspec(dllexport) void rotateCamera(float angle)
{
	return FacadeModele::obtenirInstance()->rotateCamera(angle);
}


void getSlavePosition(float* position)
{
	glm::vec3 pos = ModeleEtatJeu::obtenirInstance()->getOpponentPosition();
	for (int i = 0; i < 3; i++)
	{
		position[i] = pos[i];
	}
}

void setOnlineClientType(int clientType)
{
	ModeleEtatJeu::obtenirInstance()->setCurrentOnlineClientType(static_cast<ModeleEtatJeu::OnlineClientType>(clientType));
}

void setOnGoalCallback(GoalCallback goalCallback)
{
	ModeleEtatJeu::obtenirInstance()->setOnGoalCallback(goalCallback);
}

void slaveGoal()
{
	ModeleEtatJeu::obtenirInstance()->player2Goal();
}

void masterGoal()
{
	ModeleEtatJeu::obtenirInstance()->player1Goal();
}


__declspec(dllexport) void createPortal(char* startUuid, float* startPos, char* endUuid, float* endPos)
{
	glm::vec3 startPosVec = glm::make_vec3(startPos);
	glm::vec3 endPosVec = glm::make_vec3(endPos);

	return NodeCreator::obtenirInstance()->createPortal(startUuid, startPosVec, endUuid, endPosVec);
}

void setPortalCreationCallback(PortalCreationCallback callback)
{
	ModeleEtatCreerPortail::obtenirInstance()->setPortalCreationCallback(callback);
}

void createWall(const char* uuid, const float* startPosition, const float* endPosition)
{
	glm::vec3 startPosVec = glm::make_vec3(startPosition);
	glm::vec3 endPosVec = glm::make_vec3(endPosition);
	return NodeCreator::obtenirInstance()->createWall(uuid, startPosVec, endPosVec);
}

void setWallCreationCallback(WallCreationCallback callback)
{
	ModeleEtatCreerMuret::obtenirInstance()->setWallCreationCallback(callback);
}

void createBoost(const char* uuid, const float* position)
{
	glm::vec3 posVec = glm::make_vec3(position);
	return NodeCreator::obtenirInstance()->createBoost(uuid, posVec);
}

void setBoostCreationCallback(BoostCreationCallback callback)
{
	ModeleEtatCreerBoost::obtenirInstance()->setBoostCreationCallback(callback);
}


void setSelectionEventCallback(SelectionEventCallback callback)
{
	ModeleEtatSelection::obtenirInstance()->setSelectionEventCallback(callback);
}

void setElementSelection(const char* username, const char* uuid, const bool isSelected, const bool deselectAll)
{
	if (FacadeModele::obtenirInstance()->getUserManager().userExist(std::string(username)))
	{
		if (deselectAll)
		{
			FacadeModele::obtenirInstance()->getUserManager().getUser(std::string(username))->deselectAll();
		}
		else
		{
			if (isSelected)
			{
				FacadeModele::obtenirInstance()->getUserManager().getUser(std::string(username))->select(std::string(uuid, 36));
			}
			else
			{
				FacadeModele::obtenirInstance()->getUserManager().getUser(std::string(username))->deselect(std::string(uuid, 36));
			}
		}
	}
}


void setTransformByUUID(const char* username, const char* uuid, const float* transformMatrix)
{
	NoeudAbstrait* node = FacadeModele::obtenirInstance()->getUserManager().getUser(std::string(username))->findNode(
		std::string(uuid));
	if (node)
	{
		node->setMatriceTransformation(glm::make_mat4(transformMatrix));
	}
	else //if it isnt in the selected list of the other player anymore, we find it in the entire tree 
	{
		NoeudAbstrait* nodeInTree = FacadeModele::obtenirInstance()->findNodeInTree(uuid);
		if (nodeInTree)
		{
			nodeInTree->setMatriceTransformation(glm::make_mat4(transformMatrix));
		}
	}
}

void setTransformEventCallback(TransformEventCallback callback)
{
	ModeleEtatJeu::obtenirInstance()->setTransformEventCallback(callback);
}


void addNewUser(char* username, char* hexColor)
{
	FacadeModele::obtenirInstance()->getUserManager().addNewUser(username, hexColor);
}

void removeUser(char* username)
{
	FacadeModele::obtenirInstance()->getUserManager().removeUser(username);
}

void clearUsers()
{
	FacadeModele::obtenirInstance()->getUserManager().clearUsers();
}

void setCurrentPlayerSelectionColor(char* userHexColor)
{
	glm::vec4 color = OnlineUser::hexadecimalToRGB(userHexColor);
	NoeudAbstrait::selectionColor_ = color;
}

void setCurrentPlayerSelectionColorToDefault()
{
	NoeudAbstrait::selectionColor_ = glm::vec4(0, 0, 1, 0.25f);
}


void setControlPointEventCallback(ControlPointEventCallback callback)
{
	ModeleEtatPointControl::obtenirInstance()->setControlPointEventCallback(callback);
}

void setControlPointPosition(const char* username, const char* uuid, const float* position)
{
	NoeudAbstrait* node = FacadeModele::obtenirInstance()->getUserManager().getUser(std::string(username))->findNode(
		std::string(uuid));
	if (node)
	{
		if (dynamic_cast<NoeudPointControl*>(node))
		{
			node->assignerPositionRelative(glm::make_vec3(position));
		}
	}
	else //if it isnt in the selected list of the other player anymore, we find it in the entire tree 
	{
		NoeudAbstrait* nodeInTree = FacadeModele::obtenirInstance()->findNodeInTree(uuid);
		if (nodeInTree)
		{
			if (dynamic_cast<NoeudPointControl*>(node))
			{
				node->assignerPositionRelative(glm::make_vec3(position));
			}
		}
	}
}
