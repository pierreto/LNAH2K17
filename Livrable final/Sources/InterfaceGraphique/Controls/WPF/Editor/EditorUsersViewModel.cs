using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using InterfaceGraphique.Editor;
using InterfaceGraphique.Entities.Editor;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorUsersViewModel : ViewModelBase
    {
        private MapManager mapManager;

        private string mapName ="";
        private ObservableCollection<OnlineUser> users;
        private readonly Dispatcher _dispatcher;
        protected Dispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }
        protected void Execute(Action action)
        {
            if (this.Dispatcher.CheckAccess())
            {
                action.Invoke();
            }
            else
            {
                this.Dispatcher.Invoke(DispatcherPriority.DataBind, action);
            }
        }
        public EditorUsersViewModel(MapManager mapManager)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            users = new ObservableCollection<OnlineUser>();

            this.mapManager = mapManager;
        }

        public override void InitializeViewModel()
        {
            this.Execute(
                () =>
                {
                    users.Clear();
                }
            );
            this.MapName = this.mapManager.CurrentMapInfo.Name;

        }

        public ObservableCollection<OnlineUser> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }
        public void AddUser(OnlineUser user)
        {
            this.Execute(
                () =>
                {
                    user.HexColor = "#" + user.HexColor;
                    Users.Add(user);
                }
            );

     
        }
        public void RemoveByUsername(string username)
        {
            this.Execute(
                () =>
                {
                    foreach (var user in Users)
                    {
                        if (user.Username.Equals(username))
                        {
                            Users.Remove(user);
                            break;
                        }
                    }
                }
            );
        
        }
        public string MapName
        {
            get { return mapName; }
            set
            {
                mapName = value;
                OnPropertyChanged();
            }
        }
    }
}
