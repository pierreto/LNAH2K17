using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class Editeur
    /// @brief UI du mode éditeur
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class Editeur : Form {

        private MODELE_ETAT outilCourrant = MODELE_ETAT.AUCUN;
        private string nameSavedMap;
        private bool savedOnline;

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe Editeur
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public Editeur() {
            InitializeComponent();
            InitializeEvents();

            nameSavedMap = null;
            savedOnline = false;
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
        public async Task InitializeOpenGlPanel() {
            Program.OpenGLPanel.Controls.Clear();
            Program.OpenGLPanel.Controls.Add(this.Toolbar);
            Program.OpenGLPanel.Controls.Add(this.MenuBar);
            Program.OpenGLPanel.Controls.Add(this.Panel_PropertiesBack);
            this.Controls.Add(Program.OpenGLPanel);

            Program.OpenGLPanel.MouseDown += new MouseEventHandler(mouseDown);
            Program.OpenGLPanel.MouseUp += new MouseEventHandler(mouseUp);
            Program.OpenGLPanel.MouseMove += new MouseEventHandler(mouseMove);
            Program.OpenGLPanel.MouseWheel += new MouseEventHandler(mouseWheel);
            Program.FormManager.SizeChanged += new EventHandler(sizeChanged);
            Program.FormManager.LocationChanged += new EventHandler(DessinerOpenGL);

            this.Panel_PropertiesBack.Location = new Point(Program.OpenGLPanel.Width - this.Panel_PropertiesBack.Width, Program.OpenGLPanel.Height - this.Panel_PropertiesBack.Height);
            this.MenuBar.Renderer = new Renderer_MenuBar();

            await ToggleOrbit(false);
            this.Cursor = Cursors.Default;

            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width + Toolbar.Size.Width, this.Size.Height - MenuBar.Size.Height);
            FonctionsNatives.changeGridVisibility(true);
            FonctionsNatives.gererRondelleMaillets(false);
            FonctionsNatives.toggleControlPointsVisibility(true);
            FonctionsNatives.setLights(0, true);
            FonctionsNatives.setLights(1, true);
            FonctionsNatives.setLights(2, true);
        }

        
        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction fait la mise à jour de l'interface
        /// 
        /// @param[in]  tempsInterAffichage : Temps entre deux affichages
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage){
            try {
                this.Invoke((MethodInvoker)delegate {
                    FonctionsNatives.animer(tempsInterAffichage);
                    FonctionsNatives.dessinerOpenGL();
                });
            }
            catch (Exception){

            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction actualise l'affichage openGL
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void DessinerOpenGL(object sender, EventArgs e) {
            FonctionsNatives.dessinerOpenGL();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction initialise les événements
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {

            // Window events
            Toolbar.MouseEnter += (sender, e) => this.Cursor = Cursors.Default;
            MenuBar.MouseEnter += (sender, e) => this.Cursor = Cursors.Default;
            Panel_PropertiesBack.MouseEnter += (sender, e) => this.Cursor = Cursors.Default;

            // Toolbar icons events
            this.Toolbar_Select.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.SELECTION);
            this.Toolbar_Move.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.DEPLACEMENT);
            this.Toolbar_Rotate.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.ROTATION);
            this.Toolbar_Scale.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.MISE_A_ECHELLE);
            this.Toolbar_Duplicate.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.DUPLIQUER);
            this.Toolbar_Zoom.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.ZOOM);
            this.Toolbar_ControlPoint.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.POINTS_CONTROLE);
            this.Toolbar_Booster.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.CREATION_ACCELERATEUR);
            this.Toolbar_Portal.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.CREATION_PORTAIL);
            this.Toolbar_Wall.Click += async (sender, e) => await changerEtatEdition(sender, e, MODELE_ETAT.CREATION_MURET);

            // Menu dropdown options events
            this.Fichier_Enregistrer.Click += async (sender, e) => await SaveFile();
            this.Fichier_EnregistrerSous_Ordinateur.Click += (sender, e) => SaveFileAs();
            this.Fichier_EnregistrerSous_Serveur.Click += async (sender, e) => await SaveMapOnline(); 
            this.Fichier_Ouvrir.Click += (sender, e) => OpenFile();
            this.Fichier_Nouveau.Click += async (sender, e) => await ResetDefaultTable();
            this.Fichier_MenuPrincipal.Click += async (sender, e) => { await ResetDefaultTable(); Program.FormManager.CurrentForm = Program.MainMenu; };
            this.Fichier_ModeTest.Click += (sender, e) => Program.FormManager.CurrentForm = Program.TestMode;
            this.Fichier_Propriete.Click += (sender, e) => Program.GeneralProperties.ShowDialog();

            this.Edition_Supprimer.Click += async (sender, e) => { FonctionsNatives.deleteSelection(); await selectionSupprimee(); };

            this.Outils_Selection.Click += async (sender, e) => await changerEtatEdition(Toolbar_Select, e, MODELE_ETAT.SELECTION);
            this.Outils_Deplacement.Click += async (sender, e) => await changerEtatEdition(Toolbar_Move, e, MODELE_ETAT.DEPLACEMENT);
            this.Outils_Rotation.Click += async (sender, e) => await changerEtatEdition(Toolbar_Rotate, e, MODELE_ETAT.ROTATION);
            this.Outils_Echelle.Click += async (sender, e) => await changerEtatEdition(Toolbar_Scale, e, MODELE_ETAT.MISE_A_ECHELLE);
            this.Outils_Duplication.Click += async (sender, e) => await changerEtatEdition(Toolbar_Duplicate, e, MODELE_ETAT.DUPLIQUER);
            this.Outils_Zoom.Click += async (sender, e) => await changerEtatEdition(Toolbar_Zoom, e, MODELE_ETAT.ZOOM);
            this.Outils_PointsControles.Click += async (sender, e) => await changerEtatEdition(Toolbar_ControlPoint, e, MODELE_ETAT.POINTS_CONTROLE);
            this.Outils_Creation_Accelerateur.Click += async (sender, e) => await changerEtatEdition(Toolbar_Booster, e, MODELE_ETAT.CREATION_ACCELERATEUR);
            this.Outils_Creation_Portail.Click += async (sender, e) => await changerEtatEdition(Toolbar_Portal, e, MODELE_ETAT.CREATION_PORTAIL);
            this.Outils_Creation_Muret.Click += async (sender, e) => await changerEtatEdition(Toolbar_Wall, e, MODELE_ETAT.CREATION_MURET);

            this.Vues_Orthographique.Click += async (sender, e) => await ToggleOrbit(false);
            this.Vue_Orbite.Click += async (sender, e) => await ToggleOrbit(true);

            this.Informations_Aide.Click += (sender, e) => { EditorHelp form = new EditorHelp(); form.ShowEditorHelpText();  form.ShowDialog(); };

            // Properties panel events
            this.ResetButton.Click += new EventHandler(resetProprietesPanel);
            this.ApplyButton.Click += new EventHandler(applyProprietesPanel);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction désinscrits les events de la form courante sur le 
        /// panneau openGL.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UnsuscribeEventHandlers() {
            Program.OpenGLPanel.MouseDown -= new MouseEventHandler(mouseDown);
            Program.OpenGLPanel.MouseUp -= new MouseEventHandler(mouseUp);
            Program.OpenGLPanel.MouseMove -= new MouseEventHandler(mouseMove);
            Program.OpenGLPanel.MouseWheel -= new MouseEventHandler(mouseWheel);

            Program.FormManager.SizeChanged -= new EventHandler(sizeChanged);
            Program.FormManager.LocationChanged -= new EventHandler(DessinerOpenGL);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement d'un clique gauche
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (souris)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void mouseDown(object sender, MouseEventArgs e) {
            FonctionsNatives.modifierKeys((Control.ModifierKeys == Keys.Alt), (Control.ModifierKeys == Keys.Control));
            if (e.Button == MouseButtons.Left) {
                FonctionsNatives.mouseDownL();
            }
            if (e.Button == MouseButtons.Right) {
                FonctionsNatives.mouseDownR();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement d'un relâchement du bouton
        /// gauche de la souris
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (souris)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void mouseUp(object sender, MouseEventArgs e) {
            FonctionsNatives.modifierKeys((Control.ModifierKeys == Keys.Alt), (Control.ModifierKeys == Keys.Control));
            if (e.Button == MouseButtons.Left) {
                FonctionsNatives.mouseUpL();
                this.Edition_Supprimer.Enabled = FonctionsNatives.verifierSelection();
                resetProprietesPanel(null, null);
            }
            if (e.Button == MouseButtons.Right) {
                FonctionsNatives.mouseUpR();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement du déplacement de la 
        /// souris.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (souris)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void mouseMove(object sender, MouseEventArgs e) {
            FonctionsNatives.playerMouseMove(e.Location.X, e.Location.Y - MenuBar.Size.Height);
            if (outilCourrant == MODELE_ETAT.CREATION_ACCELERATEUR || outilCourrant == MODELE_ETAT.CREATION_PORTAIL || outilCourrant == MODELE_ETAT.CREATION_MURET)
                this.Cursor = (FonctionsNatives.mouseOverTable()) ? Cursors.Default : Cursors.No;
            if(outilCourrant == MODELE_ETAT.POINTS_CONTROLE && e.Button != MouseButtons.Left)
                this.Cursor = (FonctionsNatives.mouseOverControlPoint()) ? Cursors.Hand : Cursors.No;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement de la roulette de la 
        /// souris (dans les deux directions).
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (souris)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void mouseWheel(object sender, MouseEventArgs e)  {
            if(e.Delta > 0) {
                for(int i = 0; i < 3; i++) {
                    FonctionsNatives.zoomIn();
                }
            }

            if (e.Delta < 0) {
                for (int i = 0; i < 3; i++) {
                    FonctionsNatives.zoomOut();
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement d'une modification de la
        /// taille de la fenêtre du programme.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (fenêtre)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void sizeChanged(object sender, EventArgs e) {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
            FonctionsNatives.redimensionnerFenetre(this.Size.Width + Toolbar.Size.Width, this.Size.Height - MenuBar.Size.Height);
            FonctionsNatives.dessinerOpenGL();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement d'un clique sur le
        /// bouton reset.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (fenêtre)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void resetProprietesPanel(object sender, EventArgs e) {
            float[] infos = new float[9];
            if (FonctionsNatives.selectedNodeInfos(infos) && outilCourrant == MODELE_ETAT.SELECTION) {
                this.Panel_PropertiesBack.Visible = true;

                this.PositionXText.Text = infos[0].ToString();
                this.PositionYText.Text = infos[1].ToString();
                this.PositionZText.Text = infos[2].ToString();

                //this.RotationXText.Text = infos[3].ToString();
                this.RotationYText.Text = infos[4].ToString();
                //this.RotationZText.Text = infos[5].ToString();

                this.ScaleXText.Text = infos[6].ToString();
                //this.ScaleYText.Text = infos[7].ToString();
                this.ScaleZText.Text = infos[8].ToString();
            }
            else {
                this.Panel_PropertiesBack.Visible = false;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction tranforme les valeurs dans la fenêtre de propriétés
        /// en float qui peuvent être utilisés par le programme.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (fenêtre)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void applyProprietesPanel(object sender, EventArgs e) {
            float[] infos = new float[] {
                (float)this.PositionXText.Value,
                (float)this.PositionYText.Value,
                (float)this.PositionZText.Value,

                (float)this.RotationXText.Value,
                (float)this.RotationYText.Value,
                (float)this.RotationZText.Value,

                (float)this.ScaleXText.Value,
                (float)this.ScaleYText.Value,
                (float)this.ScaleZText.Value,
            };

            FonctionsNatives.applyNodeInfos(infos);
            resetProprietesPanel(null, null);
        }



        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'ouverture d'un fichier de 
        /// sauvegarde deja existant en JSON.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void OpenFile() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON Files (JSON)|*.json";
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + "\\zones";

            if (ofd.ShowDialog() == DialogResult.OK) {
                StringBuilder filePath = new StringBuilder(ofd.FileName.Length);
                filePath.Append(ofd.FileName);
                float[] coefficients = new float[3];
                FonctionsNatives.ouvrir(filePath, coefficients);
                Program.GeneralProperties.SetCoefficientValues(coefficients);
                nameSavedMap = ofd.FileName;
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge la sauvegarde d'un fichier .json 
        /// déjà existant. Dans le cas il n'existe pas, ouvrir la fentetre
        /// d'enregistrment complete.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private async Task SaveFile() {
            if (nameSavedMap != null)
            {
                if (savedOnline)
                {
                    await _SaveMapOnline(nameSavedMap);
                }
                else if (File.Exists(nameSavedMap))
                {
                    StringBuilder filePath = new StringBuilder(nameSavedMap.Length);
                    filePath.Append(nameSavedMap);
                    FonctionsNatives.enregistrerSous(filePath, Program.GeneralProperties.GetCoefficientValues());
                }
            }
            else
            {
                Editor.SaveMapForm form = new Editor.SaveMapForm();
                form.ShowDialog();
                if (form.SaveOnline)
                {
                    await SaveMapOnline();
                }
                else
                {
                    SaveFileAs();
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge la sauvegarde d'un nouveau 
        /// fichier .json pour conserver la carte en cours d'édition.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void SaveFileAs() {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "json";
            sfd.AddExtension = true;
            sfd.Filter = "JSON Files (JSON)|*.json";
            sfd.InitialDirectory = Directory.GetCurrentDirectory() + "\\zones";

            if (sfd.ShowDialog() == DialogResult.OK) {
                StringBuilder filePath = new StringBuilder(sfd.FileName.Length);
                filePath.Append(sfd.FileName);
                FonctionsNatives.enregistrerSous(filePath, Program.GeneralProperties.GetCoefficientValues());
                nameSavedMap = sfd.FileName;
            }
        }

        private async Task _SaveMapOnline(string mapName)
        {
                StringBuilder sb = new StringBuilder(2000);
                FonctionsNatives.getMapJson(Program.GeneralProperties.GetCoefficientValues(), sb);
                string json = sb.ToString();

                MapEntity map = new MapEntity {
                    Creator = Program.user.Username,
                    MapName = mapName,
                    LastBackup = DateTime.Now,
                    Json = json
                };

                HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/maps/save", map);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        @"Impossible de sauvegarder la carte. Veuillez ré-essayer plus tard.",
                        @"Internal error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    nameSavedMap = mapName;
                    savedOnline = true;
                }
        }

        private async Task SaveMapOnline()
        {
            Editor.SaveMapOnlineForm form = new Editor.SaveMapOnlineForm();

            if (form.ShowDialog() == DialogResult.OK && form.Text_MapName.Text.Length > 0)
            {
                await _SaveMapOnline(form.Text_MapName.Text);
            }
            else
            {
                MessageBox.Show(
                    @"Le nom de carte ne peut être vide.",
                    @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de remettre la table à zéro (points de contrôle
        /// en position initiale et enfants détruits).
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private async Task ResetDefaultTable() {
            FonctionsNatives.resetNodeTree();
            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width + Toolbar.Size.Width, this.Size.Height - MenuBar.Size.Height);
            Program.GeneralProperties.ResetProperties();
            await changerEtatEdition(this.Toolbar_Select, null, MODELE_ETAT.SELECTION);
            nameSavedMap = null;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de rendre invisible la fenêtre de propriétés
        /// lorsque l'objet sélectionné est supprimé.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private async Task selectionSupprimee() {
            Edition_Supprimer.Enabled = false;
            this.Panel_PropertiesBack.Visible = false;

            if (nameSavedMap != null)
            {
                await SaveFile();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de rendre invisible la fenêtre de propriétés
        /// lorsque l'objet sélectionné est supprimé.
        /// 
        /// @param[in]  sender        : L'objet qui envoie l'événement (fenêtre)
        /// @param[in]  e             : Propriétés de l'événement
        /// @param[in]  outil         : Identifiant de l'outil
        /// @param[in]  typeObject    : Identifiant du type d'objet
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private async Task changerEtatEdition(object sender, EventArgs e, MODELE_ETAT etatEdition) {
            resetToolbar();
            outilCourrant = etatEdition;
            resetProprietesPanel(sender, e);

            ((ToolStripButton)sender).BackColor = SystemColors.ControlDarkDark;
            FonctionsNatives.changerModeleEtat((int)etatEdition);
            this.Edition_Supprimer.Enabled = FonctionsNatives.verifierSelection();
            this.Cursor = Cursors.Default;

            if (nameSavedMap != null)
            {
                await SaveFile();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Désélection visuelle de tous les boutons de la barre d'outils 
        /// ainsi que dans le menu d'outils
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void resetToolbar() {
            this.Toolbar_Select.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Rotate.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Zoom.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Move.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Scale.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Duplicate.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Wall.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Portal.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_Booster.BackColor = GlobalVariables.defaultGrey;
            this.Toolbar_ControlPoint.BackColor = GlobalVariables.defaultGrey;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet changer en le mode orbit et le mode 
        /// perspective.
        /// 
        /// @param[in]  isOrbit : Etat d'activation du mode orbit
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private async Task ToggleOrbit(bool isOrbit) {
            FonctionsNatives.toggleOrbit(isOrbit);
            await changerEtatEdition(this.Toolbar_Select, null, MODELE_ETAT.SELECTION);
            this.Outils_Zoom.Enabled = !isOrbit;
            this.Toolbar_Zoom.Enabled = !isOrbit;

            this.Vue_Orbite.Enabled = !isOrbit;
            this.Vues_Orthographique.Enabled = isOrbit;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction gère toutes les touches du clavier
        /// 
        /// @param[in]  msg        : Message de l'event
        /// @param[in]  keyData    : Information sur la touche
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            switch (keyData) {
                case Keys.Up:
                    FonctionsNatives.fleches(0, GlobalVariables.deplacementVue);
                    return true;

                case Keys.Down:
                    FonctionsNatives.fleches(0, -GlobalVariables.deplacementVue);
                    return true;

                case Keys.Left:
                    FonctionsNatives.fleches(-GlobalVariables.deplacementVue, 0);
                    return true;

                case Keys.Right:
                    FonctionsNatives.fleches(GlobalVariables.deplacementVue, 0);
                    return true;

                case Keys.D:
                    Task.Run(() => changerEtatEdition(Toolbar_Move, null, MODELE_ETAT.DEPLACEMENT)).Wait();
                    return true;

                case Keys.S:
                    Task.Run(() => changerEtatEdition(Toolbar_Select, null, MODELE_ETAT.SELECTION));
                    return true;

                case Keys.R:
                    Task.Run(() => changerEtatEdition(Toolbar_Rotate, null, MODELE_ETAT.ROTATION));
                    return true;

                case Keys.E:
                    Task.Run(() => changerEtatEdition(Toolbar_Scale, null, MODELE_ETAT.MISE_A_ECHELLE));
                    return true;

                case Keys.C:
                    Task.Run(() => changerEtatEdition(Toolbar_Duplicate, null, MODELE_ETAT.DUPLIQUER));
                    return true;

                case Keys.Z:
                    if(Toolbar_Zoom.Enabled)
                        Task.Run(() => changerEtatEdition(Toolbar_Zoom, null, MODELE_ETAT.ZOOM));
                    return true;

                case Keys.G:
                    Task.Run(() => changerEtatEdition(Toolbar_ControlPoint, null, MODELE_ETAT.POINTS_CONTROLE));
                    return true;

                case Keys.M:
                    Task.Run(() => changerEtatEdition(Toolbar_Wall, null, MODELE_ETAT.CREATION_MURET));
                    return true;

                case Keys.P:
                    Task.Run(() => changerEtatEdition(Toolbar_Portal, null, MODELE_ETAT.CREATION_PORTAIL));
                    return true;

                case Keys.B:
                    Task.Run(() => changerEtatEdition(Toolbar_Booster, null, MODELE_ETAT.CREATION_ACCELERATEUR));
                    return true;

                case Keys.T:
                    Program.FormManager.CurrentForm = Program.TestMode;
                    return true;

                case (Keys.N | Keys.Control):
                    Task.Run(() => ResetDefaultTable());
                    return true;

                case (Keys.O | Keys.Control):
                    OpenFile();
                    return true;

                case (Keys.S | Keys.Control):
                    Task.Run(() => SaveFile());
                    return true;

                case (Keys.Q | Keys.Control):
                    Task.Run(() => ResetDefaultTable());
                    Program.FormManager.CurrentForm = Program.MainMenu;
                    return true;

                case Keys.Oemplus:
                    FonctionsNatives.zoomIn();
                    return true;

                case Keys.OemMinus:
                    FonctionsNatives.zoomOut();
                    return true;

                case Keys.Escape:
                    FonctionsNatives.escape();
                    return true;

                case Keys.Delete:
                    FonctionsNatives.deleteSelection();
                    return true;
                    
                case Keys.D1:
                    Task.Run(() => ToggleOrbit(false));
                    return true;

                case Keys.D2:
                    Task.Run(() => ToggleOrbit(true));
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
 