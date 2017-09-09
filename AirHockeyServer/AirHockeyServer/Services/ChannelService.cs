using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class ChannelService : IChannelService
    {
        public ChannelService(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; }

        public async Task<List<Channel>> GetChannels()
        {
            return await DataProvider.GetEntities<Channel>("");
        }
    }
}