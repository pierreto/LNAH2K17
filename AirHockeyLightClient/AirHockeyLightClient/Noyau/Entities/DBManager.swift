///////////////////////////////////////////////////////////////////////////////
/// @file DBManager.swift
/// @author Mikael Ferland
/// @date 2017-10-29
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import RealmSwift

///////////////////////////////////////////////////////////////////////////
/// @class DBManager
/// @brief Classe qui assure la sauvegarde des cartes localement via Realm.
///
/// @author Mikael Ferland
/// @date 2017-10-29
///////////////////////////////////////////////////////////////////////////
class DBManager {
    
    /// Instance singleton
    static var instance = DBManager()
    
    // Base de données locale
    private let realm = try! Realm()
    
    func sauvegarderCarte(map : MapEntity) {
        // Persist map in the realm
        try! self.realm.write {
            self.realm.add(map)
        }
    }
    
    func recupererCartes() -> [MapEntity] {
        return Array(self.realm.objects(MapEntity.self))
    }
    
    func effacerCarte(mapName: String) {
        try! realm.write {
            let map = self.realm.objects(MapEntity.self).filter("name == %@", mapName)
            realm.delete(map)
        }
    }
    
    func isMapNameUnique(mapName: String) -> Bool {
        let maps = self.realm.objects(MapEntity.self).filter("name == %@", mapName)
        return maps.count == 0
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
