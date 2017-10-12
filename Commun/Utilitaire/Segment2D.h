//////////////////////////////////////////////////////////////////////////////
/// @file Segment2D.h
/// @author Julien Charbonneau
/// @date 2016-08-24
/// @version 1.0
///
/// @addtogroup utilitaire Segment2D
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef __UTILITAIRE_SEGMENT2D_H__
#define __UTILITAIRE_SEGMENT2D_H__

#include "glm\glm.hpp"

namespace utilitaire {

	class Segment2D {
	public:
		Segment2D() {};
		Segment2D(const glm::vec3& init, const glm::vec3& end, const float& offset);
		~Segment2D() {};

		bool isAbove(const glm::dvec3& point);
		bool isBelow(const glm::dvec3& point);
		glm::dvec3 projectedPos(const glm::dvec3& pos);
		float Segment2D::distance(const glm::dvec3& pos);
		glm::dvec3 correctedPosA(const glm::dvec3& pos);
		glm::dvec3 correctedPosB(const glm::dvec3& pos);

	private:
		glm::dvec3 init_;
		glm::dvec3 end_;
		glm::dvec3 segment_;
		float offset_;
	};

} // Fin du namespace utilitaire.


#endif // __UTILITAIRE_SEGMENT2D_H__


///////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////
