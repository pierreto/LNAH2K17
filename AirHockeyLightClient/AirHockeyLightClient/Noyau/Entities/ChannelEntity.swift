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
    public var isPrivate: Bool
    public var profile: String
    public var isJoinable: Bool
    public var privateUserId: Int
    public var hasUnreadMessage: Bool
    public var messages = [ChatMessageEntity]()
    init(name: String, isPrivate: Bool = false, isJoinable: Bool = false, privateUserId: Int = 0, profile: String = "") {
        self.name = name;
        self.hasUnreadMessage = false
        self.isPrivate = isPrivate
        self.isJoinable = isJoinable
        self.privateUserId = privateUserId
        self.profile = profile
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

