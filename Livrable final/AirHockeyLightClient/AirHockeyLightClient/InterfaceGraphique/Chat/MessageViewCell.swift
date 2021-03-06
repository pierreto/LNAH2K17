///////////////////////////////////////////////////////////////////////////////
/// @file MessageViewCell.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-27
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class MessageViewCell
/// @brief Cellule personnalisée d'un UI table pour afficher les messages
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-27
///////////////////////////////////////////////////////////////////////////
class MessageViewCell: UITableViewCell {
    @IBOutlet weak var sender: UILabel!
    @IBOutlet weak var messageValue: PaddingLabel!
    @IBOutlet weak var timestamp: UILabel!
    @IBOutlet var leadingConstraint: NSLayoutConstraint!
    @IBOutlet var bubbleConstraint: NSLayoutConstraint!
    @IBOutlet weak var messageContainer: UIView!
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
