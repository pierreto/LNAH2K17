using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        public ChannelRepository()
        {
        }

        public async Task<List<ChannelEntity>> GetChannels()
        {
            return new List<ChannelEntity>();
        }
        
    }
}