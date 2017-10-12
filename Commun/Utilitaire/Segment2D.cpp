//////////////////////////////////////////////////////////////////////////////
/// @file Segment2D.cpp
/// @author Julien Charbonneau
/// @date 2016-08-24
/// @version 1.0
///
/// @addtogroup utilitaire Segment2D
/// @{
//////////////////////////////////////////////////////////////////////////////

#include "Segment2D.h"
#include "AideCollision.h"
#include "glm\gtx\norm.hpp"
#include "glm\gtx\projection.hpp"
#include <cmath>

namespace utilitaire {

	Segment2D::Segment2D(const glm::vec3& init, const glm::vec3& end, const float& offset) 
	: init_(init), end_(end), offset_(offset) {
		segment_ = end - init;
	}


	bool Segment2D::isAbove(const glm::dvec3& point) {
		double ratio = glm::dot(point - init_, segment_) / glm::length2(segment_);
		glm::dvec3 pointPerpendiculaire = (1 - ratio) * init_ + ratio * end_;
		glm::dvec3 directionCollision = point - pointPerpendiculaire;
		double distance = glm::length(directionCollision);

		if (distance > offset_ && utilitaire::SIGN(directionCollision.x) == 1.0f)
			return true;

		return false;
	}


	bool Segment2D::isBelow(const glm::dvec3& point) {
		double ratio = glm::dot(point - init_, segment_) / glm::length2(segment_);
		glm::dvec3 pointPerpendiculaire = (1 - ratio) * init_ + ratio * end_;
		glm::dvec3 directionCollision = point - pointPerpendiculaire;
		double distance = glm::length(directionCollision);

		if (distance > offset_ && utilitaire::SIGN(directionCollision.x) == -1.0f)
			return true;

		return false;
	}


	glm::dvec3 Segment2D::projectedPos(const glm::dvec3& pos) {
		glm::dvec3 newPos = init_ + glm::proj(pos - init_, end_ - init_);
		glm::dvec3 minX = (init_.x < end_.x) ? init_ : end_;
		glm::dvec3 maxX = (init_.x < end_.x) ? end_ : init_;

		if (newPos.z < init_.z)
			newPos = init_;
		else if (newPos.z > end_.z)
			newPos = end_;
		else if (newPos.x < minX.x)
			newPos = minX;
		else if (newPos.x > maxX.x)
			newPos = maxX;

		return newPos;
	}


	float Segment2D::distance(const glm::dvec3& pos) {
		glm::dvec3 newPos = init_ + glm::proj(pos - init_, end_ - init_);
		glm::dvec3 minX = (init_.x < end_.x) ? init_ : end_;
		glm::dvec3 maxX = (init_.x < end_.x) ? end_ : init_;

		if (newPos.z < init_.z)
			newPos = init_;
		else if (newPos.z > end_.z)
			newPos = end_;
		else if (newPos.x < minX.x)
			newPos = minX;
		else if (newPos.x > maxX.x)
			newPos = maxX;

		return glm::length(pos - newPos);
	}


	glm::dvec3 Segment2D::correctedPosA(const glm::dvec3& pos) {
		glm::dvec3 normal = glm::normalize(glm::cross(segment_, glm::dvec3(0, 1, 0)));
		return pos + (((double)offset_ + 0.01) * normal);
	}


	glm::dvec3 Segment2D::correctedPosB(const glm::dvec3& pos) {
		glm::dvec3 normal = glm::normalize(glm::cross(segment_, glm::dvec3(0, -1, 0)));
		return pos + (((double)offset_ + 0.01) * normal);
	}
}

///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
