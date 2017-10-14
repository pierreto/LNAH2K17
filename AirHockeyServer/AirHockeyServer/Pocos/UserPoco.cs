using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "test_users")]
    public class UserPoco : Poco
    {
        [Column(IsPrimaryKey = true, Name = "id_user")]
        public override int? Id { get; set; }

        [Column(Name = "username")]
        public string Username { get; set; }

    }
}


