using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Data.Linq;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "test_friends")]
    public class FriendPoco : Poco
    {
        public override int? Id { get; set; }

        [Column(Name = "requestor", DbType = "int(11) NOT NULL", CanBeNull = false)]
        public int Requestor { get; set; }

        [Association(Name = "test_friends_ibfk_2", ThisKey = "Requestor", IsForeignKey = true)]
        public EntityRef<UserPoco> RequestorUserPoco;

        [Column(Name = "friend", DbType = "int(11) NOT NULL", CanBeNull = false)]
        public int Friend { get; set; }

        [Association(Name = "test_friends_ibfk_1", ThisKey = "Friend", IsForeignKey = true)]
        public EntityRef<UserPoco> FriendUserPoco;

        [Column(Name = "status", DbType = "tinyint(4) NOT NULL", CanBeNull = false)]
        public int Status { get; set; }
    }
}