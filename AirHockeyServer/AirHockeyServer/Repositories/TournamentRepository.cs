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

        protected MapRepository MapRepository { get; private set; }

        public TournamentRepository()
        {
            MapRepository = new MapRepository();
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

                TournamentPoco poco = results.Length > 0 ? results.First() : null;
                
                TournamentEntity result = new TournamentEntity();
                IEnumerable<MapEntity> maps = await MapRepository.GetMaps();
                // TODO : UPDATE WHEN MAP DONE
                result.SelectedMap = maps.First();
                // TODO GET USER
                result.Winner = new UserEntity
                {
                    Id = poco.Winner
                };
                
                result.Id = poco.Id;
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.GetGame] " + e.ToString());
                return null;
            }
        }
    }
}