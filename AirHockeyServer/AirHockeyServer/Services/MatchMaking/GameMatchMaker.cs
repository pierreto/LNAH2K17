﻿using AirHockeyServer.Entities;
using AirHockeyServer.Events;
using AirHockeyServer.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AirHockeyServer.Services.MatchMaking
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
    public class GameMatchMaker : MatchMaker
    {
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
        public List<GamePlayerEntity> GetGameOpponents()
        {
            WaitingPlayersMutex.WaitOne();

            if (WaitingPlayers.Count > 1)
            {
                List<GamePlayerEntity> users = new List<GamePlayerEntity>()
                {
                    WaitingPlayers.Dequeue(),
                    WaitingPlayers.Dequeue()
                };

                WaitingPlayersMutex.ReleaseMutex();

                return users;
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
            var opponents = GetGameOpponents();
            if (opponents != null)
            {
                PlayersMatchEntity match = new PlayersMatchEntity()
                {
                    PlayersMatch = opponents
                };

                InvokeMatchFound(match);
            }
        }

        protected void ExecuteMatch(List<GamePlayerEntity> players)
        {
            PlayersMatchEntity match = new PlayersMatchEntity()
            {
                PlayersMatch = players
            };
            InvokeMatchFound(match);
        }

        public void CreateMatch(UserEntity recipient, UserEntity sender)
        {
            var users = new List<GamePlayerEntity> { new GamePlayerEntity(recipient), new GamePlayerEntity(sender) };
            Thread myThread = new Thread(new ThreadStart(() => ExecuteMatch(users)));
            myThread.Start();
        }
    }
}