using AirHockeyServer.Entities;
using AirHockeyServer.Events;
using AirHockeyServer.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AirHockeyServer.Services
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file MatchMakerService.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe cherche à trouver des adversaires pour chaque
    /// partie en attente de joueurs. Cette classe est statique pour permettre l'ajout
    /// de manière centralisée
    ///////////////////////////////////////////////////////////////////////////////
    public class MatchMakerService
    {
        public static event EventHandler<MatchFoundArgs> MatchFoundEvent;

        private static Mutex WaitingPlayersMutex = new Mutex();

        private static Queue<UserEntity> _WaitingPlayers;
        private static Queue<UserEntity> WaitingPlayers
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

        private static Mutex WaitingGamesMutex = new Mutex();

        // TODO : refactor needed for tournaments
        private static Queue<GameEntity> _WaitingGames;
        private static Queue<GameEntity> WaitingGames
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

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn static UserEntity GetGameOpponent()
        ///
        /// Cette fonction cherche à retourner un joueur 
        /// en attente
        /// 
        /// @return le joueur en attente trouvé
        ///
        ////////////////////////////////////////////////////////////////////////
        public static UserEntity GetGameOpponent()
        {
            WaitingPlayersMutex.WaitOne();

            if (WaitingPlayers.Count > 0)
            {
                var player = WaitingPlayers.Dequeue();
                WaitingPlayersMutex.ReleaseMutex();

                return player;
            }
            else
            {
                WaitingPlayersMutex.ReleaseMutex();
            }

            return null;
        }

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
        public static IEnumerable<UserEntity> GetTournamentOpponents()
        {
            WaitingPlayersMutex.WaitOne();

            if (WaitingPlayers.Count > 2)
            {
                var players = WaitingPlayers.Take(3);
                WaitingPlayersMutex.ReleaseMutex();

                return players;
            }
            else
            {
                WaitingPlayersMutex.ReleaseMutex();
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void AddOpponent(UserEntity user)
        ///
        /// Cette fonction ajoute un utilisateur dans la file d'attente pour jouer
        ///
        ////////////////////////////////////////////////////////////////////////
        public static void AddOpponent(UserEntity user)
        {
            WaitingPlayersMutex.WaitOne();

            WaitingPlayers.Enqueue(user);

            WaitingPlayersMutex.ReleaseMutex();

            StartPlayersMatching();
        }

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static void AddGame(GameEntity game)
        ///
        /// Cette fonction ajoute une partie en attente de joueur(s) à la liste
        /// d'attente
        ///
        ////////////////////////////////////////////////////////////////////////
        public static void AddGame(GameEntity game)
        {
            WaitingGamesMutex.WaitOne();

            WaitingGames.Enqueue(game);

            WaitingGamesMutex.ReleaseMutex();

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
        private static void StartPlayersMatching()
        {

            Thread myThread = new Thread(new ThreadStart(ExecuteMatch));
            myThread.Start();

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
        private static void ExecuteMatch()
        {
            WaitingGamesMutex.WaitOne();

            if (WaitingGames.Count > 0)
            {
                WaitingGamesMutex.ReleaseMutex();

                var opponent = GetGameOpponent();
                if (opponent != null)
                {
                    WaitingGamesMutex.WaitOne();

                    var game = WaitingGames.Dequeue();

                    WaitingGamesMutex.ReleaseMutex();

                    game.Players[1] = opponent;
                    MatchFoundEvent?.Invoke(new UserEntity(), new MatchFoundArgs { GameEntity = game });
                }
            }
            else
            {
                WaitingGamesMutex.ReleaseMutex();
            }
        }
    }
}