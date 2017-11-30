///////////////////////////////////////////////////////////////////////////////
/// @file Text2D.h
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __TEXT2D_H__
#define __TEXT2D_H__

#include "glm\glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "glm/gtc/type_ptr.hpp"
#include "GL/glew.h"
#include "ft2build.h"
#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"

#include <map>
#include <string>
#include <iostream>
#include "freetype\freetype.h"

///////////////////////////////////////////////////////////////////////////
/// @struct Character
/// @brief Struct contenant les informations d'un charactère
///
/// @author Julien Charbonneau
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
struct Character {
	GLuint     textureID_;  // ID handle of the glyph texture
	glm::ivec2 size_;       // Size of glyph
	glm::ivec2 bearing_;    // Offset from baseline to left/top of glyph
	GLuint     advance_;    // Offset to advance to next glyph
};


///////////////////////////////////////////////////////////////////////////
/// @class Text2D
/// @brief Classe repésentant le texte openGL à l'écran durant la partie
///
/// @author Julien Charbonneau
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
class Text2D {
public:
	/// Constructeur
	Text2D(const int& pointSize, const char*);
	/// Destructeur
	~Text2D();

	/// Afficher le texte à l'écran
	void renderText2D(std::string text, GLfloat x, GLfloat y, glm::vec4 color, int textAlign);
	/// Obtenir la hauteur du font initialisé
	int getFontHeight() { return fontHeight_; };
	/// Obtenir la largeur que prendrait un string à l'écran
	float textWidth(std::string text);

private:
	GLuint vao_;
	GLuint vbo_;
	opengl::Programme prog_;
	opengl::Nuanceur fragShader_;
	opengl::Nuanceur vertexShader_;
	std::map<GLchar, Character> characters_;
	int fontHeight_;

	/// Initialise freetype
	void genCharData(const int& pointSize, const char* fontFile);
	/// Initialise les nuanceurs
	void initShaders();
	/// Initialise les buffers openGL
	void initBuffers();
};

#endif