using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF.UserProfile
{
    public class UserViewModel : ViewModelBase
    {

        public UserViewModel(UserEntity user)
        {
            UserName = user.Username;
            Id = user.Id;
            Name = user.Name;
            Profile = user.Profile;
            Email = user.Email;
            Date = user.Created;
        }

        public override void InitializeViewModel()
        {
            throw new NotImplementedException();
        }

        private string username;
        public string UserName
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string date;
        public string Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        private string profile;
        public string Profile
        {
            get => profile;
            set
            {
                profile = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }
    }
}
