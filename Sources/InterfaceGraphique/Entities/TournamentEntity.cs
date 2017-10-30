using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class TournamentEntity
    {
        public int Id { get; set; }

        public List<UserEntity> Players { get; set; }

        public List<GameEntity> SemiFinals { get; set; }

        public GameEntity Final { get; set; }

        public MapEntity SelectedMap { get; set; }

        public UserEntity Winner { get; set; }

        public TournamentState State { get; set; }

        public TournamentEntity()
        {
            this.Players = new List<UserEntity>();
            this.SemiFinals = new List<GameEntity>();
        }
    }

    public enum TournamentState
    {
        Default,
        WaitingForPlayers,
        TournamentConfiguration,
        SemiFinals,
        Final,
        Done
    }
}
