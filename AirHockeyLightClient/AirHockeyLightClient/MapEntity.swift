///////////////////////////////////////////////////////////////////////////////
/// @file MapEntity.swift
/// @author Mikael Ferland
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import RealmSwift

///////////////////////////////////////////////////////////////////////////
/// @class MapEntity
/// @brief Classe pour encapsuler une carte
///
/// @author Mikael Ferland
/// @date 2017-10-01
///////////////////////////////////////////////////////////////////////////
class MapEntity : Object {
    
    dynamic var name: String?
    dynamic var creationDate: String?
    dynamic var creator: String?
    dynamic var type: String?
    dynamic var json: String?
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
