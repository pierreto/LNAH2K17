using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Editor;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class CreateMapViewModel : ViewModelBase
    {
        private MapManager mapService;
        private string mapName ="";
        private string password = "";
        private bool nameFailed = false;

        public bool NameFailed
        {
            get => nameFailed;
            set
            {
                nameFailed = value;
                OnPropertyChanged();
            }
        }
  
        private bool isPrivate;
        public bool IsPrivate
        {
            get => isPrivate;
            set
            {
                isPrivate = value;
                OnPropertyChanged();
            }
        }

        public CreateMapViewModel(MapManager mapService)
        {
            this.mapService = mapService;
        }

        public string MapName
        {
            get => mapName;
            set
            {
                mapName = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public ICommand createCommand;
        public ICommand CreateCommand
        {
            get
            {
                return createCommand ??
                       (createCommand = new RelayCommandAsync(CreateMap, (o) => CanCreateMap()));
            }
        }

        public bool CanCreateMap()
        {
            if (!IsPrivate) //public mode
            {
                if (MapName.Length > 0)
                {
                    return true;
                }
            }
            else
            {
                if (MapName.Length > 0 && Password.Length >= 5)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task CreateMap()
        {
            if (this.mapName.Length > 127)
            {
                NameFailed = true;
            }
            else
            {
                NameFailed = false;

                await this.mapService.SaveNewOnlineMap(new MapMetaData()
                {

                    Creator = User.Instance.UserEntity.Username,
                    Name = this.mapName,
                    Password = this.password,
                    Private = IsPrivate
                });
                Program.EditorHost.Close();
            }
        }

        public override void InitializeViewModel()
        {
            MapName = "";
            NameFailed = false;
            this.Password = "";
            IsPrivate = false;
        }
    }
}
