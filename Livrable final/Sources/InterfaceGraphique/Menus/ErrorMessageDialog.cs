using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class ErrorMessageDialog
    /// @brief UI de la fentre popup d'erreur
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class ErrorMessageDialog : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe ErrorMessageDialog
        ///
        ////////////////////////////////////////////////////////////////////////
        public ErrorMessageDialog(string warning) {
            InitializeComponent();
            InitializeEvents();

            this.Label_WarningText.Text = warning;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            // Button events
            this.Button_Close.Click += (sender, e) => this.Close();
        }
    }
}
