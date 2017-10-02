﻿using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;

namespace AirHockeyServer.Events.EventManagers
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file GameWaitingRoomEventManager.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe permet de gérer les évènements relatifs à la préparation
    /// d'une partie en ligne
    ///////////////////////////////////////////////////////////////////////////////
    public class GameWaitingRoomEventManager
    {
        private const int waitingRoomTimeoutTime = 15000;

        //private static Mutex RemainingTimeMutex = new Mutex();

        private ConcurrentDictionary<Guid, int> RemainingTime { get; set; }

        private IHubContext HubContext { get; set; }

        public GameWaitingRoomEventManager()
        {
            MatchMakerService.MatchFoundEvent += OnMatchFound;
            this.RemainingTime = new ConcurrentDictionary<Guid, int>();
            HubContext = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void OnMatchFound(object sender, MatchFoundArgs args)
        ///
        /// Cette fonction est appelé lorsqu'un évènement du type "MatchFoundEvent"
        /// est lancé. Il doit donc récupérer les opposants d'un match et les 
        /// avertir qu'un adversaire leur a été attribué.
        ///
        ////////////////////////////////////////////////////////////////////////
        private void OnMatchFound(object sender, MatchFoundArgs args)
        {

            Thread.Sleep(3000);
            var stringGameId = args.GameEntity.GameId.ToString();
            
            var connection = ConnectionMapper.GetConnection(args.GameEntity.Players[1].Id);
            HubContext.Groups.Add(connection, stringGameId);

            HubContext.Clients.Group(args.GameEntity.GameId.ToString()).OpponentFoundEvent(args.GameEntity);

            //RemainingTimeMutex.WaitOne();
            this.RemainingTime[args.GameEntity.GameId] = 0;
            //RemainingTimeMutex.ReleaseMutex();

            System.Timers.Timer timer = CreateTimeoutTimer(args.GameEntity.GameId);
            timer.Start();
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn Timer CreateTimeoutTimer(Guid gameId)
        ///
        /// Cette fonction crée un timer qui appellera la fonction WaitingRoomTimeOut
        /// à chaque seconde
        /// 
        /// @return le timer créé
        ///
        ////////////////////////////////////////////////////////////////////////
        private System.Timers.Timer CreateTimeoutTimer(Guid gameId)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => WaitingRoomTimeOut(timerSender, e, gameId, timer);

            return timer;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void WaitingRoomTimeOut(object source, ElapsedEventArgs e, Guid gameId, Timer timer)
        ///
        /// Fonction appellé à chaque seconde. Vérifie si le temps est échoué pour la modification
        /// des paramètre de parties. Si ce n'est pas le cas, avertis les clients du temps restants. 
        /// Sinon, il vérifier s'il mettre par defaut la carte et la configuration et avertis
        /// les clients du démarrage de la partie
        ///
        ////////////////////////////////////////////////////////////////////////
        private void WaitingRoomTimeOut(object source, ElapsedEventArgs e, Guid gameId, System.Timers.Timer timer)
        {
            if(RemainingTime[gameId] < waitingRoomTimeoutTime)
            {
                RemainingTime[gameId] += 1000;

                IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
                hub.Clients.Group(gameId.ToString()).WaitingRoomRemainingTime((waitingRoomTimeoutTime - RemainingTime[gameId]) / 1000);
            }
            else
            {
                timer.Stop();

                // TODO : get game from db
                GameEntity game = new GameEntity();

                if (game == null)
                {
                    return;
                }

                if (game.SelectedMap == null)
                {
                    // TODO : select default map
                    game.SelectedMap = new MapEntity();

                    // TODO update game on bd

                }

                if (game.SelectedConfiguration == null)
                {
                    // TODO : select default configuration
                    game.SelectedConfiguration = new ConfigurationEntity();

                    // TODO update game on bd
                }

                // start the game
                HubContext.Clients.Group(game.GameId.ToString()).GameStartingEvent(game);
            }
        }
    }
}