using InterfaceGraphique.Controls.WPF.UserProfile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Menus
{
    public partial class UserProfileMenu : Form
    {
        public UserProfileMenu()
        {
            InitializeComponent();
            UserProfileView = new UserProfileView();
            this.elementHost1.Child = UserProfileView;
        }

        public UserProfileView UserProfileView { get; private set; }
    }
}
