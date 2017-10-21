using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities.Model
{

 
    public class OnlineEditedMapInfo
    {
        private bool isPublic;
        private string editionGroupId;
        private string mapName;
        private int numberOfPlayer;
        private int maxNumberOfPlayer;


        public string OnlineGroupId
        {
            get => editionGroupId;
            set => editionGroupId = value;
        }

        public string MapName
        {
            get => mapName;
            set => mapName = value;
        }

        public int NumberOfPlayer
        {
            get => numberOfPlayer;
            set => numberOfPlayer = value;
        }

        public int MaxNumberOfPlayer
        {
            get => maxNumberOfPlayer;
            set => maxNumberOfPlayer = value;
        }

        public bool IsPublic
        {
            get => isPublic;
            set => isPublic = value;
        }
    }

    public class OnlineEditedMapInfoMock
    {
        public OnlineEditedMapInfoMock()
        {
            OnlineEditedMapInfos = new ObservableCollection<OnlineEditedMapInfo>
            {
                new OnlineEditedMapInfo() {MapName = "abstr", MaxNumberOfPlayer = 10, NumberOfPlayer = 2, IsPublic = true},
                new OnlineEditedMapInfo() {MapName = "test text 3",  MaxNumberOfPlayer = 10, NumberOfPlayer = 2 ,IsPublic = false},
                new OnlineEditedMapInfo() {MapName = "test text 3",  MaxNumberOfPlayer = 4, NumberOfPlayer = 2, IsPublic = false}
            };
        }
        public ObservableCollection<OnlineEditedMapInfo> OnlineEditedMapInfos { get; set; }
    }
}
