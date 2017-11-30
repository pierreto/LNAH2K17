//
//  ConnectServerViewModel.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-10-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//
import PromiseKit
import Foundation

class ConnectServerViewModel: NSObject, IConnectServerViewModel {

    let connectServerModel: ConnectServer
    
    let ipAddressError: Dynamic<String>
    
    init(connectServerModel: ConnectServer) {
        self.connectServerModel = connectServerModel
        self.ipAddressError = Dynamic(connectServerModel.ipAddressError)
        super.init()
        subscribeToNotifications()
    }
    
    func connect(ipAddress: String) -> Promise<Bool> {
        return connectServerModel.validateIp(ipAddress: ipAddress)
    }
    
    fileprivate func subscribeToNotifications() {
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(connectServerPressed(_:)),
                                               name: NSNotification.Name(rawValue: ConnectServerNotification.ConnectServerNotification),
                                               object: connectServerModel)
    }
    
    @objc fileprivate func connectServerPressed(_ notification: NSNotification){
        self.ipAddressError.value = connectServerModel.ipAddressError
    }
}
