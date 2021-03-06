///////////////////////////////////////////////////////////////////////////////
/// @file IMapViewModel.swift
/// @author Mikael Ferland
/// @date 2017-10-30
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

protocol IMapViewModel {
    
    var mapNameError: Dynamic<String> { get }
    var passwordError: Dynamic<String> { get }
    var passwordConfirmationError: Dynamic<String> { get }
    var unlockPasswordError: Dynamic<String> { get }
    
    func save(name: String, isPrivate: Bool, password: String, passwordConfirmation: String) -> Bool
    func unlock(map: MapEntity, unlockPassword: String) -> Bool
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
