using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Controls
{
    public partial class WebBrowser : Form
    {
        public WebBrowser()
        {
            InitializeComponent();
        }

        /*
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            System.Console.WriteLine(e.Url.ToString());
            var parameters = HttpUtility.ParseQueryString(e.Url.Fragment);
            if (parameters["#access_token"] != null)
            {
                User.Instance.UserEntity.FacebookToken = parameters["#access_token"];
            }
        }
        */
    }
}
