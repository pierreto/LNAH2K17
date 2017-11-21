///////////////////////////////////////////////////////////////////////////////
/// @file StoreItemEntity.swift
/// @author Pierre To
/// @date 2017-11-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftyJSON

///////////////////////////////////////////////////////////////////////////
/// @class StoreItemEntity
/// @brief Classe pour encapsuler les attributs d'un item du magasin
///
/// @author Pierre To
/// @date 2017-11-19
///////////////////////////////////////////////////////////////////////////
class StoreItemEntity : Entity {

    private var name : String = ""
    private var price : Int = 0
    private var description : String = ""
    private var textureName : String = ""
    private var imageUrl: String = ""
    private var id: Int = 0
    private var isGameEnabled: Bool = false
    
    func fromJSON(json: JSON) {
        self.name = json["Name"].string!
        self.price = json["Price"].int!
        self.description = json["Description"].string!
        self.textureName = json["TextureName"].string!
        self.imageUrl = json["ImageUrl"].string!
        self.id = json["Id"].int!
        self.isGameEnabled = json["IsGameEnabled"].bool!
    }
    
    func getName() -> String {
        return self.name
    }
    
    func setName(name: String) {
        self.name = name
    }

    func getPrice() -> Int {
        return self.price
    }
    
    func setPrice(price: Int) {
        self.price = price
    }
    
    func getDescription() -> String {
        return self.description
    }
    
    func setDescription(description: String) {
        self.description = description
    }
    
    func getTextureName() -> String {
        return self.textureName
    }
    
    func setTextureName(name: String) {
        self.textureName = name
    }
    
    func getImageUrl() -> String {
        return self.imageUrl
    }
    
    func setImageUrl(url: String) {
        self.imageUrl = url
    }
    
    func getId() -> Int {
        return self.id
    }
    
    func setId(id: Int) {
        self.id = id
    }
    
    func getIsGameEnabled() -> Bool {
        return self.isGameEnabled
    }
    
    func setIsGameEnabled(isGameEnabled: Bool) {
        self.isGameEnabled = isGameEnabled
    }
    
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////


