///////////////////////////////////////////////////////////////////////////////
/// @file NoeudTable.cpp
/// @author Nam Lesage
/// @date 2015-09-08
/// @version 0.1
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#include "NoeudTable.h"
#include "Visiteurs\VisiteurAbstrait.h"
#include "Utilitaire.h"

#include "GL/glew.h"
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_VBO.h"
#include "FacadeModele.h"
#include "ArbreRenduINF2990.h"
#include "AideGl.h"
#include "Primitives.h"
#include "LightManager.h"
#include <iostream>

opengl::Programme NoeudTable::programme_;
opengl::Nuanceur NoeudTable::nuanceurFragment_;
opengl::Nuanceur NoeudTable::nuanceurSommet_;
opengl::Nuanceur NoeudTable::nuanceurGeo_;

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTable::NoeudTable(const std::string& typeNoeud)
///
/// Appel le constructeur de la classe de base
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudTable::NoeudTable(const std::string& typeNoeud, const char* uuid)
	: NoeudComposite{ typeNoeud }, vao_(0), tampon_faces_(0), tampon_sommets_(0), showGrid_(true)
{
	initialiserTable();
	initialiserNuanceurs();
	generateTexture();
	creerPointsDeControle();
	materiau = table::obtenirMateriau();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTable::~NoeudTable()
///
/// Ce destructeur désallouee la liste d'affichage de la table.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudTable::~NoeudTable()
{
	glDeleteVertexArrays(1, &vao_);
	glDeleteBuffers(1, &tampon_sommets_);
	glDeleteBuffers(1, &tampon_faces_);
	glDeleteBuffers(1, &tampon_coords);
	glDeleteTextures(1, &texture_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTable::initialiserTable()
///
/// Cette fonction permet d'initialiser la table
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::initialiserTable()
{
	// Sommets	
	sommets_ = table::obtenirSommets();

	// Faces
	faces_ = table::obtenirFaces();

	// Coordonnées de texture
	texCoords_ = table::obtenirTexCoords();

	// Creation du vao
	glGenVertexArrays(1, &vao_);
	glBindVertexArray(vao_);

	glGenBuffers(1, &tampon_sommets_);
	glGenBuffers(1, &tampon_faces_);
	glGenBuffers(1, &tampon_coords);

	glBindBuffer(GL_ARRAY_BUFFER, tampon_sommets_);
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3) * sommets_.size(), &sommets_[0], GL_DYNAMIC_DRAW);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);
	glEnableVertexAttribArray(0);

	glBindBuffer(GL_ARRAY_BUFFER, tampon_coords);
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec2) * texCoords_.size(), &texCoords_[0], GL_STATIC_DRAW);
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 0, nullptr);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, tampon_faces_);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLuint) * faces_.size(), &faces_[0], GL_STATIC_DRAW);
	
	glBindVertexArray(0);
}

void NoeudTable::generateTexture()
{
	glGenTextures(1, &texture_);

	// Charger la texture
	aidegl::glLoadTexture(std::string{ "media/textures/table_texture.png" }, texture_, false);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTable::initialiserNuanceurs()
///
/// Cette fonction permet d'initialiser les nuanceurs
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::initialiserNuanceurs()
{
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
/// @fn NoeudTable::creerPointsDeControle()
///
/// Cette fonction permet de créer des points de controle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::creerPointsDeControle()
{
	// Obtention de l'arbre de rendu
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	
	// Vecteur contenant les noeuds (utilise pour associer les noeuds entre eux par apres)
	std::vector<NoeudPointControl*> noeuds;
	// Creation des noeuds
	for (int i = 1; i < sommets_.size() - 1; i += 5)
	{
		NoeudPointControl* noeudControl{ (NoeudPointControl*)arbre->creerNoeud(ArbreRenduINF2990::NOM_POINT_CONTROL) };
		NoeudBooster* noeudBooster{ (NoeudBooster*)arbre->creerNoeud(ArbreRenduINF2990::NOM_BOOSTER) };
		noeudBooster->assignerAxisLock(glm::ivec3(1, 1, 1));
		noeudBooster->appliquerDeplacement(glm::vec3(0, -TABLE_BASE_HEIGHT, 0));
		noeudBooster->assignerAxisLock(glm::ivec3(1, 0, 1));
		
		std::vector<glm::vec3*> points;
		for (int j = 0; j < VERTEX_PER_CORNER; ++j)
		{
			points.push_back(&sommets_[i + j]);
		}

		noeudControl->assignerPoints(points);
		noeudControl->assignerAxisLock(glm::ivec3(1, 1, 1));
		noeudControl->assignerPositionRelative(sommets_[i+1]);
		noeudControl->assignerAxisLock(glm::ivec3(1, 0, 1));

		if (noeudControl->obtenirPositionRelative().x != 0) {
			noeudControl->assignerBooster(noeudBooster);
			ajouter(noeudBooster);
		}

		noeuds.push_back(noeudControl);
	}

	// Associer chaque noeud avec son oppose
	unsigned int association[8] = { 4, 3, 6, 1, 0, 7, 2, 5 };
	for (int i = 0; i < noeuds.size(); i++)
	{
		glm::vec3 symmetrie(1, 1, -1);
		if (i == 2 || i == 6)
			symmetrie = glm::vec3(-1, 1, 1);
	
		noeuds[i]->assignerNoeudOppose(noeuds[association[i]], symmetrie);
	}

	// Assigner les voisins
	for (int i = 0; i < noeuds.size(); ++i)
	{
		std::vector<NoeudPointControl*> voisins;
		voisins.push_back(noeuds[(i - 1) % noeuds.size()]);
		voisins.push_back(noeuds[(i + 1) % noeuds.size()]);
		noeuds[i]->assignerVoisins(voisins);
	}

	// Blocage du deplacement sur certains points
	noeuds[2]->assignerAxisLock(glm::ivec3(1, 0, 0));
	noeuds[6]->assignerAxisLock(glm::ivec3(1, 0, 0));
	noeuds[0]->assignerAxisLock(glm::ivec3(0, 0, 1));
	noeuds[4]->assignerAxisLock(glm::ivec3(0, 0, 1));
	 
	// Zone de deplacement pour chaque point
	noeuds[0]->assignerZoneDeplacement(glm::vec4(-MAX_TABLE_LENGTH, MAX_TABLE_LENGTH, 50, MAX_TABLE_LENGTH));
	noeuds[1]->assignerZoneDeplacement(glm::vec4(30, MAX_TABLE_LENGTH, 50, MAX_TABLE_LENGTH));
	noeuds[2]->assignerZoneDeplacement(glm::vec4(30, MAX_TABLE_LENGTH, 0, 0));
	noeuds[3]->assignerZoneDeplacement(glm::vec4(30, MAX_TABLE_LENGTH, -MAX_TABLE_LENGTH, -50));
	noeuds[4]->assignerZoneDeplacement(glm::vec4(-MAX_TABLE_LENGTH, MAX_TABLE_LENGTH, -MAX_TABLE_LENGTH, -50 ));
	noeuds[5]->assignerZoneDeplacement(glm::vec4(-MAX_TABLE_LENGTH, -30 , -MAX_TABLE_LENGTH, -50));
	noeuds[6]->assignerZoneDeplacement(glm::vec4(-MAX_TABLE_LENGTH, -30, 0, 0));
	noeuds[7]->assignerZoneDeplacement(glm::vec4(-MAX_TABLE_LENGTH, -30, 50, MAX_TABLE_LENGTH));

	// Creation des buts
	creerButs(noeuds[4], noeuds[0]);

	// Ajout des noeuds à la table
	for (auto& noeud : noeuds)
	{
		ajouter(noeud);
		noeud->ajusterPoints();
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTable::creerButs(NoeudPointControl * pointGauche, NoeudPointControl * pointDroite)
///
/// Cette fonction permet de créer des buts
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::creerButs(NoeudPointControl * pointGauche, NoeudPointControl * pointDroite)
{
	// Obtention de l'arbre de rendu
	ArbreRenduINF2990* arbre = FacadeModele::obtenirInstance()->obtenirArbreRenduINF2990();
	NoeudBut* butGauche{ (NoeudBut*)arbre->creerNoeud(ArbreRenduINF2990::NOM_BUT) };
	NoeudBut* butDroite{ (NoeudBut*)arbre->creerNoeud(ArbreRenduINF2990::NOM_BUT) };

	// Deplacer les buts
	float offset = 10;
	butGauche->deplacer(pointGauche->obtenirPositionRelative() + glm::vec3(0, 0, offset));
	butDroite->deplacer(pointDroite->obtenirPositionRelative() + glm::vec3(0, 0, -offset));

	// Associer avec un point controle
	pointGauche->assignerBut(butGauche);
	pointDroite->assignerBut(butDroite);

	// Ajout dans l'arbre
	ajouter(butGauche);
	ajouter(butDroite);

	buts_.push_back(butGauche);
	buts_.push_back(butDroite);

}

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudTable::dessiner(const glm::mat4& matrice) const
///
/// Cette fonction permet de dessiner la table
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::dessiner(const glm::mat4& matrice) const
{
	MatricesPipeline matrices =  obtenirMatricePipeline();

	// Dessiner la grid
	if (showGrid_) {
		glDisable(GL_DEPTH_TEST);
		glm::vec3 pointMin(-MAX_TABLE_LENGTH, 0, -MAX_TABLE_LENGTH);
		glm::vec3 pointMax(MAX_TABLE_LENGTH, 0, MAX_TABLE_LENGTH);
		aidegl::showGrid(pointMin, pointMax, 6, 6, 1.0f, glm::vec4(1, 1, 1, 1), matrices);
		glEnable(GL_DEPTH_TEST);    
	}

	// Appliquer le nuanceur
	opengl::Programme::Start(programme_);
	programme_.assignerUniforme("modelViewProjection", matrice);
	programme_.assignerUniforme("matrModel", matrices.matrModel);
	programme_.assignerUniforme("matrVisu", matrices.matrVisu);
	programme_.assignerUniforme("matrNormale", glm::transpose(glm::inverse(glm::mat3(matrices.matrModel))));
	programme_.assignerUniforme("positionCamera", matrices.positionCamera);
	programme_.assignerUniforme("colorAppliedToTexture", 1);

	int disableAmbiant = light::LightManager::obtenirInstance()->obtenirAmbiantState() ? 1 : 0;
	programme_.assignerUniforme("disableAmbiant", disableAmbiant);
	
	// TODO :  Enlever ce uniform quand la specularité va fonctionner
	programme_.assignerUniforme("disableSpeculaire", 1);

	appliquerMateriau();

	glBindVertexArray(vao_);
	// Reload Data
	glBindBuffer(GL_ARRAY_BUFFER, tampon_sommets_);
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3) * sommets_.size(), &sommets_[0], GL_DYNAMIC_DRAW);
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);

	// Dessiner la table
	glDrawElements(GL_TRIANGLES, faces_.size(), GL_UNSIGNED_INT, 0);
	glBindVertexArray(0);

	// Desactiver le nuanceur
	opengl::Programme::Stop(programme_);

	// Dessiner la ligne du centre
	opengl::Rectangle ligne(glm::vec3(0), 2.5f, glm::length(sommets_[31] - sommets_[11]) + 5.0f, glm::vec4(1, 0, 0, 1));
	ligne.afficher(matrices);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTable::afficherConcret() const
///
/// Cette fonction effectue le véritable rendu de l'objet.
///
/// @param[in] vueProjection : La matrice qui permet de 
///					transformer le modèle à sa position voulue.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::afficherConcret(const glm::mat4& vueProjection) const
{
	// Affichage du modèle.
	dessiner(vueProjection);

	// Appel à la version de la classe de base pour l'affichage des enfants.
	NoeudComposite::afficherConcret(vueProjection);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTable::effacerSelection() const
///
/// Cette fonction ne fait rien
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::effacerSelection() const
{
	// Ne rien faire, on ne veut pas effacer les points de controle
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTable::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur et effectue la bonne methode selon le type
///
/// @param[in] visiteur : Un visiteur
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::accepterVisiteur(VisiteurAbstrait* visiteur) {

	// Envoie le visiteur aux enfants
	NoeudComposite::accepterVisiteur(visiteur);
	visiteur->visiterTable(this);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudTable::appliquerMateriau() const
///
/// Cette fonction appliquer un matériau
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudTable::appliquerMateriau() const
{
	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, texture_);

	programme_.assignerUniforme("material.diffuse", materiau.diffuse_);
	programme_.assignerUniforme("material.transparence", materiau.opacite_);
	programme_.assignerUniforme("material.emission", materiau.emission_);
	programme_.assignerUniforme("material.speculaire", materiau.speculaire_);
	programme_.assignerUniforme("material.ambiant", materiau.ambiant_);
	programme_.assignerUniforme("material.shininess", 0.0f);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const std::vector<glm::vec3> NoeudTable::obtenirSommets() const
///
/// Cette fonction permet d'obtenir les sommets de la table.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
const std::vector<glm::vec3> NoeudTable::obtenirSommets() const
{
	return sommets_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const std::vector<glm::vec3> NoeudTable::obtenirSommetsPatinoire() const
///
/// Obtient les sommets composant la patinoire
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
const std::vector<glm::vec3> NoeudTable::obtenirSommetsPatinoire() const
{
	std::vector<glm::vec3> patinoire;
	patinoire.push_back(sommets_[0]);
	for (int i = 1; i < sommets_.size() - 1 ; i += 5)
	{
		patinoire.push_back(sommets_[i]);
	}

	return patinoire;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn const std::vector<NoeudBut*> NoeudTable::obtenirButs() const
///
/// Obtient les buts de la table
///
/// @return Les buts
///
////////////////////////////////////////////////////////////////////////
const std::vector<NoeudBut*> NoeudTable::obtenirButs() const
{
	return buts_;
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
