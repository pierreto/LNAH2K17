using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Controls
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // On empeche fb de vouloir fermer notre browser via javascript, on le ferme
            // nous meme (pour eviter une fenetre de warning d'ie):
            string url = e.Url.ToString();
            if (url.StartsWith("https://www.facebook.com/dialog/return/close"))
                this.Close();
        }
    }
}
