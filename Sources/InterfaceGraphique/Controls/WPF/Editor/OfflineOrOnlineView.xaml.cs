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
using InterfaceGraphique.Editor;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Editor
{
    /// <summary>
    /// Logique d'interaction pour OfflineOrOnlineView.xaml
    /// </summary>
    public partial class OfflineOrOnlineView : UserControl
    {
        public OfflineOrOnlineView()
        {
            InitializeComponent();
        }

        private void Local_Click(object sender, RoutedEventArgs e)
        {
            Program.EditorHost.LocalSaveAndCloseThreadSafe();
        }

        private void Online_Click(object sender, RoutedEventArgs e)
        {
            Program.EditorHost.SwitchViewToMapModeView();
        }
    }
}
