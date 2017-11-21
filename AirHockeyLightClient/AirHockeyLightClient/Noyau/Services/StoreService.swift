///////////////////////////////////////////////////////////////////////////////
/// @file StoreService.swift
/// @author Pierre To
/// @date 2017-11-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Alamofire
import PromiseKit
import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class StoreService
/// @brief Classe qui permet de faire les requêtes du magasin
///
/// @author Pierre To
/// @date 2017-11-19
///////////////////////////////////////////////////////////////////////////
class StoreService {

    private let clientConnection = HubManager.sharedConnection
    
    func buyElement(items: [StoreItemEntity], userId: Int) -> Promise<Bool> {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            return Promise { fullfil, error in
                let storeItems = self.convertStoreItemEntity(items: items)
                
                Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/store/" + String.init(userId), method: .post,
                                  parameters: storeItems, encoding: JSONEncoding.default)
                    .responseJSON { response in
                        if response.response?.statusCode == 200 {
                            print("Success: buy element success.")
                            fullfil(true)
                        }
                        else {
                            print("Error: buy element failed.")
                            fullfil(false)
                        }
                }
            }
        }
        else {
            return Promise(value: false)
        }
    }
    
    func convertStoreItemEntity(items: [StoreItemEntity]) -> [String: Any] {
        var storeItems = [[String: Any]]()

        for item in items {
            let storeItem = [
                "Name": item.getName(),
                "Price": item.getPrice(),
                "Description": item.getDescription(),
                "TextureName": item.getTextureName(),
                "ImageUrl": item.getImageUrl(),
                "Id": item.getId(),
                "IsGameEnabled": item.getIsGameEnabled()
                ] as [String : Any]
            storeItems.append(storeItem)
        }
        
        return JSON(storeItems).dictionary!
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
