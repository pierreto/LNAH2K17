///////////////////////////////////////////////////////////////////////////////
/// @file MapService.swift
/// @author Mikael Ferland
/// @date 2017-11-11
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Alamofire
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class MapService
/// @brief Classe qui permet de chercher et de sauvegarder des cartes en ligne.
///
/// @author Mikael Ferland
/// @date 2017-11-11
///////////////////////////////////////////////////////////////////////////
class MapService {
    
    private let clientConnection = HubManager.sharedConnection
    private var overridenMaps = [String]()

    func getMap(id: String) {
        // TODO
    }
    
    func getMaps(completionHandler: @escaping (JSON?, Error?) -> ()) {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps", method: .get, parameters: nil, encoding: JSONEncoding.default).responseJSON { response in
                    switch response.result {
                        case .success(let value):
                            let maps = JSON(value)
                            completionHandler(maps, nil)
                        case .failure(let error):
                            completionHandler(nil, error)
                    }
            }
        }
    }
    
    func returnFullMaps(completionHandler: @escaping (JSON?, Error?) -> ()) {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps/sync", method: .get, parameters: nil, encoding: JSONEncoding.default).responseJSON { response in
                switch response.result {
                case .success(let value):
                    let maps = JSON(value)
                    completionHandler(maps, nil)
                case .failure(let error):
                    completionHandler(nil, error)
                }
            }
        }
    }
    
    func saveMap(map: MapEntity) {
        let map = self.convertMapEntity(mapEntity: map) as! [String : Any]
        
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps/save",
                              method: .post, parameters: map, encoding: JSONEncoding.default)
                .responseJSON { response in
                    if(response.response?.statusCode != 200) {
                        print("Error: saving the map has failed.")
                    }
            }
        }
    }
    
    func syncMap(map: MapEntity) {
        let convertedMap = self.convertMapEntity(mapEntity: map) as! [String : Any]
        
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps/sync",
                              method: .post, parameters: convertedMap, encoding: JSONEncoding.default)
                .responseJSON { response in
                    switch response.result {
                    case .success(let value):
                        let mapIsOverriden = value as! Int
                        if mapIsOverriden == 1 {
                            // TODO: notify user instead
                            print(map.mapName)
                        }
                    case .failure(let error):
                        print("Error: syncing the map has failed.")
                    }
            }
        }
    }
    
    func deleteMap(map: MapEntity, completionHandler: @escaping (Bool?, Error?) -> ()) {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/maps/remove/" + map.id!,
                              method: .get, parameters: nil, encoding: JSONEncoding.default)
                .responseJSON { response in
                    completionHandler(response.response?.statusCode == 200, nil)
            }
        } else {
            DBManager.instance.effacerCarte(mapName: map.mapName!)
            completionHandler(true, nil)
        }
    }
    
    func exportLocalMapsToServer() {
        DBManager.instance.updateCreatorOfLocalMaps(creator: self.clientConnection.getUsername()!)
        
        for map in DBManager.instance.recupererCartes() {
            if map.id == nil {
                self.saveMap(map: map)
            } else {
                self.syncMap(map: map)
            }
        }
        
        DBManager.instance.effacerToutesCartes()
    }
    
    func convertMapEntity(mapEntity: MapEntity) -> Any {
        let map = [
            "Id": mapEntity.id == nil ? "" : mapEntity.id!,
            "Creator": mapEntity.creator!,
            "MapName": mapEntity.mapName!,
            "LastBackup": mapEntity.lastBackup!.description,
            "Json": mapEntity.json!,
            "Icon": mapEntity.icon!,
            "Private": mapEntity.privacy.value!.description,
            "Password": mapEntity.password?.description as Any,
            "CurrentNumberOfPlayer": mapEntity.currentNumberOfPlayer.value!.description
            ] as [String : Any]
        return map
    }
    
    func buildMapEntity(json: JSON) -> MapEntity {
        let mapEntity = MapEntity()
        mapEntity.id = json["Id"].rawString()
        mapEntity.creator = json["Creator"].rawString()
        mapEntity.mapName = json["MapName"].rawString()
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
        let date = dateFormatter.date(from: json["LastBackup"].rawString()!)
        mapEntity.lastBackup = date
        mapEntity.json = json["Json"].rawString()
        mapEntity.icon = json["Icon"].rawString()
        mapEntity.privacy.value = json["Private"].bool
        mapEntity.password = json["Password"].rawString()
        mapEntity.currentNumberOfPlayer.value = json["CurrentNumberOfPlayer"].int
        return mapEntity
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
