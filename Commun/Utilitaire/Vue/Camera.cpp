///////////////////////////////////////////////////////////////////////////////
/// @file Camera.cpp
/// @author DGI
/// @date 2006-12-15
/// @version 1.0
///
/// @addtogroup utilitaire Utilitaire
/// @{
///////////////////////////////////////////////////////////////////////////////

#include "GL/glew.h"
#include "Utilitaire.h"
#include "Camera.h"
#include "glm\gtc\matrix_transform.hpp"

namespace vue {


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn Camera::Camera(const glm::dvec3& position, const glm::dvec3& pointVise, const glm::dvec3& directionHautCamera, const glm::dvec3& directionHautMonde)
	///
	/// Constructeur de la caméra à partir des coordonnées cartésiennes.
	///
	/// @param[in]  position            : position de la caméra.
	/// @param[in]  pointVise           : point visé.
	/// @param[in]  directionHautCamera : direction du haut de la caméra.
	/// @param[in]  directionHautMonde  : direction du haut du monde de la
	///                                   caméra.
	///
	/// @return Aucune (constructeur).
	///
	////////////////////////////////////////////////////////////////////////////
	Camera::Camera(const glm::dvec3& position,
		const glm::dvec3& pointVise,
		const glm::dvec3& directionHautCamera,
		const glm::dvec3& directionHautMonde
		)
		: position_{ position },
		pointVise_{ pointVise },
		directionHaut_{ directionHautCamera },
		directionHautMonde_{ directionHautMonde }
	{
	}


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Camera::deplacerXY( double deplacementX, double deplacementY )
	///
	/// Déplace la caméra dans le plan perpendiculaire à la direction visée
	///
	/// @param[in]  deplacementX : Déplacement sur l'axe horizontal du plan de
	///                            la caméra.
	/// @param[in]  deplacementY : Déplacement sur l'axe vertical du plan de la
	///                            caméra.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::deplacerXY(double deplacementX, double deplacementY)
	{
		// Vecteur normal au plan de déplacement
		glm::dvec3 normal = glm::normalize(position_ - pointVise_);
		// Vecteur normalisé vertical du plan
		glm::dvec3 axeVertical = glm::normalize(directionHaut_);
		// Vecteur normalisé horizontal du plan
		glm::dvec3 axeHorizontal = glm::normalize(glm::cross(axeVertical, normal));

		pointVise_ += deplacementX * axeHorizontal + deplacementY * axeVertical;
		position_ += deplacementX * axeHorizontal + deplacementY * axeVertical;
		gluLookAt(position_.x, position_.y, position_.z, pointVise_.x, pointVise_.y, pointVise_.z, directionHaut_.x, directionHaut_.y, directionHaut_.z);
	}


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Camera::deplacerZ( double deplacement, bool bougePointVise )
	///
	/// Déplace la caméra dans l'axe de la direction visée.
	///
	/// @param[in]  deplacement    : Déplacement sur l'axe de la direction visée
	/// @param[in]  bougePointVise : Si vrai, le point de visé est également
	///                              déplacé.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::deplacerZ(double deplacement, bool bougePointVise)
	{
		// Vecteur normal au plan de déplacement
		glm::dvec3 direction = glm::normalize(pointVise_ - position_);

		// Déplacement de la position
		position_ += deplacement * direction;

		// Déplacement du point visé s'il y a lieu
		if (bougePointVise)
		{
			pointVise_ += deplacement * direction;
		}
	}


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Camera::tournerXY( double rotationX, double rotationY, bool empecheInversion )
	///
	/// Rotation de la caméra autour de sa position (et donc déplacement du
	/// point visé en gardant la position fixe.
	///
	/// @param[in] rotationX        : Modification de l'angle de rotation du
	///                               point visé par rapport à la position.
	/// @param[in] rotationY        : Modification de l'angle d'élévation du
	///                               point visé par rapport à la position.
	/// @param[in] empecheInversion : Si vrai, la rotation n'est pas effectuée
	///                               si elle amènerait une inversion de la
	///                               caméra.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::tournerXY(double rotationX,
		double rotationY,
		bool   empecheInversion //=true
		)
	{
	}


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Camera::orbiterXY( double rotationX, double rotationY, bool empecheInversion )
	///
	/// Rotation de la caméra autour de son point de visé (et donc déplacement
	/// de la position en gardant le point de visé fixe.
	///
	/// @param[in]  rotationX        : Modification de l'angle de rotation de la
	///                                position par rapport au point de visé.
	/// @param[in]  rotationY        : Modification de l'angle d'élévation de la
	///                                position par rapport au point de visé.
	/// @param[in]  empecheInversion : Si vrai, la rotation n'est pas effectué
	///                                si elle amènerait une inversion de la
	///                                caméra.
	///
	///  @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::orbiterXY(double rotationX,
		double rotationY,
		bool   empecheInversion //=true
		)
	{
		/// Direction du point visée vers la caméra
		glm::dvec3 direction = position_ - pointVise_;

		/// Angle entre le plan et la direction
		float anglePtoD = glm::asin(glm::dot(glm::normalize(directionHautMonde_), glm::normalize(direction)));

		/// Transformation en radians
		rotationX = glm::radians(rotationX);
		rotationY = glm::radians(rotationY);
		
		/// Limitation sur la roation en X
		if (anglePtoD - rotationX > glm::radians(88.0))
			rotationX = anglePtoD - glm::radians(88.0);
		else if (anglePtoD - rotationX < 0)
			rotationX = anglePtoD;

		/// Axe de rotation
		glm::dvec3 axeX = glm::normalize(glm::cross(directionHautMonde_, direction));
		glm::dvec3 axeY = glm::normalize(directionHautMonde_);
		
		/// Transformation pour la position
		glm::dmat4 transformation(1);
		transformation = glm::rotate(transformation, rotationY, axeY);
		transformation = glm::rotate(transformation, rotationX, axeX);

		/// Nouvelle position
		direction = glm::dvec3(transformation * glm::dvec4(direction, 1.0));
		position_ = direction + pointVise_;
		/// Nouvelle direction haut
		axeX = glm::normalize(glm::cross(directionHautMonde_, direction));
		directionHaut_ = glm::cross(glm::normalize(direction), axeX);
 	}



	////////////////////////////////////////////////////////////////////////
	///
	/// @fn glm::mat4 Camera::obtenirMatrice() const
	///
	/// Calcule la matrice aidant à positionner la caméra dans la scène.
	///
	/// @return Matrice de la caméra.
	///
	////////////////////////////////////////////////////////////////////////
	glm::mat4 Camera::obtenirMatrice() const
	{
		return glm::lookAt(position_,pointVise_, directionHaut_);
	}


}; // Fin du namespace vue.


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
