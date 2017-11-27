using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Validation;
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

                canCreateMap = InputValidationRule.ValidateInput(this.MapName);

                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                if (!IsPrivate) //public mode
                {
                    if (InputValidationRule.ValidateInput(this.MapName))
                    {
                        canCreateMap = true;
                    }
                    else
                    {
                        canCreateMap = false;

                    }
                }
                else
                {
                    if (InputValidationRule.ValidateInput(this.MapName) &&
                        InputValidationRule.ValidateInput(this.Password))
                    {
                        canCreateMap = true;
                    }
                    else
                    {
                        canCreateMap = false;

                    }
                }
                OnPropertyChanged();
            }
        }

        public ICommand createCommand;
        public ICommand CreateCommand
        {
            get
            {
                return createCommand ??
                       (createCommand = new RelayCommandAsync(CreateMap, (o) => CanCreateMapWithGoodPwd()));
            }
        }
        public bool canCreateMap = false;

        public bool CanCreateMapWithGoodPwd()
        {

            if (!IsPrivate) //public mode
            {
                return canCreateMap;

            }
            else
            {
               return canCreateMap &&  this.Password.Length > 0;
            }
        
        }

        private async Task CreateMap()
        {
           /* if (this.mapName.Length > 127)
            {
                NameFailed = true;
            }
            else
            {
                NameFailed = false;
               */
                await this.mapService.SaveNewOnlineMap(new MapMetaData()
                {

                    Creator = User.Instance.UserEntity.Username,
                    Name = this.mapName,
                    Password = this.password,
                    Private = IsPrivate
                });
                Program.EditorHost.Close();
           // }
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
