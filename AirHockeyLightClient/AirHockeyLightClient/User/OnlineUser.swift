///////////////////////////////////////////////////////////////////////////////
/// @file OnlineUser.swift
/// @author Pierre To
/// @date 2017-11-04
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class OnlineUser
/// @brief Classe qui représente un utilisateur en ligne
///
/// @author Pierre To
/// @date 2017-11-04
///////////////////////////////////////////////////////////////////////////
class OnlineUser {
    
    private var username : String
    private var hexColor : UIColor
    private var uuidsSelected = [String]() // TODO : Agir avec les uuids deja sélectionné (lorsqu'on join une room)
    private var nodesSelected: [NoeudCommun]
    
    init(username: String, hexColor: String) {
        self.username = username
        self.hexColor = MathHelper.hexToUIColor(hex: hexColor)
        self.nodesSelected = [NoeudCommun]()
    }
    
    public func getUsername() -> String {
        return self.username
    }
    
    public func setUsername(username: String) {
        self.username = username
    }
    
    public func getHexColor() -> UIColor {
        return self.hexColor
    }
    
    public func setHexColor(hexColor: UIColor) {
        self.hexColor = hexColor
    }
    
    public func getUuidsSelected() -> [String] {
        return self.uuidsSelected
    }
    
    public func getNodesSelected() -> [NoeudCommun] {
        return self.nodesSelected
    }
    
    public func select(uuid: String) {
        let node = self.getNodeFromRenderTree(uuid: uuid)
        
        if node != nil {
            node?.assignerSelectionneByAnotherUser(estSelectionneByAnotherUser: true)
            node?.useOtherColor(activer: true, color: self.hexColor)
            self.nodesSelected.append(node!)
        }
    }
    
    public func deselect(uuid: String) {
        let index = self.nodesSelected.index(where: { $0.obtenirUUID() == uuid })
        
        if (index != nil) {
            let node = self.nodesSelected[index!]
        
            node.assignerSelectionneByAnotherUser(estSelectionneByAnotherUser: false)
            node.useOtherColor(activer: false)
        
            self.nodesSelected.remove(at: index!)
        }
    }
    
    public func findNode(uuid: String) -> NoeudCommun? {
        let index = self.nodesSelected.index(where: { $0.obtenirUUID() == uuid })
        
        if index != nil {
            return self.nodesSelected[index!]
        }
        else {
            return nil
        }
    }
    
    public func deselectAll() {
        for node in self.nodesSelected {
            node.assignerSelectionneByAnotherUser(estSelectionneByAnotherUser: false)
            node.useOtherColor(activer: false)
        }
        self.nodesSelected.removeAll()
    }
    
    private func getNodeFromRenderTree(uuid: String) -> NoeudCommun? {
        let visiteur = VisiteurByUUID(wantedUUID: uuid)
        FacadeModele.instance.obtenirArbreRendu().accepterVisiteur(visiteur: visiteur)
        return visiteur.getNode()
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
