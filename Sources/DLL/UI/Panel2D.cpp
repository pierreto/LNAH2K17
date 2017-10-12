///////////////////////////////////////////////////////////////////////////////
/// @file Panel2D.cpp
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "Panel2D.h"
#include "FacadeModele.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn Panel2D::Panel2D()
///
/// Constructeur de la classe Pannel2D. Initialise les éléments openGL
/// nécessaire pour l'affichage.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
Panel2D::Panel2D() {
	initShaders();
	initBuffers();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn Panel2D::~Panel2D()
///
/// Destructeur de la classe Pannel2D. Delete les éléments openGL
/// initilisés pour l'affichage.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
Panel2D::~Panel2D() {
	glDeleteVertexArrays(1, &vao_);
	glDeleteBuffers(1, &vbo_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Panel2D::initShaders()
///
/// Initialise les nuanceurs et le programme pour l'affichage des panneaux.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Panel2D::initShaders() {
	if (!prog_.estInitialise()) {
		fragShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Panel2D/fragment_panel2D.glsl");
		vertexShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Panel2D/sommet_panel2D.glsl");
		prog_.initialiser();
		prog_.attacherNuanceur(fragShader_);
		prog_.attacherNuanceur(vertexShader_);

		opengl::Programme::Start(prog_);
		if ((locVertex_ = glGetAttribLocation(prog_.obtenirHandle(), "Vertex")) == -1) std::cerr << "Vextex location not found." << std::endl;
		if ((locColor_ = glGetAttribLocation(prog_.obtenirHandle(), "Color")) == -1) std::cerr << "Color location not found." << std::endl;
		opengl::Programme::Stop(prog_);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Panel2D::initBuffers()
///
/// Initialise les buffers openGL pour l'affichage des panneaux.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Panel2D::initBuffers() {
	glGenVertexArrays(1, &vao_);
	glGenBuffers(1, &vbo_);
	glBindVertexArray(vao_);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_);
	glBufferData(GL_ARRAY_BUFFER, sizeof(GLfloat) * 6 * 2, NULL, GL_DYNAMIC_DRAW);
	glEnableVertexAttribArray(locVertex_);
	glVertexAttribPointer(locVertex_, 2, GL_FLOAT, GL_FALSE, 0, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Panel2D::drawElement(int posX, int posY, float width, float height, glm::vec4 color, int alignX, int alignY)
///
/// Permet d'afficher le panneau openGL à l'écran selon les parametres 
/// desirés.
///
/// @param[in] posX : Position en X
/// @param[in] posY : Position de la souris en X
/// @param[in] width : Largeur du panneau
/// @param[in] height : Hauteur du panneau
/// @param[in] color : Couleur du panneau
/// @param[in] alignX : Alignement horizontal (gauche, centre, droite)
/// @param[in] alignY : Alignement vertical (haut, centre, bas)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Panel2D::drawElement(int posX, int posY, float width, float height, glm::vec4 color, int alignX, int alignY) {
	opengl::Programme::Start(prog_);

	GLint Cloture[4];
	glGetIntegerv(GL_VIEWPORT, Cloture);
	glm::mat4 matrProj = glm::ortho(0.0f, static_cast<GLfloat>(Cloture[2]), 0.0f, static_cast<GLfloat>(Cloture[3]));

	prog_.assignerUniforme("matrProj", matrProj);
	glVertexAttrib4f(locColor_, color.r, color.g, color.b, color.a);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glDisable(GL_DEPTH_TEST);

	// Align center
	if (alignX == 1) posX -= width / 2;
	if (alignY == 1) posY -= height / 2;
	// Align right
	if (alignX == 2) posX -= width;
	// Align bot
	if (alignY == 2) posY -= height;

	GLfloat vertices[6][2] = {
		{ posX,			posY + height },
		{ posX,			posY          },
		{ posX + width, posY		  },

		{ posX,			posY + height },
		{ posX + width, posY,         },
		{ posX + width, posY + height }
	};

	glBindVertexArray(vao_);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_);
	glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(vertices), vertices);
	glDrawArrays(GL_TRIANGLES, 0, 6);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);

	glDisable(GL_BLEND);
	glEnable(GL_DEPTH_TEST);

	opengl::Programme::Stop(prog_);
}
