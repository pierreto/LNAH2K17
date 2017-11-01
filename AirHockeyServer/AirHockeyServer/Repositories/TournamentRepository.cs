using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public class TournamentRepository : Repository<TournamentEntity>
    {
        private Table<TournamentPoco> TournamentTable;

        public TournamentRepository()
        {
            this.TournamentTable = DataProvider.DC.GetTable<TournamentPoco>();
        }

        public async Task<TournamentEntity> CreateTournament(TournamentEntity tournament)
        {
            try
            {
                TournamentPoco tournamentToCreate = MapperManager.Map<TournamentEntity, TournamentPoco>(tournament);
                this.TournamentTable.InsertOnSubmit(tournamentToCreate);

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());

                return await GetTournament(tournament.Id);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.CreateGame] " + e.ToString());
                return null;
            }
        }

        public async Task<TournamentEntity> GetTournament(int tournamentId)
        {
            try
            {
                IQueryable<TournamentPoco> queryable =
                    from tournaments in this.TournamentTable where tournaments.Id == tournamentId select tournaments;

                var results = await Task<IEnumerable<TournamentPoco>>.Run(
                    () => queryable.ToArray());

                TournamentPoco result = results.Length > 0 ? results.First() : null;
                return MapperManager.Map<TournamentPoco, TournamentEntity>(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.GetGame] " + e.ToString());
                return null;
            }
        }
    }
}