using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.Editor;

namespace InterfaceGraphique.Entities.MockObject
{
    public class MapEntityMock
    {
        public MapEntityMock()
        {
            OnlineEditedMapInfos = new ObservableCollection<MapEntity>
            {
                new MapEntity() {MapName = "abstr",  CurrentNumberOfPlayer = 2, Private = true},
                new MapEntity() {MapName = "test text ",   CurrentNumberOfPlayer = 2 ,Private = false},
                new MapEntity() {MapName = "test text 3",  CurrentNumberOfPlayer = 2, Private = false}
            };
        }
        public ObservableCollection<MapEntity> OnlineEditedMapInfos { get; set; }
    }

    public class UserMock
    {
        public UserMock()
        {
            FriendList = new List<UserEntity>
            {
                new UserEntity() {Username = "abstr",},
                new UserEntity() {Username = "test text "},
                new UserEntity() {Username = "test text 3"}
            };
        }
        public List<UserEntity> FriendList { get; set; }
    }

    public class OnlineUsersMock
    {
        public OnlineUsersMock()
        {
            Users = new ObservableCollection<OnlineUser>
            {
                new OnlineUser() {Username = "abstr",HexColor = "#0000FF"},
                new OnlineUser() {Username = "test text ",HexColor = "#00FF00"},
                new OnlineUser() {Username = "test text 3",HexColor = "#FF0000"}
            };
        }
        public ObservableCollection<OnlineUser> Users { get; set; }
    }
}

