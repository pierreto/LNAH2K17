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

///////////////////////////////////////////////////////////////////////////
/// @class EditionHub
/// @brief Classe pour gérer les appels au serveur pour l'édition en ligne
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class EditionHub: BaseHub {
    private var map: MapEntity?
    
    init(connection: SignalR?) {
        super.init()
        self.hubProxy = connection?.createHubProxy("EditionHub")
        
        /// Reception de l'évènement d'une commande
        self.hubProxy?.on("NewCommand") { args in
            print("NEW COMMAND")
            self.receiveCommand(command: args?[0] as! Dictionary<String, String>)
        }
        
        /// Reception de l'évènement quand un utilisateur rejoint une salle d'édition
        self.hubProxy?.on("NewUser") { args in
            let newUser = args?[0] as! Dictionary<String, String>
            let username = newUser["Username"]
            let hexColor = newUser["HexColor"]
            print("Joining user: \(String(describing: username! + " (" + hexColor! + ")"))\n")
            
            FacadeModele.instance.obtenirUserManager()?.addUser(username: username!, hexColor: hexColor!)
        }
        
        /// Réception de l'évènement quand un utilisateur quitte la salle d'édition
        self.hubProxy?.on("UserLeaved") { args in
            let username = args?[0] as! String
            print("Leaving user: \(String(describing: username))\n")
            
            FacadeModele.instance.obtenirUserManager()?.removeUser(username: username)
        }
    }
    
    func receiveCommand(command: Dictionary<String, String>) {
        print(command.description)
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
        var usersInRoom = [Dictionary<String, String>]()
        
        do {
            try self.hubProxy?.invoke("JoinPublicRoom",
                                      arguments: [username, convertMapEntity(mapEntity: mapEntity)])
            { (result, error) in
                if let e = error {
                    print("Error JoinPublicRoom: \(e)")
                }
                else {
                    print("Success JoinPublicRoom")
                    usersInRoom = result as! [Dictionary<String, String>]
                    print("Users in room: " + usersInRoom.description)
                    
                    for user in usersInRoom {
                        FacadeModele.instance.obtenirUserManager()?.addUser(username: user["Username"]!,
                                                                            hexColor: user["HexColor"]!)
                    }
                }
            }
        }
        catch {
            print("Error JoinPublicRoom")
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
    
    func sendEditionCommand(command: EditionCommand) {
        let jsonCommand = command.toJSON()
        
        do {
            try self.hubProxy?.invoke("SendEditionCommand", arguments: [self.map?.id.value as Any, jsonCommand])
        }
        catch {
            print("Error SendEditionCommand")
        }
    }
    
    func leaveRoom() {
        do {
            try self.hubProxy?.invoke("LeaveRoom", arguments: [self.map?.id.value as Any])
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
