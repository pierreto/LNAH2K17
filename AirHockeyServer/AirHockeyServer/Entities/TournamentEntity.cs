using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class TournamentEntity
    {
        public int Id { get; set; }

        public List<UserEntity> Players { get; set; }

        public List<GameEntity> Games { get; set; }

        public MapEntity SelectedMap { get; set; }

        public TournamentEntity()
        {
            this.Players = new List<UserEntity>();
            this.Games = new List<GameEntity>();
        }
    }
}