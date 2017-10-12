//////////////////////////////////////////////////////////////////////////////
/// @file BancTests.h
/// @author Julien Gascon-Samson
/// @date 2011-07-16
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef _TESTS_BANCTESTS_H
#define _TESTS_BANCTESTS_H

#include "Singleton.h"

///////////////////////////////////////////////////////////////////////////
/// @class BancTests
/// @brief Banc de tests qui permet d'ex�cuter tous les tests unitaires.
///        C'est une classe singleton.
///
/// @author Julien Gascon-Samson
/// @date 2011-07-16
///////////////////////////////////////////////////////////////////////////
class BancTests : public Singleton<BancTests>
{
   SINGLETON_DECLARATION_CLASSE(BancTests);

public:
	/// Ex�cuter tous les tests unitaires
	bool executer();
};

#endif // _TESTS_BANCTESTS_H


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
