using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique
{
    public partial class ListMaps : Form
    {
        public DataGridViewRow SelectedMap;

        public ListMaps()
        {
            InitializeComponent();
        }

        private void DataGridView_Maps_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("DoubleClick");

            if (DataGridView_Maps.SelectedRows.Count > 0)
            {
                SelectedMap = DataGridView_Maps.SelectedRows[0];
                this.Close();
            }
        }
    }
}