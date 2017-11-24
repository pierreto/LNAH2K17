using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    /// <summary>
    /// Logique d'interaction pour FriendContentControl.xaml
    /// </summary>
    public partial class FriendContentControl : UserControl
    {
        //private FriendListViewModel friendList;
        //private AddUserViewModel addUser;
        public FriendContentControl()
        {
            //this.friendList = friendList;
            //this.addUser = addUser;
            InitializeComponent();
            this.DataContext = Program.unityContainer.Resolve<FriendListViewModel>();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender !=null)
            {
                ListBoxItem element = ((sender as ListBox).SelectedItem as ListBoxItem);
                ViewModelBase vm;
                if (element != null)
                {
                    if (element.Uid.Equals("ListeAmis"))
                    {
                        this.DataContext = Program.unityContainer.Resolve<FriendListViewModel>();
                    }
                    else
                    {
                        this.DataContext = Program.unityContainer.Resolve<AddUserViewModel>();

                    }
                }
            }
        }

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.unityContainer.Resolve<FriendListViewModel>().HasNewFriendRequest = false;
            System.Diagnostics.Debug.WriteLine("Clicked on notif tab");
        }

        private void TabItem_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Program.unityContainer.Resolve<FriendListViewModel>().HasNewFriend = false;
            System.Diagnostics.Debug.WriteLine("Clicked on friends tab");
        }
    }
}
