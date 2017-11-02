///////////////////////////////////////////////////////////////////////////////
/// @file HubManager.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-22
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftR

///////////////////////////////////////////////////////////////////////////
/// @class HubManager
/// @brief Classe singleton pour gérer la connection avec le serveur via
///        SignalR
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-22
///////////////////////////////////////////////////////////////////////////
class HubManager {
    
    /// Instance singleton de l'objet HubManager
    static let sharedConnection = HubManager()
    
    private var connection: SignalR?
    private var hubs = [BaseHub]()
    
    /// Adresse IP du serveur
    private var ipAddress: String?
    private var username: String?
    
    private var _connected: Bool?
    
    var connected: Bool? {
        get {
            return _connected
        }
        set {
            _connected = newValue
        }
    }
    
    public func getConnection() -> SignalR? {
        return self.connection
    }
    
    public func getChatHub() -> ChatHub {
        return self.hubs.first(where: { $0 is ChatHub }) as! ChatHub
    }
    
    public func getEditionHub() -> EditionHub {
        return self.hubs.first(where: { $0 is EditionHub }) as! EditionHub
    }
    
    public func getIpAddress() -> String? {
        return self.ipAddress
    }
    
    public func setIpAddress(ipAddress: String) {
        self.ipAddress = ipAddress
    }
    
    public func getUsername() -> String? {
        return self.username
    }
    
    public func setUsername(username: String) {
        self.username = username
    }
    
    ////////////////////////////////////////////////////////////////////////
    ///
    /// @fn EstablishConnection(ipAddress: String, hubName: String)
    ///
    /// Entammer une connection avec le serveur.
    ///
    /// @param[in] ipAddress : L'adresse IP du serveur
    /// @param[in] hubName : Nom du hub hébergé dans le serveur
    ///
    /// @return Aucune
    ///
    ////////////////////////////////////////////////////////////////////////
    public func EstablishConnection(ipAddress: String) {
        self.connection = SignalR("http://" + ipAddress + ":63056")
        self.AddHubs()
        
        self.connection!.starting = { print("started") }
        self.connection!.connected = { print("connected: \(String(describing: self.connection!.connectionID))") }
        self.connection!.connectionSlow = { print("connectionSlow") }
        self.connection!.reconnecting = { print("reconnecting") }
        self.connection!.reconnected = { print("reconnected") }
        self.connection!.disconnected = { print("disconnected") }
        
        self.connection!.start()
    }
    
    /// Crée les hubs et les ajoute à la connexion
    public func AddHubs() {
        self.hubs.append(ChatHub(connection: self.connection))
        self.hubs.append(EditionHub(connection: self.connection))
    }
    
    /// Déconnecter tous les hubs et terminer la connexion
    public func Logout() {
        for hub in hubs {
            hub.logout()
        }
        self.connection?.stop()
    }
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
