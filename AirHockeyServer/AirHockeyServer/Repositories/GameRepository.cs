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
    public class GameRepository : Repository<GameEntity>, IGameRepository
    {
        private Table<GamePoco> GameTable;

        public GameRepository()
        {
            this.GameTable = DataProvider.DC.GetTable<GamePoco>();
        }

        public async Task<GameEntity> CreateGame(GameEntity game)
        {
            try
            {
                GamePoco gameToCreate = MapperManager.Map<GameEntity, GamePoco>(game);
                this.GameTable.InsertOnSubmit(gameToCreate);

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());

                return await GetGame(game.GameId);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.CreateGame] " + e.ToString());
                return null;
            }
        }

        public async Task<GameEntity> GetGame(int gameId)
        {
            try
            {
                IQueryable<GamePoco> queryable =
                    from games in this.GameTable where games.Id == gameId select games;

                var results = await Task<IEnumerable<GamePoco>>.Run(
                    () => queryable.ToArray());

                GamePoco result = results.Length > 0 ? results.First() : null;
                return MapperManager.Map<GamePoco, GameEntity>(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.GetGame] " + e.ToString());
                return null;
            }
        }
    }
}