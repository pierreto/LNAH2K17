using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public interface IChatRepository
    {
        void SendMessage(ChatMessage chatMessage);
    }
}