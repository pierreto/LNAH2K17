using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class QuickPlayMenu
    /// @brief UI de la configuration de la partie rapide
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class QuickPlayMenu : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe QuickPlayMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        public QuickPlayMenu() {
            InitializeComponent();
            InitializeEvents();
            InitializeBaseSettings();
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
            this.Button_Play.Click += new EventHandler(LoadGameSettings);
            this.Button_Cancel.Click += (sender, e) => this.Close();
            this.Button_OpenMap.Click += (sender, e) => fileDialog.ShowDialog();
            this.Button_DefaultMap.Click += (sender, e) => { this.Button_DefaultMap.Enabled = false; this.Text_MapName.Text = DefaultValues.mapName; };
            this.Button_PlayerHuman.Click += (sender, e) => { SwitchButtonsState(this.Button_PlayerHuman, this.Button_PlayerVirtual); this.List_VirtualProfile.Enabled = false; };
            this.Button_PlayerVirtual.Click += (sender, e) => { SwitchButtonsState(this.Button_PlayerVirtual, this.Button_PlayerHuman); this.List_VirtualProfile.Enabled = true; };

            // Paint events
            this.Button_DefaultMap.Paint += new PaintEventHandler(StatePaintButton);

            // Window events
            this.Shown += (sender, e) => InitializeBaseSettings();
            this.List_VirtualProfile.DropDownClosed += (sender, e) => this.Text_VirtualProfile.Focus();
            this.fileDialog.FileOk += new CancelEventHandler(MapFileOpened);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les paramètres initiaux de la fenêtre.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeBaseSettings() {
            this.fileDialog.Filter = "JSON Files (JSON)|*.json";
            this.fileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\zones";
            
            this.Text_MapName.Text = DefaultValues.mapName;
            this.Button_DefaultMap.Enabled = false;
            this.Text_VirtualProfile.Focus();

            this.List_VirtualProfile.Items.Clear();
            this.List_VirtualProfile.Items.Add(DefaultValues.profileName);
            foreach(string profile in Program.ConfigurationMenu.GetProfileList()) {
                this.List_VirtualProfile.Items.Add(profile);
            }
            this.List_VirtualProfile.SelectedIndex = 0;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les paramètres de parite rapide choisis, puis lance la 
        /// partie avec ces paramètres.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void LoadGameSettings(object sender, EventArgs e) {
            string mapFilePath = null;

            if (this.Button_DefaultMap.Enabled)
                mapFilePath = fileDialog.FileName;

            this.Close();
            Program.QuickPlay.IsTournementMode = false;
            Program.QuickPlay.MapFilePath = mapFilePath;

            StringBuilder player1Name = new StringBuilder(DefaultValues.playerLeftName.Length);
            StringBuilder player2Name = new StringBuilder(DefaultValues.playerRightName.Length);
            player1Name.Append(DefaultValues.playerLeftName);
            player2Name.Append(DefaultValues.playerRightName);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);

            float[] playerColor = new float[4] { Color.White.R, Color.White.G, Color.White.B, Color.White.A };
            FonctionsNatives.setPlayerColors(playerColor, playerColor);

            PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(this.List_VirtualProfile.Text);
            FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
            FonctionsNatives.aiStatus(this.List_VirtualProfile.Enabled);

            Program.FormManager.CurrentForm = Program.QuickPlay;       
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Change la couleur de la bordure du button selon son état afin de 
        /// distinguer sa version désactivée de celle activée.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void StatePaintButton(object sender, PaintEventArgs e) {
            if (((Button)sender).Enabled)
                ((Button)sender).ForeColor = Color.White;
            else
                ((Button)sender).ForeColor = SystemColors.GrayText;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Permet de gérer le fichier de carte ouvert
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void MapFileOpened(object sender, CancelEventArgs e) {
            this.Button_DefaultMap.Enabled = true;
            string currentDirectory = Path.GetDirectoryName(fileDialog.FileName) + "\\";
            string fileType = ".json";
            string fileName = fileDialog.FileName.Remove(0, currentDirectory.Length);
            fileName = fileName.Remove(fileName.Length - fileType.Length);
            this.Text_MapName.Text = fileName;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Sélectionne le premier bouton et désélectionne le deuxième en 
        /// changeant les couleurs associées.
        /// 
        ///	@param[in]  select : Bouton à sélectionner (blanc --> couleur)
        /// @param[in]  deselect : Bouton à désélectionner (couleur--> blanc)
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void SwitchButtonsState(Button select, Button deselect) {
            if (select.ForeColor == SystemColors.GrayText) {
                select.ForeColor = Color.White;
                deselect.ForeColor = SystemColors.GrayText;
                deselect.FlatAppearance.MouseDownBackColor = Color.Empty;
                deselect.FlatAppearance.MouseOverBackColor = Color.Empty;
                select.FlatAppearance.MouseDownBackColor = GlobalVariables.defaultGrey;
                select.FlatAppearance.MouseOverBackColor = GlobalVariables.defaultGrey;
                select.Cursor = Cursors.Default;
                deselect.Cursor = Cursors.Hand;
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private FileDialog fileDialog = new OpenFileDialog();

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Classe contenant les valeurs par défaut utilisées pour la classe
        /// QuickPlayMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        private class DefaultValues {
            public static string mapName = "Défaut";
            public static string profileName = "Défaut";
            public static string playerLeftName = "Joueur 1";
            public static string playerRightName = "Joueur 2";
        }
    }
}
