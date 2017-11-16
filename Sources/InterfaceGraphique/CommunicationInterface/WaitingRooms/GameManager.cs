using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public class GameManager
    {
        public GameManager(StoreService storeService)
        {
            StoreService = storeService;
            Textures = new string[2];
        }

        public string[] Textures { get; set; }

        private GameEntity currentOnlineGame;
        public GameEntity CurrentOnlineGame
        {
            get => currentOnlineGame;
            set
            {
                currentOnlineGame = value;
                //Task.Run(() => SetTextures());
            }
        }

        public StoreService StoreService { get; }

        public async Task SetTextures()
        {
            for(int i = 0; i < Textures.Length; i++)
            {
                var userId = CurrentOnlineGame.Players[i].Id;
                var items = await StoreService.GetUserStoreItems(userId);

                var enabledItem = items.Find(x => x.IsGameEnabled);
                if (enabledItem != null)
                {
                    if(userId == User.Instance.UserEntity.Id)
                    {
                        Textures[0] = enabledItem.TextureName;
                    }
                    else
                    {
                        Textures[1] = enabledItem.TextureName;
                    }
                }
            }
        }
    }
}
