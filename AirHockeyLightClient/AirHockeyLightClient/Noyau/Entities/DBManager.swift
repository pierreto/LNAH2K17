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
import SwiftyJSON

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
    private var realm: Realm
    
    init() {
        let config = Realm.Configuration(
            // Set the new schema version. This must be greater than the previously used
            // version (if you've never set a schema version before, the version is 0).
            schemaVersion: 4,
            
            // Set the block which will be called automatically when opening a Realm with
            // a schema version lower than the one set above
            migrationBlock: { migration, oldSchemaVersion in
                // We haven’t migrated anything yet, so oldSchemaVersion == 0
                if (oldSchemaVersion < 4) {
                    // Nothing to do!
                    // Realm will automatically detect new properties and removed properties
                    // And will update the schema on disk automatically
                }
        });
        
        // Tell Realm to use this new configuration object for the default Realm
        Realm.Configuration.defaultConfiguration = config
        
        self.realm = try! Realm()
    }
    
    func sauvegarderCarte(map: MapEntity, json: String?) {
        // Persist map in the realm
        try! self.realm.write {
            map.json = json
            map.lastBackup = Date()
            self.realm.add(map)
        }
    }
    
    func recupererCartes() -> [MapEntity] {
        return Array(self.realm.objects(MapEntity.self))
    }
    
    func effacerCarte(mapName: String) {
        try! realm.write {
            let map = self.realm.objects(MapEntity.self).filter("mapName == %@", mapName)
            realm.delete(map)
        }
    }
    
    func effacerToutesCartes() {
        try! realm.write {
            self.realm.deleteAll()
        }
    }
    
    func isMapNameUnique(mapName: String) -> Bool {
        let maps = self.realm.objects(MapEntity.self).filter("mapName == %@", mapName)
        return maps.count == 0
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
