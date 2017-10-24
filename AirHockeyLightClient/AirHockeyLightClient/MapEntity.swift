///////////////////////////////////////////////////////////////////////////////
/// @file MapEntity.swift
/// @author Mikael Ferland
/// @date 2017-10-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

///////////////////////////////////////////////////////////////////////////
/// @class MapEntity
/// @brief Classe pour encapsuler une carte
///
/// @author Mikael Ferland
/// @date 2017-10-01
///////////////////////////////////////////////////////////////////////////
class MapEntity : Entity {
    
    private var name: String
    private var creationDate: String?
    private var creator: String?
    private var type: String?
    private var json: String?
    
    init(name: String) {
        self.name = name;
    }
    
    public func getName() -> String {
        return self.name
    }
    
    public func setName(name: String) {
        self.name = name
    }
    
    public func getJson() -> String {
        return self.json!
    }
    
    public func setJson(name: String) {
        self.json = name
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
