using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Core;
using System.Threading.Tasks;

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
        public GameService(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; set; }

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
            // TODO : call bd
            gameEntity.GameId = Guid.NewGuid();

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
            MatchMakerService.AddOpponent(userEntity);
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
    }
}