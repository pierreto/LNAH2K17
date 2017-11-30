using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name ="user_items")]
    public class StoreItemPoco
    {
        [Column(IsPrimaryKey = true, Name = "item_id")]
        public int Id { get; set; }

        [Column(IsPrimaryKey = true, Name = "user_id")]
        public int UserId { get; set; }

        [Column(Name = "is_enabled")]
        public bool IsGameEnabled { get; set; }
        
    }
}