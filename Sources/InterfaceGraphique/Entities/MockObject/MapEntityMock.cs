using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
