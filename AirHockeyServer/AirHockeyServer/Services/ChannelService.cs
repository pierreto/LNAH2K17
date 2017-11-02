using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using System.Threading.Tasks;
using AirHockeyServer.Services.Interfaces;

namespace AirHockeyServer.Services
{
    public class ChannelService : IChannelService
    {
        public ChannelService()
        {
        }

        public async Task<List<ChannelEntity>> GetChannels()
        {
            //return await DataProvider.GetEntities<ChannelEntity>("");
            return null;
        }

        public async Task<ChannelEntity> CreateChannel(ChannelEntity channel)
        {
            channel.Id = Guid.NewGuid();
            return channel;
        }
    }
}