using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique
{

    ///////////////////////////////////////////////////////////////////////////
    /// @class FonctionsNatives
    /// @brief Classe contenant les fonctions communiquant avec le code c++
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    static partial class FonctionsNatives
    {

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialiserOpenGL(IntPtr handle);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libererOpenGL();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void dessinerOpenGL();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void animer(double temps);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void fleches(double x, double y);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void redimensionnerFenetre(int largeur, int hauteur);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zoomIn();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void zoomOut();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void escape();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void space();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deleteSelection();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool verifierSelection();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void changerModeleEtat(int etat);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mouseDownL();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mouseDownR();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mouseUpL();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mouseUpR();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void playerMouseMove(int x, int y);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opponentMouseMove(int x, int y);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void modifierKeys(bool alt, bool ctrl);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool selectedNodeInfos(float[] infos);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void applyNodeInfos(float[] infos);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getMapJson(float[] coefficients, StringBuilder map);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getMapIcon(byte[] icon);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void enregistrerSous(StringBuilder filePath, float[] coefficients);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ouvrir(StringBuilder filePath, float[] coefficients);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void chargerCarte(StringBuilder json, float[] coefficients);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool mouseOverTable();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool mouseOverControlPoint();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void resetNodeTree();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void changeGridVisibility(bool visible);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void resetCameraPosition();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gererRondelleMaillets(bool toggle);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void toggleControlPointsVisibility(bool visible);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void moveMaillet();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setSpeedXMaillet(float speedX);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setSpeedYMaillet(float speedY);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCoefficients(float friction, float acceleration, float rebondissement);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int isGameOver(int neededGoals);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getGameScore(int[] score);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void resetGame();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getDebugStatus(bool enableCollision, bool enableSpeed, bool enableLight,
            bool enablePortal);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void toggleLights(int lumType);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLights(int lumType, bool lumEtat);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCurrentOpponentType(int opponentType);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void aiActiveProfile(int speed, int passivity);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setPlayerNames(StringBuilder player1, StringBuilder player2);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setPlayerColors(float[] player1, float[] player2);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void toggleTestMode(bool isActive);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isGameStarted();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void loadSounds();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void playMusic(bool quickPlay);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void toggleOrbit(bool orbit);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getGameElementPositions([Out] float[] slavePosition, [Out] float[] masterPosition,
            [Out] float[] puckPosition);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setSlaveGameElementPositions(float[] slavePosition, float[] masterPosition,
            float[] puckPosition);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setMasterGameElementPositions(float[] slavePosition);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rotateCamera(float angle);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void getSlavePosition([Out] float[] position);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setOnlineClientType(int clientType);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GoalCallback(int player);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setOnGoalCallback(GoalCallback goalCallback);


        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void slaveGoal();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void masterGoal();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void createPortal(string startUuid, float[] startPosition, float startRotation, float[] startScale, string endUuid, float[] endPosition, float endRotation, float[] endScale);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PortalCreationCallback([Out, MarshalAs(UnmanagedType.LPStr)] string startUuid,
            [Out] IntPtr startPosition, [Out] float startRotation, [Out] IntPtr startScale, [Out, MarshalAs(UnmanagedType.LPStr)] string endUuid, [Out] IntPtr endPosition, [Out] float endRotation, [Out] IntPtr endScale);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setPortalCreationCallback(PortalCreationCallback callback);


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void WallCreationCallback([Out, MarshalAs(UnmanagedType.LPStr)] string uuid,
            [Out] IntPtr position, [Out] float rotation, [Out] IntPtr scale);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setWallCreationCallback(WallCreationCallback callback);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void createWall(string uuid, float[] position, float rotation, float[] scale);


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void BoostCreationCallback([Out, MarshalAs(UnmanagedType.LPStr)] string uuid, [Out] IntPtr position, [Out] float rotation, [Out] IntPtr scale);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBoostCreationCallback(BoostCreationCallback callback);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void createBoost(string uuid, float[] position, float rotation, float[] scale);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void SelectionEventCallback([Out, MarshalAs(UnmanagedType.LPStr)] string uuid, bool isSelected,
            bool deselectAll);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setSelectionEventCallback(SelectionEventCallback callback);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setElementSelection(string username, string uuid, bool isSelected, bool deselectAll);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void TransformEventCallback([Out, MarshalAs(UnmanagedType.LPStr)] string uuidSelected,
            [Out] IntPtr position, [Out] float rotation, [Out] IntPtr scale);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTransformEventCallback(TransformEventCallback callback);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTransformByUUID(string username, string uuid, float[] position, float rotation, float[] scale);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void addNewUser(string username, string userHexColor);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void removeUser(string toCharArray);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void clearUsers();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deselectAll(string username);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCurrentPlayerSelectionColor(string userHexColor);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCurrentPlayerSelectionColorToDefault();


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ControlPointEventCallback([Out, MarshalAs(UnmanagedType.LPStr)] string uuid,[Out] IntPtr position);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setControlPointEventCallback(ControlPointEventCallback callback);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setControlPointPosition(string username, string uuid, float[] position);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DeleteEventCallback([Out, MarshalAs(UnmanagedType.LPStr)] string uuid);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setDeleteEventCallback(DeleteEventCallback callback);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deleteNode(string username, string uuid);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLocalPlayerSkin(string skinName);
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setOpponentPlayerSkin(string skinName);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLocalPlayerSkinToDefault();
        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setOpponentPlayerSkinToDefault();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setGameEnded();

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCanSendPreviewToServer(bool canSendPreviewToServer);
    }
}

