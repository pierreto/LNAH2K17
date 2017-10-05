////////////////////////////////////////////////
/// @file   FacadeInterfaceNative.h
/// @author INF2990
/// @date   2014-08-16
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////
#ifndef __FACADE_INTERFACE_NATIVE_H__
#define __FACADE_INTERFACE_NATIVE_H__

extern "C" {

	__declspec(dllexport) void initialiserOpenGL(int * handle);
	__declspec(dllexport) void libererOpenGL();
	__declspec(dllexport) void dessinerOpenGL();
	__declspec(dllexport) void redimensionnerFenetre(int largeur, int hauteur);
	__declspec(dllexport) void animer(double temps);
	__declspec(dllexport) void zoomIn();
	__declspec(dllexport) void zoomOut();
	__declspec(dllexport) int  obtenirAffichagesParSeconde();
	__declspec(dllexport) bool executerTests();

	__declspec(dllexport) void fleches(double x, double y);
	__declspec(dllexport) void escape();
	__declspec(dllexport) void space();
	__declspec(dllexport) void deleteSelection();
	__declspec(dllexport) bool verifierSelection();
	__declspec(dllexport) void changerModeleEtat(int etat);
	__declspec(dllexport) void mouseDownL();
	__declspec(dllexport) void mouseDownR();
	__declspec(dllexport) void mouseUpL();
	__declspec(dllexport) void mouseUpR();
	__declspec(dllexport) void mouseMove(int x, int y);
	__declspec(dllexport) void mouseMoveOpponent(int x, int y);


	__declspec(dllexport) void modifierKeys(bool alt, bool ctrl);
	__declspec(dllexport) bool selectedNodeInfos(float infos[]);
	__declspec(dllexport) void applyNodeInfos(float infos[]);
	__declspec(dllexport) void enregistrerSous(char* filePath, float coefficients[]);
	__declspec(dllexport) void ouvrir(char* filePath, float coefficients[]);
	__declspec(dllexport) bool mouseOverTable();
	__declspec(dllexport) bool mouseOverControlPoint();
	__declspec(dllexport) void resetNodeTree();
	__declspec(dllexport) void changeGridVisibility(bool visibility);
	__declspec(dllexport) void resetCameraPosition();
	__declspec(dllexport) void gererRondelleMaillets(bool toggle);
	__declspec(dllexport) void toggleControlPointsVisibility(bool visible);
	__declspec(dllexport) void moveMaillet();
	__declspec(dllexport) void setSpeedXMaillet(float speedX);
	__declspec(dllexport) void setSpeedYMaillet(float speedX);
	__declspec(dllexport) void setCoefficients(float friction, float acceleration, float rebondissement);
	__declspec(dllexport) int isGameOver(int neededGoals);
	__declspec(dllexport) void getGameScore(int score[]);
	__declspec(dllexport) void resetGame();
	__declspec(dllexport) void getDebugStatus(bool enableCollision, bool enableSpeed, bool enableLight, bool enablePortal);
	__declspec(dllexport) void toggleLights(int lumType);
	__declspec(dllexport) void setLights(int lumType, bool ouverte);
	__declspec(dllexport) void aiStatus(bool enabled);
	__declspec(dllexport) void aiActiveProfile(int speed, int passivity);
	__declspec(dllexport) void setPlayerNames(char* player1, char* player2);
	__declspec(dllexport) void setPlayerColors(float player1[], float player2[]);
	__declspec(dllexport) void toggleTestMode(bool isActive);
	__declspec(dllexport) bool isGameStarted();
	__declspec(dllexport) void loadSounds();
	__declspec(dllexport) void playMusic(bool quickPlay);
	__declspec(dllexport) void toggleOrbit(bool orbit);
}

#endif // __FACADE_INTERFACE_NATIVE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
