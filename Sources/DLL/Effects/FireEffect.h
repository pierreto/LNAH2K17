///////////////////////////////////////////////////////////////////////////////
/// @file FireEffect.h
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __FIREEFFECT_H__
#define __FIREEFFECT_H__

#include "glm\glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "glm/gtc/type_ptr.hpp"
#include "GL/glew.h"
#include "OpenGL_Nuanceur.h"
#include "OpenGL_Programme.h"
#include "AideGL.h"

#include <string>
#include <iostream>

#define NB_PARTICLES 1000

///////////////////////////////////////////////////////////////////////////
/// @struct Particle
/// @brief Struct contenant les informations d'une particule
///
/// @author Julien Charbonneau
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
struct Particle {
	GLfloat position[3];
	GLfloat speed[3];
	GLfloat timeLeft;
};


///////////////////////////////////////////////////////////////////////////
/// @class FireEffect
/// @brief Fonction permettant l'affiche d'un systeme de particules afin 
///		   de représenter un effet de feu.
///
/// @author Julien Charbonneau
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
class FireEffect {
public:
	/// Constructeur
	FireEffect();
	/// Destructeur
	~FireEffect();

	/// Affichage du système de particule
	void renderFire(const glm::vec3& initPos, const float& initRadius, const float& endRadius, const float& maxTime, const float& maxSpeed, const float& maxHeight, const MatricesPipeline& matrices);

private:
	GLuint vao_[2];
	GLuint vbo_[2];
	GLuint textureFire_;;

	// Programme affichage
	opengl::Programme progRender_;
	opengl::Nuanceur fragShader_;
	opengl::Nuanceur geoShader_;
	opengl::Nuanceur vertexShader_;
	GLint locVertex_;
	
	// Programme retro
	opengl::Programme progRetro_;
	opengl::Nuanceur retroShader_;
	GLint locPositionRetro_;
	GLint locSpeedRetro_;
	GLint locTimeLeftRetro_;

	Particle particles_[NB_PARTICLES];

	float DT = (1.0f / 120.0f);

	/// Initialise les nuanceurs
	void initShaders();
	/// Initialise les buffers OpenGL
	void initBuffers();
	/// Met à jour la position des particules
	void updateParticles(const glm::vec3& initPos, const float& initRadius, const float& endRadius, const float& maxTime, const float& maxSpeed, const float& maxHeight);
	/// Affiche les particule à l'écran
	void renderParticles(const MatricesPipeline& matrices);
};

#endif