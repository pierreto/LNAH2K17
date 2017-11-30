using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IChatService
    {
        void SendPrivateMessage(ChatMessageEntity message);

        void SendMessageToChannel(ChatMessageEntity message, ChannelEntity channel);
    }
}