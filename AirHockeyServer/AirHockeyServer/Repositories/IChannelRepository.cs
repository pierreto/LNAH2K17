using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public interface IChannelRepository
    {
        Task<List<Channel>> GetChannels();
    }
}