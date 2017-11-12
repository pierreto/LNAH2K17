using AirHockeyServer.Core;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class StoreItemEntity
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Id { get; set; }

        public bool IsGameEnabled { get; set; }

        public StoreItemEntity(StoreItemPoco poco)
        {
            var entity = Cache.StoreItems[poco.Id];
            Name = entity.Name;
            IsGameEnabled = poco.IsGameEnabled;
            Description = entity.Description;
            Id = poco.Id;
            ImageUrl = entity.ImageUrl;
        }

        public StoreItemEntity()
        {

        }
    }
}