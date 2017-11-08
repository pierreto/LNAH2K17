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
    
    dynamic var id: String?
    dynamic var creator: String?
    dynamic var mapName: String?
    dynamic var lastBackup: Date?
    dynamic var json: String?
    let privacy = RealmOptional<Bool>()
    dynamic var password: String?
    let currentNumberOfPlayer = RealmOptional<Int>()
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
