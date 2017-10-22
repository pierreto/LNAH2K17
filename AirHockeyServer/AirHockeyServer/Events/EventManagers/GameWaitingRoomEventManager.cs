using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using AirHockeyServer.Services.MatchMaking;
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
        protected GameEntity OfficialGame { get; set; }

        protected const int WAITING_TIMEOUT = 2000;

        protected ConcurrentDictionary<int, int> RemainingTime { get; set; }

        protected IHubContext HubContext { get; set; }
        public IGameService GameService { get; }

        public GameWaitingRoomEventManager(IGameService gameService)
        {
            this.RemainingTime = new ConcurrentDictionary<int, int>();
            GameMatchMakerService.Instance().MatchFoundEvent += OnMatchFound;
            HubContext = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
            GameService = gameService;
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
        private async void OnMatchFound(object sender, MatchFoundArgs args)
        {
            GameEntity game = new GameEntity()
            {
                CreationDate = DateTime.Now,
                Players = new UserEntity[2] { args.PlayersMatch.PlayersMatch[0], args.PlayersMatch.PlayersMatch[1] },
                Master = args.PlayersMatch.PlayersMatch[0],
                Slave = args.PlayersMatch.PlayersMatch[1]
            };

            GameEntity gameCreated = await GameService.CreateGame(game);

            var stringGameId = gameCreated.GameId.ToString();
            
            foreach(var player in gameCreated.Players)
            {
                var connection = ConnectionMapper.GetConnection(player.UserId);
                await HubContext.Groups.Add(connection, stringGameId);
            }

            OfficialGame = gameCreated;
            HubContext.Clients.Group(stringGameId).OpponentFoundEvent(gameCreated);
            
            this.RemainingTime[gameCreated.GameId] = 0;

            System.Timers.Timer timer = CreateTimeoutTimer(gameCreated.GameId);
            timer.Start();
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
        protected void WaitingRoomTimeOut(object source, ElapsedEventArgs e, int gameId, System.Timers.Timer timer)
        {
            if (RemainingTime[gameId] < WAITING_TIMEOUT)
            {
                RemainingTime[gameId] += 1000;

                // ONLY WORKING FOR GAME OR TOURNAMENT

                //Hub.Clients.Group(gameId.ToString()).WaitingRoomRemainingTime();
                var remainingTime = ((WAITING_TIMEOUT - RemainingTime[gameId]) / 1000);
                HubContext.Clients.Group(gameId.ToString()).WaitingRoomRemainingTime(remainingTime); 
            }
            else
            {
                timer.Stop();

                // TODO : get game from db
                //GameEntity game = GameService.GetGameEntityById(gameId);

                //if (game == null)
                //{
                //    return;
                //}

                //if (game.SelectedMap == null)
                //{
                //    // TODO : select default map
                //    game.SelectedMap = new MapEntity();

                //    // TODO update game on bd

                //}

                //if (game.SelectedConfiguration == null)
                //{
                //    // TODO : select default configuration
                //    game.SelectedConfiguration = new ConfigurationEntity();

                //    // TODO update game on bd
                //}

                //var Hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
                // start the game
                //Hub.Clients.Group(game.GameId.ToString()).GameStartingEvent(game);
                HubContext.Clients.Group(gameId.ToString()).GameStartingEvent(OfficialGame);
            }
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
        protected System.Timers.Timer CreateTimeoutTimer(int gameId)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => WaitingRoomTimeOut(timerSender, e, gameId, timer);

            return timer;
        }
    }
}
