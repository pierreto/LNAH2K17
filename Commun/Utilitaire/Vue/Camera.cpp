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
	/// Constructeur de la cam�ra � partir des coordonn�es cart�siennes.
	///
	/// @param[in]  position            : position de la cam�ra.
	/// @param[in]  pointVise           : point vis�.
	/// @param[in]  directionHautCamera : direction du haut de la cam�ra.
	/// @param[in]  directionHautMonde  : direction du haut du monde de la
	///                                   cam�ra.
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
	/// D�place la cam�ra dans le plan perpendiculaire � la direction vis�e
	///
	/// @param[in]  deplacementX : D�placement sur l'axe horizontal du plan de
	///                            la cam�ra.
	/// @param[in]  deplacementY : D�placement sur l'axe vertical du plan de la
	///                            cam�ra.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::deplacerXY(double deplacementX, double deplacementY)
	{
		// Vecteur normal au plan de d�placement
		glm::dvec3 normal = glm::normalize(position_ - pointVise_);
		// Vecteur normalis� vertical du plan
		glm::dvec3 axeVertical = glm::normalize(directionHaut_);
		// Vecteur normalis� horizontal du plan
		glm::dvec3 axeHorizontal = glm::normalize(glm::cross(axeVertical, normal));

		pointVise_ += deplacementX * axeHorizontal + deplacementY * axeVertical;
		position_ += deplacementX * axeHorizontal + deplacementY * axeVertical;
		gluLookAt(position_.x, position_.y, position_.z, pointVise_.x, pointVise_.y, pointVise_.z, directionHaut_.x, directionHaut_.y, directionHaut_.z);
	}


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Camera::deplacerZ( double deplacement, bool bougePointVise )
	///
	/// D�place la cam�ra dans l'axe de la direction vis�e.
	///
	/// @param[in]  deplacement    : D�placement sur l'axe de la direction vis�e
	/// @param[in]  bougePointVise : Si vrai, le point de vis� est �galement
	///                              d�plac�.
	///
	/// @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::deplacerZ(double deplacement, bool bougePointVise)
	{
		// Vecteur normal au plan de d�placement
		glm::dvec3 direction = glm::normalize(pointVise_ - position_);

		// D�placement de la position
		position_ += deplacement * direction;

		// D�placement du point vis� s'il y a lieu
		if (bougePointVise)
		{
			pointVise_ += deplacement * direction;
		}
	}


	////////////////////////////////////////////////////////////////////////////
	///
	/// @fn void Camera::tournerXY( double rotationX, double rotationY, bool empecheInversion )
	///
	/// Rotation de la cam�ra autour de sa position (et donc d�placement du
	/// point vis� en gardant la position fixe.
	///
	/// @param[in] rotationX        : Modification de l'angle de rotation du
	///                               point vis� par rapport � la position.
	/// @param[in] rotationY        : Modification de l'angle d'�l�vation du
	///                               point vis� par rapport � la position.
	/// @param[in] empecheInversion : Si vrai, la rotation n'est pas effectu�e
	///                               si elle am�nerait une inversion de la
	///                               cam�ra.
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
	/// Rotation de la cam�ra autour de son point de vis� (et donc d�placement
	/// de la position en gardant le point de vis� fixe.
	///
	/// @param[in]  rotationX        : Modification de l'angle de rotation de la
	///                                position par rapport au point de vis�.
	/// @param[in]  rotationY        : Modification de l'angle d'�l�vation de la
	///                                position par rapport au point de vis�.
	/// @param[in]  empecheInversion : Si vrai, la rotation n'est pas effectu�
	///                                si elle am�nerait une inversion de la
	///                                cam�ra.
	///
	///  @return Aucune.
	///
	////////////////////////////////////////////////////////////////////////////
	void Camera::orbiterXY(double rotationX,
		double rotationY,
		bool   empecheInversion //=true
		)
	{
		/// Direction du point vis�e vers la cam�ra
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
	/// Calcule la matrice aidant � positionner la cam�ra dans la sc�ne.
	///
	/// @return Matrice de la cam�ra.
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
