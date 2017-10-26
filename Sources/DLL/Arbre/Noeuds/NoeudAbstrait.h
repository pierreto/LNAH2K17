///////////////////////////////////////////////////////////////////////////////
/// @file NoeudAbstrait.h
/// @author DGI-INF2990
/// @date 2007-01-24
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////
#ifndef __ARBRE_NOEUDS_NOEUDABSTRAIT_H__
#define __ARBRE_NOEUDS_NOEUDABSTRAIT_H__

#include "GL/glew.h"
#include <string>
#include <iostream>

#include "glm\glm.hpp"
#include "glm\gtc\matrix_transform.hpp"
#include "OptionsDessin.h"

#include "RigidBody.h"
#include "utilitaire.h"

/// Déclarations avancées pour contenir un pointeur vers un modèle3D et son storage
namespace modele{
	class Modele3D;
}
namespace opengl{
	class VBO;
}

class VisiteurAbstrait;


///////////////////////////////////////////////////////////////////////////
/// @class NoeudAbstrait
/// @brief Classe de base du patron composite utilisée pour créer l'arbre
///        de rendu.
///
///        Cette classe abstraite comprend l'interface de base que doivent
///        implanter tous les noeuds pouvant être présent dans l'arbre de
///        rendu.
///
/// @author DGI-2990
/// @date 2007-01-24
///////////////////////////////////////////////////////////////////////////
class NoeudAbstrait
{
public:
	/// Constructeur.
	NoeudAbstrait(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudAbstrait();

	/// Obtient le parent de ce noeud.
	inline NoeudAbstrait* obtenirParent();

	/// Obtient le parent de ce noeud (version constante).
	inline const NoeudAbstrait* obtenirParent() const;

	/// Assigne le parent de ce noeud.
	inline void assignerParent(NoeudAbstrait* parent);


	// Transformation
	/// Obtient le scale du noeud
	inline glm::vec3 obtenirScale() const;
	inline void saveScale();
	inline void revertScale();

	/// Obtenir la rotation du noeud
	inline glm::vec3 obtenirRotation() const;
	inline glm::mat4 obtenirRotationMatrice() const;

	/// Obtient la position relative du noeud.
	inline glm::vec3 obtenirPositionRelative() const;

	/// Assigne la position relative du noeud.
	inline virtual void assignerPositionRelative(const glm::vec3& positionRelative);
	/// Permet de sauver la position courante du noeud
	inline void savePosition();
	inline void revertPosition();
    
	/// Obtient la position relative du noeud.
    inline glm::mat4 obtenirMatriceTransformation();

	inline glm::mat4 obtenirMatriceRotationTranslation();

	/// Change la mise à l'échelle du noeud (overwrite l'ancienne)
	inline void deplacer(const glm::vec3& position);
	/// Applique un deplacement sur la matrice de transformation
	inline void appliquerDeplacement(const glm::vec3& position);

	/// Change la mise à l'échelle du noeud (overwrite l'ancienne)
	inline void scale(const glm::vec3& scale);
	/// Applique une mise a l'echelle sur la matrice de transformation
	inline void appliquerScale(const glm::vec3& scale);
	
	/// Change la roation du noeud (overwrite l'ancienne)
	inline void rotate(const float& angle, const glm::vec3& axes);
	/// Applique une rotation sur la matrice de transformation
	inline void appliquerRotation(const float& angle, const glm::vec3& axes);


	/// Change les axes valides pour le deplacement
	inline void assignerAxisLock(const glm::ivec3& axisLock);

	/// Obtient le type du noeud.
	inline const std::string& obtenirType() const;

	/// Écrit l'état de l'affichage du du noeud.
	inline void assignerAffiche(bool affiche);
	/// Vérifie si le noeud se fait afficher.
	inline bool estAffiche() const;

	/// Écrit l'état de la sélection du noeud.
	inline void assignerSelection(bool selectionne);
	/// Vérifie si le noeud est sélectionné.
	inline bool estSelectionne() const;
	/// Écrit si le noeud peut être sélectionné ou non.
	inline void assignerEstSelectionnable(bool selectionnable);
	/// Vérifie si le noeud est sélectionnable.
	inline bool estSelectionnable() const;
	/// Écrit si le noeud peut être enregistré ou non.
	inline void assignerEstEnregistrable(bool enregistrable);
	/// Vérifie si le noeud est enregistrable.
	inline bool estEnregistrable() const;

	/// Assigne le modèle3D et la liste d'affichage du noeud courant
	inline void assignerObjetRendu(modele::Modele3D const* modele, opengl::VBO const* liste);

	// Interface d'un noeud

	/// Calcule la profondeur de l'arbre sous le noeud courant.
	virtual unsigned int calculerProfondeur() const;

	/// Vide le noeud de ses enfants.
	virtual void vider();
	/// Efface le noeud passé en paramètre.
	virtual void effacer(const NoeudAbstrait* noeud);

	/// Cherche un noeud par le type (sur un noeud constant).
	virtual const NoeudAbstrait* chercher(const std::string& typeNoeud) const;
	/// Cherche un noeud par le type.
	virtual NoeudAbstrait* chercher(const std::string& typeNoeud);
	/// Cherche un noeud enfant selon l'indice (sur un noeud constant).
	virtual const NoeudAbstrait* chercher(unsigned int indice) const;
	/// Cherche un noeud enfant selon l'indice.
	virtual NoeudAbstrait* chercher(unsigned int indice);

	/// Ajoute un noeud enfant.
	virtual bool ajouter(NoeudAbstrait* enfant);
	/// Obtient le nombre d'enfants du noeud.
	virtual unsigned int obtenirNombreEnfants() const;

	/// Changer la sélection du noeud.
	virtual void inverserSelection();
	/// Efface les enfants sélectionnés.
	virtual void effacerSelection();
	/// Sélectionne tous les enfants de même que le noeud.
	virtual void selectionnerTout();
	/// Désélectionne tous les enfants de même que le noeud.
	virtual void deselectionnerTout();
	/// Vérifier si le noeud ou un de ses enfants est sélectionné.
	virtual bool selectionExiste() const;

	/// Change le mode d'affichage des polygones.
	virtual void changerModePolygones(bool estForce);
	/// Assigne le mode d'affichage des polygones.
	virtual void assignerModePolygones(GLenum modePolygones);
	/// Affiche le noeud.
	virtual void afficher(const glm::mat4& vueProjection) const;
	/// Affiche le noeud de manière concrète.
	virtual void afficherConcret(const glm::mat4& vueProjection) const;
	/// Anime le noeud.
	virtual void animer(float dt);

	// Patron visiteur
	virtual void accepterVisiteur(VisiteurAbstrait* visiteur) = 0;

	virtual modele::Modele3D const* obtenirModele3D();

	// Modificateur des  de dessin
	void effetFantome(const bool& activer);
	void useOtherColor(const bool& activer, glm::vec4 color = glm::vec4(1));
	void invisible(const bool& activer) { options_.invisible_ = activer; };

	// Collision
	bool enCollision() const;
	void setEnCollision(bool enCollision);
	virtual void calculerCollider() {};

	char* getUUID() const { return uuid_; };

protected:
	char* generateUUID();

	MatricesPipeline obtenirMatricePipeline() const;

	/// Type du noeud.
	std::string      type_;

	/// Mode d'affichage des polygones.
	GLenum           modePolygones_{ GL_FILL };

	/// Transformation relative du noeud.
	glm::mat4         transformationRelative_;

	/// Matrice de mise a l'echelle
	glm::mat4        scale_;
	
	/// Matrice de rotation
	glm::mat4        rotation_;
	glm::vec3		 rotationAngle_;

	/// Vecteur specifiant les axes bloques pour le deplacement
	glm::ivec3 axisLock_;

	// Attributs de sauvegarde
	glm::vec3        savedScale_;
	float			 savedRotation_;
	glm::vec3        savedPosition_;

	/// Vrai si on doit afficher le noeud.
	bool             affiche_{ true };

	/// Sélection du noeud.
	bool             selectionne_{ false };

	/// Vrai si le noeud est sélectionnable.
	bool             selectionnable_{ true };

	/// Détermine si l'objet peut être sauvegardé en XML.
	bool             enregistrable_{ true };

	/// Pointeur vers le parent.
	NoeudAbstrait*   parent_{ nullptr };

	/// Modèle 3D correspondant à ce noeud.
	modele::Modele3D const* modele_;
	/// Storage pour le dessin du modèle
	opengl::VBO const* vbo_;

	bool enCollision_;

	// Options de dessin
	OptionsDessin options_;

	char* uuid_;
};




////////////////////////////////////////////////////////////////////////
///
/// @fn inline NoeudAbstrait* NoeudAbstrait::obtenirParent()
///
/// Cette fonction retourne le pointeur vers le parent de ce noeud.
///
/// @return Le pointeur vers le parent.
///
////////////////////////////////////////////////////////////////////////
inline NoeudAbstrait* NoeudAbstrait::obtenirParent()
{
	return parent_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline const NoeudAbstrait* NoeudAbstrait::obtenirParent() const
///
/// Cette fonction retourne le pointeur constant vers le parent de ce noeud.
///
/// @return Le pointeur constant vers le parent.
///
////////////////////////////////////////////////////////////////////////
inline const NoeudAbstrait* NoeudAbstrait::obtenirParent() const
{
	return parent_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerParent( NoeudAbstrait* parent )
///
/// Cette fonction assigne le parent du noeud afin qu'il soit possible
/// de remonter dans l'arbre.
///
/// @param[in] parent : Le parent du noeud.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerParent(
	NoeudAbstrait* parent
	)
{
	parent_ = parent;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline glm::vec3 NoeudAbstrait::obtenirScale() const
///
/// Cette fonction retourne le scale du noeud
///
/// @return Les facteurs de scale en x,y,z.
///
////////////////////////////////////////////////////////////////////////
inline glm::vec3 NoeudAbstrait::obtenirScale() const {
	return glm::vec3(scale_[0].x, scale_[1].y, scale_[2].z);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::saveScale()
///
/// Cette fonction sauve le scaling du noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::saveScale()
{
	savedScale_ = obtenirScale();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::revertScale()
///
/// Cette fonction applique la position sauvee au noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::revertScale()
{
	this->scale(savedScale_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline glm::vec3 NoeudAbstrait::obtenirRotation() const
///
/// Cette fonction retourne la rotation du noeud selon les 3 axes en degrés
///
/// @return Les facteurs de rotation x,y,z
///
////////////////////////////////////////////////////////////////////////
inline glm::vec3 NoeudAbstrait::obtenirRotation() const {
	return rotationAngle_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline glm::mat4 NoeudAbstrait::obtenirRotationMatrice() const
///
/// Cette fonction retourne la matrice de rotation
///
/// @return Matrice de rotation
///
////////////////////////////////////////////////////////////////////////
inline glm::mat4 NoeudAbstrait::obtenirRotationMatrice() const
{
	return rotation_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline const glm::dvec3& NoeudAbstrait::obtenirPositionRelative() const
///
/// Cette fonction retourne la position relative du noeud par rapport
/// à son parent.
///
/// @return La position relative.
///
////////////////////////////////////////////////////////////////////////
inline glm::vec3 NoeudAbstrait::obtenirPositionRelative() const
{
	return glm::vec3(transformationRelative_[3]);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerPositionRelative( const glm::dvec3& positionRelative )
///
/// Cette fonction permet d'assigner la position relative du noeud par
/// rapport à son parent.
///
/// @param positionRelative : La position relative.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerPositionRelative(
	const glm::vec3& positionRelative
	)
{
	glm::ivec3 inverse = axisLock_;
	for (int i = 0; i < 3; ++i) {
		inverse[i] = !inverse[i];
	}
	glm::vec3 p = positionRelative * glm::vec3(axisLock_) + obtenirPositionRelative() * glm::vec3(inverse);
	transformationRelative_= glm::translate(glm::mat4(1.0f), p );
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerPositionRelative( const glm::dvec3& positionRelative )
///
/// Cette fonction sauve la position courante du noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::savePosition()
{
	savedPosition_ = obtenirPositionRelative();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::revertPosition()
///
/// Cette fonction applique la position sauvee au noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::revertPosition()
{
	deplacer(savedPosition_);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::obtenirMatriceTransformation() 
///
/// Cette fonction permet d"obtenir la matrice de transformation du noeud
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline glm::mat4 NoeudAbstrait::obtenirMatriceTransformation() 
{
	return transformationRelative_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline glm::mat4 NoeudAbstrait::obtenirMatriceRotationTranslation() 
///
/// Cette fonction permet d"obtenir la matrice de rotation * translatiom
///
/// @return Matrice de tranformation sans scaling
///
////////////////////////////////////////////////////////////////////////
inline glm::mat4 NoeudAbstrait::obtenirMatriceRotationTranslation()
{
	return glm::translate(glm::mat4(1.0f), glm::vec3(transformationRelative_[3]) * glm::vec3(axisLock_)) * rotation_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::deplacer(const glm::vec3 & position)
///
/// Cette fonction permet de changer la position relative en conservant
/// le scaling et la rotation
///
/// @param position : Nouvelle position
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::deplacer(const glm::vec3 & position)
{
	assignerPositionRelative(position);
	transformationRelative_ *= rotation_ * scale_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::appliquerDeplacement(const glm::vec3 & position)
///
/// Cette fonction permet de changer la position relative en appliquant
/// une transformation a la suite des autres changements
///
/// @param position : Nouvelle deplacement
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::appliquerDeplacement(const glm::vec3 & deplacement)
{
	transformationRelative_ = glm::translate(glm::mat4(1.0f), deplacement * glm::vec3(axisLock_)) * transformationRelative_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::scale(glm::vec3& scale)
///
/// Cette fonction permet d'appliquer une transformation de mise à l'échelle
///
/// @param transformation : Le vecteur de mise à l'échelle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::scale(const glm::vec3& scale)
{
	scale_ = glm::scale(glm::mat4(1), scale);
	// Changer la matrice de transformation relative
	assignerPositionRelative(obtenirPositionRelative());
	transformationRelative_ *= rotation_ * scale_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::appliquerScale(const glm::vec3 & scale)
///
/// Cette fonction permet de changer le scaling en appliquant
/// une transformation à la suite des autres changements
///
/// @param scale : facteur de mise a l'échelle
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::appliquerScale(const glm::vec3& scale)
{
	glm::mat4 m = glm::scale(glm::mat4(1), scale);
	transformationRelative_ = m * transformationRelative_;
	scale_ *= m;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::rotate(glm::vec3& rotation)
///
/// Cette fonction permet d'appliquer une transformation de rotation
///
/// @param angle : Angle de rotation en degre.
/// @param axes : Axe(s) sur lequel(s) s'applique la rotation.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::rotate(const float& angle, const glm::vec3& axes)
{
	rotationAngle_ = angle * axes;
	rotation_ = glm::rotate(glm::mat4(1), angle, axes);
	// Changer la matrice de transformation relative
	assignerPositionRelative(obtenirPositionRelative());
	transformationRelative_ *= rotation_ * scale_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::appliquerRotation(const float & angle, const glm::vec3 & axes)
///
/// Cette fonction permet de changer le rotation en appliquant
/// une transformation à la suite des autres changements
///
/// @param angle : angle de la rotation
/// @param axex : axes sur lesquels s'effectue la rotation
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::appliquerRotation(const float & angle, const glm::vec3 & axes)
{
	rotationAngle_ += angle;
	glm::mat4 m = glm::rotate(glm::mat4(1), angle, axes);
	transformationRelative_ = m * transformationRelative_;
	rotation_ *= m;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void assignerAxisLock(const glm::ivec3& axisLock)
///
/// Cette fonction permet de changer les axes de deplacement valides
///
/// @param axisLock : Vecteur ou la valeur 1 active l'axe pour le deplacement
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerAxisLock(const glm::ivec3& axisLock)
{
	axisLock_ = axisLock;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline const std::string& NoeudAbstrait::obtenirType() const
///
/// Cette fonction retourne une chaîne représentante le type du noeud.
///
/// @return Le type du noeud.
///
////////////////////////////////////////////////////////////////////////
inline const std::string& NoeudAbstrait::obtenirType() const
{
	return type_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerAffiche( bool affiche )
///
/// Cette fonction permet d'assigner si le noeud se fait afficher ou
/// non lorsque l'arbre de rendu se fait afficher.  Elle permet donc
/// de temporairement suspendre ou activer l'affichage d'un noeud.
///
/// @param affiche : L'état affiché ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerAffiche(bool affiche)
{
	affiche_ = affiche;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estAffiche() const
///
/// Cette fonction retourne l'état que le noeud se fait afficher ou non.
///
/// @return L'état affiché ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estAffiche() const
{
	return affiche_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerSelection( bool selectionne )
///
/// Cette fonction permet d'assigner l'état d'être sélectionné ou non du noeud.
///
/// @param selectionne : L'état sélectionné ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerSelection(bool selectionne)
{
	// Un objet non sélectionnable n'est jamais sélectionné.
	selectionne_ = (selectionne && selectionnable_);
	
	if (selectionne_)
		this->effetFantome(true);
	else
		this->effetFantome(false);	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estSelectionne() const
///
/// Cette fonction retourne l'état d'être sélectionné ou non du noeud.
///
/// @return L'état sélectionné ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estSelectionne() const
{
	// Un objet non sélectionnable n'est jamais sélectionné.
	return (selectionne_ && selectionnable_);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerEstSelectionnable( bool selectionnable )
///
/// Cette fonction permet d'assigner l'état d'être sélectionnable ou non du noeud.
///
/// @param selectionnable : L'état sélectionnable ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerEstSelectionnable(bool selectionnable)
{
	selectionnable_ = selectionnable;
	selectionne_ = selectionne_ && selectionnable_;

	if (selectionne_) 
		this->effetFantome(true);
	else 
		this->effetFantome(false);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estSelectionnable() const
///
/// Cette fonction retourne l'état d'être sélectionnable ou non du noeud.
///
/// @return L'état sélectionnable ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estSelectionnable() const
{
	return selectionnable_;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerEstEnregistrable( bool enregistrable )
///
/// Cette fonction permet d'assigner l'état d'être entregistrable ou non du noeud.
///
/// @param enregistrable : L'état enregistrable ou non.
///
/// @return Aucune
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerEstEnregistrable(bool enregistrable)
{
	enregistrable_ = enregistrable;
}


////////////////////////////////////////////////////////////////////////
///
/// @fn inline bool NoeudAbstrait::estEnregistrable() const
///
/// Cette fonction retourne l'état d'être enregistrable en XML ou non du
/// noeud.
///
/// @return L'état enregistrable ou non.
///
////////////////////////////////////////////////////////////////////////
inline bool NoeudAbstrait::estEnregistrable() const
{
	return enregistrable_;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn inline void NoeudAbstrait::assignerObjetRendu(modele::Modele3D* modele, modele::opengl_storage::OpenGL_Liste* liste)
///
/// Cette fonction assigne l'objet de rendu au modèle, c'est-à-dire son
/// modèle 3D et sa liste d'affichage
///
/// @param modele : le modèle 3D
/// @param liste : la liste d'affichage OpenGL
///
////////////////////////////////////////////////////////////////////////
inline void NoeudAbstrait::assignerObjetRendu(modele::Modele3D const* modele, opengl::VBO const* liste)
{
	modele_ = modele;
	vbo_ = liste;
}
#endif // __ARBRE_NOEUDS_NOEUDABSTRAIT_H__


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
