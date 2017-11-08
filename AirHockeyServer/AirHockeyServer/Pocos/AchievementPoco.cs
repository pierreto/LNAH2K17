using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "achievements")]
    public class AchievementPoco 
    {
        [Column(Name = "user_id", IsPrimaryKey = true)]
        public int UserId { get; set; }

        [Column(Name = "achievement_type", IsPrimaryKey = true)]
        public string AchievementType { get; set; }

        [Column(Name = "is_enabled")]
        public bool IsEnabled { get; set; }
    }
}