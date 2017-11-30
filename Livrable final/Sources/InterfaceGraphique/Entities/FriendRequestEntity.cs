using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceGraphique.Entities
{
    public enum RequestStatus { Refused = -1, Pending = 0, Accepted = 1 };

    public class FriendRequestEntity
    {
        public int? Id { get; set; }
        public UserEntity Requestor { get; set; }
        public UserEntity Friend { get; set; }
        public RequestStatus Status { get; set; }
    }
}