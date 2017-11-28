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
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Controls.WPF.Tutorial;
using InterfaceGraphique.Editor.EditorState;
using InterfaceGraphique.Services;
using InterfaceGraphique.Editor;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.MainMenu;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class Editeur
    /// @brief UI du mode éditeur
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class Editeur : Form
    {
        public static MapManager mapManager; // Initialized in Program.cs
        private OfflineEditorState offlineState;
        public OnlineEditorState onlineState;
        public EditorViewModel editorViewModel;
        private MODELE_ETAT outilCourrant = MODELE_ETAT.AUCUN;

        public MODELE_ETAT OutilCourrant
        {
            get => outilCourrant;
        }

        public AbstractEditorState CurrentState { get; set; }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe Editeur
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public Editeur(OfflineEditorState offlineState, OnlineEditorState onlineState, EditorViewModel editorViewModel) {
            InitializeComponent();
            userLists.Child = Program.unityContainer.Resolve<EditorUsers>();

            InitializeEvents();

            this.offlineState = offlineState;
            this.onlineState = onlineState;
            CurrentState = offlineState;

            this.editorViewModel = editorViewModel;
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
            Program.OpenGLPanel.Controls.Add(this.userPanel);
            this.Controls.Add(Program.OpenGLPanel);

            Program.OpenGLPanel.MouseDown += new MouseEventHandler(mouseDown);
            Program.OpenGLPanel.MouseUp += new MouseEventHandler(mouseUp);
            Program.OpenGLPanel.MouseMove += new MouseEventHandler(mouseMove);
            Program.OpenGLPanel.MouseWheel += new MouseEventHandler(mouseWheel);
            Program.FormManager.SizeChanged += new EventHandler(sizeChanged);
            Program.FormManager.LocationChanged += new EventHandler(DessinerOpenGL);

            this.Panel_PropertiesBack.Location = new Point(Program.OpenGLPanel.Width - this.Panel_PropertiesBack.Width, Program.OpenGLPanel.Height - this.Panel_PropertiesBack.Height - 40);
            this.userPanel.Location = new Point(Program.OpenGLPanel.Width - this.userPanel.Width, 24);

            this.MenuBar.Renderer = new Renderer_MenuBar();

            ToggleOrbit(false);
            this.Cursor = Cursors.Default;

            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width + Toolbar.Size.Width, this.Size.Height - MenuBar.Size.Height);
            FonctionsNatives.changeGridVisibility(true);
            FonctionsNatives.gererRondelleMaillets(false);
            FonctionsNatives.toggleControlPointsVisibility(true);
            FonctionsNatives.setLights(0, true);
            FonctionsNatives.setLights(1, true);
            FonctionsNatives.setLights(2, true);

            this.CurrentState = this.offlineState;
            this.CurrentState.JoinEdition();

            EnableOrDisableOnlineFeatures();


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
                    this.CurrentState.frameUpdate(tempsInterAffichage);
                });
            }
            catch (Exception e ){
                System.Diagnostics.Debug.WriteLine(e);
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
            this.Toolbar_Select.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.SELECTION);
            this.Toolbar_Move.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.DEPLACEMENT);
            this.Toolbar_Rotate.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.ROTATION);
            this.Toolbar_Scale.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.MISE_A_ECHELLE);
            this.Toolbar_Duplicate.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.DUPLIQUER);
            this.Toolbar_Zoom.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.ZOOM);
            this.Toolbar_ControlPoint.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.POINTS_CONTROLE);
            this.Toolbar_Booster.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.CREATION_ACCELERATEUR);
            this.Toolbar_Portal.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.CREATION_PORTAIL);
            this.Toolbar_Wall.Click += (sender, e) => changerEtatEdition(sender, e, MODELE_ETAT.CREATION_MURET);

            // Menu dropdown options events
            this.Fichier_Enregistrer.Click += async (sender, e) => await mapManager.SaveMap();
            this.Fichier_EnregistrerSous_Ordinateur.Click += (sender, e) => mapManager.ManageSavingLocalMap();
            this.Fichier_EnregistrerSous_Serveur.Click += OpenLocalMap;
            this.Fichier_OuvrirLocalement.Click += (sender, e) =>
            {
                mapManager.OpenLocalMap();
            };
            this.Fichier_OuvrirEnLigne.Click += async (sender, e) => await OpenOnlineMap();
            this.Fichier_Nouveau.Click += async (sender, e) =>
            {
                if (!User.Instance.IsConnected)
                {
                    Program.Editeur.LeaveOnlineEdition();

                }

                await CurrentState.LeaveEdition();
            };
            this.Fichier_MenuPrincipal.Click += async (sender, e) =>
            {
                await CurrentState.LeaveEdition();
                Program.FormManager.CurrentForm = Program.HomeMenu;
                Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
            };
            this.Fichier_ModeTest.Click += (sender, e) => Program.FormManager.CurrentForm = Program.TestMode;
            this.Fichier_Propriete.Click += (sender, e) => Program.GeneralProperties.ShowDialog();

            this.Edition_Supprimer.Click += (sender, e) => { FonctionsNatives.deleteSelection(); selectionSupprimee(); };

            this.Outils_Selection.Click += (sender, e) => changerEtatEdition(Toolbar_Select, e, MODELE_ETAT.SELECTION);
            this.Outils_Deplacement.Click += (sender, e) => changerEtatEdition(Toolbar_Move, e, MODELE_ETAT.DEPLACEMENT);
            this.Outils_Rotation.Click += (sender, e) => changerEtatEdition(Toolbar_Rotate, e, MODELE_ETAT.ROTATION);
            this.Outils_Echelle.Click += (sender, e) => changerEtatEdition(Toolbar_Scale, e, MODELE_ETAT.MISE_A_ECHELLE);
            this.Outils_Duplication.Click += (sender, e) => changerEtatEdition(Toolbar_Duplicate, e, MODELE_ETAT.DUPLIQUER);
            this.Outils_Zoom.Click += (sender, e) => changerEtatEdition(Toolbar_Zoom, e, MODELE_ETAT.ZOOM);
            this.Outils_PointsControles.Click += (sender, e) => changerEtatEdition(Toolbar_ControlPoint, e, MODELE_ETAT.POINTS_CONTROLE);
            this.Outils_Creation_Accelerateur.Click += (sender, e) => changerEtatEdition(Toolbar_Booster, e, MODELE_ETAT.CREATION_ACCELERATEUR);
            this.Outils_Creation_Portail.Click += (sender, e) => changerEtatEdition(Toolbar_Portal, e, MODELE_ETAT.CREATION_PORTAIL);
            this.Outils_Creation_Muret.Click += (sender, e) => changerEtatEdition(Toolbar_Wall, e, MODELE_ETAT.CREATION_MURET);

            this.Vues_Orthographique.Click += (sender, e) => ToggleOrbit(false);
            this.Vue_Orbite.Click += (sender, e) => ToggleOrbit(true);

            this.Informations_Aide.Click += (sender, e) => { EditorHelp form = new EditorHelp(); form.ShowEditorHelpText();  form.ShowDialog(); };
            this.Informations_Tutoriel.Click += async (sender, e) =>
                {
                    await Program.unityContainer.Resolve<TutorialViewModel>().SwitchToEditorSlides();
                    Program.TutorialHost.ShowDialog();
                };

            // Properties panel events
            this.ResetButton.Click += new EventHandler(resetProprietesPanel);
            this.ApplyButton.Click += new EventHandler(applyProprietesPanel);

        }

        public void EnableOrDisableOnlineFeatures()
        {
            Fichier_EnregistrerSous_Serveur.Visible = User.Instance.IsConnected;
            Fichier_OuvrirEnLigne.Visible = User.Instance.IsConnected;
            Fichier_ModeTest.Visible = !User.Instance.IsConnected;
        }

        public void OpenLocalMap(object sender=null, EventArgs e=null)
        {
            Program.EditorHost.SwitchViewToMapModeView();
            Program.EditorHost.ShowDialog();
        }

        public async void LeaveOnlineEdition() // Called by the edition hub
        {
            if (this.Handle != null)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    ResetDefaultTable();
                    this.CurrentState = this.offlineState;
                    this.CurrentState.JoinEdition(null);
                    this.userPanel.Visible = false;
                }));
            }
         
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
            CurrentState.MouseDown(sender,e);
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
            CurrentState.MouseUp(sender,e);

            //if (mapManager.CurrentMapAlreadySaved())
                //Task.Run(() => mapManager.SaveMap());
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
        public void resetProprietesPanel(object sender, EventArgs e) {
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


        public async Task JoinEdition(MapEntity map)
        {
            await Task.Run(() =>
            {
                this.BeginInvoke(new MethodInvoker(async delegate
                {
                    await this.CurrentState.LeaveEdition();

                    await mapManager.OpenOnlineMap(map);
                    mapManager.SaveIcon();
                    this.CurrentState = this.onlineState;
                    this.CurrentState.JoinEdition(map);
                    this.userPanel.Visible = true;
                }));
            });
        }

        private async Task OpenOnlineMap()
        {
            await Program.EditorHost.SwitchViewToServerBrowser();
            Program.EditorHost.ShowDialog();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de remettre la table à zéro (points de contrôle
        /// en position initiale et enfants détruits).
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ResetDefaultTable() {
            FonctionsNatives.resetNodeTree();
            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width + Toolbar.Size.Width, this.Size.Height - MenuBar.Size.Height);
            Program.GeneralProperties.ResetProperties();


            changerEtatEdition(this.Toolbar_Select, null, MODELE_ETAT.SELECTION);

            mapManager.resetMapInfo();
          

        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de rendre invisible la fenêtre de propriétés
        /// lorsque l'objet sélectionné est supprimé.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void selectionSupprimee() {
            Edition_Supprimer.Enabled = false;
            this.Panel_PropertiesBack.Visible = false;
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
        private void changerEtatEdition(object sender, EventArgs e, MODELE_ETAT etatEdition) {
            resetToolbar();
            outilCourrant = etatEdition;
            resetProprietesPanel(sender, e);

            ((ToolStripButton)sender).BackColor = SystemColors.ControlDarkDark;
            FonctionsNatives.changerModeleEtat((int)etatEdition);
            this.Edition_Supprimer.Enabled = FonctionsNatives.verifierSelection();
            this.Cursor = Cursors.Default;
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
        private void ToggleOrbit(bool isOrbit) {
            FonctionsNatives.toggleOrbit(isOrbit);
            changerEtatEdition(this.Toolbar_Select, null, MODELE_ETAT.SELECTION);
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
                    changerEtatEdition(Toolbar_Move, null, MODELE_ETAT.DEPLACEMENT);
                    return true;

                case Keys.S:
                    changerEtatEdition(Toolbar_Select, null, MODELE_ETAT.SELECTION);
                    return true;

                case Keys.R:
                    changerEtatEdition(Toolbar_Rotate, null, MODELE_ETAT.ROTATION);
                    return true;

                case Keys.E:
                    changerEtatEdition(Toolbar_Scale, null, MODELE_ETAT.MISE_A_ECHELLE);
                    return true;

                case Keys.C:
                    changerEtatEdition(Toolbar_Duplicate, null, MODELE_ETAT.DUPLIQUER);
                    return true;

                case Keys.Z:
                    if(Toolbar_Zoom.Enabled)
                        changerEtatEdition(Toolbar_Zoom, null, MODELE_ETAT.ZOOM);
                    return true;

                case Keys.G:
                    changerEtatEdition(Toolbar_ControlPoint, null, MODELE_ETAT.POINTS_CONTROLE);
                    return true;

                case Keys.M:
                    changerEtatEdition(Toolbar_Wall, null, MODELE_ETAT.CREATION_MURET);
                    return true;

                case Keys.P:
                    changerEtatEdition(Toolbar_Portal, null, MODELE_ETAT.CREATION_PORTAIL);
                    return true;

                case Keys.B:
                    changerEtatEdition(Toolbar_Booster, null, MODELE_ETAT.CREATION_ACCELERATEUR);
                    return true;

                case Keys.T:
                    Program.FormManager.CurrentForm = Program.TestMode;
                    return true;

                case (Keys.N | Keys.Control):
                    ResetDefaultTable();
                    return true;

                case (Keys.O | Keys.Control):
                    mapManager.OpenLocalMap();
                    return true;

                case (Keys.S | Keys.Control):
                    Task.Run(async () => await mapManager.SaveMap());
                    return true;

                case (Keys.Q | Keys.Control):
                    ResetDefaultTable();
                    Program.FormManager.CurrentForm = Program.HomeMenu;
                    Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
                    return true;

                case Keys.Oemplus:
                    FonctionsNatives.zoomIn();
                    return true;

                case Keys.OemMinus:
                    FonctionsNatives.zoomOut();
                    return true;

                case Keys.Escape:
                    CurrentState.Escape();
                    FonctionsNatives.escape();
                    return true;

                case Keys.Delete:
                    FonctionsNatives.deleteSelection();
                    selectionSupprimee();
                    return true;
                    
                case Keys.D1:
                    ToggleOrbit(false);
                    return true;

                case Keys.D2:
                    ToggleOrbit(true);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public ToolStripMenuItem EditionSupprimer
        {
            get => Edition_Supprimer;
            set => Edition_Supprimer = value;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void HandleCoefficientChanges(float coefficientFriction, float coefficientAcceleration, float coefficientRebond)
        {
            this.CurrentState.HandleCoefficientChanges(coefficientFriction, coefficientAcceleration, coefficientRebond);
        }

        private void Edition_Paint(object sender, PaintEventArgs e)
        {

        }

        public void ExecuteCommandOnMainThread(Action executeCommand)
        {
            if (InvokeRequired)
            {
                this.Invoke(executeCommand);
                return;
            }
        }
    }
}
 