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

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    /// <summary>
    /// Interaction logic for ChannelView.xaml
    /// </summary>
    public partial class ChannelView : UserControl
    {
        public ChannelView()
        {
            InitializeComponent();
        }

        private void Popup_LostFocus(object sender, RoutedEventArgs e)
        {
            Program.unityContainer.Resolve<ChatViewModel>().Opacity = 1.0f;
            Program.unityContainer.Resolve<ChatViewModel>().OnPropertyChanged("Opacity");
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            Program.unityContainer.Resolve<ChatViewModel>().Opacity = 1.0f;
            Program.unityContainer.Resolve<ChatViewModel>().OnPropertyChanged("Opacity");
        }
    }
}
