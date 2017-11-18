///////////////////////////////////////////////////////////////////////////////
/// @file FacadeModele.cpp
/// @author Martin Bisson
/// @date 2007-05-22
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////


// Commentaire Doxygen mis sur la première page de la documentation Doxygen.
/**

@mainpage Projet intégrateur de deuxième année -- INF2990

*/

#include <windows.h>
#include <cassert>

#include "GL/glew.h"
#include "FreeImage.h"

#include "FacadeModele.h"
#include "States/EtatTypes.h"

#include "VueOrtho.h"
#include "VuePerspective.h"
#include "Camera.h"
#include "Projection.h"
#include "Logger.h"
#include "Audio.h"

#include "Utilitaire.h"
#include "AideGL.h"
#include "ArbreRenduINF2990.h"
#include "LightManager.h"

#include "ConfigScene.h"
#include "CompteurAffichage.h"

// Remlacement de EnveloppeXML/XercesC par TinyXML
// Julien Gascon-Samson, été 2011
#include "tinyxml2.h"

#include "glm/glm.hpp"
#include "glm/gtc/type_ptr.hpp"
#include "../VisitorByUUID.h"

/// Pointeur vers l'instance unique de la classe.
FacadeModele* FacadeModele::instance_{ nullptr };


/// Chaîne indiquant le nom du fichier de configuration du projet.
const std::string FacadeModele::FICHIER_CONFIGURATION{ "configuration.xml" };


/// Constructeur par défaut
FacadeModele::FacadeModele()
	: etat_(ModeleEtatSelection::obtenirInstance()), rectangleActif_(false)
{
	userManager_ = UserManager();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn FacadeModele* FacadeModele::obtenirInstance()
///
/// Cette fonction retourne un pointeur vers l'instance unique de la
/// classe.  Si cette instance n'a pas été créée, elle la crée.  Cette
/// création n'est toutefois pas nécessairement "thread-safe", car
/// aucun verrou n'est pris entre le test pour savoir si l'instance
/// existe et le moment de sa création.
///
/// @return Un pointeur vers l'instance unique de cette classe.
///
////////////////////////////////////////////////////////////////////////
FacadeModele* FacadeModele::obtenirInstance() {
	if (instance_ == nullptr)
		instance_ = new FacadeModele;

	return instance_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::libererInstance()
///
/// Cette fonction libère l'instance unique de cette classe.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::libererInstance() {
	delete instance_;
	instance_ = nullptr;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn FacadeModele::~FacadeModele()
///
/// Ce destructeur libère les objets du modèle.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
FacadeModele::~FacadeModele() {
	delete arbre_;
	delete vue_;
	delete skybox_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::initialiserOpenGL(HWND hWnd)
///
/// Cette fonction permet d'initialiser le contexte OpenGL.  Elle crée
/// un contexte OpenGL sur la fenêtre passée en paramètre, initialise
/// FreeImage (utilisée par le chargeur de modèles) et assigne des 
/// paramètres du contexte OpenGL.
///
/// @param[in] hWnd : La poignée ("handle") vers la fenêtre à utiliser.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::initialiserOpenGL(HWND hWnd) {
	hWnd_ = hWnd;
	bool succes{ aidegl::creerContexteGL(hWnd_, hDC_, hGLRC_) };
	assert(succes && "Le contexte OpenGL n'a pu être créé.");

	// Initialisation des extensions de OpenGL
	glewInit();

	// Initialisation de la configuration
	chargerConfiguration();

	// FreeImage, utilisée par le chargeur, doit être initialisée
	FreeImage_Initialise();

	// La couleur de fond
	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

	/// Pour normaliser les normales dans le cas d'utilisation de glScale[fd]
	glEnable(GL_NORMALIZE);

	// Qualité
	glShadeModel(GL_SMOOTH);
	glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);

	// Profondeur
	glEnable(GL_DEPTH_TEST);

	// Le cull face
	glEnable(GL_CULL_FACE);
	glCullFace(GL_BACK);

	// Création de l'arbre de rendu.  À moins d'être complètement certain
	// d'avoir une bonne raison de faire autrement, il est plus sage de créer
	// l'arbre après avoir créé le contexte OpenGL.
	arbre_ = new ArbreRenduINF2990;
	arbre_->initialiser();
	
	// On crée une vue orbite par défaut.
	vue_ = new vue::VuePerspective{
		vue::Camera{
		glm::dvec3(-200, 200, 0), glm::dvec3(0, 0, 0),
		glm::dvec3(0, 1, 0),   glm::dvec3(0, 1, 0) },
		vue::ProjectionPerspective{
		500, 500,
		1, 1000, 1, 10000, 0.05,
		200, 200, 45.0 }
	};

	modeOrbite_ = true;

	// Appel au LightManager
	lightManager_ = light::LightManager::obtenirInstance();
	lightManager_->usePresetLighting();

	skybox_ = new utilitaire::BoiteEnvironnement(
		"media/skybox/nebula_xpos.png", "media/skybox/nebula_xneg.png",
		"media/skybox/nebula_ypos.png", "media/skybox/nebula_yneg.png",
		"media/skybox/nebula_zpos.png", "media/skybox/nebula_zneg.png",
		4096);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::chargerConfiguration() const
///
/// Cette fonction charge la configuration à partir d'un fichier XML si
/// ce dernier existe.  Sinon, le fichier de configuration est généré à
/// partir de valeurs par défaut directement dans le code.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::chargerConfiguration() const {
	// Vérification de l'existance du ficher

	// Si le fichier n'existe pas, on le crée.
	if (!utilitaire::fichierExiste(FICHIER_CONFIGURATION)) {
		enregistrerConfiguration();
	}
	// si le fichier existe on le lit
	else {
		tinyxml2::XMLDocument document;

		// Lire à partir du fichier de configuration
		document.LoadFile(FacadeModele::FICHIER_CONFIGURATION.c_str());

		// On lit les différentes configurations.
		ConfigScene::obtenirInstance()->lireDOM(document);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::enregistrerConfiguration() const
///
/// Cette fonction génère un fichier XML de configuration à partir de
/// valeurs par défaut directement dans le code.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::enregistrerConfiguration() const {
	tinyxml2::XMLDocument document;
	// Écrire la déclaration XML standard...
	document.NewDeclaration(R"(?xml version="1.0" standalone="yes"?)");

	// On enregistre les différentes configurations.
	ConfigScene::obtenirInstance()->creerDOM(document);

	// Écrire dans le fichier
	document.SaveFile(FacadeModele::FICHIER_CONFIGURATION.c_str());
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::libererOpenGL()
///
/// Cette fonction libère le contexte OpenGL et désinitialise FreeImage.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::libererOpenGL() {
	utilitaire::CompteurAffichage::libererInstance();

	// On libère les instances des différentes configurations.
	ConfigScene::libererInstance();


	bool succes{ aidegl::detruireContexteGL(hWnd_, hDC_, hGLRC_) };
	assert(succes && "Le contexte OpenGL n'a pu être détruit.");

	FreeImage_DeInitialise();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::afficher() const
///
/// Cette fonction affiche le contenu de la scène.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::afficher() const {
	if (!rectangleActif_) {
		// Efface l'ancien rendu
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

		// Afficher la scène
		afficherBase();
		
		// Afficher texte
		etat_->afficher();	
	
		// Compte de l'affichage
		utilitaire::CompteurAffichage::obtenirInstance()->signalerAffichage();

		// Échange les tampons pour que le résultat du rendu soit visible.
		::SwapBuffers(hDC_);
	}

}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::afficherBase() const
///
/// Cette fonction affiche la base du contenu de la scène, c'est-à-dire
/// qu'elle met en place l'éclairage et affiche les objets.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::afficherBase() const {
	// Positionner la caméra
	glm::mat4 vueProjection(vue_->obtenirProjection().obtenirMatrice() * vue_->obtenirCamera().obtenirMatrice());

	// Afficher le skybox
	skybox_->afficher(vue_->obtenirProjection().obtenirMatrice(), vue_->obtenirCamera().obtenirMatrice());

	// Afficher la scène.
	arbre_->afficher(vueProjection);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::reinitialiser()
///
/// Cette fonction réinitialise la scène à un état "vide".
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::reinitialiser() {
	// Réinitialisation de la scène.
	arbre_->initialiser();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::animer(float temps)
///
/// Cette fonction effectue les différents calculs d'animations
/// nécessaires pour le mode jeu, tel que les différents calculs de
/// physique du jeu.
///
/// @param[in] temps : Intervalle de temps sur lequel effectuer le calcul.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::animer(float temps) {
	// Mise à jour des objets.
	arbre_->animer(temps);

	// Mise à jour de la vue.
	vue_->animer(temps);

	// Mise à jour de la lumière
	lightManager_->animer(temps);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::fleches(double x, double y)
///
/// Cette fonction permet de gérer les touches fléchés du clavier
///
/// @param[in] x : Déplacement en x
/// @param[in] y : Déplacement en y
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::fleches(double x, double y) {
	if (modeOrbite_) {
		vue_->obtenirCamera().orbiterXY(-10*y, 10*x);
	}
	else {
		etat_->fleches(x, y);
	}	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::escape()
///
/// Cette fonction permet de gérer le bouton escape.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::escape() {
	etat_->escape();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::escape()
///
/// Cette fonction permet de gérer la touche d'espacement.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::space() {
	etat_->space();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mouseDownL()
///
/// Cette fonction permet de gérer le clic gauche lorsqu'il est enfoncé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::mouseDownL() {
	etat_->mouseDownL();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mouseDownR()
///
/// Cette fonction permet de gérer le clic droit lorsqu'il est enfoncé.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::mouseDownR() {
	etat_->mouseDownR();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mouseUpL()
///
/// Cette fonction permet de gérer le clic gauche lorsqu'il est relaché.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::mouseUpL() {
	etat_->mouseUpL();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mouseUpR()
///
/// Cette fonction permet de gérer le clic droit lorsqu'il est relaché.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::mouseUpR() {
	etat_->mouseUpR();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::playerMouseMove()
///
/// Cette fonction permet de gérer 
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::playerMouseMove(int x, int y) {
	etat_->playerMouseMove(x, y);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::modifierKeys(bool alt, bool ctrl)
///
/// Cette fonction permet de gérer les boutons "ctrl" et "alt"
///
/// @param[in] alt : Bool sur l'état de la touche ALT
/// @param[in] ctrl : Bool sur l'état de la touche CTRL
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::modifierKeys(bool alt, bool ctrl) {
	etat_->modifierKeys(alt, ctrl);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::selectedNodeInfos(float infos[])
///
/// Cette fonction retourne l'information sur un noeud sélectionné.
///
/// @param[in,out] infos : Tableau initialement vide qui est retourné 
///						   avec les informations de position, rotation, 
///						   scaling (x,y,z)
///
/// @return bool sur la réussite de l'opération (1 seul noeud trouvé)
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::selectedNodeInfos(float infos[]) {
	VisiteurInformation information = VisiteurInformation();

	arbre_->accepterVisiteur(&information);
	return information.lireInformations(infos);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::applyNodeInfos(float infos[])
///
/// Cette fonction applique l'information sur un noeud sélectionné.
///
/// @param[in] infos : Tableau de position, rotation, scaling (x,y,z)
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::applyNodeInfos(float infos[]) {

	VisiteurObtenirSelection selection = VisiteurObtenirSelection();
	arbre_->accepterVisiteur(&selection);
	NoeudAbstrait* noeud = selection.obtenirNoeuds().front();

	float rotation = noeud->obtenirRotation().y;
	glm::vec3 scale = noeud->obtenirScale();
	glm::vec3 position = noeud->obtenirPositionRelative();

	VisiteurInformation information = VisiteurInformation();
	information.ecrireInformations(infos);
	arbre_->accepterVisiteur(&information);

	if (!etat_->noeudsSurLaTable()) {
		noeud->assignerPositionRelative(position);
		noeud->scale(scale);
		noeud->rotate(rotation, glm::vec3(0, 1, 0));
	}else if (ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION)
	{
		ModeleEtatJeu::obtenirInstance()->getTransformEventCallback()(noeud->getUUID(), glm::value_ptr(noeud->obtenirPositionRelative()), noeud->obtenirRotation().y, glm::value_ptr(noeud->obtenirScale()));
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mouseOverTable()
///
/// Cette fonction vérifie si le curseur est au dessus de la table.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::mouseOverTable() {
	return etat_->mouseOverTable();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::mouseOverControlPoint()
///
/// Cette fonction vérifie si le curseur est au dessus d'un point de contrôle
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
bool FacadeModele::mouseOverControlPoint() {
	return etat_->mouseOverControlPoint();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::deleteSelection()
///
/// Supprime tous les noeuds sélectionnés
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::deleteSelection() {
	VisiteurSuppression suppression = VisiteurSuppression();
	arbre_->accepterVisiteur(&suppression);
	suppression.deleteAllSelectedNode(ModeleEtatJeu::obtenirInstance()->currentOnlineClientType() == ModeleEtatJeu::ONLINE_EDITION);
}


std::string FacadeModele::_getMapJson(float coefficients[]) {
	char json[] = " {} ";
	char buffer[sizeof(json)];



	memcpy(buffer, json, sizeof(json));
	docJSON_.ParseInsitu(buffer);

	if (!docJSON_.HasMember("PointControle")) {
		rapidjson::Value pcArray(rapidjson::kArrayType);
		docJSON_.AddMember("PointControle", pcArray, docJSON_.GetAllocator());
	}

	if (!docJSON_.HasMember("Accelerateur")) {
		rapidjson::Value acclArray(rapidjson::kArrayType);
		docJSON_.AddMember("Accelerateur", acclArray, docJSON_.GetAllocator());
	}

	if (!docJSON_.HasMember("Muret")) {
		rapidjson::Value murArray(rapidjson::kArrayType);
		docJSON_.AddMember("Muret", murArray, docJSON_.GetAllocator());
	}

	if (!docJSON_.HasMember("Portail")) {
		rapidjson::Value portailArray(rapidjson::kArrayType);
		docJSON_.AddMember("Portail", portailArray, docJSON_.GetAllocator());
	}

	if (!docJSON_.HasMember("Coefficients")) {
		rapidjson::Value coefArray(rapidjson::kArrayType);
		coefArray.PushBack(coefficients[0], docJSON_.GetAllocator());
		coefArray.PushBack(coefficients[1], docJSON_.GetAllocator());
		coefArray.PushBack(coefficients[2], docJSON_.GetAllocator());
		docJSON_.AddMember("Coefficients", coefArray, docJSON_.GetAllocator());
	}

	VisiteurSauvegarde visiteur = VisiteurSauvegarde();
	arbre_->accepterVisiteur(&visiteur);


	//createMapIcon();


	rapidjson::StringBuffer buffer2;
	rapidjson::Writer<rapidjson::StringBuffer>writer(buffer2);
	docJSON_.Accept(writer);
	std::string data(buffer2.GetString(), buffer2.GetSize());
	return data;
}


void FacadeModele::createMapIcon(unsigned char* dest) {
	/*
	if (!docJSON_.HasMember("Icon")) {
		rapidjson::Value iconBytes(rapidjson::kArrayType);
		docJSON_.AddMember("Icon", iconBytes, docJSON_.GetAllocator());
	}
	*/

	glm::ivec2 oldDim = vue_->obtenirProjection().getLargeurFenetre();

	glm::ivec2 newLargeur = glm::ivec2(1184,600);


	//vue::VueOrtho*  vueOrtho = dynamic_cast<vue::ProjectionOrtho*>(vue_);

	vue_->setLargeurFenetre(newLargeur.x, newLargeur.y);


	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

	// Afficher la scène
	afficherBase();

	// Afficher texte
	etat_->afficher();


	glm::ivec2 newDim = vue_->obtenirProjection().obtenirDimensionCloture();


	int sizex = 775;
	GLubyte* outPixels = new GLubyte[3 * sizex * newDim.y];

	glPixelStorei(GL_PACK_ALIGNMENT, 1);

	glReadPixels(350, 0, sizex, newDim.y, GL_BGR, GL_UNSIGNED_BYTE, outPixels);

	FIBITMAP* image = FreeImage_ConvertFromRawBits(outPixels, sizex, newDim.y, 3 * sizex, 24, 0x0000FF, 0xFF0000, 0x00FF00, false);

	FIBITMAP* imageRescaled = FreeImage_Rescale(image, 128, 128, FILTER_BOX);

	BYTE bytes[128 * 128 * 3];
	//FreeImage_ConvertToRawBits(bytes, imageRescaled, 3 * 128, 24, 0x0000FF, 0xFF0000, 0x00FF00, false);
	//FreeImage_Save(FIF_JPEG, imageRescaled, "test.jpg", 0);

	/*
	rapidjson::Value tempArray(rapidjson::kArrayType);
	for(int i =0; i<128*128*3;i++ )
	{
		tempArray.PushBack(bytes[i], docJSON_.GetAllocator());
	}

	docJSON_["Icon"].PushBack(tempArray, docJSON_.GetAllocator());
	*/
/*
	for (int i = 0; i < 128 * 128 * 3; i++)
	{
		dest[i] = (unsigned char)bytes[i];
	}*/

	FIMEMORY *hmem = NULL;

	// load and decode a regular file

	// open a memory stream
	hmem = FreeImage_OpenMemory();

	// encode and save the image to the memory
	FreeImage_SaveToMemory(FIF_JPEG, imageRescaled, hmem, 0);


	// seek to the start of the memory stream
	FreeImage_SeekMemory(hmem, 0L, SEEK_SET);

	// get the file type
	FREE_IMAGE_FORMAT mem_fif = FreeImage_GetFileTypeFromMemory(hmem, 0);

	// load an image from the memory handle
	FIBITMAP *check = FreeImage_LoadFromMemory(mem_fif, hmem, 0);


	FreeImage_ConvertToRawBits(dest, check, 3 * 128, 24, 0x0000FF, 0xFF0000, 0x00FF00, false);



	FreeImage_Save(FIF_JPEG, check, "test.jpg", 0);




	// Free resources
	FreeImage_Unload(image);
	FreeImage_Unload(imageRescaled);

	vue_->setLargeurFenetre(oldDim.x, oldDim.y);
}

void FacadeModele::getMapIcon(unsigned char* icon) {
	createMapIcon(icon);
}

void FacadeModele::getMapJson(float coefficients[], char* map) {
	std::string json = _getMapJson(coefficients);
	std::strcpy(map, json.c_str());
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::enregistrerSous(std::string filePath, float coefficients[])
///
/// Cette fonction permet d'enregistrer le patinoire dans un fichier .json 
/// dont le chemin d'accès est spécifié en paramètre. Le fichier est 
/// créé s'il n'existe pas.
///
/// @param[in] filePath : Nom du filepath choisi
/// @param[in] coefficients[] : Coefficients de la carte
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::enregistrerSous(std::string filePath, float coefficients[]) {
	
	std::string json = _getMapJson(coefficients);
	std::ofstream(filePath) << json.c_str();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn wstring s2ws(const string& s)
///
/// Cette fonction convertit un string en LCPWSTR pour le MessageBox
///
/// @param[in] s : Le string à convertir
///
/// @return wstring : Le LCPWSTR (après la conversion)
///
////////////////////////////////////////////////////////////////////////
std::wstring s2ws(const std::string& s) {
	int len;
	int slength = (int)s.length() + 1;
	len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0);
	wchar_t* buf = new wchar_t[len];
	MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
	std::wstring r(buf);
	delete[] buf;
	return r;
}

void FacadeModele::chargerCarte(std::string json, float coefficients[])
{
	if (docJSON_.Parse(json.c_str()).HasParseError()) {
			std::string temp = "Le fichier de sauvegarde contient des erreurs... \n\nsorry   \n\n:(";
			std::wstring stemp = s2ws(temp);
			LPCWSTR message = stemp.c_str();

			std::string temp2 = "SAUVEGARDE CORROMPUE";
			std::wstring stemp2 = s2ws(temp2);
			LPCWSTR title = stemp2.c_str();

			MessageBox(0, message, title, MB_OK);
	}
	else {
		obtenirArbreRenduINF2990()->initialiser();

		chargerPntCtrl();

		creerNoeuds("Accelerateur", ArbreRenduINF2990::NOM_ACCELERATEUR);
		creerNoeuds("Portail", ArbreRenduINF2990::NOM_PORTAIL);
		creerNoeuds("Muret", ArbreRenduINF2990::NOM_MUR);

		coefficients[0] = docJSON_["Coefficients"][0].GetDouble();
		coefficients[1] = docJSON_["Coefficients"][1].GetDouble();
		coefficients[2] = docJSON_["Coefficients"][2].GetDouble();

		std::cout << "Project succesfully loaded!" << std::endl;
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::ouvrir(std::string filePath, float coefficients[])
///
/// Cette fonction permet de charger une patinoire préalablement suavegarder
///
/// @param[in] filePath : Nom du filepath du fichier d'ouverture
/// @param[in] coefficients[] : Coefficients de la carte
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::ouvrir(std::string filePath, float coefficients[]) {
	std::ifstream file(filePath);
	std::stringstream buffer;
	buffer << file.rdbuf();
	chargerCarte(buffer.str(), coefficients);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::creerNoeuds(char* type, std::string nomType)
///
/// Cette fonction permet de créer les noeuds sur la patinoire en fonction
/// de l'information contenu dans le fichier de sauvegarde.
///
/// @param[in] type		: Type de noeud (string)
/// @param[in] nomType	: Type de noeud (enum de l'Arbre)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::creerNoeuds(char* type, std::string nomType) {
	if (nomType == ArbreRenduINF2990::NOM_ACCELERATEUR || nomType == ArbreRenduINF2990::NOM_MUR) {
		for (unsigned int i = 0; i < docJSON_[type].Size(); i++) {
			NoeudAbstrait* noeud = arbre_->creerNoeud(nomType, static_cast<const char*>(docJSON_[type][i][7].GetString()));
			noeud->appliquerRotation(docJSON_[type][i][6].GetDouble(), glm::vec3(0, 1, 0));

			glm::dvec3 scale;
			scale.x = docJSON_[type][i][3].GetDouble();
			scale.y = docJSON_[type][i][4].GetDouble();
			scale.z = docJSON_[type][i][5].GetDouble();
			noeud->appliquerScale(scale);
			obtenirArbreRenduINF2990()->ajouter(noeud);

			glm::dvec3 temp;
			temp.x = docJSON_[type][i][0].GetDouble();
			temp.y = docJSON_[type][i][1].GetDouble();
			temp.z = docJSON_[type][i][2].GetDouble();




			glm::vec3 pos(temp);
			noeud->deplacer(pos);

		}
	}
	else if (nomType == ArbreRenduINF2990::NOM_PORTAIL) {
		for (unsigned int i = 0; i < docJSON_[type].Size(); i+=2) {
			std::vector<NoeudPortail*> linkedPortals;
			for (unsigned int j = 0; j < 2; j++) {
				std::string idPOrtal = static_cast<const char*>(docJSON_[type][i + j][7].GetString());
				linkedPortals.push_back((NoeudPortail*)arbre_->creerNoeud(nomType, static_cast<const char*>(docJSON_[type][i + j][7].GetString())));

				linkedPortals[j]->appliquerRotation(docJSON_[type][i + j][6].GetDouble(), glm::vec3(0, 1, 0));

				glm::dvec3 scale;
				scale.x = docJSON_[type][i + j][3].GetDouble();
				scale.y = docJSON_[type][i + j][4].GetDouble();
				scale.z = docJSON_[type][i + j][5].GetDouble();
				linkedPortals[j]->appliquerScale(scale);
				obtenirArbreRenduINF2990()->ajouter(linkedPortals[j]);

				glm::dvec3 temp;
				temp.x = docJSON_[type][i + j][0].GetDouble();
				temp.y = docJSON_[type][i + j][1].GetDouble();
				temp.z = docJSON_[type][i + j][2].GetDouble();
				glm::vec3 pos(temp);
				linkedPortals[j]->deplacer(pos);
			}

			linkedPortals[0]->assignerOppose(linkedPortals[1]);
			linkedPortals[1]->assignerOppose(linkedPortals[0]);
		}
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::chargerPntCtrl()
///
/// Cette fonction permet de placer les points de contrôle sur la patinoire 
/// en fonction de l'information contenu dans le fichier de sauvegarde.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::chargerPntCtrl() {
	NoeudComposite* table = (NoeudComposite*)arbre_->chercher(ArbreRenduINF2990::NOM_TABLE);
	int i = 0;
	glm::dvec3 temp;
	for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); ++it)
	{
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL) {
			temp.x = docJSON_["PointControle"][i][0].GetDouble();
			temp.y = docJSON_["PointControle"][i][1].GetDouble();
			temp.z = docJSON_["PointControle"][i][2].GetDouble();
			glm::vec3 pos(temp);
			(*it)->assignerPositionRelative(pos);
			(*it)->setUUID(const_cast<char*>(docJSON_["PointControle"][i][7].GetString()));
			i++;
		}
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::changeGridVisibility(bool visibility)
///
/// Cette fonction change la visibilité de la grid derrière la table.
///
/// @param[in] visibility : Visibilité de la grid
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::changeGridVisibility(bool visibility) {
	NoeudTable* table = (NoeudTable*)arbre_->chercher(ArbreRenduINF2990::NOM_TABLE);
	table->changeGridVisibility(visibility);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::resetCameraPosition()
///
/// Cette fonction reinitialise la position de la caméra.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::resetCameraPosition() {
	// Obtenir les dimensions de la cloture pour transferer a l'autre vue
	glm::ivec2 cloture = vue_->obtenirProjection().obtenirDimensionCloture();
	delete vue_;

	if (modeOrbite_)
		vue_ = new vue::VuePerspective{
			vue::Camera{
			glm::dvec3(-200, 200, 0), glm::dvec3(0, 0, 0),
			glm::dvec3(0, 1, 0),   glm::dvec3(0, 1, 0) },
			vue::ProjectionPerspective{
			500, 500,
			1, 1000, 1, 10000, 0.005,
			200, 200, 45.0 }
		};
	else
		vue_ = new vue::VueOrtho{
			vue::Camera{
			glm::dvec3(0, 200, 0), glm::dvec3(0, 0, 0),
			glm::dvec3(1, 0, 0),   glm::dvec3(0, 1, 0) },
			vue::ProjectionOrtho{
			500, 500,
			1, 1000, 1, 10000, 1.25,
			200, 200 }
		};

	vue_->redimensionnerFenetre(cloture.x, cloture.y);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::gererRondelleMaillets(bool toggle)
///
/// Cette fonction cree ou supprime la rondelle et les 2 maillets.
///
/// @param[in] toggle : Interrupteur pour creer ou detruire
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::gererRondelleMaillets(bool toggle) {
	if (toggle) {
		// Obtention de l'arbre de rendu
		NoeudRondelle* rondelle{ (NoeudRondelle*)arbre_->creerNoeud(ArbreRenduINF2990::NOM_RONDELLE) };
		NoeudMaillet* maillet1{ (NoeudMaillet*)arbre_->creerNoeud(ArbreRenduINF2990::NOM_MAILLET) };
		NoeudMaillet* maillet2{ (NoeudMaillet*)arbre_->creerNoeud(ArbreRenduINF2990::NOM_MAILLET) };

		// Assigner un id a chaque maillet
		maillet1->assignerId(1);
		maillet2->assignerId(2);

		// Assigner une position a la rondelle
		rondelle->assignerPositionRelative(glm::vec3(0, 0, 0));

		// Assigner une position aux maillets
		NoeudTable* table = (NoeudTable*)(arbre_->chercher(ArbreRenduINF2990::NOM_TABLE));
		float noeudGauche, noeudDroit;

		for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); it++) {
			if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL) {
				glm::vec3 pos = ((NoeudPointControl*)(*it))->obtenirPositionRelative();
				if (pos.x == 0 && pos.z < 0)
					noeudGauche = ((NoeudPointControl*)(*it))->getInternalPosition().z;
				if (pos.x == 0 && pos.z > 0)
					noeudDroit = ((NoeudPointControl*)(*it))->getInternalPosition().z;
			}
		}

		maillet1->assignerPositionRelative(glm::vec3(0, 0, fmin(noeudGauche / 2, 0)));
		maillet2->assignerPositionRelative(glm::vec3(0, 0, fmax(noeudDroit / 2, 0)));

		// Ajout dans l'arbre
		arbre_->ajouter(rondelle);
		arbre_->ajouter(maillet1);
		arbre_->ajouter(maillet2);
	}
	else {
		// Retire la rondelle de l'arbre
		arbre_->effacer(arbre_->chercher(ArbreRenduINF2990::NOM_RONDELLE));
		arbre_->effacer(arbre_->chercher(ArbreRenduINF2990::NOM_MAILLET));
		arbre_->effacer(arbre_->chercher(ArbreRenduINF2990::NOM_MAILLET));
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void toggleControlPointsVisibility(bool visible)
///
/// Cette fonction change l'état d'affichage des points de contrôles;
///
/// @param[in] visible : Visibilité des points (vrai si visible)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::toggleControlPointsVisibility(bool visible) {
	NoeudTable* table = (NoeudTable*)arbre_->chercher(ArbreRenduINF2990::NOM_TABLE);
	for (auto it = table->obtenirIterateurBegin(); it != table->obtenirIterateurEnd(); it++) {
		std::string type = (*it)->obtenirType();
		if ((*it)->obtenirType() == ArbreRenduINF2990::NOM_POINT_CONTROL)
			((NoeudPointControl*)(*it))->invisible(!visible);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::loadSounds()
///
/// Cette fonction charge tous les sons du jeu.
///
/// @param[in] Aucun
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::loadSounds() {
	Audio::obtenirInstance()->loadSounds();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::playMusic(bool quickPlay) 
///
/// Cette fonction active ou desactive la musique de fond.
///
/// @param[in] quickPlay : True si on est en Partie Rapide, False sinon.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::playMusic(bool quickPlay) {
	Audio::obtenirInstance()->playMusic(quickPlay);	
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::toggleOrbit(bool orbit) 
///
/// Active ou desactive le mode orbite de la caméra
///
/// @param[in] orbit : Etat du mode orbit
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::toggleOrbit(bool orbit) {
	modeOrbite_ = orbit;
	FacadeModele::resetCameraPosition();
}

void FacadeModele::rotateCamera(float angle)
{
	vue_->obtenirCamera().orbiterXY(0, angle);
}


NoeudAbstrait* FacadeModele::findNodeInTree(const char* uuid)
{
	VisitorByUUID visitorWrapper = VisitorByUUID(uuid);
	arbre_->accepterVisiteur(&visitorWrapper);
	NoeudAbstrait* node = visitorWrapper.getNode();
	if(visitorWrapper.hasFound)
	{
		return node;
	}
	return nullptr;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void FacadeModele::changerModeleEtat(MODELE_ETAT etat)
///
/// Cette fonction modifie le modèle état en cours.
///
/// @param[in] etat : Etat à appliquer
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FacadeModele::changerModeleEtat(MODELE_ETAT etat) {
	// Nettoyer l'état courant
	etat_->nettoyerEtat();

	switch (etat) {
	case MODELE_ETAT::AUCUN:
		etat_ = nullptr;
		break;
	case MODELE_ETAT::SELECTION:
		etat_ = ModeleEtatSelection::obtenirInstance();
		break;
	case MODELE_ETAT::DEPLACEMENT:
		etat_ = ModeleEtatDeplacement::obtenirInstance();
		break;
	case MODELE_ETAT::ROTATION:
		etat_ = ModeleEtatRotation::obtenirInstance();
		break;
	case MODELE_ETAT::MISE_A_ECHELLE:
		etat_ = ModeleEtatScale::obtenirInstance();
		break;
	case MODELE_ETAT::ZOOM:
		etat_ = ModeleEtatZoom::obtenirInstance();
		break;
	case MODELE_ETAT::DUPLIQUER:
		etat_ = ModeleEtatDuplication::obtenirInstance();
		break;
	case MODELE_ETAT::POINTS_CONTROLE:
		etat_ = ModeleEtatPointControl::obtenirInstance();
		break;
	case MODELE_ETAT::JEU:
		etat_ = ModeleEtatJeu::obtenirInstance();
		break;
	case MODELE_ETAT::CREATION_ACCELERATEUR:
		etat_ = ModeleEtatCreerBoost::obtenirInstance();
		break;
	case MODELE_ETAT::CREATION_MURET:
		etat_ = ModeleEtatCreerMuret::obtenirInstance();
		break;
	case MODELE_ETAT::CREATION_PORTAIL:
		etat_ = ModeleEtatCreerPortail::obtenirInstance();
		break;
	default:
		break;
	}
	// Initialisation de l'etat
	etat_->initialiser();
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
