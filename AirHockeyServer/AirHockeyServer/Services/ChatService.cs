﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AirHockeyServer.Services
{
    public class ChatService : IChatService, IService
    {   
        public ChatService(IRequestsManager requestsManager)
        {
            RequestsManager = requestsManager;
        }
        
        public IRequestsManager RequestsManager { get; }

        public void SendPrivateMessage(ChatMessage message)
        {
            RequestsManager.SendPostRequest(message.Recipient, message);
        }

        public void SendMessageToChannel(ChatMessage message, Channel channel)
        {
            foreach(var member in channel.Members)
            {
                RequestsManager.SendPostRequest(member.IpAdress, message);
            }
        }
    }
}