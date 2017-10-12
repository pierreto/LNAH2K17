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
    public partial class SaveMapForm : Form
    {
        public bool SaveOnline;

        public SaveMapForm()
        {
            InitializeComponent();
            SaveOnline = false;
        }

        private void Button_SaveLocally_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_SaveOnline_Click(object sender, EventArgs e)
        {
            SaveOnline = true;
            this.Close();
        }
    }
}