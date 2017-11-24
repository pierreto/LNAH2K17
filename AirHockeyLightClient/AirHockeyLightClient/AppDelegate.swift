///////////////////////////////////////////////////////////////////////////////
/// @file AppDelegate.swift
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-10
/// @version 1
///
/// @addtogroup log3900 LOG3990
/// @{
///////////////////////////////////////////////////////////////////////////////

import UIKit
import Alamofire
import PromiseKit

///////////////////////////////////////////////////////////////////////////
/// @class AppDelegate
/// @brief Class permettant de gérer le comportement de l'application selon
///        différents états
///
/// @author Mikael Ferland et Pierre To
/// @date 2017-09-10
///////////////////////////////////////////////////////////////////////////
@UIApplicationMain
class AppDelegate: UIResponder, UIApplicationDelegate {

    var window: UIWindow?

    func application(_ application: UIApplication, didFinishLaunchingWithOptions launchOptions: [UIApplicationLaunchOptionsKey: Any]?) -> Bool {
//        let mainStoryboard: UIStoryboard = UIStoryboard(name: "Main", bundle: nil)
//        // Override point for customization after application launch.
//        let splitViewController = mainStoryboard.instantiateViewController(withIdentifier: "SplitViewController") as! UISplitViewController
//        
//        let leftNavController = splitViewController.viewControllers.first as! UINavigationController
//        let masterViewController = leftNavController.topViewController as! MasterViewController
//        let chatAreaViewController = splitViewController.viewControllers.last as! ChatAreaViewController
//        
//        let firstChannel = channels.first
//        chatAreaViewController.channel = firstChannel
//        
//        masterViewController.delegate = chatAreaViewController
//        chatAreaViewController.delegate = masterViewController
        return true
    }

    func applicationWillResignActive(_ application: UIApplication) {
        // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
        // Use this method to pause ongoing tasks, disable timers, and invalidate graphics rendering callbacks. Games should use this method to pause the game.
        print("applicationWillResignActive")
    }

    func applicationDidEnterBackground(_ application: UIApplication) {
        // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
        // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
        
        // App does not support background running
        print("applicationDidEnterBackground")
        
        _ = HubManager.sharedConnection.DisconnectUser().then(execute: { response -> Void in
            _ = HubManager.sharedConnection.StopConnection().then(execute: { response -> Void in
                print("applicationDidEnterBackground success")
            })
        })
    }

    func applicationWillEnterForeground(_ application: UIApplication) {
        // Called as part of the transition from the background to the active state; here you can undo many of the changes made on entering the background.
        print("applicationWillEnterForeground")
    }

    func applicationDidBecomeActive(_ application: UIApplication) {
        // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
        print("applicationDidBecomeActive")
    }

    func applicationWillTerminate(_ application: UIApplication) {
        // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
        print("applicationWillTerminate")

        _ = HubManager.sharedConnection.DisconnectUser().then(execute: { response -> Void in
            _ = HubManager.sharedConnection.StopConnection().then(execute: { response -> Void in
                print("applicationWillTerminate success")
            })
        })
    }
    
}

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
