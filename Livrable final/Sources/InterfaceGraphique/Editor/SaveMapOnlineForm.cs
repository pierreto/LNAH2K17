using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Editor
{
    public partial class SaveMapOnlineForm : Form
    {
        public SaveMapOnlineForm()
        {
            InitializeComponent();
            Text_PwdMap.Visible = false;
            Label_PwdMap.Visible = false;
        }

        private void Button_PrivateMap_CheckedChanged(object sender, EventArgs e)
        {
            if (Button_PrivateMap.Checked)
            {
                Label_PwdMap.Visible = true;
                Text_PwdMap.Visible = true;
            }
            else
            {
                Label_PwdMap.Visible = false;
                Text_PwdMap.ResetText();
                Text_PwdMap.Visible = false;
            }
        }
    }
}
