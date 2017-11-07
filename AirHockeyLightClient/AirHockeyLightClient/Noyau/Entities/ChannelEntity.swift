///////////////////////////////////////////////////////////////////////////////
/// @file ChannelEntity.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

///////////////////////////////////////////////////////////////////////////
/// @class ChannelEntity
/// @brief Classe pour encapsuler un canal de la messagerie
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
///////////////////////////////////////////////////////////////////////////
class ChannelEntity : Entity {
    
    public var name : String
    public var messages = [ChatMessageEntity]()
    init(name: String) {
        self.name = name;
    }
    
    public func getName() -> String {
        return self.name
    }
    
    public func setName(name: String) {
        self.name = name
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

