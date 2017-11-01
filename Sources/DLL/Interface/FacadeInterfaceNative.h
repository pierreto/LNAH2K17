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
#include <string>

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
	__declspec(dllexport) void playerMouseMove(int x, int y);
	__declspec(dllexport) void opponentMouseMove(int x, int y);


	__declspec(dllexport) void modifierKeys(bool alt, bool ctrl);
	__declspec(dllexport) bool selectedNodeInfos(float infos[]);
	__declspec(dllexport) void applyNodeInfos(float infos[]);
	__declspec(dllexport) void enregistrerSous(char* filePath, float coefficients[]);
	__declspec(dllexport) void ouvrir(char* filePath, float coefficients[]);
	__declspec(dllexport) void chargerCarte(const char* filePath, float coefficients[]);
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



	__declspec(dllexport) void getGameElementPositions(float* slavePosition, float* masterPosition, float* puckPosition);
	__declspec(dllexport) void setSlaveGameElementPositions(float* slavePosition, float* masterPosition, float* puckPosition);
	__declspec(dllexport) void setMasterGameElementPositions(float* slavePosition);

	__declspec(dllexport) void rotateCamera(float angle);
	__declspec(dllexport) void getSlavePosition(float* position);

	__declspec(dllexport) void setOnlineClientType(int clientType);


	typedef void(__stdcall * GoalCallback)(int playerNumber);

	__declspec(dllexport) void setOnGoalCallback(GoalCallback goalCallback);

	__declspec(dllexport) void slaveGoal();
	
	__declspec(dllexport) void masterGoal();

	__declspec(dllexport) void createPortal(char* startUuid, float* startPos, char* endUuid, float* endPos);
	typedef void(__stdcall * PortalCreationCallback)(const char* startUuid, const float* startPos, const char* endUuid, const float* endPosd);
	__declspec(dllexport) void setPortalCreationCallback(PortalCreationCallback callback);

	__declspec(dllexport) void createWall(const char* uuid, const float* startPosition, const float* endPosition);
	typedef void(__stdcall * WallCreationCallback)(const char* uuid, const float* startPos, const float* endPos);
	__declspec(dllexport) void setWallCreationCallback(WallCreationCallback callback);

	__declspec(dllexport) void createBoost(const char* uuid, const float* position);
	typedef void(__stdcall * BoostCreationCallback)(const char* uuid, const float* pos);
	__declspec(dllexport) void setBoostCreationCallback(BoostCreationCallback callback);

	typedef void(__stdcall * SelectionEventCallback)(const char* username, const char* uuid);
	__declspec(dllexport) void setSelectionEventCallback(SelectionEventCallback callback);
	__declspec(dllexport) void setElementAsSelected(char* username, char* uuid);


	typedef void(__stdcall * MoveEventCallback)(const char* uuid, const float* newPosition);
	__declspec(dllexport) void setMoveEventCallback(MoveEventCallback callback);
	__declspec(dllexport) void moveByUUID(const char* uuid,const float* position);

	__declspec(dllexport) void addNewUser(char* username, char* hexColor);
	__declspec(dllexport) void removeUser(char* username);


} 

#endif // __FACADE_INTERFACE_NATIVE_H__

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
