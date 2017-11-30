///////////////////////////////////////////////////////////////////////////////
/// @file AudioService.swift
/// @author Pierre To
/// @date 2017-11-12
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import AVFoundation

/// Les différents sons du mode d'édition
enum EDITION_SOUND : String {
    case HUD = "HUD"
    case SELECTION1 = "Selection1"
    case SELECTION2 = "Selection2"
    case TRANSFORM = "Transform"
    case DUPLICATE = "Duplicate"
    case OBJECT1 = "Object1"
    case OBJECT2 = "Object2"
    case SAVE = "Save"
    case DELETE = "Delete"
    case ERROR = "Error"
}

///////////////////////////////////////////////////////////////////////////
/// @class AudioService
/// @brief Classe qui permet de jouer des sons. Implémente le singleton.
///
/// @author Pierre To
/// @date 2017-11-12
///////////////////////////////////////////////////////////////////////////
class AudioService {
    
    /// Instance singleton
    static let instance = AudioService()
    
    private var player: AVAudioPlayer?
    
    /// Joue le son à partir du nom du fichier (sans extension)
    func playSound(soundName: String) {
        guard let url = Bundle.main.url(forResource: soundName, withExtension: "wav") else { return }
        
        do {
            try AVAudioSession.sharedInstance().setCategory(AVAudioSessionCategoryPlayback)
            try AVAudioSession.sharedInstance().setActive(true)
            
            player = try AVAudioPlayer(contentsOf: url)
            guard let player = player else { return }
            
            player.play()
        }
        catch let error
        {
            print(error.localizedDescription)
        }
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
