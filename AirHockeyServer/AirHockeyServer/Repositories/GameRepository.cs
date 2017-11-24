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
    public class GameRepository : Repository, IGameRepository
    {
        private Table<GamePoco> GameTable;
        protected IMapRepository MapRepository { get; private set; }

        public GameRepository(IMapRepository mapRepository, MapperManager mapperManager) : base(mapperManager)
        {
            MapRepository = mapRepository;
        }

        public async Task<GameEntity> CreateGame(GameEntity game)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    GamePoco gameToCreate = MapperManager.Map<GameEntity, GamePoco>(game);
                    DC.GetTable<GamePoco>().InsertOnSubmit(gameToCreate);

                    await Task.Run(() => DC.SubmitChanges());

                    return await GetGame(game.GameId);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.CreateGame] " + e.ToString());
                return null;
            }
        }

        public async Task<GameEntity> GetGame(Guid gameId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<GamePoco> queryable =
                    from games in DC.GetTable<GamePoco>() where games.Id == gameId.ToString() select games;

                    var results = await Task.Run(
                        () => queryable.ToArray());

                    GamePoco gamePoco = results.Length > 0 ? results.First() : null;

                    GameEntity result = new GameEntity();

                    IEnumerable<MapEntity> maps = await MapRepository.GetMaps();
                    // TODO : UPDATE WHEN MAP DONE
                    result.SelectedMap = maps.First();
                    // TODO GET USER
                    result.Winner = new GamePlayerEntity
                    {
                        Id = gamePoco.Winner
                    };
                    result.Players[0] = new GamePlayerEntity
                    {
                        Id = gamePoco.Player1
                    };
                    result.Players[1] = new GamePlayerEntity
                    {
                        Id = gamePoco.Player2
                    };

                    result.GameId = new Guid(gamePoco.Id);

                    return result;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.GetGame] " + e.ToString());
                return null;
            }
        }

        public async Task<int> GetUserGamesNb(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<GamePoco> queryable =
                    from games in DC.GetTable<GamePoco>() where games.Player1 == userId || games.Player2 == userId select games;

                    var results = await Task.Run(
                        () => queryable.ToArray());

                    return results.Length;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[GameRepository.GetGame] " + e.ToString());
                return 0;
            }
        }


    }
}