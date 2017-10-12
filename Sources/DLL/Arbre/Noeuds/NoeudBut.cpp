///////////////////////////////////////////////////////////////////////////////
/// @file NoeudBut.cpp
/// @author Nam Lesage
/// @date 2015-10-02
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudBut.h"
#include "But.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"
#include "LightManager.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"

#define BUT_COULEUR glm::vec3(109.0f/255.0f, 189.0f/255.0f, 97.0f/225.0f)

opengl::Programme NoeudBut::programme_;
opengl::Nuanceur NoeudBut::nuanceurFragment_;
opengl::Nuanceur NoeudBut::nuanceurSommet_;
opengl::Nuanceur NoeudBut::nuanceurGeo_;

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudBut::NoeudBut(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudBut::NoeudBut(const std::string& typeNoeud)
	: NoeudAbstrait{ typeNoeud }, sommets_{12, glm::vec3(0)}
{
	initialiserBut();
	initialiserNuanceurs();
	materiau = but::obtenirMateriau();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudAccelerateur::~NoeudAccelerateur()
///
/// Ce destructeur désallouee la liste d'affichage du but.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudBut::~NoeudBut()
{
	glDeleteVertexArrays(1, &vao_);
	glDeleteBuffers(1, &tampon_sommets_);
	glDeleteBuffers(1, &tampon_faces_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBut::initialiserBut()
///
/// Initialise le openGl des buts
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::initialiserBut()
{
	faces_ = but::obtenirFaces();

	glGenVertexArrays(1, &vao_);
	glBindVertexArray(vao_);

	glGenBuffers(1, &tampon_sommets_);
	glGenBuffers(1, &tampon_faces_);

	glBindBuffer(GL_ARRAY_BUFFER, tampon_sommets_);
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3) * sommets_.size(), &sommets_[0], GL_DYNAMIC_DRAW);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
	glEnableVertexAttribArray(0);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, tampon_faces_);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLuint) * faces_.size(), &faces_[0], GL_STATIC_DRAW);

	glBindVertexArray(0);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBut::initialiserNuanceurs()
///
/// Initialise le programme de nuanceurs
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::initialiserNuanceurs() {
	if (!programme_.estInitialise())
	{
		nuanceurFragment_.initialiser(opengl::Nuanceur::Type::NUANCEUR_FRAGMENT, "nuanceurs/Base/fragment.glsl");
		nuanceurSommet_.initialiser(opengl::Nuanceur::Type::NUANCEUR_VERTEX, "nuanceurs/Base/sommet.glsl");
		nuanceurGeo_.initialiser(opengl::Nuanceur::Type::NUANCEUR_GEOMETRIE, "nuanceurs/Lighting/geometrieLambert.glsl");
		programme_.initialiser();
		programme_.attacherNuanceur(nuanceurFragment_);
		programme_.attacherNuanceur(nuanceurGeo_);
		programme_.attacherNuanceur(nuanceurSommet_);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBut::dessiner(const glm::mat4 & matrice) const
///
/// Dessine le but
///
/// @param[in] matrice : La matrice de transformation
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::dessiner(const glm::mat4 & matrice) const
{

	MatricesPipeline matrices = obtenirMatricePipeline();

	// Appliquer le nuanceur
	opengl::Programme::Start(programme_);
	programme_.assignerUniforme("modelViewProjection", matrice);
	programme_.assignerUniforme("matrModel", matrices.matrModel);
	programme_.assignerUniforme("matrVisu", matrices.matrVisu);
	programme_.assignerUniforme("matrNormale", glm::transpose(glm::inverse(glm::mat3(matrices.matrModel))));
	programme_.assignerUniforme("positionCamera", matrices.positionCamera);

	int disableAmbiant = light::LightManager::obtenirInstance()->obtenirAmbiantState() ? 1 : 0;
	programme_.assignerUniforme("disableAmbiant", disableAmbiant);
	
	// TODO :  Enlever ce uniform quand la specularité va fonctionner
	programme_.assignerUniforme("disableSpeculaire", 1);

	appliquerMateriau();

	glBindVertexArray(vao_);
	// Change color
	GLint useDiffuseColorLoc = glGetUniformLocation(programme_.obtenirHandle(), "useDiffuseColor");
	GLint matDiffuseLoc = glGetUniformLocation(programme_.obtenirHandle(), "material.diffuse");
	glm::vec3 color = BUT_COULEUR;
	glUniform3f(matDiffuseLoc, color.r, color.g, color.b);
	glUniform1i(useDiffuseColorLoc, true);

	// Reload Data
	glBindBuffer(GL_ARRAY_BUFFER, tampon_sommets_);
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3) * sommets_.size(), &sommets_[0], GL_DYNAMIC_DRAW);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);

	// Dessiner la table
	glDrawElements(GL_TRIANGLES, faces_.size(), GL_UNSIGNED_INT, 0);
	glBindVertexArray(0);

	// Desactiver le nuanceur
	opengl::Programme::Stop(programme_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudAccelerateur::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::afficherConcret(const glm::mat4& vueProjection) const
{   
	// Affichage du modèle.
	dessiner(vueProjection);
}

void NoeudBut::appliquerMateriau() const
{
	programme_.assignerUniforme("material.diffuse", materiau.diffuse_);
	programme_.assignerUniforme("material.transparence", materiau.opacite_);
	programme_.assignerUniforme("material.emission", materiau.emission_);
	programme_.assignerUniforme("material.speculaire", materiau.speculaire_);
	programme_.assignerUniforme("material.ambiant", materiau.ambiant_);
	programme_.assignerUniforme("material.shininess", materiau.shininess_ * materiau.shininessStrength_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBut::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::accepterVisiteur(VisiteurAbstrait* visiteur) {
	visiteur->visiterBut(this);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBut::assignerPointControl(NoeudPointControl * pointControl)
///
/// Assigne un point de contrôle à un but
///
/// @param[in] pointControl : Le point de contrôle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::assignerPointControl(NoeudPointControl * pointControl)
{
	pointControl_ = pointControl;
	ajusterPoints();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudBut::ajusterPoints()
///
/// Ajuste les sommets du but en fonction de la position du point de contrôle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudBut::ajusterPoints()
{
	if (pointControl_ == nullptr)
	{
		return;
	}

	glm::vec3 pc0 = pointControl_->obtenirPositionRelative();
	glm::vec3 pc1 = pointControl_->obtenirVoisins()[0]->obtenirPositionRelative();
	glm::vec3 pc2 = pointControl_->obtenirVoisins()[1]->obtenirPositionRelative();

	glm::vec3 v1 = glm::normalize(pc1 - pc0);
	glm::vec3 v2 = glm::normalize(pc2 - pc0);

	// Calcul de la bisectrice
	glm::vec3 bisect;
	if (glm::length(v1 + v2) < 0.001f) { // Droite perpendiculaire
		bisect = glm::normalize(glm::vec3(-v1.z, 0, v1.x));
	}
	else {
		bisect = glm::normalize(v1 + v2);
	}

	// Inverse bisect if needed
	float dotProduct = glm::clamp(glm::dot(bisect, glm::normalize(pc0)), -1.0f, 1.0f);
	if (glm::acos(dotProduct) > utilitaire::PI / 2)
		bisect = -bisect;


	// Calcul de la diagonale de la bordure
	float angle = utilitaire::PI / 2 - glm::acos(glm::clamp(glm::dot(v1, v2), -1.0f, 1.0f)) / 2;
	float h = TABLE_BORDER_WIDTH / glm::cos(angle) * 0.5;

	sommets_[0].x = pc0.x; sommets_[0].z = pc0.z - 0.5 * bisect.z; // En bas
	sommets_[1] = pc0; sommets_[1].y += 1; sommets_[1].z = pc0.z - 0.5 * bisect.z; // En haut
	
	glm::vec3 p1 = pc0 + v1 * GOAL_WIDTH / 2.0f;
	sommets_[2].x = p1.x; sommets_[2].z = p1.z - 0.5 * bisect.z; // En bas
	sommets_[3] = p1; sommets_[3].y += 1; sommets_[3].z = p1.z - 0.5 * bisect.z; // En haut

	glm::vec3 p2 = pc0 + v1 * GOAL_WIDTH / 2.0f + bisect * h;
	sommets_[4].x = p2.x; sommets_[4].z = p2.z; // En bas
	sommets_[5] = p2; sommets_[5].y += 1; // En haut

	glm::vec3 p3 = pc0 + bisect * h;;
	sommets_[6].x = p3.x; sommets_[6].z = p3.z; // En bas
	sommets_[7] = p3; sommets_[7].y += 1; // En haut

	glm::vec3 p4 = pc0 + v2 * GOAL_WIDTH / 2.0f + bisect * h;
	sommets_[8].x = p4.x; sommets_[8].z = p4.z; // En bas
	sommets_[9] = p4; sommets_[9].y += 1; // En haut

	glm::vec3 p5 = pc0 + v2 * GOAL_WIDTH / 2.0f;
	sommets_[10].x = p5.x; sommets_[10].z = p5.z - 0.5 * bisect.z; // En bas
	sommets_[11] = p5; sommets_[11].y += 1; sommets_[11].z = p5.z - 0.5 * bisect.z; // En haut

	collider_.clear();
	// Donnees les points du collider
	collider_.push_back(sommets_[2]);
	collider_.push_back(sommets_[4]);
	collider_.push_back(sommets_[6]);
	collider_.push_back(sommets_[8] );
	collider_.push_back(sommets_[10]);
	collider_.push_back(sommets_[0]);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn std::vector<glm::vec3> NoeudBut::obtenirCollider()
///
/// Cette fonction obtient le collider
///
/// @return Le collider
///
////////////////////////////////////////////////////////////////////////
std::vector<glm::vec3> NoeudBut::obtenirCollider()
{
	return collider_;
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
