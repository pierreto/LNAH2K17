using AirHockeyServer.Core;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using AirHockeyServer.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Hubs
{
    public class TournamentWaitingRoomHub : Hub
    {
        public TournamentWaitingRoomHub(ITournamentService tournamentService, ConnectionMapper connectionMapper, FriendService friendService)
        {
            TournamentService = tournamentService;
            ConnectionMapper = connectionMapper;
            FriendService = friendService;
        }
        protected FriendService FriendService { get; }
        public ITournamentService TournamentService { get; }
        public ConnectionMapper ConnectionMapper { get; set; }

        public void Join(List<GamePlayerEntity> players)
        {
            foreach (var player in players)
            {
                if (!player.IsAi)
                {
                    ConnectionMapper.AddConnection(player.Id, Context.ConnectionId);
                    Cache.AddPlayer(FriendService.UsersIdConnected.Find(x => x.Id == player.Id));
                }
            }
            TournamentService.JoinTournament(players);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<GameEntity> UpdateMap(GameEntity gameEntity)
        ///
        /// Cette fonction permet d'updater la carte de 
        /// partie et d'avertir les autres clients
        /// 
        /// @return la partie mise à jour
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UpdateMap(int tournamentId, MapEntity selectedMap)
        {
            Clients.Group(tournamentId.ToString()).TournamentMapUpdatedEvent(selectedMap);
            TournamentService.UpdateTournament(tournamentId, selectedMap);
        }

        public void LeaveTournament(GamePlayerEntity user, int tournamentId)
        {
            Groups.Remove(ConnectionMapper.GetConnection(user.Id), tournamentId.ToString());
            TournamentService.LeaveTournamentWaitingRoom(user, tournamentId);
            Cache.RemovePlayer(user.Id);
        }

        public void Disconnect(string username)
        {
            // TODO FIND A WAY TO GET USER
        }
    }
}