///////////////////////////////////////////////////////////////////////////////
/// @file IGeneralPropertiesViewModel.swift
/// @author Pierre To
/// @date 2017-10-26
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation
import PromiseKit

protocol IGeneralPropertiesViewModel {
    var coefficientFrictionError: Dynamic<String> { get }
    var coefficientRebondError: Dynamic<String> { get }
    var coefficientAccelerationError: Dynamic<String> { get }
    
    func save(coefficientFriction: String, coefficientRebond: String, coefficientAcceleration: String) -> Bool
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
