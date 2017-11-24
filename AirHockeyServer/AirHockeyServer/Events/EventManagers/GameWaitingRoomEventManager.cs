using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using AirHockeyServer.Manager;
using AirHockeyServer.Services;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Services.MatchMaking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        protected const int WAITING_TIMEOUT = 15000;

        protected ConcurrentDictionary<Guid, int> RemainingTime { get; set; }

        protected ConcurrentDictionary<Guid, GameEntity> Games { get; set; }
        
        

        public IGameManager GameManager { get; private set; }
        public MapService MapService { get; set; }
        public ConnectionMapper ConnectionMapper { get; set; }

        public GameWaitingRoomEventManager(IGameManager gameManager, MapService mapService, ConnectionMapper connectionMapper)
        {
            this.RemainingTime = new ConcurrentDictionary<Guid, int>();

            GameMatchMakerService.Instance().MatchFoundEvent += OnMatchFound;

            GameManager = gameManager;
            MapService = mapService;
            ConnectionMapper = connectionMapper;
            Games = new ConcurrentDictionary<Guid, GameEntity>();
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
            GameEntity gameCreated = new GameEntity()
            {
                GameId = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                Players = new GamePlayerEntity[2] { args.PlayersMatch.PlayersMatch[0], args.PlayersMatch.PlayersMatch[1] },
                Master = args.PlayersMatch.PlayersMatch[0],
                Slave = args.PlayersMatch.PlayersMatch[1]
            };

            var stringGameId = gameCreated.GameId.ToString();
            
            foreach(var player in gameCreated.Players)
            {
                var connection = ConnectionMapper.GetConnection(player.Id);
                try
                {
                   await GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Groups.Add(connection, stringGameId);
                }
                catch(Exception e)
                {

                }
            }

            Games[gameCreated.GameId] = gameCreated;
            GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Clients.Group(stringGameId).OpponentFoundEvent(gameCreated);
            
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
        protected async void WaitingRoomTimeOut(object source, ElapsedEventArgs e, Guid gameId, System.Timers.Timer timer)
        {
            if (RemainingTime[gameId] < WAITING_TIMEOUT)
            {
                RemainingTime[gameId] += 1000;

                var remainingTime = ((WAITING_TIMEOUT - RemainingTime[gameId]) / 1000);
                GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Clients.Group(gameId.ToString()).WaitingRoomRemainingTime(remainingTime); 
            }
            else
            {
                timer.Stop();

                int mapId = 0;
                if (Games[gameId].SelectedMap == null)
                {
                    var maps = await MapService.GetMaps();
                    mapId = maps.First().Id.Value;
                }
                else
                {
                    mapId = Games[gameId].SelectedMap.Id.Value;
                }

                Games[gameId].SelectedMap = await MapService.GetMap(mapId);

                GameManager.AddGame(Games[gameId]);
                GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>().Clients.Group(gameId.ToString()).GameStartingEvent(Games[gameId]);
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
        protected System.Timers.Timer CreateTimeoutTimer(Guid gameId)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => WaitingRoomTimeOut(timerSender, e, gameId, timer);

            return timer;
        }

        public void SetMap(Guid gameId, MapEntity map)
        {
            if(Games.ContainsKey(gameId))
            {
                Games[gameId].SelectedMap = map;
            }
        }
        
    }
}
