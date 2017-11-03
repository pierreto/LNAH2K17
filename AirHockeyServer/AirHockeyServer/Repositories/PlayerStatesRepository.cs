using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using AirHockeyServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public class PlayerStatsRepository : Repository, IPlayerStatsRepository
    {
        private Table<StatsPoco> StatsTable;

        public PlayerStatsRepository(DataProvider dataProvider, MapperManager mapperManager)
            :base(dataProvider, mapperManager)
        {
            this.StatsTable = DataProvider.DC.GetTable<StatsPoco>();
        }

        public async Task<StatsEntity> CreatePlayerStats(StatsEntity playerStats)
        {
            try
            {
                StatsPoco newStats = MapperManager.Map<StatsEntity, StatsPoco>(playerStats);
                this.StatsTable.InsertOnSubmit(newStats);

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());

                return await GetPlayerStat(playerStats.UserId);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.CreateNewMap] " + e.ToString());
                return null;
            }
        }

        public async Task<StatsEntity> GetPlayerStat(int userId)
        {
            try
            {
                IQueryable<StatsPoco> queryable =
                    from stats in this.StatsTable where stats.UserId == userId select stats;

                var results = await Task<IEnumerable<StatsPoco>>.Run(
                    () => queryable.ToArray());

                StatsPoco result = results.Length > 0 ? results.First() : null;
                return MapperManager.Map<StatsPoco, StatsEntity>(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.GetPlayerStat] " + e.ToString());
                return null;
            }
        }

        public async Task UpdatePlayerStats(int userId, StatsEntity updatedPlayerStats)
        {
            try
            {
                StatsPoco _updatedMap = MapperManager.Map<StatsEntity, StatsPoco>(updatedPlayerStats);
                var query =
                    from stats in this.StatsTable where stats.UserId == updatedPlayerStats.UserId select stats;

                var results = query.ToArray();
                var existingStats = results.First();

                existingStats.Points = updatedPlayerStats.Points;
                existingStats.GamesWon = updatedPlayerStats.GamesWon;
                existingStats.TournamentsWon = updatedPlayerStats.TournamentsWon;

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.UpdatePlayerStats] " + e.ToString());
            }
        }

    }
}