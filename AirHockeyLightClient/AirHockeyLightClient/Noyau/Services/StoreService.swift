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
    
    func getStoreItems() -> Promise<[StoreItemEntity]> {
        var storeItems = [StoreItemEntity]()
        
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            return Promise { fullfil, error in
                Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/store/", method: .get)
                    .responseJSON { response in
                        if let jsonValue = response.result.value {
                            print("Success: fetch store items success.")
                            let json = JSON(jsonValue)
                            
                            for jsonItem in json.array! {
                                let storeItem = StoreItemEntity()
                                storeItem.fromJSON(json: jsonItem)
                                storeItems.append(storeItem)
                            }
                            
                            fullfil(storeItems)
                        }
                        else {
                            print("Error: fetch store items element failed.")
                            fullfil(storeItems)
                        }
                }
            }
        }
        else {
            return Promise(value: storeItems)
        }
    }
    
    func getUserStoreItems() -> Promise<[StoreItemEntity]> {
        var storeItems = [StoreItemEntity]()
        
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            return Promise { fullfil, error in
                Alamofire.request("http://" + self.clientConnection.getIpAddress()! + ":63056/api/store/"
                    + self.clientConnection.getId()!.description, method: .get)
                    .responseJSON { response in
                        if let jsonValue = response.result.value {
                            print("Success: fetch user store items success.")
                            let json = JSON(jsonValue)
                            
                            for jsonItem in json.array! {
                                let storeItem = StoreItemEntity()
                                storeItem.fromJSON(json: jsonItem)
                                storeItem.setIsBoughtByUser(isBoughtByUser: true)
                                storeItems.append(storeItem)
                            }
                            
                            fullfil(storeItems)
                        }
                        else {
                            print("Error: fetch user store items element failed.")
                            fullfil(storeItems)
                        }
                }
            }
        }
        else {
            return Promise(value: storeItems)
        }
    }
    
    func buyElement(items: [StoreItemEntity], userId: Int) -> Promise<Bool> {
        if self.clientConnection.getConnection() != nil && self.clientConnection.connected! {
            return Promise { fullfil, error in
                let storeItems = self.convertStoreItemEntity(items: items)
                
                let url = "http://" + self.clientConnection.getIpAddress()! + ":63056/api/store/" + String.init(userId)
                var request = URLRequest(url: URL.init(string: url)!)
                request.httpMethod = "POST"
                request.setValue("application/json", forHTTPHeaderField: "Content-Type")
                
                request.httpBody = try! JSONSerialization.data(withJSONObject: storeItems)
                
                Alamofire.request(request)
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
    
    func convertStoreItemEntity(items: [StoreItemEntity]) -> [[String: Any]] {
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
        
        return storeItems
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
