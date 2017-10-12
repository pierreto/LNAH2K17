///////////////////////////////////////////////////////////////////////////////
/// @file Panel2D.h
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __PANEL2D_H__
#define __PANEL2D_H__

#include "glm\glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "glm/gtc/type_ptr.hpp"
#include "GL/glew.h"
#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"

#include <string>

///////////////////////////////////////////////////////////////////////////
/// @class Panel2D
/// @brief Classe repésentant un panel 2D d'affichage pour les infos de 
///		   la partie courrante.
///
/// @author Julien Charbonneau
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
class Panel2D {
public:
	/// Constructeur
	Panel2D();
	/// Destructeur
	~Panel2D();

	/// Afficher le panneau à l'écran
	void drawElement(int posX, int posY, float width, float height, glm::vec4 color, int alignX, int alignY);

private:
	GLuint vao_;
	GLuint vbo_;
	opengl::Programme prog_;
	opengl::Nuanceur fragShader_;
	opengl::Nuanceur vertexShader_;
	GLint locVertex_;
	GLint locColor_;

	/// Initialise les nuanceurs
	void initShaders();
	/// Initialise les buffers openGL
	void initBuffers();
};

#endif

