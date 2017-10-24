using InterfaceGraphique.Controls.WPF.ConnectServer;
using InterfaceGraphique.Controls.WPF.Home;
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

namespace InterfaceGraphique.Controls.WPF.Navigation
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        public NavigationView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            DataContext = new ConnectServerViewModel();
        }

    }
}
