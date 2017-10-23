using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public enum RequestStatus { Refused=-1, Pending=0, Accepted=1};

    public class FriendRequestEntity
    {
        public int? Id { get; set; }
        public int Requestor { get; set; }
        public int Friend { get; set; }
        public RequestStatus Status { get; set; }
    }
}