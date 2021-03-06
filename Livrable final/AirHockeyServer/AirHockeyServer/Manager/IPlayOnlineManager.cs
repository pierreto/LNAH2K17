﻿using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Manager
{
    public interface IPlayOnlineManager
    {
        void AddGame(GameEntity game);

        void GoalScored(Guid gameId, int playerId);

        Task GameEnded(Guid gameId);

        Task PlayerLeaveLiveTournament(int userId);

        Task PlayerLeaveLiveGame(int userId);

        GameEntity CreateTournamentGame(GamePlayerEntity player1, GamePlayerEntity player2, TournamentEntity tournament);

        void AddTournament(TournamentEntity tournament);
    }
}
