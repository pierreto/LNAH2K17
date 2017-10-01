//
//  ChatMessageEntity.swift
//  AirHockeyLightClient
//
//  Created by Pierre To on 17-09-24.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import Foundation

class ChatMessageEntity : Entity{
    private var Sender : String
    private var MessageValue : String
    private var TimeStamp : Date
    
    public func getSender() -> String {
        return self.Sender
    }
    
    public func setSender(sender: String) {
        self.Sender = sender
    }
        
    public func getMessageValue() -> String {
        return self.MessageValue
    }
    
    public func setMessageValue(messageValue: String) {
        self.MessageValue = messageValue
    }
    
    public func getTimeStamp() -> Date {
        return self.TimeStamp
    }
    
    public func setTimeStamp(timeStamp: Date) {
        self.TimeStamp = timeStamp
    }
    
    init(sender : String, messageValue : String, timeStamp : Date) {
        self.Sender = sender;
        self.MessageValue = messageValue;
        self.TimeStamp = timeStamp;
    }
}
