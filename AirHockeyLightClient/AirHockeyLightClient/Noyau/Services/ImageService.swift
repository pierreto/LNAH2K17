///////////////////////////////////////////////////////////////////////////////
/// @file StoreService.swift
/// @author Pierre To
/// @date 2017-11-19
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

extension UIImage {
    func crop(rect: CGRect) -> UIImage? {
        var scaledRect = rect
        scaledRect.origin.x *= scale
        scaledRect.origin.y *= scale
        scaledRect.size.width *= scale
        scaledRect.size.height *= scale
        guard let imageRef: CGImage = cgImage?.cropping(to: scaledRect) else {
            return nil
        }
        return UIImage(cgImage: imageRef, scale: scale, orientation: imageOrientation)
    }
}

///////////////////////////////////////////////////////////////////////////
/// @class StoreService
/// @brief Classe qui permet de faire les requêtes du magasin
///
/// @author Pierre To
/// @date 2017-11-19
///////////////////////////////////////////////////////////////////////////
class ImageService {
    
    static public func convertImgToBase64(image: UIImage) -> String {
        let imageData = UIImagePNGRepresentation(image)!
        return imageData.base64EncodedString(options: .lineLength64Characters)
    }
    
    static public func convertStrBase64ToImage(strBase64: String) -> UIImage {
        let dataDecoded : Data = Data(base64Encoded: strBase64, options: .ignoreUnknownCharacters)!
        return UIImage(data: dataDecoded)!
    }
    
    static public func cropImageToSquare(rect: CGRect) {
        
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
