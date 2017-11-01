using AirHockeyServer.Entities;
using AirHockeyServer.Services;
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
        public TournamentWaitingRoomHub( )
        {
            TournamentService = new TournamentService();
        }

        public ITournamentService TournamentService { get; }

        public void Join(UserEntity user)
        {
            // TO REMOVE, WAITING FOR AUTHENTIFICATION
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);

            TournamentService.JoinTournament(user);
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
        public void UpdateMap(TournamentEntity tournamentEntity)
        {
            Clients.Group(tournamentEntity.Id.ToString()).TournamentMapUpdatedEvent(tournamentEntity.SelectedMap);
            TournamentService.UpdateTournament(tournamentEntity);
        }

        public async Task LeaveGame(UserEntity user)
        {
            TournamentService.LeaveTournamentWaitingRoom(user);
        }

    }
}