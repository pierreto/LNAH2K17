using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "test_users")]
    public class UserPoco
    {
        [Column(IsPrimaryKey = true, Name = "id_user")]
        public int Id { get; set; }

        [Column(Name = "username")]
        public string Username { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "email")]
        public string Email { get; set; }

        [Column(Name = "created")]
        public string Created { get; set; }

        [Column(Name = "profile", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public string Profile { get; set; }

        [Column(Name = "alreadyPlayedGame")]
        public bool AlreadyPlayedGame { get; set; }

        [Column(Name = "alreadyUsedFatEditor")]
        public bool AlreadyUsedFatEditor { get; set; }

        [Column(Name = "alreadyUsedLightEditor")]
        public bool AlreadyUsedLightEditor { get; set; }
    }
}