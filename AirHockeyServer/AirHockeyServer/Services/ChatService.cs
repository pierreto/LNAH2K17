using System;
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
        public ChatService()
        {
        }

        public void SendPrivateMessage(ChatMessageEntity message)
        {
           // RequestsManager.SendPostRequest(message.Recipient, message);
        }

        public void SendMessageToChannel(ChatMessageEntity message, ChannelEntity channel)
        {
            //foreach(var member in channel.Members)
            //{
            //    RequestsManager.SendPostRequest(member.IpAdress, message);
            //}
        }
    }
}