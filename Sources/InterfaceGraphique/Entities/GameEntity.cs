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
            this.Players = new GamePlayerEntity[2];
            TournamentId = -1;
            Score = new int[2];
        }

        public Guid GameId { get; set; }

        public UserEntity Creator { get; set; }

        public DateTime CreationDate { get; set; }

        public int[] Score { get; set; }

        public GamePlayerEntity[] Players { get; set; }

        public GamePlayerEntity Master { get; set; }

        public GamePlayerEntity Slave { get; set; }

        public GameState GameState { get; set; }

        public MapEntity SelectedMap { get; set; }

        public GamePlayerEntity Winner { get; set; }

        public int TournamentId { get; set; }
    }

    public enum GameState
    {
        Default,
        WaitingForOpponent,
        SelectingParameters,
        InProgress,
        Ended
    }
}
