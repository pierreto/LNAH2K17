﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class GameEntity
    {
        public GameEntity()
        {
            this.Players = new UserEntity[2];

        }

        public int GameId { get; set; }

        public UserEntity Creator { get; set; }

        public DateTime CreationDate { get; set; }
        public UserEntity[] Players { get; set; }

        public UserEntity Master { get; set; }
        public UserEntity Slave { get; set; }


        public GameState GameState { get; set; }

        public MapEntity SelectedMap { get; set; }
    }

    public enum GameState
    {
        Default,
        WaitingForOpponent,
        InProgress,
        Ended
    }
}
