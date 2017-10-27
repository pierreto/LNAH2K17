﻿using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using AirHockeyServer.Services.MatchMaking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace AirHockeyServer.Events.EventManagers
{
    //public abstract class WaitingRoomEventManager
    //{
        

        //protected IHubContext HubContext { get; set; }

    //    protected IGameService GameService { get; }

    //    public WaitingRoomEventManager(IGameService gameService)
    //    {
    //        //this.RemainingTime = new ConcurrentDictionary<int, int>();
    //        GameService = gameService;
    //    }

    //    ////////////////////////////////////////////////////////////////////////
    //    ///
    //    /// @fn void WaitingRoomTimeOut(object source, ElapsedEventArgs e, Guid gameId, Timer timer)
    //    ///
    //    /// Fonction appellé à chaque seconde. Vérifie si le temps est échoué pour la modification
    //    /// des paramètre de parties. Si ce n'est pas le cas, avertis les clients du temps restants. 
    //    /// Sinon, il vérifier s'il mettre par defaut la carte et la configuration et avertis
    //    /// les clients du démarrage de la partie
    //    ///
    //    ////////////////////////////////////////////////////////////////////////
    //    protected void WaitingRoomTimeOut(object source, ElapsedEventArgs e, int gameId, System.Timers.Timer timer)
    //    {
    //        if (RemainingTime[gameId] < WAITING_TIMEOUT)
    //        {
    //            RemainingTime[gameId] += 1000;

    //            // ONLY WORKING FOR GAME OR TOURNAMENT
                
    //            //Hub.Clients.Group(gameId.ToString()).WaitingRoomRemainingTime();
    //            SendRemainingTimeEvent((WAITING_TIMEOUT - RemainingTime[gameId]) / 1000);
    //        }
    //        else
    //        {
    //            timer.Stop();

    //            // TODO : get game from db
    //            //GameEntity game = GameService.GetGameEntityById(gameId);

    //            //if (game == null)
    //            //{
    //            //    return;
    //            //}

    //            //if (game.SelectedMap == null)
    //            //{
    //            //    // TODO : select default map
    //            //    game.SelectedMap = new MapEntity();

    //            //    // TODO update game on bd

    //            //}

    //            //if (game.SelectedConfiguration == null)
    //            //{
    //            //    // TODO : select default configuration
    //            //    game.SelectedConfiguration = new ConfigurationEntity();

    //            //    // TODO update game on bd
    //            //}

    //            //var Hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
    //            // start the game
    //            //Hub.Clients.Group(game.GameId.ToString()).GameStartingEvent(game);
    //            SendEndOfTimer();
    //        }
    //    }

    //    ////////////////////////////////////////////////////////////////////////
    //    ///
    //    /// @fn Timer CreateTimeoutTimer(Guid gameId)
    //    ///
    //    /// Cette fonction crée un timer qui appellera la fonction WaitingRoomTimeOut
    //    /// à chaque seconde
    //    /// 
    //    /// @return le timer créé
    //    ///
    //    ////////////////////////////////////////////////////////////////////////
    //    protected System.Timers.Timer CreateTimeoutTimer(int gameId)
    //    {
    //        System.Timers.Timer timer = new System.Timers.Timer();
    //        timer.Interval = 1000;
    //        timer.Elapsed += (timerSender, e) => WaitingRoomTimeOut(timerSender, e, gameId, timer);

    //        return timer;
    //    }

    //    //protected virtual void SendRemainingTimeEvent(int remainingTime) { }

    //    //protected virtual void SendEndOfTimer() { }
    //}
}