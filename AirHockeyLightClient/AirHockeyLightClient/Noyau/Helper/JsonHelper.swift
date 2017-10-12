///////////////////////////////////////////////////////////////////////////////
/// @file JsonHelper.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-10-01
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import Foundation

class JsonHelper {
    
    static func convertToJsonString(jsonObject: NSMutableDictionary) -> String {
        let jsonData: NSData
        var jsonString: String?
        
        do {
            jsonData = try JSONSerialization.data(withJSONObject: jsonObject, options: JSONSerialization.WritingOptions()) as NSData
            jsonString = NSString(data: jsonData as Data, encoding: String.Encoding.utf8.rawValue)! as String
            print("json string = \(String(describing: jsonString))")
        } catch _ {
            print ("JSON Failure")
        }
        
        return jsonString!
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
