using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IChannelService
    {
        Task<List<ChannelEntity>> GetChannels();

        Task<ChannelEntity> CreateChannel(ChannelEntity channel);
    }
}