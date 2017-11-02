///////////////////////////////////////////////////////////////////////////////
/// @file AuthentificationViewController.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-23
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit

///////////////////////////////////////////////////////////////////////////
/// @class AuthentificationViewController
/// @brief Controlleur de la vue de l'authentification
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-23
///////////////////////////////////////////////////////////////////////////
class AuthentificationViewController: UIViewController {
    
    private let clientConnection = HubManager.sharedConnection
    
    @IBOutlet weak var usernameInput: UITextField!
    @IBOutlet weak var ipAddessInput: UITextField!
    @IBOutlet weak var usernameInvalidErrorMessage: UILabel!
    @IBOutlet weak var usernameNotUniqueErrorMessage: UILabel!
    @IBOutlet weak var ipAddressInvalidErrorMessage: UILabel!
    @IBOutlet weak var ipAddressNotConnectedErrorMessage: UILabel!
    @IBOutlet weak var connectionIndicator: UIActivityIndicatorView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.connectionIndicator.transform = CGAffineTransform(scaleX: 2, y: 2)
    }
    
    /// Enclenché lorsque l'utilisateur pèse sur la touche Enter du clavier
    @IBAction func triggerUserLogin(_ sender: Any) {
        userLogin(nil)
    }
    
    /// Enclenché lorsque l'utilisateur pèse sur le bouton de connexion
    @IBAction func userLogin(_ sender: UIButton?) {
        self.setIpAddressInputToDefaultUI()
        self.setUsernameInputToDefaultUI()
        
        let ipAddress = ipAddessInput.text!
        let username = usernameInput.text!
        
        let validSyntaxIP = self.validateIPAddress(ipAddress: ipAddress)
        let validSyntaxUsername = self.validateUsername(username: username)
        
        if validSyntaxIP {
            self.setIpAddressInputToDefaultUI()
        } else {
            self.notifyErrorInput(textField: self.ipAddessInput)
            self.ipAddressInvalidErrorMessage.isHidden = false
        }
        
        if validSyntaxUsername {
            self.setUsernameInputToDefaultUI()
        } else {
            self.notifyErrorInput(textField: self.usernameInput)
            self.usernameInvalidErrorMessage.isHidden = false
        }
        
        /// Connecter l'usager si possible
        if (validSyntaxIP && validSyntaxUsername) {
            connectToServer(ipAddress: ipAddress, username: username)
        }
    }
    
    /// Vérifier la syntaxe de l'adresse IP
    private func validateIPAddress(ipAddress: String) -> Bool {
        print("IP address: " + ipAddress)
        
        let validIpAddressRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"
        let ipMatches = ipAddress.range(of: validIpAddressRegex, options: .regularExpression)
        
        if ipAddress.isEmpty {
            print("no input!")
            return false
        } else if (ipMatches != nil) {
            print("\(String(describing: ipAddress)) is a valid IP address")
            return true
        } else if (ipAddress == "localhost") {
            print("\(ipAddress) is a valid hostname")
            return true
        } else {
            print("\(ipAddress) is not valid")
            return false
        }
    }
    
    /// Vérifier la syntaxe du nom de l'usager
    private func validateUsername(username: String) -> Bool {
        print("Username: " + username)
        
        // Usernames must have 15 characters at most
        if (username.characters.count > 15) {
            return false
        }
        
        // Usernames contain only alphanumeric characters and underscore
        let validUsernameRegex = "^[a-zA-Z0-9_]*$"
        let userNameMatches = username.range(of: validUsernameRegex, options: .regularExpression)
        
        if username.isEmpty {
            print("no input!")
            return false
        } else if (userNameMatches != nil) {
            print("\(String(describing: username)) is a valid username")
            return true
        } else {
            print("\(username) is not valid")
            return false
        }
    }
    
    /// Modifier l'apparence du input en cas d'erreur
    private func notifyErrorInput(textField: UITextField) {
        textField.shake()
        textField.text = ""
        
        let errorColor : UIColor = UIColor.red
        textField.layer.borderColor = errorColor.cgColor
        textField.layer.borderWidth = 1.0
    }
    
    /// Remettre l'apparence du input pour l'adresse IP à celle par défaut
    private func setIpAddressInputToDefaultUI() {
        self.ipAddessInput.layer.borderWidth = 0.0
        self.ipAddressInvalidErrorMessage.isHidden = true
        self.ipAddressNotConnectedErrorMessage.isHidden = true
    }
    
    /// Remettre l'apparence du input pour le nom d'usager à celle par défaut
    private func setUsernameInputToDefaultUI() {
        self.usernameInput.layer.borderWidth = 0.0
        self.usernameInvalidErrorMessage.isHidden = true
        self.usernameNotUniqueErrorMessage.isHidden = true
    }
    
    /// Établir la connexion au serveur via SignalR
    private func connectToServer(ipAddress: String, username: String) {
        /// Désactiver les inputs
        self.ipAddessInput.isEnabled = false
        self.usernameInput.isEnabled = false
        
        clientConnection.EstablishConnection(ipAddress: ipAddress, hubName: "ChatHub")
        self.connectionIndicator.startAnimating()
        
        /// Avertir l'utilisateur s'il n'est pas possible de se connecter au serveur après 5 secondes
        let timerTask = DispatchWorkItem {
            if !(HubManager.sharedConnection.getConnection().state == .connected) {
                print("Connection timeout")
                self.notifyErrorInput(textField: self.ipAddessInput)
                self.ipAddressNotConnectedErrorMessage.isHidden = false
                self.connectionIndicator.stopAnimating()
                self.ipAddessInput.isEnabled = true
                self.usernameInput.isEnabled = true
            }
        }
        
        let timer = DispatchTime.now() + 5 // start a timer
        DispatchQueue.main.asyncAfter(deadline: timer, execute: timerTask)
        
        /// Avertir l'utilisateur en cas d'erreur au moment de la connexion
        clientConnection.getConnection().error = { error in
            print("Error: (error)")
            self.notifyErrorInput(textField: self.ipAddessInput)
            self.ipAddressNotConnectedErrorMessage.isHidden = false
            self.connectionIndicator.stopAnimating()
            self.ipAddessInput.isEnabled = true
            self.usernameInput.isEnabled = true
            
            timerTask.cancel()
        }
        
        clientConnection.getConnection().connectionFailed = { error in
            print("Connection failed")
            self.notifyErrorInput(textField: self.ipAddessInput)
            self.ipAddressNotConnectedErrorMessage.isHidden = false
            self.connectionIndicator.stopAnimating()
            self.ipAddessInput.isEnabled = true
            self.usernameInput.isEnabled = true
            
            timerTask.cancel()
        }
        
        /// Transmettre un message reçu du serveur au ChatViewController
        clientConnection.getChatHub().getHub().on("ChatMessageReceived") { args in
            ChatViewController.sharedChatViewController.receiveMessage(message: args?[0] as! Dictionary<String, String>)
        }
        
        /// Connexion au serveur réussie
        clientConnection.getConnection().connected = {
            print("Connected with ip: " + ipAddress)
            self.connectionIndicator.stopAnimating()
            self.clientConnection.setIpAddress(ipAddress: ipAddress)
            self.registerUsername(ipAddress: ipAddress, username: username)
        
            timerTask.cancel()
        }
    }
    
    /// Authentifier l'utilisateur
    func registerUsername(ipAddress: String, username: String) {
        do {
            try clientConnection.getChatHub().getHub().invoke("Authenticate", arguments: [username])  { (result, error) in
                if error != nil {
                    print("Authentification error")
                    self.notifyErrorInput(textField: self.usernameInput)
                    self.usernameNotUniqueErrorMessage.isHidden = false
                    self.ipAddessInput.isEnabled = true
                    self.usernameInput.isEnabled = true
                } else {
                    if result != nil {
                        if result as! Bool {
                            print("Authentification success")
                            self.clientConnection.setUsername(username: self.usernameInput.text!)
                            self.setUsernameInputToDefaultUI()
                            
                            let storyboard = UIStoryboard(name: "Main", bundle: nil)
                            let vc = storyboard.instantiateViewController(withIdentifier: "ChannelViewController")
                            self.present(vc, animated: true, completion: nil)
                        }
                        else {
                            /// Si le nom d'usager entré n'est pas unique
                            print("Authentification error")
                            self.notifyErrorInput(textField: self.usernameInput)
                            self.usernameNotUniqueErrorMessage.isHidden = false
                            self.ipAddessInput.isEnabled = true
                            self.usernameInput.isEnabled = true
                        }
                    }
                }
            }
        }
        catch {
            print("Authentification error")
        }
        
        /*
        // Create POST request
        var request = URLRequest(url: URL(string: "http://" + ipAddress + ":63056/api/login")!)
        request.httpMethod = "POST"
        
        // Headers
        request.allHTTPHeaderFields?["Content-Type"] = "application/json"

        // Body
        let jsonObject: NSMutableDictionary = NSMutableDictionary()
        jsonObject.setValue(self.usernameInput.text, forKey: "username")
        
        let jsonString = convertToJsonString(jsonObject: jsonObject)
        request.httpBody = jsonString.data(using: .utf8)
        
        // Send request
        sendHttpRequest(request: request)
         */
    }
    
    /*
    func sendHttpRequest(request: URLRequest) {
        var responseString: String?
        
        let task = URLSession.shared.dataTask(with: request) { data, response, error in
            guard let data = data, error == nil else {
                // check for fundamental networking error
                print("error=\(String(describing: error))")
                return
            }
            
            let httpStatus = response as? HTTPURLResponse
            if httpStatus?.statusCode != 200 {
                // check for http errors
                print("statusCode should be 200, but is \(String(describing: httpStatus?.statusCode))")
                print("response = \(String(describing: response))")
            
                DispatchQueue.main.async {
                    self.notifyErrorInput(textField: self.usernameInput)
                    self.usernameNotUniqueErrorMessage.isHidden = false
                    self.ipAddessInput.isEnabled = true
                    self.usernameInput.isEnabled = true
                }
            }
            
            responseString = String(data: data, encoding: .utf8)
            print("responseString = \(String(describing: responseString))")
            
            if httpStatus?.statusCode == 200 {
                DispatchQueue.main.async {
                    self.clientConnection.setUsername(username: self.usernameInput.text!)
                    self.setUsernameInputToDefaultUI()
                
                    let storyboard = UIStoryboard(name: "Main", bundle: nil)
                    let vc = storyboard.instantiateViewController(withIdentifier: "ChannelViewController")
                    self.present(vc, animated: true, completion: nil)
                }
            }
        }
        
        task.resume()
    }
    */
    
    override var shouldAutorotate: Bool {
        return true
    }
    
    override var supportedInterfaceOrientations: UIInterfaceOrientationMask {
        if UIDevice.current.userInterfaceIdiom == .phone {
            return .allButUpsideDown
        } else {
            return .all
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Release any cached data, images, etc that aren't in use.
    }
    
    override var prefersStatusBarHidden: Bool {
        return true
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
