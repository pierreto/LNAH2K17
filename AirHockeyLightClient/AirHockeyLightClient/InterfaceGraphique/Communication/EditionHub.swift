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
import SwiftyJSON

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
            let strArgs = args?[0] as! String
            let dataFromString = strArgs.data(using: .utf8, allowLossyConversion: false)
            let command = JSON(data: dataFromString!)
            self.receiveCommand(command: command)
        }
        
        /// Reception de l'évènement quand un utilisateur rejoint une salle d'édition
        self.hubProxy?.on("NewUser") { args in
            let newUser = args?[0] as! Dictionary<String, Any>
            let username = newUser["Username"]
            let hexColor = newUser["HexColor"]
            
            print("Joining user: \(String(describing: (username as! String) + " (" + (hexColor as! String) + ")"))\n")
            
            FacadeModele.instance.obtenirUserManager()?.addUser(username: username as! String, hexColor: hexColor as! String)
        }
        
        /// Réception de l'évènement quand un utilisateur quitte la salle d'édition
        self.hubProxy?.on("UserLeaved") { args in
            let username = args?[0] as! String
            print("Leaving user: \(String(describing: username))\n")
            
            FacadeModele.instance.obtenirUserManager()?.removeUser(username: username)
        }
    }
    
    func receiveCommand(command: JSON) {
        let type = EDITION_COMMAND(rawValue: command["$type"].string!)!
        let editionCommand: EditionCommand

        switch (type) {
            case .BOOST_COMMAND :
                print ("Boost command")
                editionCommand = BoostCommand(objectUuid: command["ObjectUuid"].string!)
                break
            case .WALL_COMMAND :
                print ("Wall command")
                editionCommand = WallCommand(objectUuid: command["ObjectUuid"].string!)
                break
            case .PORTAL_COMMAND :
                print ("Portal command")
                editionCommand = PortalCommand(objectUuid: command["ObjectUuid"].string!)
                break
            case .SELECTION_COMMAND :
                print ("Selection command")
                editionCommand = SelectionCommand(objectUuid: command["ObjectUuid"].string!)
                break
            case .TRANSFORM_COMMAND :
                // print ("Transform command")
                editionCommand = TransformCommand(objectUuid: command["ObjectUuid"].string!)
                break
            case .CONTROLPOINT_COMMAND :
                // print ("Control point command")
                editionCommand = ControlPointCommand(objectUuid: command["ObjectUuid"].string!)
                break
        }
        
        editionCommand.fromJSON(json: command)
        editionCommand.executeCommand()
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
        var usersInRoom = [Dictionary<String, Any>]()
        
        do {
            try self.hubProxy?.invoke("JoinPublicRoom",
                                      arguments: [username, convertMapEntity(mapEntity: mapEntity)])
            { (result, error) in
                if let e = error {
                    print("Error JoinPublicRoom: \(e)")
                }
                else {
                    print("Success JoinPublicRoom")
                    usersInRoom = result as! [Dictionary<String, Any>]
                    print("Users in room: " + usersInRoom.description)
                    
                    for user in usersInRoom {
                        if user["Username"] as? String == HubManager.sharedConnection.getUsername() {
                            FacadeModele.instance.setCurrentUserColor(userHexColor: user["HexColor"] as! String)
                        }
                        
                        let username = user["Username"] as! String
                        FacadeModele.instance.obtenirUserManager()?.addUser(username: username,
                                                                            hexColor: user["HexColor"] as! String)
                        
                        if (user["UuidsSelected"] != nil) {
                            // Un utilisateur dans la salle a déjà des noeuds sélectionnés
                            for uuid in user["UuidsSelected"] as! [String] {
                                FacadeModele.instance.selectNode(username: username, uuid: uuid, isSelected: true, deselectAll: false)
                            }
                        }
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
        let jsonCommand = command.toJSON()?.rawString(options: [])
        //print(jsonCommand!)
        
        do {
            try self.hubProxy?.invoke("SendEditionCommand", arguments: [self.map?.id.value as Any, jsonCommand!])
        }
        catch {
            print("Error SendEditionCommand")
        }
    }
    
    func sendSelectionCommand(command: SelectionCommand) {
        let convertedCommand = command.toDictionary()
        
        do {
            try self.hubProxy?.invoke("SendSelectionCommand", arguments: [self.map?.id.value as Any, convertedCommand])
        }
        catch {
            print("Error SendSelectionCommand")
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
