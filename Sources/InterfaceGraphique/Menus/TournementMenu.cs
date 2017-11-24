using Newtonsoft.Json;
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
using Microsoft.Practices.Unity;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique
{

    ///////////////////////////////////////////////////////////////////////////
    /// @class TournementMenu
    /// @brief UI de la configuration du tournoi
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class TournementMenu : Form
    {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe TournementMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        public TournementMenu()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeBaseSettings();
            LoadFromJson();
            InitializeAvailableColors();
        }

        bool isOnline { get; set; }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents()
        {
            bool onlineMode = User.Instance.IsConnected;

            this.Button_Play.Click += new EventHandler(ValidateSettings);
            this.Button_MainMenu.Click += (sender, e) => Program.FormManager.CurrentForm = Program.MainMenu;
            this.Button_OpenMap.Click += (sender, e) => fileDialog.ShowDialog();
            this.Button_DefaultMap.Click += (sender, e) => { this.Button_DefaultMap.Enabled = false; this.Label_MapName.Text = DefaultValues.mapName; fileDialog.FileName = null; };
            this.Button_DefaultMap.Paint += new PaintEventHandler(StatePaintButton);
            this.Button_Player1Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player1Human, this.Button_Player1Virtual); this.List_VirtualProfile1.Enabled = false; };
            this.Button_Player1Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player1Virtual, this.Button_Player1Human); this.List_VirtualProfile1.Enabled = true; };
            this.Button_Player2Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player2Human, this.Button_Player2Virtual); this.List_VirtualProfile2.Enabled = false; };
            this.Button_Player2Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player2Virtual, this.Button_Player2Human); this.List_VirtualProfile2.Enabled = true; };
            this.Button_Player3Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player3Human, this.Button_Player3Virtual); this.List_VirtualProfile3.Enabled = false; };
            this.Button_Player3Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player3Virtual, this.Button_Player3Human); this.List_VirtualProfile3.Enabled = true; };
            this.Button_Player4Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player4Human, this.Button_Player4Virtual); this.List_VirtualProfile4.Enabled = false; };
            this.Button_Player4Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player4Virtual, this.Button_Player4Human); this.List_VirtualProfile4.Enabled = true; };

            // Window events
            this.List_VirtualProfile1.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.List_VirtualProfile2.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.List_VirtualProfile3.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.List_VirtualProfile4.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.fileDialog.FileOk += new CancelEventHandler(MapFileOpened);

            // Color events
            this.Label_Player1.Click += new EventHandler(CycleAvailableColors);
            this.Label_Player2.Click += new EventHandler(CycleAvailableColors);
            this.Label_Player3.Click += new EventHandler(CycleAvailableColors);
            this.Label_Player4.Click += new EventHandler(CycleAvailableColors);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les paramètres initiaux de la fenêtre.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeBaseSettings()
        {
            this.fileDialog.Filter = "JSON Files (JSON)|*.json";
            this.fileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\zones";
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise la liste des couleurs disponibles
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeAvailableColors()
        {
            foreach (Color color in DefaultValues.availableColors)
            {
                if (color != player1Color && color != player2Color && color != player3Color && color != player4Color)
                    availableColors.Enqueue(color);
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Reinitialise les listes de profils virtuels
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ResetProfileLists()
        {
            this.List_VirtualProfile1.Items.Clear();
            this.List_VirtualProfile2.Items.Clear();
            this.List_VirtualProfile3.Items.Clear();
            this.List_VirtualProfile4.Items.Clear();
            this.List_VirtualProfile1.Items.Add(DefaultValues.profileName);
            this.List_VirtualProfile2.Items.Add(DefaultValues.profileName);
            this.List_VirtualProfile3.Items.Add(DefaultValues.profileName);
            this.List_VirtualProfile4.Items.Add(DefaultValues.profileName);
            foreach (string profile in Program.ConfigurationMenu.GetProfileList())
            {
                this.List_VirtualProfile1.Items.Add(profile);
                this.List_VirtualProfile2.Items.Add(profile);
                this.List_VirtualProfile3.Items.Add(profile);
                this.List_VirtualProfile4.Items.Add(profile);
            }
            this.List_VirtualProfile1.SelectedIndex = 0;
            this.List_VirtualProfile2.SelectedIndex = 0;
            this.List_VirtualProfile3.SelectedIndex = 0;
            this.List_VirtualProfile4.SelectedIndex = 0;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Fonction vide appelée sur toutes les forms de facon 
        /// constante sans se soucier du type
        /// 
        ///	@param[in]  tempsInterAffichage : Temps entre chaque affichage
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage)
        {

        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Met à jour la taille de la fenetre selon celle de la fenêtre parent
        /// 
        ///	@param[in]  sender : Objet qui a causé l'évènement
        /// @param[in]  e : Arguments de l'évènement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void WindowSizeChanged(object sender, EventArgs e)
        {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cycle à travers la liste des couleurs disponible. Assigne la 
        /// prochaine et ajoute l'ancienne à la queue.
        /// 
        ///	@param[in]  sender : Objet qui a causé l'évènement
        /// @param[in]  e : Arguments de l'évènement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void CycleAvailableColors(object sender, EventArgs e)
        {
            availableColors.Enqueue(((Label)sender).ForeColor);
            Color newColor = availableColors.Dequeue();

            if ((Label)sender == this.Label_Player1)
            {
                this.Label_Player1.ForeColor = newColor;
                this.Panel_Player1BorderColor.BackColor = newColor;
                this.Button_Player1Human.ForeColor = (this.Button_Player1Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player1Virtual.ForeColor = (this.Button_Player1Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
            else if ((Label)sender == this.Label_Player2)
            {
                this.Label_Player2.ForeColor = newColor;
                this.Panel_Player2BorderColor.BackColor = newColor;
                this.Button_Player2Human.ForeColor = (this.Button_Player2Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player2Virtual.ForeColor = (this.Button_Player2Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
            else if ((Label)sender == this.Label_Player3)
            {
                this.Label_Player3.ForeColor = newColor;
                this.Panel_Player3BorderColor.BackColor = newColor;
                this.Button_Player3Human.ForeColor = (this.Button_Player3Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player3Virtual.ForeColor = (this.Button_Player3Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
            else if ((Label)sender == this.Label_Player4)
            {
                this.Label_Player4.ForeColor = newColor;
                this.Panel_Player4BorderColor.BackColor = newColor;
                this.Button_Player4Human.ForeColor = (this.Button_Player4Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player4Virtual.ForeColor = (this.Button_Player4Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
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
        private void StatePaintButton(object sender, PaintEventArgs e)
        {
            if (((Button)sender).Enabled)
                ((Button)sender).ForeColor = Color.White;
            else
                ((Button)sender).ForeColor = SystemColors.GrayText;
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
        private void SwitchButtonsState(Button select, Button deselect)
        {
            if (select.ForeColor == Color.White)
            {
                select.ForeColor = deselect.ForeColor;
                deselect.ForeColor = Color.White;
                deselect.FlatAppearance.MouseDownBackColor = Color.Empty;
                deselect.FlatAppearance.MouseOverBackColor = Color.Empty;
                select.FlatAppearance.MouseDownBackColor = DefaultValues.defaultGrey;
                select.FlatAppearance.MouseOverBackColor = DefaultValues.defaultGrey;
                select.Cursor = Cursors.Default;
                deselect.Cursor = Cursors.Hand;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les paramètres de tournois choisis, puis lance la 
        /// partie avec ces paramètres.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void LoadGame()
        {
            string mapFilePath = null;

            if (this.Button_DefaultMap.Enabled)
                mapFilePath = fileDialog.FileName;

            SaveGameSettings();
            Program.QuickPlay.CurrentGameState.IsTournementMode = true;
            Program.QuickPlay.CurrentGameState.MapFilePath = mapFilePath;
            if (!isOnline)
            {
                Program.TournementTree.CurrentRound = 0;
                Program.FormManager.CurrentForm = Program.TournementTree;
            }
            else
            {
                Program.FormManager.CurrentForm = Program.OnlineTournament;
                var vm = Program.unityContainer.Resolve<Controls.WPF.Tournament.TournamentViewModel>();
                vm.Initialize();

                // fonctionne ici
                //Program.FormManager.CurrentForm = Program.QuickPlay;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Sauvergarde les paramètres de tournois choisis en mémoire
        /// locale et externe.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void SaveGameSettings()
        {
            player1Profile = this.List_VirtualProfile1.SelectedItem.ToString();
            player2Profile = this.List_VirtualProfile2.SelectedItem.ToString();
            player3Profile = this.List_VirtualProfile3.SelectedItem.ToString();
            player4Profile = this.List_VirtualProfile4.SelectedItem.ToString();

            player1Name = this.Input_Player1Name.Text;
            player2Name = this.Input_Player2Name.Text;
            player3Name = this.Input_Player3Name.Text;
            player4Name = this.Input_Player4Name.Text;

            player1Color = this.Label_Player1.ForeColor;
            player2Color = this.Label_Player2.ForeColor;
            player3Color = this.Label_Player3.ForeColor;
            player4Color = this.Label_Player4.ForeColor;

            isPlayer1Virtual = (this.Button_Player1Human.ForeColor == Color.White) ? true : false;
            isPlayer2Virtual = (this.Button_Player2Human.ForeColor == Color.White) ? true : false;
            isPlayer3Virtual = (this.Button_Player3Human.ForeColor == Color.White) ? true : false;
            isPlayer4Virtual = (this.Button_Player4Human.ForeColor == Color.White) ? true : false;

            mapFilePath = fileDialog.FileName;
            mapFileName = this.Label_MapName.Text;

            ConfigData data = FillJSONStructure();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DefaultValues.saveFilePath, json);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les paramètres de tournois enregistrés
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void LoadSavedSettings()
        {

            if (this.List_VirtualProfile1.Items.Contains(player1Profile)) this.List_VirtualProfile1.SelectedItem = player1Profile; else this.List_VirtualProfile1.SelectedIndex = 0;
            if (this.List_VirtualProfile2.Items.Contains(player2Profile)) this.List_VirtualProfile2.SelectedItem = player2Profile; else this.List_VirtualProfile2.SelectedIndex = 0;
            if (this.List_VirtualProfile3.Items.Contains(player3Profile)) this.List_VirtualProfile3.SelectedItem = player3Profile; else this.List_VirtualProfile3.SelectedIndex = 0;
            if (this.List_VirtualProfile4.Items.Contains(player4Profile)) this.List_VirtualProfile4.SelectedItem = player4Profile; else this.List_VirtualProfile4.SelectedIndex = 0;

            this.Input_Player1Name.Text = player1Name;
            this.Input_Player2Name.Text = player2Name;
            this.Input_Player3Name.Text = player3Name;
            this.Input_Player4Name.Text = player4Name;

            this.Label_Player1.ForeColor = player1Color;
            this.Panel_Player1BorderColor.BackColor = player1Color;
            this.Button_Player1Virtual.ForeColor = (isPlayer1Virtual) ? player1Color : Color.White;
            this.Button_Player1Human.ForeColor = (isPlayer1Virtual) ? Color.White : player1Color;

            this.Label_Player2.ForeColor = player2Color;
            this.Panel_Player2BorderColor.BackColor = player2Color;
            this.Button_Player2Virtual.ForeColor = (isPlayer2Virtual) ? player2Color : Color.White;
            this.Button_Player2Human.ForeColor = (isPlayer2Virtual) ? Color.White : player2Color;

            this.Label_Player3.ForeColor = player3Color;
            this.Panel_Player3BorderColor.BackColor = player3Color;
            this.Button_Player3Virtual.ForeColor = (isPlayer3Virtual) ? player3Color : Color.White;
            this.Button_Player3Human.ForeColor = (isPlayer3Virtual) ? Color.White : player3Color;

            this.Label_Player4.ForeColor = player4Color;
            this.Panel_Player4BorderColor.BackColor = player4Color;
            this.Button_Player4Virtual.ForeColor = (isPlayer4Virtual) ? player4Color : Color.White;
            this.Button_Player4Human.ForeColor = (isPlayer4Virtual) ? Color.White : player4Color;

            if (isPlayer1Virtual) this.Button_Player1Virtual.PerformClick(); else this.Button_Player1Human.PerformClick();
            if (isPlayer2Virtual) this.Button_Player2Virtual.PerformClick(); else this.Button_Player2Human.PerformClick();
            if (isPlayer3Virtual) this.Button_Player3Virtual.PerformClick(); else this.Button_Player3Human.PerformClick();
            if (isPlayer4Virtual) this.Button_Player4Virtual.PerformClick(); else this.Button_Player4Human.PerformClick();

            fileDialog.FileName = mapFilePath;
            this.Label_MapName.Text = mapFileName;
            this.Button_DefaultMap.Enabled = (mapFileName == DefaultValues.mapName) ? false : true;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les paramètres de tournois enregistrés dans le fichier JSON
        /// vers les variables locales.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void LoadFromJson()
        {
            if (File.Exists(DefaultValues.saveFilePath))
            {
                string fileData = File.ReadAllText(DefaultValues.saveFilePath);
                ConfigData data = JsonConvert.DeserializeObject<ConfigData>(fileData);

                player1Profile = data.player1Profile;
                player2Profile = data.player2Profile;
                player3Profile = data.player3Profile;
                player4Profile = data.player4Profile;

                player1Name = data.player1Name;
                player2Name = data.player2Name;
                player3Name = data.player3Name;
                player4Name = data.player4Name;

                player1Color = data.player1Color;
                player2Color = data.player2Color;
                player3Color = data.player3Color;
                player4Color = data.player4Color;

                isPlayer1Virtual = data.isPlayer1Virtual;
                isPlayer2Virtual = data.isPlayer2Virtual;
                isPlayer3Virtual = data.isPlayer3Virtual;
                isPlayer4Virtual = data.isPlayer4Virtual;

                mapFilePath = data.mapFilePath;
                mapFileName = data.mapFileName;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Vérifie que tout les paramètres sont valide pour débuter la partie.
        /// Émet un avritissement si ce n'est pas le cas et charge la partie 
        /// si tout est bon.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ValidateSettings(object sender, EventArgs e)
        {
            if (isOnline)
            {
                LoadGame();
            }
            else
            {

                if (this.Button_Player1Human.ForeColor == Color.White && this.Button_Player2Human.ForeColor == Color.White && this.Button_Player3Human.ForeColor == Color.White && this.Button_Player4Human.ForeColor == Color.White)
                {
                    string warning = "La partie ne peut être lancée sans qu'il y ait au moins un joueur humain.";
                    ErrorMessageDialog dialog = new ErrorMessageDialog(warning);
                    dialog.ShowDialog();
                }
                else if (this.Input_Player1Name.Text == "" || this.Input_Player2Name.Text == "" || this.Input_Player3Name.Text == "" || this.Input_Player4Name.Text == "")
                {
                    string warning = "La partie ne peut être lancée sans que tous les joueurs aient un nom.";
                    ErrorMessageDialog dialog = new ErrorMessageDialog(warning);
                    dialog.ShowDialog();
                }
                else if (PlayerNameTheSame())
                {
                    string warning = "La partie ne peut être lancée sans que tous les joueurs aient des noms différents.";
                    ErrorMessageDialog dialog = new ErrorMessageDialog(warning);
                    dialog.ShowDialog();
                }
                else
                {
                    LoadGame();
                }
            }
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
        private void MapFileOpened(object sender, CancelEventArgs e)
        {
            this.Button_DefaultMap.Enabled = true;
            string currentDirectory = Path.GetDirectoryName(fileDialog.FileName) + "\\";
            string fileType = ".json";
            string fileName = fileDialog.FileName.Remove(0, currentDirectory.Length);
            fileName = fileName.Remove(fileName.Length - fileType.Length);
            this.Label_MapName.Text = fileName;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Ajoute le panneau openGL à la Form en cours. Les controles sont
        /// modifiés afin d'ajouter les éléments visuels nécessaires et les 
        /// events sur le panel sont ajoutés.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void InitializeOpenGlPanel()
        {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);
            ResetProfileLists();
            LoadSavedSettings();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction désinscrits les events de la form courante sur le 
        /// panneau openGL.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UnsuscribeEventHandlers()
        {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Vérifie que les joueurs n'ont pas les mêmes nom.
        ///
        /// @return Vrai si deux joeurs ont le même nom 
        ///
        ////////////////////////////////////////////////////////////////////////
        public bool PlayerNameTheSame()
        {
            if (this.Input_Player1Name.Text == this.Input_Player2Name.Text || this.Input_Player1Name.Text == this.Input_Player3Name.Text || this.Input_Player1Name.Text == this.Input_Player4Name.Text)
                return true;
            if (this.Input_Player2Name.Text == this.Input_Player3Name.Text || this.Input_Player2Name.Text == this.Input_Player4Name.Text)
                return true;
            if (this.Input_Player3Name.Text == this.Input_Player4Name.Text)
                return true;

            return false;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Rempli la structure de données en préparation à la sauvegarde JSON.
        /// 
        /// @return La struture de sauvegarde remplie
        ///
        ////////////////////////////////////////////////////////////////////////
        ConfigData FillJSONStructure()
        {
            ConfigData data;

            data.player1Profile = player1Profile;
            data.player2Profile = player2Profile;
            data.player3Profile = player3Profile;
            data.player4Profile = player4Profile;

            data.player1Name = player1Name;
            data.player2Name = player2Name;
            data.player3Name = player3Name;
            data.player4Name = player4Name;

            data.player1Color = player1Color;
            data.player2Color = player2Color;
            data.player3Color = player3Color;
            data.player4Color = player4Color;

            data.isPlayer1Virtual = isPlayer1Virtual;
            data.isPlayer2Virtual = isPlayer2Virtual;
            data.isPlayer3Virtual = isPlayer3Virtual;
            data.isPlayer4Virtual = isPlayer4Virtual;

            data.mapFilePath = mapFilePath;
            data.mapFileName = mapFileName;

            return data;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private FileDialog fileDialog = new OpenFileDialog();
        private Queue<Color> availableColors = new Queue<Color>();

        private string player1Profile = "";
        private string player2Profile = "";
        private string player3Profile = "";
        private string player4Profile = "";
        private string player1Name = DefaultValues.player1Name;
        private string player2Name = DefaultValues.player2Name;
        private string player3Name = DefaultValues.player3Name;
        private string player4Name = DefaultValues.player4Name;
        private Color player1Color = DefaultValues.player1Color;
        private Color player2Color = DefaultValues.player2Color;
        private Color player3Color = DefaultValues.player3Color;
        private Color player4Color = DefaultValues.player4Color;
        private bool isPlayer1Virtual = DefaultValues.isplayerVirtual;
        private bool isPlayer2Virtual = DefaultValues.isplayerVirtual;
        private bool isPlayer3Virtual = DefaultValues.isplayerVirtual;
        private bool isPlayer4Virtual = DefaultValues.isplayerVirtual;
        private string mapFilePath;
        private string mapFileName = DefaultValues.mapName;


        // Accessors
        public string Player1Name { get { return player1Name; } }
        public string Player2Name { get { return player2Name; } }
        public string Player3Name { get { return player3Name; } }
        public string Player4Name { get { return player4Name; } }
        public Color Player1Color { get { return player1Color; } }
        public Color Player2Color { get { return player2Color; } }
        public Color Player3Color { get { return player3Color; } }
        public Color Player4Color { get { return player4Color; } }
        public string Player1Type { get { if (isPlayer1Virtual) return "Virtuel"; else return "Humain"; } }
        public string Player2Type { get { if (isPlayer2Virtual) return "Virtuel"; else return "Humain"; } }
        public string Player3Type { get { if (isPlayer3Virtual) return "Virtuel"; else return "Humain"; } }
        public string Player4Type { get { if (isPlayer4Virtual) return "Virtuel"; else return "Humain"; } }
        public string Player1Profile { get { return this.List_VirtualProfile1.Text; } }
        public string Player2Profile { get { return this.List_VirtualProfile2.Text; } }
        public string Player3Profile { get { return this.List_VirtualProfile3.Text; } }
        public string Player4Profile { get { return this.List_VirtualProfile4.Text; } }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Classe contenant les valeurs par défaut utilisées pour la classe
        /// TournementMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        private class DefaultValues
        {
            public static string mapName = "Défaut";
            public static string profileName = "Défaut";
            public static string player1Name = "Joueur 1";
            public static string player2Name = "Joueur 2";
            public static string player3Name = "Joueur 3";
            public static string player4Name = "Joueur 4";
            public static Color player1Color = Color.Chartreuse;
            public static Color player2Color = Color.Blue;
            public static Color player3Color = Color.Fuchsia;
            public static Color player4Color = Color.DarkOrange;
            public static bool isplayerVirtual = true;
            public static Color defaultGrey = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            public static string saveFilePath = "données//TournementConfig.json";
            public static List<Color> availableColors = new List<Color>() { Color.Chartreuse, Color.Blue, Color.Fuchsia, Color.DarkOrange, Color.Yellow, Color.Cyan, Color.Red, Color.DarkOrchid };
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Struct contenant les valeurs à sauvegarder en externe en JSON
        ///
        ////////////////////////////////////////////////////////////////////////
        private struct ConfigData
        {
            public string player1Profile;
            public string player2Profile;
            public string player3Profile;
            public string player4Profile;

            public string player1Name;
            public string player2Name;
            public string player3Name;
            public string player4Name;

            public Color player1Color;
            public Color player2Color;
            public Color player3Color;
            public Color player4Color;

            public bool isPlayer1Virtual;
            public bool isPlayer2Virtual;
            public bool isPlayer3Virtual;
            public bool isPlayer4Virtual;

            public string mapFilePath;
            public string mapFileName;
        }
    }
}
