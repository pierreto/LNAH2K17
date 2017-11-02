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
        
        /// Reception de l'évènement de rejoindre une salle d'édition
        self.hubProxy?.on("NewUser") { args in
            print("NEW USER")
        }
    }
    
    func convertMapEntity(mapEntity: MapEntity) -> Any {
        let map = [
            "Id": mapEntity.id.value!.description,
            "Creator": mapEntity.creator!,
            "MapName": mapEntity.mapName!,
            "LastBackup": mapEntity.lastBackup!.description,
            "Json": mapEntity.json!,
            "Private": mapEntity.privacy.value!.description,
            "Password": mapEntity.password?.description as Any,
            "CurrentNumberOfPlayer": mapEntity.currentNumberOfPlayer.value!.description
        ] as [String : Any]
        return map
    }
    
    func joinPublicRoom(username: String, mapEntity: MapEntity) {
        self.map = mapEntity
        
        do {
            try self.hubProxy?.invoke("JoinPublicRoom", arguments: [username, convertMapEntity(mapEntity: mapEntity)])
        }
        catch {
            print("Error JoinPublicRoom")
        }
        
        self.hubProxy?.on("NewCommand") { args in
            print("new command received")
        }
    }
    
    func joinPrivateRoom(username: String, mapEntity: MapEntity, password: String) {
        self.map = mapEntity
        
        do {
            try self.hubProxy?.invoke("JoinPrivateRoom", arguments: [username, mapEntity, password])
        }
        catch {
            print("Error JoinPrivateRoom")
        }
    }
    
    func sendEditionCommand(mapId: Int, command: AnyObject) {
        // TODO : convertir command en json
        do {
            try self.hubProxy?.invoke("SendEditionCommand", arguments: [mapId, command])
        }
        catch {
            print("Error SendEditionCommand")
        }
    }
    
    func leaveRoom(mapId: Int) {
        do {
            try self.hubProxy?.invoke("LeaveRoom", arguments: [mapId])
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
