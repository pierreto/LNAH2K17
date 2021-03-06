﻿using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task CreateGame(GameEntity game);

        Task<int> GetUserGamesNb(int userId);
    }
}
