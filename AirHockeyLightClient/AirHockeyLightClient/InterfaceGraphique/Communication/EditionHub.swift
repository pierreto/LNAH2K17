///////////////////////////////////////////////////////////////////////////////
/// @file EditionHub.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftR

class EditionHub: BaseHub {
    private var map: MapEntity?
    
    init(connection: SignalR?) {
        super.init()
        self.hubProxy = connection?.createHubProxy("EditionHub")
    }
    
    func joinPublicRoom(mapEntity: MapEntity) {
        self.map = mapEntity
        
        do {
            try self.hubProxy?.invoke("JoinPublicRoom", arguments: [mapEntity])
        }
        catch {
            print("Error JoinPublicRoom")
        }
        
        self.hubProxy?.on("NewCommand") { args in
            print("new command received")
        }
    }
    
    func joinPrivateRoom(mapEntity: MapEntity, password: String) {
        self.map = mapEntity
        
        do {
            try self.hubProxy?.invoke("JoinPrivateRoom", arguments: [mapEntity, password])
        }
        catch {
            print("Error JoinPrivateRoom")
        }
    }
    
    func sendEditorCommand(command: AnyObject) {
        // TODO : convertir command en json
        do {
            try self.hubProxy?.invoke("SendEditionCommand", arguments: ["map_id", "command_string"])
        }
        catch {
            print("Error SendEditionCommand")
        }
    }
    
    func leaveRoom() {
        do {
            try self.hubProxy?.invoke("LeaveRoom", arguments: ["map_id"])
        }
        catch {
            print("Error LeaveRoom")
        }
    }
    
    override func logout() {
        // TODO
        print("logout")
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
