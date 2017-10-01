///////////////////////////////////////////////////////////////////////////////
/// @file ChatMessageEntity.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-24
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

///////////////////////////////////////////////////////////////////////////
/// @class ChatMessageEntity
/// @brief Classe pour encapsuler un message
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-24
///////////////////////////////////////////////////////////////////////////
class ChatMessageEntity : Entity {

    private var sender : String
    private var messageValue : String
    private var timestamp : String
    
    /// Constructeur
    init(sender : String, messageValue : String, timestamp : String) {
        self.sender = sender;
        self.messageValue = messageValue;
        self.timestamp = timestamp;
    }
    
    public func getSender() -> String {
        return self.sender
    }
    
    public func setSender(sender: String) {
        self.sender = sender
    }
        
    public func getMessageValue() -> String {
        return self.messageValue
    }
    
    public func setMessageValue(messageValue: String) {
        self.messageValue = messageValue
    }
    
    public func getTimestamp() -> String {
        return self.timestamp
    }
    
    public func setTimestamp(timestamp: String) {
        self.timestamp = timestamp
    }

}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
