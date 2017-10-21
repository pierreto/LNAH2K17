using System;
using System.Collections.Generic;
using System.Linq;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services.MatchMaking;

namespace AirHockeyServer.Services
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file GameService.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe gère les actions relatives à un match en ligne
    ///////////////////////////////////////////////////////////////////////////////
    public class GameService : IGameService
    {
        private List<GameEntity> games;
        private static IGameRepository GameRepository = new GameRepository();

        public GameService()
        {
            this.games = new List<GameEntity>();
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<GameEntity> CreateGame(GameEntity gameEntity)
        ///
        /// Cette fonction gère la création d'une partie en ligne. Elle
        /// commence par créer la partie dans la bd puis ajoute la partie
        /// pour que le "MatchMakerService" puisse trouver un (des) adversaire(s).
        /// 
        /// @return Id du match créé
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<GameEntity> CreateGame(GameEntity gameEntity)
        {
            gameEntity.GameId = new Random().Next();
            this.games.Add(gameEntity);

            return gameEntity;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn void JoinGame(UserEntity userEntity)
        ///
        /// Cette fonction permet d'ajouter l'utilisateur pour que le "MatchMakerService"
        /// puisse trouver un (des) adversaire(s)
        ///
        ////////////////////////////////////////////////////////////////////////
        public void JoinGame(UserEntity userEntity)
        {
            GameMatchMakerService.Instance().AddOpponent(userEntity);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn async Task<GameEntity> UpdateGame(GameEntity gameEntity)
        ///
        /// Cette fonction permet d'updater une partie dans la base de données
        /// 
        /// @return la partie mise à jour
        ///
        ////////////////////////////////////////////////////////////////////////
        public async Task<GameEntity> UpdateGame(GameEntity gameEntity)
        {
            // update game bd
            return gameEntity;
        }

        public GameEntity GetGameEntityById(int id)
        {
            return this.games.First(a => a.GameId.Equals(id));
        }

        public void LeaveGame(UserEntity user)
        {
            GameMatchMakerService.Instance().RemoveUser(user.UserId);
        }
    }
}