using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class ConfigurationMenu
    /// @brief UI du menu de configuation du menu principal
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class ConfigurationMenu : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe ConfigurationMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        public ConfigurationMenu() {
            InitializeComponent();
            LoadFromJson();
            ReloadUIFromSave();
            InitializeEvents();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            // Tab events
            this.Tab_Inputs.Enter += (sender, e) => ReloadUIFromSave();
            this.Tab_GameOptions.Enter += (sender, e) => ReloadUIFromSave();
            this.Tab_Debug.Enter += (sender, e) => ReloadUIFromSave();
            this.Tab_Profiles.Enter += (sender, e) => ReloadUIFromSave();

            // Button events
            this.Button_SaveProfile.Click += new EventHandler(SaveCurrentValues);
            this.Button_ResetControls.Click += new EventHandler(ResetToDefaultControls);
            this.Button_ResetOptions.Click += new EventHandler(ResetToDefaultOptions);
            this.Button_DeleteProfile.Click += (sender, e) => { DeleteProfile(sender, e); SaveCurrentValues(sender, e); };
            this.Button_PlayerHuman.Click += (sender, e) => { SwitchButtonsState(this.Button_PlayerHuman, this.Button_PlayerVirtual); SaveCurrentValues(sender, e); };
            this.Button_PlayerVirtual.Click += (sender, e) => { SwitchButtonsState(this.Button_PlayerVirtual, this.Button_PlayerHuman); SaveCurrentValues(sender, e); };
            this.Button_ChangeKeyUp.Click += (sender, e) => { ReloadUIFromSave(); this.Text_ControlKeyUp_Key.Text = "?"; currentKeyWait = KEY_WAIT.UP; };
            this.Button_ChangeKeyDown.Click += (sender, e) => { ReloadUIFromSave(); this.Text_ControlKeyDown_Key.Text = "?"; currentKeyWait = KEY_WAIT.DOWN; };
            this.Button_ChangeKeyLeft.Click += (sender, e) => { ReloadUIFromSave(); this.Text_ControlKeyLeft_Key.Text = "?"; currentKeyWait = KEY_WAIT.LEFT; };
            this.Button_ChangeKeyRight.Click += (sender, e) => { ReloadUIFromSave(); this.Text_ControlKeyRight_Key.Text = "?"; currentKeyWait = KEY_WAIT.RIGHT; };

            // Checkbox events
            this.Checkbox_DebugActivation.CheckedChanged += (sender, e) => { SaveCurrentValues(sender, e); ChangeCheckboxEnabledState(); };
            this.Checkbox_DebugCollision.CheckedChanged += (sender, e) => SaveCurrentValues(sender, e);
            this.Checkbox_DebugSpeed.CheckedChanged += (sender, e) => SaveCurrentValues(sender, e);
            this.Checkbox_DebugLight.CheckedChanged += (sender, e) => SaveCurrentValues(sender, e);
            this.Checkbox_DebugPortal.CheckedChanged += (sender, e) => SaveCurrentValues(sender, e);

            // List event
            this.List_SavedProfileList.SelectedIndexChanged += new EventHandler(ProfileChanged);

            // Paint events
            this.Button_SaveProfile.Paint += new PaintEventHandler(StatePaintButton);
            this.Button_DeleteProfile.Paint += new PaintEventHandler(StatePaintButton);
            this.Input_ProfileName.EnabledChanged += new EventHandler(StatePaintTextBox);

            // Inputs events
            this.Input_GoalsNeeded.ValueChanged += (sender, e) => SaveCurrentValues(sender, e);

            // Form events
            this.FormClosing += (sender, e) => ReloadUIFromSave();
            this.Shown += (sender, e) => this.TabHolder.SelectTab(0);
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


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les valeurs enregistré à partir du fichier JSON si existant.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void LoadFromJson() {
            if (File.Exists(DefaultValues.saveFilePath)) {
                string fileData = File.ReadAllText(DefaultValues.saveFilePath);
                ConfigData data = JsonConvert.DeserializeObject<ConfigData>(fileData);

                // Inputs Tab
                moveUpKey = (Keys)Enum.Parse(typeof(Keys), data.moveUp_, true);
                moveDownKey = (Keys)Enum.Parse(typeof(Keys), data.moveDown_, true);
                moveLeftKey = (Keys)Enum.Parse(typeof(Keys), data.moveLeft_, true);
                moveRightKey = (Keys)Enum.Parse(typeof(Keys), data.moveRight_, true);

                // Options Tab
                neededGoalsToWin = data.winGoals_;
                isPlayer2Virtual = data.p2Virtual_;

                // Debug Tab
                debugActivated = data.debugMode_;
                collisionDebug = data.collDebug_;
                speedDebug = data.spdDebug_;
                lightDebug = data.lgtDebug_;
                portalDebug = data.ptlDebug_;

                // Profile Tab
                foreach (PlayerProfile profile in data.ppList_) {
                    playerProfiles.Add(profile);
                }
            }
        }



        ////////////////////////////////////////////////////////////////////////
        ///
        /// Sauvegarde les valeur en mémoire local, ainsi qu'en mémoire externe.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void SaveCurrentValues(object sender, EventArgs e) {
            // Inputs Tab
            moveUpKey = (this.Text_ControlKeyUp_Key.Text == "?") ? moveUpKey : (Keys)Enum.Parse(typeof(Keys), this.Text_ControlKeyUp_Key.Text, true);
            moveDownKey = (this.Text_ControlKeyDown_Key.Text == "?") ? moveDownKey : (Keys)Enum.Parse(typeof(Keys), this.Text_ControlKeyDown_Key.Text, true);
            moveLeftKey = (this.Text_ControlKeyLeft_Key.Text == "?") ? moveLeftKey : (Keys)Enum.Parse(typeof(Keys), this.Text_ControlKeyLeft_Key.Text, true);
            moveRightKey = (this.Text_ControlKeyRight_Key.Text == "?") ? moveRightKey : (Keys)Enum.Parse(typeof(Keys), this.Text_ControlKeyRight_Key.Text, true);

            // Options Tab
            neededGoalsToWin = (int)this.Input_GoalsNeeded.Value;
            isPlayer2Virtual = (this.Button_PlayerVirtual.ForeColor == Color.White);

            // Debug Tab
            debugActivated = this.Checkbox_DebugActivation.Checked;
            collisionDebug = this.Checkbox_DebugCollision.Checked;
            speedDebug = this.Checkbox_DebugSpeed.Checked;
            lightDebug = this.Checkbox_DebugLight.Checked;
            portalDebug = this.Checkbox_DebugPortal.Checked;

            // Profile Tab
            if (this.List_SavedProfileList.SelectedIndex == 0) {
                PlayerProfile currentProfile = new PlayerProfile(this.Input_ProfileName.Text, (int)this.Input_PlayerMoveSpeed.Value, (int)this.Input_PlayerPassivity.Value);
                bool alreadyExisting = false;

                foreach (PlayerProfile profile in playerProfiles) {
                    if (profile.Name == currentProfile.Name)
                        alreadyExisting = true;
                }

                if (currentProfile.Name == DefaultValues.defaultProfile.Name)
                    ShowWarning("Veuillez utiliser un nom de profil différent que celui par défaut.");
                else if (currentProfile.Name.Trim() == "")
                    ShowWarning("Veuiller utiliser un nom de profil qui n'est pas vide.");
                else if (alreadyExisting)
                    ShowWarning("Veuiller utiliser un nom de profil qui n'est pas déjà utilisé.");
                else
                    playerProfiles.Add(currentProfile);
            }
            else if (this.List_SavedProfileList.SelectedIndex > 0) {
                PlayerProfile currentProfile = new PlayerProfile(this.Input_ProfileName.Text, (int)this.Input_PlayerMoveSpeed.Value, (int)this.Input_PlayerPassivity.Value);

                foreach (PlayerProfile profile in playerProfiles) {
                    if (profile.Name == currentProfile.Name) {
                        profile.Passivity = currentProfile.Passivity;
                        profile.Speed = currentProfile.Speed;
                    }
                }
            }

            ReloadUIFromSave();

            ConfigData data = FillJSONStructure();
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DefaultValues.saveFilePath, json);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Remet les valeurs de contrôle de défaut
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ResetToDefaultControls(object sender, EventArgs e) {
            moveUpKey = DefaultValues.moveUpKey;
            moveDownKey = DefaultValues.moveDownKey;
            moveLeftKey = DefaultValues.moveLeftKey;
            moveRightKey = DefaultValues.moveRightKey;

            ReloadUIFromSave();
            SaveCurrentValues(sender, e);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Remet les valeurs de d'options de défaut
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ResetToDefaultOptions(object sender, EventArgs e) {
            neededGoalsToWin = DefaultValues.neededGoalsToWin;
            isPlayer2Virtual = DefaultValues.isPlayer2Virtual;

            ReloadUIFromSave();
            SaveCurrentValues(sender, e);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Charge les valeurs enregistrées pour le profil de joueur virtuel
        /// sélectionné. Charge le profil par défaut si rien n'est sélectionné.
        /// L'affichage est réinitialisé ensuite.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ProfileChanged(object sender, EventArgs e) {
            PlayerProfile currentProfile = DefaultValues.defaultProfile;
            foreach (PlayerProfile profile in playerProfiles) {
                if (profile.Name == this.List_SavedProfileList.Text)
                    currentProfile = profile;
            }

            this.Input_ProfileName.Text = currentProfile.Name;
            this.Input_PlayerMoveSpeed.Text = currentProfile.Speed.ToString();
            this.Input_PlayerPassivity.Text = currentProfile.Passivity.ToString();
            this.Input_ProfileName.Enabled = (this.List_SavedProfileList.SelectedIndex == 0) ? true : false;
            this.Input_PlayerMoveSpeed.Enabled = true;
            this.Input_PlayerPassivity.Enabled = true;
            this.Button_SaveProfile.Enabled = true;
            this.Button_DeleteProfile.Enabled = (this.List_SavedProfileList.SelectedIndex == 0) ? false : true;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Supprime le profil du joueur virtuel sélectionné. Le profil par
        /// Défaut ne peut être supprimé. L'affichage est réinitialisé ensuite.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void DeleteProfile(object sender, EventArgs e) {
            PlayerProfile currentProfile = null;
            foreach (PlayerProfile profile in playerProfiles) {
                if (profile.Name == this.List_SavedProfileList.Text)
                    currentProfile = profile;
            }

            if (currentProfile != null)
                playerProfiles.Remove(currentProfile);

            this.List_SavedProfileList.SelectedIndex = -1;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Permet d'obtenir la liste des nom des profils virtuels créés.
        ///
        /// @return La liste de noms (string).
        ///
        ////////////////////////////////////////////////////////////////////////
        public List<string> GetProfileList() {
            List<string> profilesList = new List<string>();
            foreach (PlayerProfile profile in playerProfiles) {
                profilesList.Add(profile.Name);
            }
            return profilesList;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Permet d'obtenir un profil de joueur à partir du nom du profil.
        ///
        /// @return Le profil de joueur demandé
        ///
        ////////////////////////////////////////////////////////////////////////
        public PlayerProfile GetProfile(string profileName) {
            PlayerProfile requestedProfile = DefaultValues.defaultProfile;
            foreach (PlayerProfile profile in playerProfiles) {
                if (profile.Name == profileName)
                    requestedProfile = profile;
            }
            return requestedProfile;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Réinitialise l'état des éléments présents selon les valeurs en 
        /// mémoire pour chacun d'entre eux. Actualise également les états
        /// actif/desactivé.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ReloadUIFromSave() {
            // Inputs Tab
            this.Text_ControlKeyUp_Key.Text = moveUpKey.ToString();
            this.Text_ControlKeyDown_Key.Text = moveDownKey.ToString();
            this.Text_ControlKeyLeft_Key.Text = moveLeftKey.ToString();
            this.Text_ControlKeyRight_Key.Text = moveRightKey.ToString();

            // Options Tab
            this.Input_GoalsNeeded.Text = neededGoalsToWin.ToString();
            if (isPlayer2Virtual)
                SwitchButtonsState(this.Button_PlayerVirtual, this.Button_PlayerHuman);
            else
                SwitchButtonsState(this.Button_PlayerHuman, this.Button_PlayerVirtual);

            this.Checkbox_DebugActivation.Checked = debugActivated;
            this.Checkbox_DebugCollision.Checked = collisionDebug;
            this.Checkbox_DebugSpeed.Checked = speedDebug;
            this.Checkbox_DebugLight.Checked = lightDebug;
            this.Checkbox_DebugPortal.Checked = portalDebug;
            ChangeCheckboxEnabledState();

            // Profiles Tab
            this.List_SavedProfileList.Items.Clear();
            this.List_SavedProfileList.Items.Add(DefaultValues.defaultProfile.Name);
            foreach (PlayerProfile profile in playerProfiles) {
                this.List_SavedProfileList.Items.Add(profile.Name);
            }

            this.List_SavedProfileList.SelectedIndex = -1;
            this.Input_ProfileName.Text = "";
            this.Input_ProfileName.Enabled = false;
            this.Input_PlayerMoveSpeed.Text = "";
            this.Input_PlayerMoveSpeed.Enabled = false;
            this.Input_PlayerPassivity.Text = "";
            this.Input_PlayerPassivity.Enabled = false;
            this.Button_SaveProfile.Enabled = false;
            this.Button_DeleteProfile.Enabled = false;
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
        /// Change la couleur de la bordure du Textbox selon son état afin de 
        /// distinguer sa version désactivée de celle activée.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void StatePaintTextBox(object sender, EventArgs e) {
            if (((TextBox)sender).Enabled)
                ((TextBox)sender).BorderStyle = BorderStyle.Fixed3D;
            else
                ((TextBox)sender).BorderStyle = BorderStyle.FixedSingle;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Change l'état d'activité des checkboxs du menu débug selon la 
        /// valeur de "debugActivated" qui agit comme clé d'activation globale.
        /// Change également la visibilité de la console.
        /// 
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ChangeCheckboxEnabledState() {
            this.Checkbox_DebugCollision.Enabled = debugActivated;
            this.Checkbox_DebugSpeed.Enabled = debugActivated;
            this.Checkbox_DebugLight.Enabled = debugActivated;
            this.Checkbox_DebugPortal.Enabled = debugActivated;

            var handle = GetConsoleWindow();
            if (debugActivated) {
                ShowWindow(handle, SW_SHOW);
                // Activation des affichages selectionnes
                FonctionsNatives.getDebugStatus(this.Checkbox_DebugCollision.Checked, this.Checkbox_DebugSpeed.Checked, this.Checkbox_DebugLight.Checked, this.Checkbox_DebugPortal.Checked);
            }
            else {
                ShowWindow(handle, SW_HIDE);
                // Desactivation des affichages selectionnes
                FonctionsNatives.getDebugStatus(false, false, false, false);
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Vérifie que la touche en paramètre ne soit pas déjà assignée 
        /// comme contrôle.
        /// 
        /// @param[in]  keyData : Touche à vérifier
        /// @return     Faux si la touche n'est pas déjà utilisée
        ///
        ////////////////////////////////////////////////////////////////////////
        private bool KeyAlreadyInUse(Keys keyData) {
            bool isAlreadyUsed = false;
            if (keyData.ToString() == Text_ControlKeyUp_Key.Text || keyData.ToString() == Text_ControlKeyDown_Key.Text || keyData.ToString() == Text_ControlKeyLeft_Key.Text || keyData.ToString() == Text_ControlKeyRight_Key.Text)
                isAlreadyUsed =  true;

            foreach(Keys key in keysAlreadyUsed) {
                if (keyData == key)
                    isAlreadyUsed = true;
            }

            if (isAlreadyUsed)
                ShowWarning("La touche est déjà réservée pour un autre contrôle.");

            return isAlreadyUsed;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Affiche une fenetre dialog avec un message d'erreur
        /// 
        /// @param[in]  warning : Message d'erreur à afficher
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ShowWarning(string warning) {
            ErrorMessageDialog dialog = new ErrorMessageDialog(warning);
            dialog.ShowDialog();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Rempli la structure de données en préparation à la sauvegarde JSON.
        /// 
        /// @return Struture de la config remplie
        ///
        ////////////////////////////////////////////////////////////////////////
        ConfigData FillJSONStructure() {
            ConfigData data;
            data.moveUp_ = moveUpKey.ToString();
            data.moveDown_ = moveDownKey.ToString();
            data.moveLeft_ = moveLeftKey.ToString();
            data.moveRight_ = moveRightKey.ToString();

            data.winGoals_ = neededGoalsToWin;
            data.p2Virtual_ = isPlayer2Virtual;

            data.debugMode_ = debugActivated;
            data.collDebug_ = collisionDebug;
            data.spdDebug_ = speedDebug;
            data.lgtDebug_ = lightDebug;
            data.ptlDebug_ = portalDebug;

            data.ppList_ = playerProfiles;

            return data;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction gère toutes les touches du clavier. Elle est 
        /// surchargé dans le cas ou le menu est en attente d'une touche de 
        /// contrôle à modifier.
        /// 
        /// @param[in]  msg          : Message de l'event
        /// @param[in]  keyData      : Touche appuyée
        /// @return     True si la touche est gérée 
        ///
        ////////////////////////////////////////////////////////////////////////
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if(currentKeyWait == KEY_WAIT.NONE) {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            else if (!KeyAlreadyInUse(keyData)) {
                switch (currentKeyWait) {
                    case KEY_WAIT.NONE:
                        return base.ProcessCmdKey(ref msg, keyData);
                    case KEY_WAIT.UP:
                        this.Text_ControlKeyUp_Key.Text = keyData.ToString();
                        currentKeyWait = KEY_WAIT.NONE;
                        break;
                    case KEY_WAIT.DOWN:
                        this.Text_ControlKeyDown_Key.Text = keyData.ToString();
                        currentKeyWait = KEY_WAIT.NONE;
                        break;
                    case KEY_WAIT.LEFT:
                        this.Text_ControlKeyLeft_Key.Text = keyData.ToString();
                        currentKeyWait = KEY_WAIT.NONE;
                        break;
                    case KEY_WAIT.RIGHT:
                        this.Text_ControlKeyRight_Key.Text = keyData.ToString();
                        currentKeyWait = KEY_WAIT.NONE;
                        break;
                }

                SaveCurrentValues(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // Inputs Tab
        private Keys moveUpKey = DefaultValues.moveUpKey;
        private Keys moveDownKey = DefaultValues.moveDownKey;
        private Keys moveLeftKey = DefaultValues.moveLeftKey;
        private Keys moveRightKey = DefaultValues.moveRightKey;
        // Options Tab
        private int neededGoalsToWin = DefaultValues.neededGoalsToWin;
        private bool isPlayer2Virtual = DefaultValues.isPlayer2Virtual;
        // Debug Tab
        private bool debugActivated = DefaultValues.debugActivated;
        private bool collisionDebug = DefaultValues.collisionDebug;
        private bool speedDebug = DefaultValues.speedDebug;
        private bool lightDebug = DefaultValues.lightDebug;
        private bool portalDebug = DefaultValues.portalDebug;
        // Profiles Tab
        private List<PlayerProfile> playerProfiles = new List<PlayerProfile>();
        // Other
        private KEY_WAIT currentKeyWait = KEY_WAIT.NONE;
        private Keys[] keysAlreadyUsed = new Keys[] { Keys.T, Keys.J, Keys.K, Keys.L, Keys.Escape, Keys.Space, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Oemplus, Keys.OemMinus, Keys.Oem1, Keys.Oem2 };


        // Accessors
        public int NeededGoalsToWin { get { return neededGoalsToWin; } }
        public bool IsPlayer2Virtual { get { return isPlayer2Virtual; } }
        public Keys MoveUpKey { get { return moveUpKey; } }
        public Keys MoveDownKey { get { return moveDownKey; } }
        public Keys MoveLeftKey { get { return moveLeftKey; } }
        public Keys MoveRightKey { get { return moveRightKey; } }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Classe contenant les valeurs par défaut utilisées pour la classe
        /// ConfigurationMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        private class DefaultValues {
            public static Keys moveUpKey = Keys.W;
            public static Keys moveDownKey = Keys.S;
            public static Keys moveLeftKey = Keys.A;
            public static Keys moveRightKey = Keys.D;
            public static int neededGoalsToWin = 2;
            public static bool isPlayer2Virtual = true;
            public static bool debugActivated = true;
            public static bool collisionDebug = true;
            public static bool speedDebug = true;
            public static bool lightDebug = true;
            public static bool portalDebug = true;
            public static PlayerProfile defaultProfile = new PlayerProfile("* Nouveau *", 5, 5);
            public static string saveFilePath = "données//GeneralConfig.json";
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Struct contenant les valeurs à sauvegarder en externe en JSON
        ///
        ////////////////////////////////////////////////////////////////////////
        private struct ConfigData {
            public string moveUp_;
            public string moveDown_;
            public string moveLeft_;
            public string moveRight_;

            public int winGoals_;
            public bool p2Virtual_;

            public bool debugMode_;
            public bool collDebug_;
            public bool spdDebug_;
            public bool lgtDebug_;
            public bool ptlDebug_;

            public List<PlayerProfile> ppList_;
        }


        enum KEY_WAIT {
            NONE = 0,
            UP = 1,
            DOWN = 2,
            LEFT = 3,
            RIGHT = 4
        };


        // Hide/Show Console
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
    }
}
