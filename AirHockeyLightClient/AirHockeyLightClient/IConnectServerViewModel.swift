//
//  IConnectServerViewModel.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-10-18.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//
import PromiseKit
import Foundation

protocol IConnectServerViewModel {
    var ipAddressError: Dynamic<String> { get }
    func connect(ipAddress: String) -> Promise<Bool>
}
