///////////////////////////////////////////////////////////////////////////////
/// @file FireEffect.cpp
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "FireEffect.h"

#define VBO_POS1  0
#define VBO_POS2  1


////////////////////////////////////////////////////////////////////////
///
/// @fn FireEffect::FireEffect()
///
/// Constructeur de la classe FireEffect. Initialise les éléments openGL
/// nécessaire pour l'affichage.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
FireEffect::FireEffect() {
	initShaders();
	initBuffers();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn FireEffect::~FireEffect()
///
/// Destructeur de la classe FireEffect. Delete les initialisations	à
/// openGL.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
FireEffect::~FireEffect() {
	glDeleteVertexArrays(2, vao_);
	glDeleteBuffers(2, vbo_);
	glDeleteTextures(1, &textureFire_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FireEffect::initShaders()
///
/// Initialise les nuanceurs et le programme pour l'affichage du 
/// système de particule.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FireEffect::initShaders() {
	// Programme pour l'affichage des particules
	fragShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/FireParticles/fragment_fire.glsl");
	geoShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_GEOMETRIE, "nuanceurs/FireParticles/geometrie_fire.glsl");
	vertexShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/FireParticles/sommet_fire.glsl");
	progRender_.initialiser();
	progRender_.attacherNuanceur(fragShader_);
	progRender_.attacherNuanceur(geoShader_);
	progRender_.attacherNuanceur(vertexShader_);

	opengl::Programme::Start(progRender_);
	if ((locVertex_ = glGetAttribLocation(progRender_.obtenirHandle(), "Vertex")) == -1) std::cerr << "Vertex location not found." << std::endl;
	opengl::Programme::Stop(progRender_);


	// Programme pour le calcul des particules
	retroShader_.initialiser(opengl::Nuanceur::Type::NUANCEUR_RETROACTION, "nuanceurs/FireParticles/retroaction_fire.glsl");
	progRetro_.initialiser();
	progRetro_.attacherNuanceur(retroShader_);
	
	const GLchar* vars[] = { "PositionMod", "SpeedMod", "TimeLeftMod" };
	glTransformFeedbackVaryings(progRetro_.obtenirHandle(), sizeof(vars) / sizeof(vars[0]), vars, GL_INTERLEAVED_ATTRIBS);

	opengl::Programme::Start(progRetro_);
	if ((locPositionRetro_ = glGetAttribLocation(progRetro_.obtenirHandle(), "Position")) == -1) std::cerr << "PositionRetro location not found." << std::endl;
	if ((locSpeedRetro_ = glGetAttribLocation(progRetro_.obtenirHandle(), "Speed")) == -1) std::cerr << "SpeedRetro location not found." << std::endl;
	if ((locTimeLeftRetro_ = glGetAttribLocation(progRetro_.obtenirHandle(), "TimeLeft")) == -1) std::cerr << "TimeLeftRetro location not found." << std::endl;
	opengl::Programme::Stop(progRetro_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FireEffect::initBuffers()
///
/// Initialise les buffers openGL pour l'affichage du système de 
/// particule.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FireEffect::initBuffers() {
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

	for (unsigned int i = 0; i < NB_PARTICLES; i++) {
		particles_[i].timeLeft = 0;
	}

	// Initialiser les objets OpenGL
	glGenVertexArrays(2, vao_); 
	glGenBuffers(2, vbo_);    

	// remplir les VBO et faire le lien avec les attributs du nuanceur de sommets
	glBindVertexArray(vao_[0]);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_[VBO_POS1]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(particles_), particles_, GL_DYNAMIC_DRAW);
	glVertexAttribPointer(locVertex_, 3, GL_FLOAT, GL_FALSE, sizeof(Particle), reinterpret_cast<void*>(offsetof(Particle, position)));
	glEnableVertexAttribArray(locVertex_);	
	glBindVertexArray(0);

	// remplir les VBO pour les valeurs modifiées
	glBindVertexArray(vao_[1]);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_[VBO_POS2]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(particles_), particles_, GL_DYNAMIC_DRAW);
	glVertexAttribPointer(locVertex_, 3, GL_FLOAT, GL_FALSE, sizeof(Particle), reinterpret_cast<void*>(offsetof(Particle, position)));
	glEnableVertexAttribArray(locVertex_);
	glBindVertexArray(0);

	// Défaire tous les liens
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	// Charger la texture
	glGenTextures(1, &textureFire_);
	if(!aidegl::glLoadTexture(std::string{ "media/textures/fire_texture.png" }, textureFire_, false)) std::cerr << "Impossible de charger la texture." << std::endl;
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glBindTexture(GL_TEXTURE_2D, 0);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FireEffect::renderFire(const glm::vec3& initPos, const float& initRadius, const float& endRadius, const float& maxTime, const float& maxSpeed, const float& maxHeight, const MatricesPipeline& matrices)
///
/// Appelle les fonctions nécessaires pour la mise à jour et l'affichage 
/// des particules.
///
/// @param[in] initPos		: Position initiale de la particule
/// @param[in] initRadius	: Rayon pour randomiser la position initiale
/// @param[in] endRadius	: Rayon pour randomiser la position finale
/// @param[in] maxTime		: Durée de vie maximale d'une particule
/// @param[in] maxSpeed		: Vitesse maximale d'une particule
/// @param[in] maxHeight	: Hauteur de trajectoire maximale d'une particule
/// @param[in] matrices		: Matrices (model, visu, proj)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FireEffect::renderFire(const glm::vec3& initPos, const float& initRadius, const float& endRadius, const float& maxTime, const float& maxSpeed, const float& maxHeight, const MatricesPipeline& matrices) {
	glDepthMask(GL_FALSE);
	updateParticles(initPos, initRadius, endRadius, maxTime, maxSpeed, maxHeight);
	renderParticles(matrices);
	glDepthMask(GL_TRUE);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FireEffect::updateParticles(const glm::vec3& initPos, const float& initRadius, const float& endRadius, const float& maxTime, const float& maxSpeed, const float& maxHeight) 
///
/// Met à jour la position des particules en utilisant le GPU pour les 
/// calculs via un nuanceur de rétroaction.
///
/// @param[in] initPos		: Position initiale de la particule
/// @param[in] initRadius	: Rayon pour randomiser la position initiale
/// @param[in] endRadius	: Rayon pour randomiser la position finale
/// @param[in] maxTime		: Durée de vie maximale d'une particule
/// @param[in] maxSpeed		: Vitesse maximale d'une particule
/// @param[in] maxHeight	: Hauteur de trajectoire maximale d'une particule
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FireEffect::updateParticles(const glm::vec3& initPos, const float& initRadius, const float& endRadius, const float& maxTime, const float& maxSpeed, const float& maxHeight) {
	opengl::Programme::Start(progRetro_);

	progRetro_.assignerUniforme("DeltaT", DT);
	progRetro_.assignerUniforme("InitPos", initPos);
	progRetro_.assignerUniforme("InitRadius", initRadius);
	progRetro_.assignerUniforme("EndRadius", endRadius);
	progRetro_.assignerUniforme("MaxTime", maxTime);
	progRetro_.assignerUniforme("MaxSpeed", maxSpeed);
	progRetro_.assignerUniforme("MaxHeight", maxHeight);

	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	// faire les transformations de retour
	glBindBufferBase(GL_TRANSFORM_FEEDBACK_BUFFER, 0, vbo_[VBO_POS2]);

	glBindVertexArray(vao_[1]);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_[VBO_POS1]);

	glVertexAttribPointer(locPositionRetro_, 3, GL_FLOAT, GL_FALSE, sizeof(Particle), reinterpret_cast<void*>(offsetof(Particle, position)));
	glVertexAttribPointer(locSpeedRetro_, 3, GL_FLOAT, GL_FALSE, sizeof(Particle), reinterpret_cast<void*>(offsetof(Particle, speed)));
	glVertexAttribPointer(locTimeLeftRetro_, 1, GL_FLOAT, GL_FALSE, sizeof(Particle), reinterpret_cast<void*>(offsetof(Particle, timeLeft)));
	glEnableVertexAttribArray(locPositionRetro_);
	glEnableVertexAttribArray(locSpeedRetro_);
	glEnableVertexAttribArray(locTimeLeftRetro_);

	glEnable(GL_RASTERIZER_DISCARD);
	glBeginTransformFeedback(GL_POINTS);
	glDrawArrays(GL_POINTS, 0, NB_PARTICLES);
	glEndTransformFeedback();
	glDisable(GL_RASTERIZER_DISCARD);

	glBindVertexArray(0);

	// échanger les deux VBO
	std::swap(vbo_[VBO_POS1], vbo_[VBO_POS2]);

	opengl::Programme::Stop(progRetro_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void FireEffect::renderParticles(const MatricesPipeline& matrices)
///
/// Affiche les particules à l'écran à l'aide du programme d'affichage 
///	openGL prévu à cet effet. 
/// 
/// @param[in] matrices		: Matrices (model, visu, proj)
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void FireEffect::renderParticles(const MatricesPipeline& matrices) {
	opengl::Programme::Start(progRender_);

	progRender_.assignerUniforme("ModelMat", matrices.matrModel);
	progRender_.assignerUniforme("ViewMat", matrices.matrVisu);
	progRender_.assignerUniforme("ProjMat", matrices.matrProj);
	progRender_.assignerUniforme("Texture", 0);

	glBindVertexArray(vao_[0]);
	glBindBuffer(GL_ARRAY_BUFFER, vbo_[VBO_POS1]);
	glVertexAttribPointer(locVertex_, 3, GL_FLOAT, GL_FALSE, sizeof(Particle), reinterpret_cast<void*>(offsetof(Particle, position)));

	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, textureFire_);

	glDrawArrays(GL_POINTS, 0, NB_PARTICLES);

	glBindTexture(GL_TEXTURE_2D, 0);
	glBindVertexArray(0);

	opengl::Programme::Stop(progRender_);
}