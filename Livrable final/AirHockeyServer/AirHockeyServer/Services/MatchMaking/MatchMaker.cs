using AirHockeyServer.Entities;
using AirHockeyServer.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AirHockeyServer.Services.MatchMaking
{
    public abstract class MatchMaker
    {
        public MatchMaker()
        {
            this.WaitingPlayers = new Queue<GamePlayerEntity>();
        }

        protected Mutex WaitingPlayersMutex = new Mutex();

        protected Queue<GamePlayerEntity> _WaitingPlayers;
        protected Queue<GamePlayerEntity> WaitingPlayers
        {
            get
            {
                if (_WaitingPlayers == null)
                {
                    _WaitingPlayers = new Queue<GamePlayerEntity>();
                }
                return _WaitingPlayers;
            }
            set
            {
                _WaitingPlayers = value;
            }
        }

        public abstract void RemoveUser(int userId);

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void AddOpponent(UserEntity user)
        ///
        /// Cette fonction ajoute un utilisateur dans la file d'attente pour jouer
        ///
        ////////////////////////////////////////////////////////////////////////
        public abstract void AddOpponent(List<GamePlayerEntity> players);

        public abstract void AddOpponent(GamePlayerEntity player);

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void StartPlayersMatching()
        ///
        /// Cette fonction lance un nouveau thread pour exécuter l'algorithme de matching
        /// en parallèle. Cette fonction est appelé à chaque fois qu'un joueur ou qu'une
        /// partie est ajouté dans la file d'attente
        ///
        ////////////////////////////////////////////////////////////////////////
        protected void StartPlayersMatching()
        {
            Thread myThread = new Thread(new ThreadStart(ExecuteMatch));
            myThread.Start();
        }

        protected abstract void ExecuteMatch();
    }
}