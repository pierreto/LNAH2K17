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
    public class TournamentWaitingRoomHub : BaseHub
    {
        public TournamentWaitingRoomHub(ITournamentService tournamentService, ConnectionMapper connectionMapper, FriendService friendService)
            : base(connectionMapper)
        {
            TournamentService = tournamentService;
            FriendService = friendService;
        }
        protected FriendService FriendService { get; }

        public ITournamentService TournamentService { get; }

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
            string connection = ConnectionMapper.GetConnection(user.Id);
            if (!string.IsNullOrEmpty(connection))
            {
                Groups.Remove(connection, tournamentId.ToString());
            }

            TournamentService.LeaveTournament(user.Id);
            Cache.RemovePlayer(user.Id);
        }

        public override void Disconnect()
        {
            var userId = ConnectionMapper.GetIdFromConnection(Context.ConnectionId);
            TournamentService.LeaveTournament(userId);
            base.Disconnect();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Disconnect();
            return base.OnDisconnected(stopCalled);
        }
    }
}