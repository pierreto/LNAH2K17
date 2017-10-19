///////////////////////////////////////////////////////////////////////////////
/// @file VisiteurAbstrait.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////
/// @class VisiteurAbstrait
/// @brief Cette classe abstraite comprend l'interface de base que doivent
///        implanter tous les visiteurs concrets
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-10
///////////////////////////////////////////////////////////////////////////
protocol VisiteurAbstrait {
    /// Fonctions obligatoires
    //func visiterAccelerateur(noeud: NoeudAccelerateur)
    //func visiterMaillet(noeud: NoeudMaillet)
    func visiterPointControl(noeud: NoeudPointControl)
    func visiterTable(noeud: NoeudTable)
    func visiterPortail(noeud: NoeudPortail)
    func visiterMur(noeud: NoeudMur)
    //func visiterRondelle(noeud: NoeudRondelle)
    //func visiterBut(noeud: NoeudBut)
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
