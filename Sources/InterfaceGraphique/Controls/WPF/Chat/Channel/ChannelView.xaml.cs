using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Windows.Threading;

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

        private void Border_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (this.Visibility == Visibility.Visible)
            {
                ((Border)sender).Focus();
            }
            else
            {
            }
        }
    }
}
