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
    
    func getMap(id: String) {
        // TODO
    }
    
    func getMaps() -> JSON? {
        // TODO
        return nil
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
    
    func convertMapEntity(mapEntity: MapEntity) -> Any {
        let map = [
            "Id": mapEntity.id!,
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
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
