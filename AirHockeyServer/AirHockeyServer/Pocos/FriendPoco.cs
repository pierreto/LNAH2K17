using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Data.Linq;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "test_friends")]
    public class FriendPoco
    {
        private EntityRef<UserPoco> _Requestor;
        private EntityRef<UserPoco> _Friend;

        [Column(IsPrimaryKey = true, Name = "id")]
        public int Id { get; set; }

        [Column(Name = "requestor", DbType = "int(11) NOT NULL", CanBeNull = false)]
        public int RequestorID { get; set; }

        [Association(Storage = "_Requestor", Name = "test_friends_ibfk_2", ThisKey = "RequestorID", OtherKey = "Id", IsForeignKey = true)]
        public UserPoco Requestor
        {
            get { return this._Requestor.Entity; }
            set { this._Requestor.Entity = value; }
        }

        [Column(Name = "friend", DbType = "int(11) NOT NULL", CanBeNull = false)]
        public int FriendID { get; set; }

        [Association(Storage = "_Friend", Name = "test_friends_ibfk_1", ThisKey = "FriendID", OtherKey = "Id", IsForeignKey = true)]
        public UserPoco Friend
        {
            get { return this._Friend.Entity; }
            set { this._Friend.Entity = value; }
        }

        [Column(Name = "status", DbType = "tinyint(4) NOT NULL", CanBeNull = false)]
        public int Status { get; set; }
    }
}