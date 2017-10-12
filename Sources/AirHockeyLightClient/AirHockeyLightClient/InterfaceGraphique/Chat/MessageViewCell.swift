//
//  MessageViewCell.swift
//  AirHockeyLightClient
//
//  Created by Mikael Ferland Pierre To on 17-09-27.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

class MessageViewCell: UITableViewCell {
    @IBOutlet weak var sender: UILabel!
    @IBOutlet weak var messageValue: PaddingLabel!
    @IBOutlet weak var timestamp: UILabel!
}
