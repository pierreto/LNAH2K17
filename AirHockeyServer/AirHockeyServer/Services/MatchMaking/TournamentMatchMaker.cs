using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AirHockeyServer.Services.MatchMaking
{
    public class TournamentMatchMaker : MatchMaker
    {
        public event EventHandler<UserEntity> OpponentFound;

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static UserEntity GetTournamentOpponents()
        ///
        /// Cette fonction cherche à retourner trois joueurs
        /// en attente de participer à un tournoi
        /// 
        /// @return un ensemble de joueurs
        ///
        ////////////////////////////////////////////////////////////////////////
        public List<UserEntity> GetTournamentOpponents()
        {
            WaitingPlayersMutex.WaitOne();

            if (WaitingPlayers.Count >= 1)
            {
                var players = WaitingPlayers.Take(1);
                WaitingPlayersMutex.ReleaseMutex();

                return players.ToList();
            }
            else
            {
                WaitingPlayersMutex.ReleaseMutex();
            }

            return null;
        }
        
        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void ExecuteMatch()
        ///
        /// Cette fonction exécute l'algorithme pour matcher les joueurs en attente
        /// aux parties en attente de joueurs. Dans le cas qu'un match est possible,
        /// on lance un event pour avertir qu'un match a été trouvé
        ///
        ////////////////////////////////////////////////////////////////////////
        protected override void ExecuteMatch()
        {
            var opponents = GetTournamentOpponents();
            if (opponents != null && opponents.Count == 1)
            {
                OpponentFound.Invoke(this, opponents.First());
            }
        }
    }
}