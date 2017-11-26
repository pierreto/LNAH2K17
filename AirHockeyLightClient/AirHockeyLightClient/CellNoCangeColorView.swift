//
//  CellNoCangeColorView.swift
//  AirHockeyLightClient
//
//  Created by Jean-Marc Al-Romhein on 2017-11-25.
//  Copyright © 2017 LOG3900 Équipe 03 - Les Décalés. All rights reserved.
//

import UIKit

class CellNoCangeColorView: UIView {
    var permanentBackgroundColor: UIColor? {
        didSet {
            backgroundColor = permanentBackgroundColor
        }
    }

    override var backgroundColor: UIColor? {
        didSet {
            if backgroundColor != permanentBackgroundColor {
                backgroundColor = permanentBackgroundColor
            }
        }
    }
    /*
    // Only override draw() if you perform custom drawing.
    // An empty implementation adversely affects performance during animation.
    override func draw(_ rect: CGRect) {
        // Drawing code
    }
    */

}
