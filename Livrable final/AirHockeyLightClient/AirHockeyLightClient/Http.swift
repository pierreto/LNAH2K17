//
//  RestService.swift
//  Projet3
//
//  Created by Jean-Marc Al-Romhein on 2017-10-11.
//  Copyright © 2017 Jean-Marc Al-Romhein. All rights reserved.
//
import Foundation

public class Http {

    static let sharedInstance = Http()
    //TODO: Make this url dynamic
    private var _urlBase:String = ""
    var urlBase: String {
        get { return self._urlBase }
        set { self._urlBase = newValue }
    }
    
    //var urlBase = "http://192.168.1.20:63056/"
    var http: String = "http://"
    var port: String = ":63056/"
 
    public func get(_ urlEntry: String, taskCallback: @escaping (Bool, AnyObject?) -> ()) {
        let url = URL(string: http + urlBase + port + urlEntry)
        let request = URLRequest(url: url!)
        let session = URLSession(configuration: URLSessionConfiguration.default)
        
        let task = session.dataTask(with: request, completionHandler: {(data, response, error) -> Void in
            if let data = data {
                let json = try? JSONSerialization.jsonObject(with: data, options: [])
                if let response = response as? HTTPURLResponse , 200...299 ~= response.statusCode {
                    taskCallback(true, json as AnyObject?)
                } else {
                    taskCallback(false, json as AnyObject?)
                }
            }
        })
        task.resume()
    }
    
    public func post(_ urlEntry: String, _ params: Any, taskCallback: @escaping (Bool) -> ()) {
        
        let headers = [
            "content-type": "application/json",
            "cache-control": "no-cache"
        ]
        
        if let postData = (try? JSONSerialization.data(withJSONObject: params, options: [])) {
            let url = URL(string: http + urlBase + port + urlEntry)
            let request = NSMutableURLRequest(url: url!,
                                              cachePolicy: .useProtocolCachePolicy,
                                              timeoutInterval: 10.0)
            request.httpMethod = "POST"
            request.allHTTPHeaderFields = headers
            request.httpBody = postData
            request.setValue("Application/json", forHTTPHeaderField: "Content-Type")
            
            let task = URLSession.shared.dataTask(with: request as URLRequest) { (data, response, error) in
                if let response = response {
                    print("Response: ", response)
                }
                if let data = data {
                    do {
                        let json = try JSONSerialization.jsonObject(with: data, options: JSONSerialization.ReadingOptions.allowFragments)
                        print("Data: ", json)
                    }catch {
                        print("Error: ", error)
                    }
                }
            }
            task.resume()
        } else {
            return
        }
    }
    
}
