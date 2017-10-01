using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public class MatchMakerService
    {
        private static Queue<UserEntity> _WaitingPlayers;
        private Queue<UserEntity> WaitingPlayers
        {
            get
            {
                if(_WaitingPlayers == null)
                {
                    _WaitingPlayers = new Queue<UserEntity>();
                }
                return _WaitingPlayers;
            }
            set
            {
                _WaitingPlayers = value;
            }
        }

        private static Queue<GameEntity> _WaitingGames;
        private Queue<GameEntity> WaitingGames
        {
            get
            {
                if (_WaitingGames == null)
                {
                    _WaitingGames = new Queue<GameEntity>();
                }
                return _WaitingGames;
            }
            set
            {
                _WaitingGames = value;
            }
        }

        public UserEntity GetGameOpponent()
        {
            if (WaitingPlayers.Count > 0)
            {
                return WaitingPlayers.Dequeue();
            }

            return null;
        }

        public IEnumerable<UserEntity> GetTournamentOpponents()
        {
            if (WaitingPlayers.Count > 2)
            {
                return WaitingPlayers.Take(3); 
            }

            return null;
        }
    }
}