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
        public event EventHandler<MatchFoundArgs> MatchFoundEvent;

        protected Mutex WaitingPlayersMutex = new Mutex();

        protected Queue<UserEntity> _WaitingPlayers;
        protected Queue<UserEntity> WaitingPlayers
        {
            get
            {
                if (_WaitingPlayers == null)
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

        public void RemoveUser(int userId)
        {
            _WaitingPlayers = new Queue<UserEntity>(_WaitingPlayers.Where(x => x.Id != userId));
        }

        protected void InvokeMatchFound(PlayersMatchEntity match)
        {
            MatchFoundEvent?.Invoke(new UserEntity(), new MatchFoundArgs { PlayersMatch = match });
        }

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void AddOpponent(UserEntity user)
        ///
        /// Cette fonction ajoute un utilisateur dans la file d'attente pour jouer
        ///
        ////////////////////////////////////////////////////////////////////////
        public void AddOpponent(UserEntity user)
        {
            WaitingPlayersMutex.WaitOne();

            WaitingPlayers.Enqueue(user);

            WaitingPlayersMutex.ReleaseMutex();

            StartPlayersMatching();
        }

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void StartPlayersMatching()
        ///
        /// Cette fonction lance un nouveau thread pour exécuter l'algorithme de matching
        /// en parallèle. Cette fonction est appelé à chaque fois qu'un joueur ou qu'une
        /// partie est ajouté dans la file d'attente
        ///
        ////////////////////////////////////////////////////////////////////////
        private void StartPlayersMatching()
        {
            Thread myThread = new Thread(new ThreadStart(ExecuteMatch));
            myThread.Start();
        }

        protected abstract void ExecuteMatch();
    }
}