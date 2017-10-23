using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;
using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public class FriendService
    {
        private FriendRequestRepository FriendRepository;

        public FriendService()
        {
            FriendRepository = new FriendRequestRepository();
        }
    }
}