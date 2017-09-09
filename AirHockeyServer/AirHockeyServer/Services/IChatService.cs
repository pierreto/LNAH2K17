using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public interface IChatService
    {
        void SendPrivateMessage(ChatMessage message);

        void SendMessageToChannel(ChatMessage message, Channel channel);
    }
}