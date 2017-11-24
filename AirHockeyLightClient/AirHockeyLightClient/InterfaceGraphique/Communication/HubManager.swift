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
enum ConnectionNotification {
    static let Connection = "Connection"
}

class HubManager {
    
    /// Instance singleton
    static let sharedConnection = HubManager()
    
    private var connection: SignalR?
    private var hubs = [BaseHub]()
    private var user = UserEntity()
    private var ipAddress: String?
    
    private var _connected: Bool? = false
    
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
    
    public func getFriendsHub() -> FriendsHub {
        return self.hubs.first(where: { $0 is FriendsHub }) as! FriendsHub
    }
    
    public func getUser() -> UserEntity {
        return self.user
    }
    
    public func getIpAddress() -> String? {
        return self.ipAddress
    }
    
    public func setIpAddress(ipAddress: String) {
        self.ipAddress = ipAddress
    }
    
    public func getUsername() -> String? {
        return self.user.getUsername()
    }
    
    public func setUsername(username: String) {
        self.user.setUsername(username: username)
    }
    
    public func getId() -> Int? {
        return self.user.getId()
    }
    
    public func setId(id: Int) {
        self.user.setId(id: id)
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
        self.hubs.append(FriendsHub(connection: self.connection))
    }

    /// Déconnecter tous les hubs et terminer la connexion
    public func Logout() {
        for hub in hubs {
            hub.logout()
        }
        self.connection?.stop()
        //TODO user = nil ????
        self.user.setUsername(username: "")
        NotificationCenter.default.post(name: Notification.Name(rawValue: LoginNotification.LogoutNotification), object: nil)
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////

