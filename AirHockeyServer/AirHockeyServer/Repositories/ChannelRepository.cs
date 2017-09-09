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
        public ChannelRepository(IChannelService ChannelService)
        {
            this.ChannelService = ChannelService;
        }

        public IChannelService ChannelService { get; }

        public async Task<List<Channel>> GetChannels()
        {
            return await ChannelService.GetChannels();
        }
        
    }
}