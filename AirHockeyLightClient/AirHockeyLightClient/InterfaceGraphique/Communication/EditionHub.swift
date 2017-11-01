///////////////////////////////////////////////////////////////////////////////
/// @file IBaseHub.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftR

class EditionHub : IBaseHub {

    private var hubProxy: Hub?
    private var map: MapEntity?
    private var connection: SignalR?
    
    init() {
        let ipAddress = "127.0.0.1"
        self.connection = SignalR("http://" + ipAddress + ":63056")
    }
    
    func initializeHub() {
        self.hubProxy = self.connection?.createHubProxy("EditionHub")
        self.connection?.addHub(self.hubProxy!)
        self.connection?.start()
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
    
    func logout() {
        // TODO
        print("logout")
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
