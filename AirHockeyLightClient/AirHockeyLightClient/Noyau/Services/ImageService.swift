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
        let imageData = UIImageJPEGRepresentation(image, 0.2)!
        return imageData.base64EncodedString(options: .lineLength64Characters)
    }
    
    static public func convertStrBase64ToImage(strBase64: String?) -> UIImage {
        if(strBase64 != nil && strBase64 != "null" && strBase64 != "") {
            let dataDecoded : Data? = Data(base64Encoded: strBase64!, options: .ignoreUnknownCharacters)
            
            if dataDecoded != nil {
                return UIImage(data: dataDecoded!)!
            }
            else {
                return UIImage(named: "default_profile_picture.png")!
            }
        } else {
            return UIImage(named: "default_profile_picture.png")!
        }

    }
    
    static public func cropImageToSquare(image: UIImage) -> UIImage {
        let cropFrame = CGRect(x: 517, y: 0, width: 1014, height: 1014)
        return image.crop(rect: cropFrame)!
    }
    
    static public func resizeImage(image: UIImage, newSize: CGSize) -> UIImage{
        UIGraphicsBeginImageContextWithOptions(newSize, false, 0.0);
        image.draw(in: CGRect(origin: CGPoint.zero, size: CGSize(width: newSize.width, height: newSize.height)))
        let newImage:UIImage = UIGraphicsGetImageFromCurrentImageContext()!
        UIGraphicsEndImageContext()
        return newImage
    }
    
    static public func convertBase64ToMapIcon(strBase64: String) -> UIImage {
        let baseImage = self.convertStrBase64ToImage(strBase64: strBase64)
        let croppedImage = self.cropImageToSquare(image: baseImage)
        let newSize = CGSize(width: 500, height: 500)
        let resizedImage = self.resizeImage(image: croppedImage, newSize: newSize)
        return resizedImage
    }
    
    static public func convertMapIconToBase64(icon: UIImage) -> String {
        let croppedImage = self.cropImageToSquare(image: icon)
        let newSize = CGSize(width: 500, height: 500)
        let resizedImage = self.resizeImage(image: croppedImage, newSize: newSize)
        print(resizedImage.size.width)
        print(resizedImage.size.height)
        let base64Img = self.convertImgToBase64(image: resizedImage)
        return base64Img
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
