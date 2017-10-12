///////////////////////////////////////////////////////////////////////////////
/// @file Raycast.h
/// @author Julien Charbonneau
/// @date 2016-11-16
/// @version 0.3.0
///
/// @addtogroup inf2990 INF2990
/// @{
///////////////////////////////////////////////////////////////////////////////

#ifndef __RAYCAST_H__
#define __RAYCAST_H__

#include "Plan3D.h"
#include "glm\glm.hpp"

///////////////////////////////////////////////////////////////////////////
/// @class Raycast
/// @brief Classe repésentant un raycast dans l'espace virtuel
///
/// @author Julien Charbonneau
/// @date 2016-11-16
///////////////////////////////////////////////////////////////////////////
class Raycast {
public:
	/// Constructeur
	Raycast(const int& mousePosX, const int& mousePosY);
	/// Destructeur
	~Raycast() {};

	/// Retourne le point initial du raycast
	glm::dvec3 getRayStart() { return rayStart_; };
	/// Retourne le point final du raycast
	glm::dvec3 getRayEnd() { return rayEnd_; };

private :
	glm::dvec3 rayStart_;
	glm::dvec3 rayEnd_;
};

#endif

