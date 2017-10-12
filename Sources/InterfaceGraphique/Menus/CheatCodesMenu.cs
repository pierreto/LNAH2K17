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
    /// @class CheatCodesMenu
    /// @brief UI du menu de triche
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class CheatCodesMenu : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe CheatCodesMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        public CheatCodesMenu() {
            InitializeComponent();
            InitializeEvents();

            timer.Interval = 250;
            timer.Start();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            this.FormClosed += (sender, e) => MediaPlayer_Player.Ctlcontrols.stop();
            this.MediaPlayer_Player.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(MediaEnded);
            timer.Tick += new EventHandler(ChangeLocation);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Change aléatoirement la position de la fenêtre a chaque 
        /// 1/4 de secondes.
        /// 
        ///	@param[in]  sender  : Objet qui a causé l'évènement
        /// @param[in]  e       : Arguments de l'évènement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ChangeLocation(object sender, EventArgs e) {
            Random random = new Random();
            this.Left = random.Next(0, Screen.PrimaryScreen.Bounds.Width - this.Width);
            this.Top = random.Next(0, Screen.PrimaryScreen.Bounds.Height - this.Height);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// S'occupe de fermer la fenetre à la fin du media
        /// 
        ///	@param[in]  sender  : Objet qui a causé l'évènement
        /// @param[in]  e       : Arguments de l'évènement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MediaEnded(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e) {
            if (e.newState == 8)
                this.Close();
        }


        /// Timer pour ChangeLocation()
        private Timer timer = new Timer();
    }
}
