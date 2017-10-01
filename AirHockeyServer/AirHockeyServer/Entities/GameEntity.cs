using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class GameEntity : Entity
    {
        public Guid CreatorId { get; set; }

        public DateTime CreationDate { get; set; }

        public UserEntity[] Players { get; set; }

        public GameState GameState { get; set; }
    }

    public enum GameState
    {
        Default,
        WaitingForOpponent,
        InProgress,
        Ended
    }
}