///////////////////////////////////////////////////////////////////////////////
/// @file AideGL.cpp
/// @brief Ce fichier contient l'implantation de l'espace de nom aidegl.
///
/// Il contient les d�clarations de fonctions utiles pour effectuer certaines
/// t�ches OpenGL qui reviennent d'un projet � l'autre.
///
/// @author Martin Bisson
/// @date 2007-01-20
///
/// @addtogroup utilitaire Utilitaire
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "AideGL.h"
#include "Utilitaire.h"
#include "Primitives.h"

#ifdef WIN32
#include <windows.h>
#endif

#include "FreeImage.h"
#include "GL/glew.h"
#include <iostream>
#include <cassert>

#include "glm\glm.hpp"

namespace aidegl {


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn bool creerContexteGL(HWND hWnd, HDC& hDC, HGLRC& hRC)
	///
	/// Cette fonction cr�e un contexte OpenGL pour la fen�tre donn�e.
	///
	/// @param[in] hWnd : Poign�e ("handle") vers la fen�tre dans laquelle on veut cr�er un contexte.
	/// @param[out] hDC : Poign�e ("handle") vers le "Device context" de la fen�tre.
	/// @param[out] hRC : Poign�e ("handle") vers le contexte OpenGL cr��.
	///
	/// @return Vrai en cas de succ�s.
	///
	////////////////////////////////////////////////////////////////////////
	bool creerContexteGL(HWND hWnd, HDC& hDC, HGLRC& hRC)
	{
		// Obtention du "Device context"
		hDC = ::GetDC(hWnd);

		if (hDC == nullptr) {
			return false;
		}

		// On choisit le format pour le "Device context"
		PIXELFORMATDESCRIPTOR pfd;
		::ZeroMemory(&pfd, sizeof(pfd));
		pfd.nSize = sizeof(pfd);
		pfd.nVersion = 1;
		pfd.dwFlags = PFD_DRAW_TO_WINDOW |
			PFD_SUPPORT_OPENGL |
			PFD_DOUBLEBUFFER;
		pfd.iPixelType = PFD_TYPE_RGBA;
		pfd.cColorBits = 24;
		pfd.cDepthBits = 16;
		pfd.cStencilBits = 1;
		pfd.iLayerType = PFD_MAIN_PLANE;

		int format{ ::ChoosePixelFormat(hDC, &pfd) };
		if (format == 0) {
			::ReleaseDC(hWnd, hDC);
			hDC = 0;
			return false;
		}

		if (!::SetPixelFormat(hDC, format, &pfd)) {
			::ReleaseDC(hWnd, hDC);
			hDC = 0;
			return false;
		}


		// Cr�ation du contexte
		hRC = ::wglCreateContext(hDC);
		if (hRC == 0) {
			::ReleaseDC(hWnd, hDC);
			hDC = 0;
			return false;
		}

		// Activation du contexte
		if (!::wglMakeCurrent(hDC, hRC)) {
			::wglDeleteContext(hRC);
			hRC = 0;
			::ReleaseDC(hWnd, hDC);
			hDC = 0;
			return false;
		}

		// Tout s'est bien pass�
		return true;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn bool detruireContexteGL(HWND hWnd, HDC hDC, HGLRC hRC)
	///
	/// Cette fonction d�truit proprement un contexte OpenGL.
	///
	/// @param[in] hWnd : Poign�e ("handle") vers la fen�tre du contexte � d�truire.
	/// @param[in] hDC  : Poign�e ("handle") vers le "Device context" de la fen�tre.
	/// @param[in] hRC  : Poign�e ("handle") vers le contexte OpenGL � d�truire.
	///
	/// @return Vrai en cas de succ�s.
	///
	////////////////////////////////////////////////////////////////////////
	bool detruireContexteGL(HWND hWnd, HDC hDC, HGLRC hRC)
	{
		bool succes = true;

		if (!::wglMakeCurrent(0, 0))
			succes = false;

		if (!::wglDeleteContext(hRC))
			succes = false;

		if (::ReleaseDC(hWnd, hDC) == 0)
			succes = false;

		return succes;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void initialiserRectangleElastique(const glm::ivec2& point, unsigned short patron, int facteur)
	///
	/// Cette fonction initialise le mode de rendu du rectangle �lastique en
	/// mode XOR en sauvegardant l'�tat de la machine OpenGL et en la modifiant
	/// pour que le rendu soit correct.
	///
	/// @param[in] point   : Point initial de localisation du rectangle.
	/// @param[in] patron  : Patron de pointill� pour les lignes du rectangle.
	/// @param[in] facteur : Facteur du pointill� pour les lignes du rectangle.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void initialiserRectangleElastique(
		const glm::ivec2& point,
		unsigned short   patron, //= 0x3333
		int              facteur //= 1
		)
	{
		// On sauvegarde les attributs de tra�age.
		glPushAttrib(GL_LINE_BIT |
			GL_CURRENT_BIT |
			GL_COLOR_BUFFER_BIT |
			GL_DEPTH_BUFFER_BIT);

		// On dessine uniquement dans ce qui est d�j� � l'�cran.
		glDrawBuffer(GL_FRONT);

		// On d�sactive le test de profondeur et les textures.
		glDisable(GL_DEPTH_TEST);
		glDisable(GL_TEXTURE_2D);

		// On sauvegarde les matrices de transformation.
		glMatrixMode(GL_MODELVIEW);
		glPushMatrix();
		glLoadIdentity();
		glMatrixMode(GL_PROJECTION);
		glPushMatrix();

		// On initialise la matrice de projection: 1 pixel = 1 unit� virtuelle.
		GLint Cloture[4];
		glGetIntegerv(GL_VIEWPORT, Cloture);
		glLoadIdentity();
		gluOrtho2D(0.0, (GLdouble) Cloture[2], (GLdouble) Cloture[3], 0.0);

		// On trace le rectangle en pointill�s.
		glEnable(GL_LINE_STIPPLE);
		glLineStipple(1, 0x3333);

		// On choisit la couleur blanche pour que le XOR soit une inversion des
		// bits de la couleur d�j� pr�sente dans la m�moire de trame.
		glColor3f(1.0, 1.0, 1.0);

		// On active le mode XOR.
		glEnable(GL_COLOR_LOGIC_OP);
		glLogicOp(GL_XOR);

		// On trace le rectangle en un seul point.
		glBegin(GL_LINE_LOOP);
		{
			glVertex2i(point[0], point[1]);
			glVertex2i(point[0], point[1]);
			glVertex2i(point[0], point[1]);
			glVertex2i(point[0], point[1]);
		}
		glEnd();

		// On veut que le rectangle soit imm�diatement visible.
		glFlush();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void mettreAJourRectangleElastique(const glm::ivec2& pointAncrage, const glm::ivec2& pointAvant, const glm::ivec2& pointApres)
	///
	/// Cette fonction met � jour la position du rectangle �lastique en
	/// effa�ant le rectangle pr�c�dent pour le remplacer par un nouveau.
	///
	/// @param[in] pointAncrage : Point initial de localisation du rectangle.
	/// @param[in] pointAvant   : Autre coin du rectangle avant la mise � jour.
	/// @param[in] pointApres   : Autre coin du rectangle apr�s la mise � jour.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void mettreAJourRectangleElastique(
		const glm::ivec2& pointAncrage,
		const glm::ivec2& pointAvant,
		const glm::ivec2& pointApres
		)
	{
		// On trace l'ancien rectangle pour l'effacer et restaurer la couleur qui
		// �tait l� avant (gr�ce au XOR).
		glBegin(GL_LINE_LOOP);
		{
			glVertex2i(pointAncrage[0], pointAncrage[1]);
			glVertex2i(pointAncrage[0], pointAvant[1]);
			glVertex2i(pointAvant[0], pointAvant[1]);
			glVertex2i(pointAvant[0], pointAncrage[1]);
		}
		glEnd();

		// On trace le nouveau rectangle.
		glBegin(GL_LINE_LOOP);
		{
			glVertex2i(pointAncrage[0], pointAncrage[1]);
			glVertex2i(pointAncrage[0], pointApres[1]);
			glVertex2i(pointApres[0], pointApres[1]);
			glVertex2i(pointApres[0], pointAncrage[1]);
		}
		glEnd();

		// On s'arrange pour que le nouveau rectangle soit imm�diatement visible.
		glFlush();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void terminerRectangleElastique(const glm::ivec2& pointAncrage, const glm::ivec2& pointFinal)
	///
	/// Cette fonction termine le mode de rendu du rectangle �lastique en
	/// restaurant l'�tat de la machine OpenGL � ce qu'il �tait.
	///
	/// @param[in] pointAncrage : Point initial de localisation du rectangle.
	/// @param[in] pointFinal   : Point final de localisation du rectangle.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void terminerRectangleElastique(
		const glm::ivec2& pointAncrage,
		const glm::ivec2& pointFinal
		)
	{
		// On trace l'ancien rectangle pour l'effacer et restaurer la couleur qui
		// �tait l� avant (gr�ce au XOR).
		glBegin(GL_LINE_LOOP);
		{
			glVertex2i(pointAncrage[0], pointAncrage[1]);
			glVertex2i(pointAncrage[0], pointFinal[1]);
			glVertex2i(pointFinal[0], pointFinal[1]);
			glVertex2i(pointFinal[0], pointAncrage[1]);
		}
		glEnd();

		// On r�tablit les attributs initiaux.
		glPopAttrib();

		// On r�tablit la transformation initiale.
		glMatrixMode(GL_PROJECTION);
		glPopMatrix();
		glMatrixMode(GL_MODELVIEW);
		glPopMatrix();

		// On s'arrange pour que le nouveau rectangle soit imm�diatement visible.
		glFlush();

		// On se remet � dessiner dans le tampon arri�re.
		glDrawBuffer(GL_BACK);
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void showGrid(const glm::dvec3& pointMin, const glm::dvec3& pointMax, int divisionW, int divisionH, float lineWidth, glm::vec4 color, MatricesPipeline matrices)
	///
	/// Cette fonction affiche un rectangle subdivis� en grille d'un certain
	/// nombre de points.
	///
	/// @param[in] pointMin		: Point en bas a gauche du rectangle.
	/// @param[in] pointMax		: Point en haut a droite du rectangle.
	/// @param[in] divisionW    : Nombre de divisions en largeur
	/// @param[in] divisionH    : Nombre de divisions en hauteur
	/// @param[in] lineWidth	: Largeur de ligne de la grid
	/// @param[in] color        : Couleur de la grid
	/// @param[in] matrices		: Matrices de la pipeline graphique
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void showGrid(
		const glm::dvec3& pointMin, const glm::dvec3& pointMax,
		int divisionW, int divisionH, float lineWidth, 
		glm::vec4 color, MatricesPipeline matrices
		)
	{
		float depth = pointMin.y;

		float width = (pointMax.z - pointMin.z);
		float height = (pointMax.x - pointMin.x);

		float stepW = width / (divisionW - 1);
		float stepH = height / (divisionH - 1);

		float centerW = pointMin.z + (width / 2);
		float centerH = pointMin.x + (height / 2);	

		for (float posZ = pointMin.z; posZ <= pointMax.z; posZ += stepW) {
			opengl::Rectangle ligne(glm::vec3(centerH, depth, posZ), lineWidth, height, color);
			ligne.afficher(matrices);
		}

		for (float posX = pointMin.x; posX <= pointMax.x; posX += stepH) {
			opengl::Rectangle ligne(glm::vec3(posX, depth, centerW), width, lineWidth, color);
			ligne.afficher(matrices);
		}
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void afficherCercle(glm::vec3 centre, float rayon, unsigned int nbrPoints)
	///
	/// Cette fonction affiche un cercle
	///
	/// @param[in] centre : Le centre du cercle
	/// @param[in] rayon : Le rayon du cercle
	/// @param[in] nbrPoints : Le nombre de points pour former le cercle
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void afficherCercle(glm::vec3 centre, float rayon, unsigned int nbrPoints)
	{
		double angle = 2.0f * utilitaire::PI / nbrPoints;
		
		glBegin(GL_TRIANGLE_FAN);
		{
			glVertex3d(centre.x, 0.1, centre.z);
			for (unsigned int i = 0; i < nbrPoints; ++i)
			{
				glm::dvec3 point{ rayon * glm::sin(angle * i), 0.1, rayon * glm::cos(angle * i) };
				point += centre;
				glVertex3d(point.x, point.y, point.z);
			}
			glVertex3d(centre.x, 0.1, centre.z + rayon);

		} 

		glEnd();
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn std::string obtenirMessageErreur(GLenum codeErreur, int drapeau)
	///
	/// Cette fonction retourne une cha�ne d�taillant l'erreur identifi�e par
	/// le code d'erreur pass� en param�tre.  La cha�ne retourn�e contient
	/// d'abord le nom de la constante identifiant le code d'erreur, suivi
	/// du message de la fonction gluErrorString(), et une explication
	/// provenant du "man page" de glGetError().
	///
	/// @param[in] codeErreur : Code d'erreur.
	/// @param[in] drapeau    : Information d�sir�e dans le message d'erreur.
	///
	/// @return Cha�ne contenant le message d'erreur.
	///
	////////////////////////////////////////////////////////////////////////
	std::string obtenirMessageErreur(
		GLenum codeErreur, int drapeau //= AFFICHE_ERREUR_TOUT
		)
	{
		std::string chaineCode;
		std::string message;

		// Il sera possible d'avoir un "map" au lieu de ce switch, qui n'est pas
		// tr�s performant pour les cas o� on ne veut pas le message par exemple.
		switch (codeErreur) {
		case GL_NO_ERROR:
			chaineCode = "GL_NO_ERROR";
			message = "No error has been recorded.  The value of this "
				"symbolic constant is guaranteed to be 0.";
			break;

		case GL_INVALID_ENUM:
			chaineCode = "GL_INVALID_ENUM";
			message = "An unacceptable value is specified for an enumerated "
				"argument.  The offending command is ignored, and has "
				"no other side effect than to set the error flag.";
			break;

		case GL_INVALID_VALUE:
			chaineCode = "GL_INVALID_VALUE";
			message = "A numeric argument is out of range.  The offending "
				"command is ignored, and has no other side effect than "
				"to set the error flag.";
			break;

		case GL_INVALID_OPERATION:
			chaineCode = "GL_INVALID_OPERATION";
			message = "The specified operation is not allowed in the current "
				"state.  The offending command is ignored, and has no "
				"other side effect than to set the error flag.";
			break;

		case GL_STACK_OVERFLOW:
			chaineCode = "GL_STACK_OVERFLOW";
			message = "This command would cause a stack overflow.  The "
				"offending command is ignored, and has no other side "
				"effect than to set the error flag.";
			break;

		case GL_STACK_UNDERFLOW:
			chaineCode = "GL_STACK_UNDERFLOW";
			message = "This command would cause a stack underflow.  The "
				"offending command is ignored, and has no other side "
				"effect than to set the error flag.";
			break;

		case GL_OUT_OF_MEMORY:
			chaineCode = "GL_OUT_OF_MEMORY";
			message = "There is not enough memory left to execute the "
				"command.  The state of the GL is undefined, except "
				"for the state of the error flags, after this error is "
				"recorded.";
			break;

		default:
			chaineCode = "GL_????????";
			message = "Code d'erreur non identifi�.";
			break;
		};

		// Concatenation des messages
		bool aPrecedent{ false };
		std::string retour;

		if (drapeau & AFFICHE_ERREUR_CODE) {
			retour = chaineCode;
			aPrecedent = true;
		}

		if (drapeau & AFFICHE_ERREUR_GLUERRORSTRING) {
			if (aPrecedent)
				retour += " , ";
			retour += reinterpret_cast<const char*> (
				gluErrorString(codeErreur)
				);
			aPrecedent = true;
		}

		if (drapeau & AFFICHE_ERREUR_DESCRIPTION) {
			if (aPrecedent)
				retour += " , ";
			retour += message;
		}

		return retour;
	}


	////////////////////////////////////////////////////////////////////////
	///
	/// @fn void verifierErreurOpenGL(int drapeau)
	///
	/// Cette fonction v�rifie s'il y a une erreur OpenGL, et si c'est le cas
	/// affiche un message d'erreur correspondant � l'erreur.
	///
	/// @param[in] drapeau : Information d�sir�e dans le message d'erreur.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////
	void verifierErreurOpenGL(
		int drapeau //= AFFICHE_ERREUR_TOUT
		)
	{
		GLenum codeErreur{ glGetError() };

		if (codeErreur || (drapeau & AFFICHE_ERREUR_AUCUNE))
			std::cerr << obtenirMessageErreur(codeErreur, drapeau) << std::endl;
	}


	///////////////////////////////////////////////////////////////////////////
	///
	/// @fn bool glLoadTexture(const std::string& nomFichier, unsigned int& idTexture,
	///                        bool genererTexture)
	///
	/// Cette fonction cr�e une texture OpenGL � partir d'une image contenu
	/// dans un fichier.  FreeImage est utilis�e pour lire l'image, donc tous
	/// les formats reconnues par cette librairie devraient �tre support�s.
	///
	/// @param[in]  nomFichier     : Le nom du fichier image � charger.
	/// @param[out] idTexture      : L'identificateur de la texture cr��e.
	/// @param[in]  genererTexture : Doit-on demander � OpenGL de g�n�rer un num�ro
	///										de texture au pr�alable?
	///
	/// @return Vrai si le chargement a r�ussi, faux autrement.
	///
	///////////////////////////////////////////////////////////////////////////
	bool glLoadTexture(const std::string& nomFichier, unsigned int& idTexture, bool genererTexture)
	{
		FIBITMAP* dib;
		FIBITMAP* dib32;
		glLoadImage(nomFichier, dib, dib32);

		unsigned int pitch{ FreeImage_GetPitch(dib32) };

		glCreateTexture(
			FreeImage_GetBits(dib32),
			FreeImage_GetWidth(dib32),
			FreeImage_GetHeight(dib32),
			FreeImage_GetBPP(dib32),
			FreeImage_GetPitch(dib32),
			idTexture,
			genererTexture
			);

		FreeImage_Unload(dib32);
		FreeImage_Unload(dib);

		return true;
	}

	///////////////////////////////////////////////////////////////////////////
	///
	/// @fn bool glLoadImage(const std::string& nomFichier, FIBITMAP*& dib, FIBITMAP*& dib32)
	///
	/// Cette fonction extrait les donn�es d'une image.  
	/// FreeImage est utilis�e pour lire l'image, donc tous
	/// les formats reconnues par cette librairie devraient �tre support�s.
	///
	/// @param[in]  nomFichier     : Le nom du fichier image � charger.
	/// @param[out] dib		       : Les donnn�es de l'image.
	/// @param[in]  dib32		   : Les donn�es de l'image sous un format 32 bit
	///
	/// @return Vrai si le chargement a r�ussi, faux autrement.
	///
	///////////////////////////////////////////////////////////////////////////
	bool glLoadImage(const std::string& nomFichier, FIBITMAP*& dib, FIBITMAP*& dib32)
	{
		// Ce code de lecture g�n�rique d'un fichier provient de la
		// documentation de FreeImage
		FREE_IMAGE_FORMAT format{ FIF_UNKNOWN };
		// check the file signature and deduce its format
		// (the second argument is currently not used by FreeImage)
		format = FreeImage_GetFileType(nomFichier.c_str(), 0);
		if (format == FIF_UNKNOWN) {
			// no signature ?
			// try to guess the file format from the file extension
			format = FreeImage_GetFIFFromFilename(nomFichier.c_str());
		}
		// check that the plugin has reading capabilities ...
		if ((format == FIF_UNKNOWN) || !FreeImage_FIFSupportsReading(format)) {
			utilitaire::afficherErreur(
				std::string{ "Format du fichier image \"" } +
				std::string{ nomFichier.c_str() } +std::string{ "\" non support�" }
			);
			return false;
		}
		// ok, let's load the file
		dib = FreeImage_Load(format, nomFichier.c_str(), 0);

		if (dib == 0) {
			utilitaire::afficherErreur(
				std::string{ "Erreur � la lecture du fichier \"" } +
				std::string{ nomFichier.c_str() } +std::string{ "\"" }
			);
			return false;
		}

		dib32 = FreeImage_ConvertTo32Bits(dib);
		if (dib32 == 0) {
			utilitaire::afficherErreur(
				std::string{ "Incapable de convertir le fichier \"" } +
				std::string{ nomFichier.c_str() } +std::string{ "\" en 32 bpp." }
			);
			FreeImage_Unload(dib);
			return false;
		}

		return true;
	}


	///////////////////////////////////////////////////////////////////////////
	///
	/// @fn void glCreateTexture(unsigned char* data, int x, int y, int bpp, int pitch,
	///                          unsigned int& ID, bool genererTexture)
	///
	/// Cette fonction cr�e une texture � partir des pixels en m�moire.
	///
	/// @param[in]  data           : Le tableau des valeurs des pixels.
	/// @param[in]  x              : La largeur de l'image.
	/// @param[in]  y              : La hauteur de l'image.
	/// @param[in]  bpp            : Le nombre de bits par pixels.
	/// @param[in]  pitch          : La largeur en m�moire d'une rang�e de l'image.
	/// @param[out] ID             : L'identificateur de la texture cr��e.
	/// @param[in]  genererTexture : Doit-on demander � OpenGL de g�n�rer un num�ro
	///										de texture au pr�alable?
	///
	/// @return Aucune.
	///
	///////////////////////////////////////////////////////////////////////////
	void glCreateTexture(
		unsigned char* data, int x, int y, int bpp, int pitch, unsigned int& ID, bool genererTexture
		)
	{
		if (genererTexture) {
			glGenTextures(1, &ID);
		}
		glBindTexture(GL_TEXTURE_2D, ID);

		// Le pitch est ignor� pour le moment, car on le suppose toujours �gal �
		// la largeur, mais il pourrait �tre utilis� pour produire une
		// implantation plus robuste.
		if (pitch != x*bpp / 8) {
			utilitaire::afficherErreur(
				"L'implantation ne supporte pas un \"pitch\" diff�rent de la largeur."
				);
			return;
		}

		// Tel que sp�cifi� dans la documentation de FreeImage (voir Pixel Access 
		// Functions, page 29 du PDF FreeImage 3.9.0 documentation), la 
		// disposition des composantes des couleurs est d�pendante de 
		// l'architecture.  Sur little endian, le format BGR (plut�t que RGB) est 
		// utilis�.  On utilise donc l'extension GL_EXT_bgra (on pourrait v�rifie 
		// � l'ex�cution que l'extension est pr�sente)
		if (bpp == 24)
			gluBuild2DMipmaps(GL_TEXTURE_2D, 3, x, y, GL_BGR_EXT, GL_UNSIGNED_BYTE, data);
		else if (bpp == 32)
			gluBuild2DMipmaps(GL_TEXTURE_2D, 4, x, y, GL_BGRA_EXT, GL_UNSIGNED_BYTE, data);
		else
			utilitaire::afficherErreur(
			"Incapable de lire une image qui n'a pas 24 ou 32 bits par pixels."
			);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	}


} // Fin de l'espace de nom aidegl.


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
