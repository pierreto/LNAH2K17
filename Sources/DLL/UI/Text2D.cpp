///////////////////////////////////////////////////////////////////////////////
/// @file Text2D.cpp
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include <Text2D.h>


////////////////////////////////////////////////////////////////////////
///
/// @fn Text2D::Text2D()
///
/// Constructeur de la classe Text2D. Initialise les éléments openGL
/// nécessaire pour l'affichage.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
Text2D::Text2D(const int& pointSize, const char* fontFile) {
	initShaders();
	genCharData(pointSize, fontFile);
	initBuffers();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn Text2D::~Text2D()
///
/// Destructeur de la classe Text2D. Delete les éléments openGL
/// initilisés pour l'affichage.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
Text2D::~Text2D() {
	glDeleteVertexArrays(1, &vao_);
	glDeleteBuffers(1, &vbo_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Text2D::initShaders()
///
/// Initialise les nuanceurs et le programme pour l'affichage du texte.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Text2D::initShaders() {
	if (!prog_.estInitialise()) {
		fragShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Text2D/fragment_text2D.glsl");
		vertexShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Text2D/sommet_text2D.glsl");
		prog_.initialiser();
		prog_.attacherNuanceur(fragShader_);
		prog_.attacherNuanceur(vertexShader_);
	}
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Text2D::genCharData(const int& pointSize, const char* fontFile) 
///
/// Initialise freetype et enregistre l'information pour chaque 
/// charactère afin de permettre l'affichage rapide plus tard.
///
/// @param[in] pointSize : Taille de la police
/// @param[in] fontFile : Fichier .ttf de la police
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Text2D::genCharData(const int& pointSize, const char* fontFile) {
	fontHeight_ = pointSize;
	opengl::Programme::Start(prog_);

	// Initialize freetype library
	FT_Library ft;
	if (FT_Init_FreeType(&ft))
		std::cout << "ERROR::FREETYPE: Impossible d'initialiser la librairie FreeType" << std::endl;

	// Initilize font from truetype file (.ttf)
	FT_Face face;
	if (FT_New_Face(ft, fontFile, 0, &face))
		std::cout << "ERROR::FREETYPE: Echec du chargement de la police" << std::endl;
	
	// Set a base char size
	FT_Set_Char_Size(
		face,				// handle
		0,					// char_width ( in 1/64 of a point) ( 0 = same as char_height)
		pointSize * 64,		// char_height  ( in 1/64 of a point) ( 0 = same as char_width)
		96,					// horizontal device resolution (DPI)
		96);				// vertical device resolution (DPI)

	// Disable byte-alignment restriction for openGL
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

	// For each character we need
	for (GLubyte c = 0; c < 128; c++) {
		// Load character glyph 
		if (FT_Load_Char(face, c, FT_LOAD_RENDER)) {
			std::cout << "ERROR::FREETYTPE: Echec du chargement du glyphe" << std::endl;
			continue;
		}

		// Generate texture
		GLuint texture;
		glGenTextures(1, &texture);
		glBindTexture(GL_TEXTURE_2D, texture);
		glTexImage2D(
			GL_TEXTURE_2D,
			0,
			GL_RED,
			face->glyph->bitmap.width,
			face->glyph->bitmap.rows,
			0,
			GL_RED,
			GL_UNSIGNED_BYTE,
			face->glyph->bitmap.buffer
		);

		// Save the generated character for later use
		Character character = {
			texture,
			glm::ivec2(face->glyph->bitmap.width,
				face->glyph->bitmap.rows),
			glm::ivec2(face->glyph->bitmap_left,
				face->glyph->bitmap_top),
			face->glyph->advance.x };
		characters_.insert(std::pair<GLchar, Character>(c, character));

		// Set openGL texture options (clamp to edge)
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);	
	}

	// Destroy FreeType once we're finished
	FT_Done_Face(face);
	FT_Done_FreeType(ft);

	glBindTexture(GL_TEXTURE_2D, 0);
	opengl::Programme::Stop(prog_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Text2D::initBuffers()
///
/// Initialise les buffers openGL pour l'affichage du texte.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Text2D::initBuffers() {
	glGenVertexArrays(1, &vao_);
	glGenBuffers(1, &vbo_);
	glBindVertexArray(vao_);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_);
	glBufferData(GL_ARRAY_BUFFER, sizeof(GLfloat) * 6 * 4, NULL, GL_DYNAMIC_DRAW);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, 4 * sizeof(GLfloat), 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void Text2D::renderText2D(std::string text, GLfloat x, GLfloat y, glm::vec4 color, int textAlign) 
///
/// Permet l'affichage du texte à l'écran selon les paramètres désirés.
///
/// @param[in] text		 : Texte à afficher 
/// @param[in] x		 : Position en X
/// @param[in] y		 : Position en Y
/// @param[in] color	 : Couleur du texte
/// @param[in] textAlign : Alignement horizontal du texte (gauche, centre, droit)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void Text2D::renderText2D(std::string text, GLfloat x, GLfloat y, glm::vec4 color, int textAlign) {
	opengl::Programme::Start(prog_);

	// Get projection matrix
	GLint Cloture[4];
	glGetIntegerv(GL_VIEWPORT, Cloture);
	glm::mat4 projection = glm::ortho(0.0f, static_cast<GLfloat>(Cloture[2]), 0.0f, static_cast<GLfloat>(Cloture[3]));

	// Assign uniform in shader
	prog_.assignerUniforme("textColor", color);
	prog_.assignerUniforme("projection", projection);
	
	// Enable GL_BLEND for alpha rendering
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glDisable(GL_DEPTH_TEST);
	
	glActiveTexture(GL_TEXTURE0);
	glBindVertexArray(vao_);

	float totalWidth = textWidth(text);

	// For every characters of the string
	for (auto c = text.begin(); c != text.end(); c++) {
		Character ch = characters_[*c];

		// Calculate position and size
		GLfloat xpos = x + ch.bearing_.x;
		GLfloat ypos = y - (ch.size_.y - ch.bearing_.y);
		GLfloat w = ch.size_.x;
		GLfloat h = ch.size_.y;

		// Align center
		if (textAlign == 1)
			xpos -= totalWidth / 2;
		// Align right
		else if (textAlign == 2)
			xpos -= totalWidth;

		// Update VBO for each character
		GLfloat vertices[6][4] = {
			{ xpos,     ypos + h,   0.0, 0.0 },
			{ xpos,     ypos,       0.0, 1.0 },
			{ xpos + w, ypos,       1.0, 1.0 },

			{ xpos,     ypos + h,   0.0, 0.0 },
			{ xpos + w, ypos,       1.0, 1.0 },
			{ xpos + w, ypos + h,   1.0, 0.0 }
		};

		// Render texture over quad
		glBindTexture(GL_TEXTURE_2D, ch.textureID_);
		// Update the content of the VBO
		glBindBuffer(GL_ARRAY_BUFFER, vbo_);
		glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(vertices), vertices);
		glBindBuffer(GL_ARRAY_BUFFER, 0);
		// Render quad (two triangles)
		glDrawArrays(GL_TRIANGLES, 0, 6);
		// Advance cursors for next glyph (number of 1/64 pixels) (2^6 = 64)
		x += (ch.advance_ >> 6);
	}

	glBindVertexArray(0);
	glBindTexture(GL_TEXTURE_2D, 0);
	glDisable(GL_BLEND);
	glEnable(GL_DEPTH_TEST);

	opengl::Programme::Stop(prog_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn float Text2D::textWidth(std::string text)
///
/// Détermine la largeur que prendrait un texte à l'écran.
///
/// @param[in] text	: Texte à déterminer la largeur 
///
/// @return La largeur du texte à l'écran
///
////////////////////////////////////////////////////////////////////////
float Text2D::textWidth(std::string text) {
	float totalWidth = 0.0f;

	// For every characters of the string
	for (auto c = text.begin(); c != text.end(); c++) {
		Character ch = characters_[*c];
		totalWidth += (ch.advance_ >> 6);
	}

	return totalWidth;
}