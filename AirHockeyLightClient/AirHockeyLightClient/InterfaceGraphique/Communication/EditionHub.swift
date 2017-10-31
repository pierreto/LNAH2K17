///////////////////////////////////////////////////////////////////////////////
/// @file IBaseHub.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import SwiftR

class EditionHub : IBaseHub {

    private var hubProxy: Hub?
    private var map: MapEntity?
    private var connection: SignalR?
    
    init() {
        let ipAddress = "127.0.0.1"
        self.connection = SignalR("http://" + ipAddress + ":63056")
    }
    
    func initializeHub() {
        self.hubProxy = self.connection?.createHubProxy("EditionHub");
    }
    
    func joinPublicRoom(mapEntity: MapEntity) {
        self.map = mapEntity
    }
    
    func logout() {
        // TODO
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
