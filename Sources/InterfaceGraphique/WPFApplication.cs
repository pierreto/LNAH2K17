using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterfaceGraphique
{
    class WPFApplication : Application
    {
        private static WPFApplication _wpfApp;
        public static void Start()
        {
            _wpfApp = new WPFApplication();
            _wpfApp.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ResourceDictionary appResources = new ResourceDictionary();
            appResources.Source = new Uri("/InterfaceGraphique;component/appresources.xaml", UriKind.Relative);
            _wpfApp.Resources.MergedDictionaries.Add(appResources);
        }
    }


}
