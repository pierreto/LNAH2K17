using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.Editor;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    public class EditorUsersViewModel : ViewModelBase
    {
        private ObservableCollection<OnlineUser> users;

        public EditorUsersViewModel()
        {
            users = new ObservableCollection<OnlineUser>();
        }

        public override void InitializeViewModel()
        {
            users = new ObservableCollection<OnlineUser>();

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
            Users.Add(user);
        }
        public void RemoveByUsername(string username)
        {

            foreach (var user in Users)
            {
                if (user.Username.Equals(username))
                {
                    Users.Remove(user);
                }
            }
        }
    }
}
