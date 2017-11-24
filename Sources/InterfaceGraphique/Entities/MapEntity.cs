using InterfaceGraphique.Controls.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Editor;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace InterfaceGraphique.Entities
{
    public class MapEntity : BindableBase
    {
        public int? Id { get; set; }
        public string Creator { get; set; }
        public string MapName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastBackup { get; set; }
        public string Json { get; set; }
        public string Icon { get; set; }
        public bool Private { get; set; }
        public string Password { get; set; }
        public int CurrentNumberOfPlayer { get; set; }



        [JsonIgnore]
        private ICommand removeMapCommand;
        [JsonIgnore]
        public ICommand RemoveMapCommand
        {
            get
            {
                return removeMapCommand ??
                       (removeMapCommand = new RelayCommandAsync(RemoveMap, (o) => true));
            }
        }
        private async Task RemoveMap()
        {
            await Program.client.DeleteAsync(Program.client.BaseAddress + "api/maps/remove/" + Id);
            await Program.unityContainer.Resolve<EditorViewModel>().RefreshMapList();

        }
        [JsonIgnore]
        public bool CanDeleteMap
        {
            get
            {
                return this.Creator.Equals(User.Instance.UserEntity.Username) && CurrentNumberOfPlayer==0;
            }
        }
    }
}
