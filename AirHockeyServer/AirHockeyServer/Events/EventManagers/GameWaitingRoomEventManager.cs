using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public GameWaitingRoomEventManager()
        {
            MatchMakerService.MatchFoundEvent += OnMatchFound;
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
            var gameId = args.GameEntity.GameId.ToString();

            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();

            var connection = ConnectionMapper.GetConnection(args.GameEntity.Players[1].Id);
            hub.Groups.Add(connection, gameId);
            
            hub.Clients.Group(args.GameEntity.GameId.ToString()).OpponentFoundEvent(args.GameEntity);
        }
    }
}