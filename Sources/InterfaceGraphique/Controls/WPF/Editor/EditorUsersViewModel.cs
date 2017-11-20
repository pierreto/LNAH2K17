using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using InterfaceGraphique.Entities.Editor;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorUsersViewModel : ViewModelBase
    {

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
        public EditorUsersViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            users = new ObservableCollection<OnlineUser>();
        }

        public override void InitializeViewModel()
        {
            this.Execute(
                () =>
                {
                    users.Clear();
                }
            );

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
    }
}
