using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique
{
    partial class Editeur {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editeur));
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.Menu_Fichier = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_Nouveau = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_Ouvrir = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_OuvrirLocalement = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_OuvrirEnLigne = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_Enregistrer = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_EnregistrerSous = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_EnregistrerSous_Ordinateur = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_EnregistrerSous_Serveur = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_Propriete = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_ModeTest = new System.Windows.Forms.ToolStripMenuItem();
            this.Fichier_MenuPrincipal = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Edition = new System.Windows.Forms.ToolStripMenuItem();
            this.Edition_Supprimer = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Outils = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Selection = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Deplacement = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Rotation = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Echelle = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Duplication = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Zoom = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Creation = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Creation_Accelerateur = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Creation_Muret = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_Creation_Portail = new System.Windows.Forms.ToolStripMenuItem();
            this.Outils_PointsControles = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Vues = new System.Windows.Forms.ToolStripMenuItem();
            this.Vues_Orthographique = new System.Windows.Forms.ToolStripMenuItem();
            this.Vue_Orbite = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Informations = new System.Windows.Forms.ToolStripMenuItem();
            this.Informations_Aide = new System.Windows.Forms.ToolStripMenuItem();
            this.Toolbar = new System.Windows.Forms.ToolStrip();
            this.Toolbar_SeparatorTop = new System.Windows.Forms.ToolStripSeparator();
            this.Toolbar_Select = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Move = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Rotate = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Scale = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Duplicate = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Zoom = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_ControlPoint = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_SeparatorMiddle = new System.Windows.Forms.ToolStripSeparator();
            this.Toolbar_Booster = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Wall = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_Portal = new System.Windows.Forms.ToolStripButton();
            this.Toolbar_SeparatorBottom = new System.Windows.Forms.ToolStripSeparator();
            this.Edition = new System.Windows.Forms.Panel();
            this.userPanel = new System.Windows.Forms.Panel();
            this.userLists = new System.Windows.Forms.Integration.ElementHost();
            this.Panel_PropertiesBack = new System.Windows.Forms.Panel();
            this.PropertiesEditPanel = new System.Windows.Forms.Panel();
            this.ScaleZText = new System.Windows.Forms.NumericUpDown();
            this.ScaleXText = new System.Windows.Forms.NumericUpDown();
            this.ScaleYText = new System.Windows.Forms.NumericUpDown();
            this.RotationZText = new System.Windows.Forms.NumericUpDown();
            this.RotationYText = new System.Windows.Forms.NumericUpDown();
            this.RotationXText = new System.Windows.Forms.NumericUpDown();
            this.PositionZText = new System.Windows.Forms.NumericUpDown();
            this.PositionYText = new System.Windows.Forms.NumericUpDown();
            this.PositionXText = new System.Windows.Forms.NumericUpDown();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.PropertiesTitle = new System.Windows.Forms.Label();
            this.PositionZLabel = new System.Windows.Forms.Label();
            this.PositionYLabel = new System.Windows.Forms.Label();
            this.PositionXLabel = new System.Windows.Forms.Label();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.RotationZLabel = new System.Windows.Forms.Label();
            this.RotationYLabel = new System.Windows.Forms.Label();
            this.RotationXLabel = new System.Windows.Forms.Label();
            this.RotationLabel = new System.Windows.Forms.Label();
            this.ScaleZLabel = new System.Windows.Forms.Label();
            this.ScaleYLabel = new System.Windows.Forms.Label();
            this.ScaleXLabel = new System.Windows.Forms.Label();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.Informations_Tutoriel = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBar.SuspendLayout();
            this.Toolbar.SuspendLayout();
            this.Edition.SuspendLayout();
            this.userPanel.SuspendLayout();
            this.Panel_PropertiesBack.SuspendLayout();
            this.PropertiesEditPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleXText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleYText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationYText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationXText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXText)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            this.MenuBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Fichier,
            this.Menu_Edition,
            this.Menu_Outils,
            this.Menu_Vues,
            this.Menu_Informations});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.MenuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MenuBar.Size = new System.Drawing.Size(1888, 46);
            this.MenuBar.TabIndex = 0;
            this.MenuBar.Text = "menuStrip1";
            // 
            // Menu_Fichier
            // 
            this.Menu_Fichier.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Fichier_Nouveau,
            this.Fichier_Ouvrir,
            this.Fichier_Enregistrer,
            this.Fichier_EnregistrerSous,
            this.Fichier_Propriete,
            this.Fichier_ModeTest,
            this.Fichier_MenuPrincipal});
            this.Menu_Fichier.ForeColor = System.Drawing.Color.White;
            this.Menu_Fichier.Name = "Menu_Fichier";
            this.Menu_Fichier.Size = new System.Drawing.Size(97, 38);
            this.Menu_Fichier.Text = "Fichier";
            // 
            // Fichier_Nouveau
            // 
            this.Fichier_Nouveau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_Nouveau.ForeColor = System.Drawing.Color.White;
            this.Fichier_Nouveau.Name = "Fichier_Nouveau";
            this.Fichier_Nouveau.Size = new System.Drawing.Size(298, 38);
            this.Fichier_Nouveau.Text = "Nouveau";
            // 
            // Fichier_Ouvrir
            // 
            this.Fichier_Ouvrir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_Ouvrir.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Fichier_OuvrirLocalement,
            this.Fichier_OuvrirEnLigne});
            this.Fichier_Ouvrir.ForeColor = System.Drawing.Color.White;
            this.Fichier_Ouvrir.Name = "Fichier_Ouvrir";
            this.Fichier_Ouvrir.Size = new System.Drawing.Size(298, 38);
            this.Fichier_Ouvrir.Text = "Ouvrir";
            // 
            // Fichier_OuvrirLocalement
            // 
            this.Fichier_OuvrirLocalement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_OuvrirLocalement.ForeColor = System.Drawing.Color.White;
            this.Fichier_OuvrirLocalement.Name = "Fichier_OuvrirLocalement";
            this.Fichier_OuvrirLocalement.Size = new System.Drawing.Size(268, 38);
            this.Fichier_OuvrirLocalement.Text = "Localement";
            // 
            // Fichier_OuvrirEnLigne
            // 
            this.Fichier_OuvrirEnLigne.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_OuvrirEnLigne.ForeColor = System.Drawing.Color.White;
            this.Fichier_OuvrirEnLigne.Name = "Fichier_OuvrirEnLigne";
            this.Fichier_OuvrirEnLigne.Size = new System.Drawing.Size(268, 38);
            this.Fichier_OuvrirEnLigne.Text = "En ligne";
            // 
            // Fichier_Enregistrer
            // 
            this.Fichier_Enregistrer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_Enregistrer.ForeColor = System.Drawing.Color.White;
            this.Fichier_Enregistrer.Name = "Fichier_Enregistrer";
            this.Fichier_Enregistrer.Size = new System.Drawing.Size(298, 38);
            this.Fichier_Enregistrer.Text = "Enregistrer";
            // 
            // Fichier_EnregistrerSous
            // 
            this.Fichier_EnregistrerSous.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_EnregistrerSous.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Fichier_EnregistrerSous_Ordinateur,
            this.Fichier_EnregistrerSous_Serveur});
            this.Fichier_EnregistrerSous.ForeColor = System.Drawing.Color.White;
            this.Fichier_EnregistrerSous.Name = "Fichier_EnregistrerSous";
            this.Fichier_EnregistrerSous.Size = new System.Drawing.Size(298, 38);
            this.Fichier_EnregistrerSous.Text = "Enregistrer sous...";
            // 
            // Fichier_EnregistrerSous_Ordinateur
            // 
            this.Fichier_EnregistrerSous_Ordinateur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_EnregistrerSous_Ordinateur.ForeColor = System.Drawing.Color.White;
            this.Fichier_EnregistrerSous_Ordinateur.Name = "Fichier_EnregistrerSous_Ordinateur";
            this.Fichier_EnregistrerSous_Ordinateur.Size = new System.Drawing.Size(268, 38);
            this.Fichier_EnregistrerSous_Ordinateur.Text = "Ordinateur";
            // 
            // Fichier_EnregistrerSous_Serveur
            // 
            this.Fichier_EnregistrerSous_Serveur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_EnregistrerSous_Serveur.ForeColor = System.Drawing.Color.White;
            this.Fichier_EnregistrerSous_Serveur.Name = "Fichier_EnregistrerSous_Serveur";
            this.Fichier_EnregistrerSous_Serveur.Size = new System.Drawing.Size(268, 38);
            this.Fichier_EnregistrerSous_Serveur.Text = "Serveur";
            // 
            // Fichier_Propriete
            // 
            this.Fichier_Propriete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_Propriete.ForeColor = System.Drawing.Color.White;
            this.Fichier_Propriete.Name = "Fichier_Propriete";
            this.Fichier_Propriete.Size = new System.Drawing.Size(298, 38);
            this.Fichier_Propriete.Text = "Propriétés";
            // 
            // Fichier_ModeTest
            // 
            this.Fichier_ModeTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_ModeTest.ForeColor = System.Drawing.Color.White;
            this.Fichier_ModeTest.Name = "Fichier_ModeTest";
            this.Fichier_ModeTest.Size = new System.Drawing.Size(298, 38);
            this.Fichier_ModeTest.Text = "Mode test";
            // 
            // Fichier_MenuPrincipal
            // 
            this.Fichier_MenuPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Fichier_MenuPrincipal.ForeColor = System.Drawing.Color.White;
            this.Fichier_MenuPrincipal.Name = "Fichier_MenuPrincipal";
            this.Fichier_MenuPrincipal.Size = new System.Drawing.Size(298, 38);
            this.Fichier_MenuPrincipal.Text = "Menu principal";
            // 
            // Menu_Edition
            // 
            this.Menu_Edition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edition_Supprimer});
            this.Menu_Edition.ForeColor = System.Drawing.Color.White;
            this.Menu_Edition.Name = "Menu_Edition";
            this.Menu_Edition.Size = new System.Drawing.Size(101, 38);
            this.Menu_Edition.Text = "Édition";
            // 
            // Edition_Supprimer
            // 
            this.Edition_Supprimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Edition_Supprimer.Enabled = false;
            this.Edition_Supprimer.ForeColor = System.Drawing.Color.White;
            this.Edition_Supprimer.Name = "Edition_Supprimer";
            this.Edition_Supprimer.Size = new System.Drawing.Size(225, 38);
            this.Edition_Supprimer.Text = "Supprimer";
            // 
            // Menu_Outils
            // 
            this.Menu_Outils.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Outils_Selection,
            this.Outils_Deplacement,
            this.Outils_Rotation,
            this.Outils_Echelle,
            this.Outils_Duplication,
            this.Outils_Zoom,
            this.Outils_Creation,
            this.Outils_PointsControles});
            this.Menu_Outils.ForeColor = System.Drawing.Color.White;
            this.Menu_Outils.Name = "Menu_Outils";
            this.Menu_Outils.Size = new System.Drawing.Size(89, 38);
            this.Menu_Outils.Text = "Outils";
            // 
            // Outils_Selection
            // 
            this.Outils_Selection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Selection.ForeColor = System.Drawing.Color.White;
            this.Outils_Selection.Name = "Outils_Selection";
            this.Outils_Selection.Size = new System.Drawing.Size(441, 38);
            this.Outils_Selection.Text = "Sélection";
            // 
            // Outils_Deplacement
            // 
            this.Outils_Deplacement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Deplacement.ForeColor = System.Drawing.Color.White;
            this.Outils_Deplacement.Name = "Outils_Deplacement";
            this.Outils_Deplacement.Size = new System.Drawing.Size(441, 38);
            this.Outils_Deplacement.Text = "Déplacement";
            // 
            // Outils_Rotation
            // 
            this.Outils_Rotation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Rotation.ForeColor = System.Drawing.Color.White;
            this.Outils_Rotation.Name = "Outils_Rotation";
            this.Outils_Rotation.Size = new System.Drawing.Size(441, 38);
            this.Outils_Rotation.Text = "Rotation";
            // 
            // Outils_Echelle
            // 
            this.Outils_Echelle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Echelle.ForeColor = System.Drawing.Color.White;
            this.Outils_Echelle.Name = "Outils_Echelle";
            this.Outils_Echelle.Size = new System.Drawing.Size(441, 38);
            this.Outils_Echelle.Text = "Mise à l\'échelle";
            // 
            // Outils_Duplication
            // 
            this.Outils_Duplication.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Duplication.ForeColor = System.Drawing.Color.White;
            this.Outils_Duplication.Name = "Outils_Duplication";
            this.Outils_Duplication.Size = new System.Drawing.Size(441, 38);
            this.Outils_Duplication.Text = "Duplication";
            // 
            // Outils_Zoom
            // 
            this.Outils_Zoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Zoom.ForeColor = System.Drawing.Color.White;
            this.Outils_Zoom.Name = "Outils_Zoom";
            this.Outils_Zoom.Size = new System.Drawing.Size(441, 38);
            this.Outils_Zoom.Text = "Zoom";
            // 
            // Outils_Creation
            // 
            this.Outils_Creation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Creation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Outils_Creation_Accelerateur,
            this.Outils_Creation_Muret,
            this.Outils_Creation_Portail});
            this.Outils_Creation.ForeColor = System.Drawing.Color.White;
            this.Outils_Creation.Name = "Outils_Creation";
            this.Outils_Creation.Size = new System.Drawing.Size(441, 38);
            this.Outils_Creation.Text = "Création";
            // 
            // Outils_Creation_Accelerateur
            // 
            this.Outils_Creation_Accelerateur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Creation_Accelerateur.ForeColor = System.Drawing.Color.White;
            this.Outils_Creation_Accelerateur.Name = "Outils_Creation_Accelerateur";
            this.Outils_Creation_Accelerateur.Size = new System.Drawing.Size(246, 38);
            this.Outils_Creation_Accelerateur.Text = "Accélérateur";
            // 
            // Outils_Creation_Muret
            // 
            this.Outils_Creation_Muret.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Creation_Muret.ForeColor = System.Drawing.Color.White;
            this.Outils_Creation_Muret.Name = "Outils_Creation_Muret";
            this.Outils_Creation_Muret.Size = new System.Drawing.Size(246, 38);
            this.Outils_Creation_Muret.Text = "Muret";
            // 
            // Outils_Creation_Portail
            // 
            this.Outils_Creation_Portail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_Creation_Portail.ForeColor = System.Drawing.Color.White;
            this.Outils_Creation_Portail.Name = "Outils_Creation_Portail";
            this.Outils_Creation_Portail.Size = new System.Drawing.Size(246, 38);
            this.Outils_Creation_Portail.Text = "Portail";
            // 
            // Outils_PointsControles
            // 
            this.Outils_PointsControles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Outils_PointsControles.ForeColor = System.Drawing.Color.White;
            this.Outils_PointsControles.Name = "Outils_PointsControles";
            this.Outils_PointsControles.Size = new System.Drawing.Size(441, 38);
            this.Outils_PointsControles.Text = "Gestion des points de contrôle";
            // 
            // Menu_Vues
            // 
            this.Menu_Vues.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Vues_Orthographique,
            this.Vue_Orbite});
            this.Menu_Vues.ForeColor = System.Drawing.Color.White;
            this.Menu_Vues.Name = "Menu_Vues";
            this.Menu_Vues.Size = new System.Drawing.Size(78, 38);
            this.Menu_Vues.Text = "Vues";
            // 
            // Vues_Orthographique
            // 
            this.Vues_Orthographique.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Vues_Orthographique.Enabled = false;
            this.Vues_Orthographique.ForeColor = System.Drawing.Color.White;
            this.Vues_Orthographique.Name = "Vues_Orthographique";
            this.Vues_Orthographique.Size = new System.Drawing.Size(285, 38);
            this.Vues_Orthographique.Text = "Orthographique";
            // 
            // Vue_Orbite
            // 
            this.Vue_Orbite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Vue_Orbite.ForeColor = System.Drawing.Color.White;
            this.Vue_Orbite.Name = "Vue_Orbite";
            this.Vue_Orbite.Size = new System.Drawing.Size(285, 38);
            this.Vue_Orbite.Text = "Orbite";
            // 
            // Menu_Informations
            // 
            this.Menu_Informations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Informations_Aide,
            this.Informations_Tutoriel});
            this.Menu_Informations.ForeColor = System.Drawing.Color.White;
            this.Menu_Informations.Name = "Menu_Informations";
            this.Menu_Informations.Size = new System.Drawing.Size(162, 38);
            this.Menu_Informations.Text = "Informations";
            // 
            // Informations_Aide
            // 
            this.Informations_Aide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Informations_Aide.ForeColor = System.Drawing.Color.White;
            this.Informations_Aide.Name = "Informations_Aide";
            this.Informations_Aide.Size = new System.Drawing.Size(268, 38);
            this.Informations_Aide.Text = "Raccourcis";
            // 
            // Toolbar
            // 
            this.Toolbar.AutoSize = false;
            this.Toolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.Toolbar.GripMargin = new System.Windows.Forms.Padding(0);
            this.Toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Toolbar.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.Toolbar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Toolbar_SeparatorTop,
            this.Toolbar_Select,
            this.Toolbar_Move,
            this.Toolbar_Rotate,
            this.Toolbar_Scale,
            this.Toolbar_Duplicate,
            this.Toolbar_Zoom,
            this.Toolbar_ControlPoint,
            this.Toolbar_SeparatorMiddle,
            this.Toolbar_Booster,
            this.Toolbar_Wall,
            this.Toolbar_Portal,
            this.Toolbar_SeparatorBottom});
            this.Toolbar.Location = new System.Drawing.Point(0, 46);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.Padding = new System.Windows.Forms.Padding(0);
            this.Toolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Toolbar.Size = new System.Drawing.Size(100, 1264);
            this.Toolbar.TabIndex = 1;
            // 
            // Toolbar_SeparatorTop
            // 
            this.Toolbar_SeparatorTop.Name = "Toolbar_SeparatorTop";
            this.Toolbar_SeparatorTop.Size = new System.Drawing.Size(99, 6);
            // 
            // Toolbar_Select
            // 
            this.Toolbar_Select.AutoSize = false;
            this.Toolbar_Select.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar_Select.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Toolbar_Select.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Select.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Select.Image")));
            this.Toolbar_Select.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Select.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Select.Name = "Toolbar_Select";
            this.Toolbar_Select.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Select.Text = "Sélection";
            this.Toolbar_Select.ToolTipText = "Sélection";
            // 
            // Toolbar_Move
            // 
            this.Toolbar_Move.AutoSize = false;
            this.Toolbar_Move.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar_Move.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Move.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Move.Image")));
            this.Toolbar_Move.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Move.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Move.Name = "Toolbar_Move";
            this.Toolbar_Move.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Move.Text = "Déplacement";
            this.Toolbar_Move.ToolTipText = "Déplacement";
            // 
            // Toolbar_Rotate
            // 
            this.Toolbar_Rotate.AutoSize = false;
            this.Toolbar_Rotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar_Rotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Rotate.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Rotate.Image")));
            this.Toolbar_Rotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Rotate.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Rotate.Name = "Toolbar_Rotate";
            this.Toolbar_Rotate.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Rotate.Text = "Rotation";
            this.Toolbar_Rotate.ToolTipText = "Rotation";
            // 
            // Toolbar_Scale
            // 
            this.Toolbar_Scale.AutoSize = false;
            this.Toolbar_Scale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar_Scale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Scale.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Scale.Image")));
            this.Toolbar_Scale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Scale.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Scale.Name = "Toolbar_Scale";
            this.Toolbar_Scale.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Scale.Text = "Mise à l\'échelle";
            this.Toolbar_Scale.ToolTipText = "Mise à l\'échelle";
            // 
            // Toolbar_Duplicate
            // 
            this.Toolbar_Duplicate.AutoSize = false;
            this.Toolbar_Duplicate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar_Duplicate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Duplicate.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Duplicate.Image")));
            this.Toolbar_Duplicate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Duplicate.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Duplicate.Name = "Toolbar_Duplicate";
            this.Toolbar_Duplicate.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Duplicate.Text = "Duplication";
            this.Toolbar_Duplicate.ToolTipText = "Duplication";
            // 
            // Toolbar_Zoom
            // 
            this.Toolbar_Zoom.AutoSize = false;
            this.Toolbar_Zoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Toolbar_Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Zoom.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Zoom.Image")));
            this.Toolbar_Zoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Zoom.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Zoom.Name = "Toolbar_Zoom";
            this.Toolbar_Zoom.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Zoom.Text = "Zoom";
            // 
            // Toolbar_ControlPoint
            // 
            this.Toolbar_ControlPoint.AutoSize = false;
            this.Toolbar_ControlPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_ControlPoint.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_ControlPoint.Image")));
            this.Toolbar_ControlPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_ControlPoint.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_ControlPoint.Name = "Toolbar_ControlPoint";
            this.Toolbar_ControlPoint.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_ControlPoint.Text = "Toolbar_ControlPoint";
            this.Toolbar_ControlPoint.ToolTipText = "Gestion des points de contrôle";
            // 
            // Toolbar_SeparatorMiddle
            // 
            this.Toolbar_SeparatorMiddle.ForeColor = System.Drawing.Color.White;
            this.Toolbar_SeparatorMiddle.Name = "Toolbar_SeparatorMiddle";
            this.Toolbar_SeparatorMiddle.Size = new System.Drawing.Size(99, 6);
            // 
            // Toolbar_Booster
            // 
            this.Toolbar_Booster.AutoSize = false;
            this.Toolbar_Booster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Booster.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Booster.Image")));
            this.Toolbar_Booster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Booster.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Booster.Name = "Toolbar_Booster";
            this.Toolbar_Booster.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Booster.Text = "Accélérateur";
            // 
            // Toolbar_Wall
            // 
            this.Toolbar_Wall.AutoSize = false;
            this.Toolbar_Wall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Wall.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Wall.Image")));
            this.Toolbar_Wall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Wall.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Wall.Name = "Toolbar_Wall";
            this.Toolbar_Wall.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Wall.Text = "Muret";
            // 
            // Toolbar_Portal
            // 
            this.Toolbar_Portal.AutoSize = false;
            this.Toolbar_Portal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Toolbar_Portal.Image = ((System.Drawing.Image)(resources.GetObject("Toolbar_Portal.Image")));
            this.Toolbar_Portal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolbar_Portal.Margin = new System.Windows.Forms.Padding(0);
            this.Toolbar_Portal.Name = "Toolbar_Portal";
            this.Toolbar_Portal.Size = new System.Drawing.Size(50, 50);
            this.Toolbar_Portal.Text = "Portail";
            // 
            // Toolbar_SeparatorBottom
            // 
            this.Toolbar_SeparatorBottom.Name = "Toolbar_SeparatorBottom";
            this.Toolbar_SeparatorBottom.Size = new System.Drawing.Size(99, 6);
            // 
            // Edition
            // 
            this.Edition.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Edition.Controls.Add(this.userPanel);
            this.Edition.Controls.Add(this.Panel_PropertiesBack);
            this.Edition.Controls.Add(this.Toolbar);
            this.Edition.Controls.Add(this.MenuBar);
            this.Edition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edition.Location = new System.Drawing.Point(0, 0);
            this.Edition.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Edition.Name = "Edition";
            this.Edition.Size = new System.Drawing.Size(1888, 1310);
            this.Edition.TabIndex = 6;
            this.Edition.Visible = false;
            this.Edition.Paint += new System.Windows.Forms.PaintEventHandler(this.Edition_Paint);
            // 
            // userPanel
            // 
            this.userPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userPanel.BackColor = System.Drawing.Color.Blue;
            this.userPanel.Controls.Add(this.userLists);
            this.userPanel.Location = new System.Drawing.Point(1376, 46);
            this.userPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.userPanel.Name = "userPanel";
            this.userPanel.Size = new System.Drawing.Size(512, 358);
            this.userPanel.TabIndex = 23;
            this.userPanel.Visible = false;
            // 
            // userLists
            // 
            this.userLists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userLists.Location = new System.Drawing.Point(8, 6);
            this.userLists.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.userLists.Name = "userLists";
            this.userLists.Size = new System.Drawing.Size(498, 346);
            this.userLists.TabIndex = 22;
            this.userLists.Text = "elementHost1";
            this.userLists.Child = null;
            // 
            // Panel_PropertiesBack
            // 
            this.Panel_PropertiesBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_PropertiesBack.BackColor = System.Drawing.Color.Blue;
            this.Panel_PropertiesBack.Controls.Add(this.PropertiesEditPanel);
            this.Panel_PropertiesBack.Location = new System.Drawing.Point(1376, 817);
            this.Panel_PropertiesBack.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Panel_PropertiesBack.Name = "Panel_PropertiesBack";
            this.Panel_PropertiesBack.Size = new System.Drawing.Size(512, 492);
            this.Panel_PropertiesBack.TabIndex = 21;
            this.Panel_PropertiesBack.Visible = false;
            // 
            // PropertiesEditPanel
            // 
            this.PropertiesEditPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertiesEditPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PropertiesEditPanel.Controls.Add(this.ScaleZText);
            this.PropertiesEditPanel.Controls.Add(this.ScaleXText);
            this.PropertiesEditPanel.Controls.Add(this.ScaleYText);
            this.PropertiesEditPanel.Controls.Add(this.RotationZText);
            this.PropertiesEditPanel.Controls.Add(this.RotationYText);
            this.PropertiesEditPanel.Controls.Add(this.RotationXText);
            this.PropertiesEditPanel.Controls.Add(this.PositionZText);
            this.PropertiesEditPanel.Controls.Add(this.PositionYText);
            this.PropertiesEditPanel.Controls.Add(this.PositionXText);
            this.PropertiesEditPanel.Controls.Add(this.ResetButton);
            this.PropertiesEditPanel.Controls.Add(this.ApplyButton);
            this.PropertiesEditPanel.Controls.Add(this.PropertiesTitle);
            this.PropertiesEditPanel.Controls.Add(this.PositionZLabel);
            this.PropertiesEditPanel.Controls.Add(this.PositionYLabel);
            this.PropertiesEditPanel.Controls.Add(this.PositionXLabel);
            this.PropertiesEditPanel.Controls.Add(this.PositionLabel);
            this.PropertiesEditPanel.Controls.Add(this.RotationZLabel);
            this.PropertiesEditPanel.Controls.Add(this.RotationYLabel);
            this.PropertiesEditPanel.Controls.Add(this.RotationXLabel);
            this.PropertiesEditPanel.Controls.Add(this.RotationLabel);
            this.PropertiesEditPanel.Controls.Add(this.ScaleZLabel);
            this.PropertiesEditPanel.Controls.Add(this.ScaleYLabel);
            this.PropertiesEditPanel.Controls.Add(this.ScaleXLabel);
            this.PropertiesEditPanel.Controls.Add(this.ScaleLabel);
            this.PropertiesEditPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertiesEditPanel.Location = new System.Drawing.Point(6, 6);
            this.PropertiesEditPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PropertiesEditPanel.Name = "PropertiesEditPanel";
            this.PropertiesEditPanel.Size = new System.Drawing.Size(500, 481);
            this.PropertiesEditPanel.TabIndex = 20;
            // 
            // ScaleZText
            // 
            this.ScaleZText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ScaleZText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScaleZText.DecimalPlaces = 2;
            this.ScaleZText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleZText.ForeColor = System.Drawing.Color.White;
            this.ScaleZText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ScaleZText.Location = new System.Drawing.Point(370, 321);
            this.ScaleZText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ScaleZText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ScaleZText.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScaleZText.Name = "ScaleZText";
            this.ScaleZText.Size = new System.Drawing.Size(120, 39);
            this.ScaleZText.TabIndex = 35;
            this.ScaleZText.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ScaleXText
            // 
            this.ScaleXText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ScaleXText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScaleXText.DecimalPlaces = 2;
            this.ScaleXText.Enabled = false;
            this.ScaleXText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleXText.ForeColor = System.Drawing.Color.White;
            this.ScaleXText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ScaleXText.Location = new System.Drawing.Point(50, 321);
            this.ScaleXText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ScaleXText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ScaleXText.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScaleXText.Name = "ScaleXText";
            this.ScaleXText.Size = new System.Drawing.Size(120, 39);
            this.ScaleXText.TabIndex = 33;
            this.ScaleXText.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ScaleYText
            // 
            this.ScaleYText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ScaleYText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScaleYText.DecimalPlaces = 2;
            this.ScaleYText.Enabled = false;
            this.ScaleYText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleYText.ForeColor = System.Drawing.Color.White;
            this.ScaleYText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ScaleYText.Location = new System.Drawing.Point(210, 321);
            this.ScaleYText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ScaleYText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ScaleYText.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScaleYText.Name = "ScaleYText";
            this.ScaleYText.Size = new System.Drawing.Size(120, 39);
            this.ScaleYText.TabIndex = 36;
            this.ScaleYText.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RotationZText
            // 
            this.RotationZText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RotationZText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RotationZText.DecimalPlaces = 2;
            this.RotationZText.Enabled = false;
            this.RotationZText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationZText.ForeColor = System.Drawing.Color.White;
            this.RotationZText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.RotationZText.Location = new System.Drawing.Point(370, 225);
            this.RotationZText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RotationZText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RotationZText.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.RotationZText.Name = "RotationZText";
            this.RotationZText.Size = new System.Drawing.Size(120, 39);
            this.RotationZText.TabIndex = 34;
            // 
            // RotationYText
            // 
            this.RotationYText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RotationYText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RotationYText.DecimalPlaces = 2;
            this.RotationYText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationYText.ForeColor = System.Drawing.Color.White;
            this.RotationYText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.RotationYText.Location = new System.Drawing.Point(210, 225);
            this.RotationYText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RotationYText.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationYText.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationYText.Name = "RotationYText";
            this.RotationYText.Size = new System.Drawing.Size(120, 39);
            this.RotationYText.TabIndex = 33;
            // 
            // RotationXText
            // 
            this.RotationXText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RotationXText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RotationXText.DecimalPlaces = 2;
            this.RotationXText.Enabled = false;
            this.RotationXText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationXText.ForeColor = System.Drawing.Color.White;
            this.RotationXText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.RotationXText.Location = new System.Drawing.Point(50, 225);
            this.RotationXText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.RotationXText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RotationXText.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.RotationXText.Name = "RotationXText";
            this.RotationXText.Size = new System.Drawing.Size(120, 39);
            this.RotationXText.TabIndex = 32;
            // 
            // PositionZText
            // 
            this.PositionZText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PositionZText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PositionZText.DecimalPlaces = 2;
            this.PositionZText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionZText.ForeColor = System.Drawing.Color.White;
            this.PositionZText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PositionZText.Location = new System.Drawing.Point(370, 129);
            this.PositionZText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.PositionZText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PositionZText.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.PositionZText.Name = "PositionZText";
            this.PositionZText.Size = new System.Drawing.Size(120, 39);
            this.PositionZText.TabIndex = 31;
            // 
            // PositionYText
            // 
            this.PositionYText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PositionYText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PositionYText.DecimalPlaces = 2;
            this.PositionYText.Enabled = false;
            this.PositionYText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionYText.ForeColor = System.Drawing.Color.White;
            this.PositionYText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PositionYText.Location = new System.Drawing.Point(210, 129);
            this.PositionYText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.PositionYText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PositionYText.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.PositionYText.Name = "PositionYText";
            this.PositionYText.Size = new System.Drawing.Size(120, 39);
            this.PositionYText.TabIndex = 30;
            // 
            // PositionXText
            // 
            this.PositionXText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PositionXText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PositionXText.DecimalPlaces = 2;
            this.PositionXText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionXText.ForeColor = System.Drawing.Color.White;
            this.PositionXText.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.PositionXText.Location = new System.Drawing.Point(50, 129);
            this.PositionXText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.PositionXText.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PositionXText.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.PositionXText.Name = "PositionXText";
            this.PositionXText.Size = new System.Drawing.Size(120, 39);
            this.PositionXText.TabIndex = 3;
            // 
            // ResetButton
            // 
            this.ResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetButton.ForeColor = System.Drawing.Color.White;
            this.ResetButton.Location = new System.Drawing.Point(270, 413);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(0);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(200, 48);
            this.ResetButton.TabIndex = 29;
            this.ResetButton.TabStop = false;
            this.ResetButton.Text = " Réinitialiser";
            this.ResetButton.UseVisualStyleBackColor = true;
            // 
            // ApplyButton
            // 
            this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApplyButton.ForeColor = System.Drawing.Color.White;
            this.ApplyButton.Location = new System.Drawing.Point(30, 413);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(0);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(200, 48);
            this.ApplyButton.TabIndex = 28;
            this.ApplyButton.TabStop = false;
            this.ApplyButton.Text = "Appliquer";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // PropertiesTitle
            // 
            this.PropertiesTitle.AutoSize = true;
            this.PropertiesTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PropertiesTitle.ForeColor = System.Drawing.Color.White;
            this.PropertiesTitle.Location = new System.Drawing.Point(142, 10);
            this.PropertiesTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PropertiesTitle.Name = "PropertiesTitle";
            this.PropertiesTitle.Size = new System.Drawing.Size(204, 44);
            this.PropertiesTitle.TabIndex = 27;
            this.PropertiesTitle.Text = "Propriétés";
            this.PropertiesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PositionZLabel
            // 
            this.PositionZLabel.AutoSize = true;
            this.PositionZLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionZLabel.ForeColor = System.Drawing.Color.White;
            this.PositionZLabel.Location = new System.Drawing.Point(330, 135);
            this.PositionZLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PositionZLabel.Name = "PositionZLabel";
            this.PositionZLabel.Size = new System.Drawing.Size(41, 32);
            this.PositionZLabel.TabIndex = 26;
            this.PositionZLabel.Text = "z :";
            this.PositionZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PositionYLabel
            // 
            this.PositionYLabel.AutoSize = true;
            this.PositionYLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionYLabel.ForeColor = System.Drawing.Color.White;
            this.PositionYLabel.Location = new System.Drawing.Point(170, 135);
            this.PositionYLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PositionYLabel.Name = "PositionYLabel";
            this.PositionYLabel.Size = new System.Drawing.Size(42, 32);
            this.PositionYLabel.TabIndex = 25;
            this.PositionYLabel.Text = "y :";
            this.PositionYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PositionXLabel
            // 
            this.PositionXLabel.AutoSize = true;
            this.PositionXLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionXLabel.ForeColor = System.Drawing.Color.White;
            this.PositionXLabel.Location = new System.Drawing.Point(10, 135);
            this.PositionXLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PositionXLabel.Name = "PositionXLabel";
            this.PositionXLabel.Size = new System.Drawing.Size(42, 32);
            this.PositionXLabel.TabIndex = 24;
            this.PositionXLabel.Text = "x :";
            this.PositionXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PositionLabel.ForeColor = System.Drawing.Color.White;
            this.PositionLabel.Location = new System.Drawing.Point(10, 90);
            this.PositionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(108, 32);
            this.PositionLabel.TabIndex = 23;
            this.PositionLabel.Text = "Position";
            this.PositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RotationZLabel
            // 
            this.RotationZLabel.AutoSize = true;
            this.RotationZLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationZLabel.ForeColor = System.Drawing.Color.White;
            this.RotationZLabel.Location = new System.Drawing.Point(330, 231);
            this.RotationZLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.RotationZLabel.Name = "RotationZLabel";
            this.RotationZLabel.Size = new System.Drawing.Size(41, 32);
            this.RotationZLabel.TabIndex = 19;
            this.RotationZLabel.Text = "z :";
            this.RotationZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RotationYLabel
            // 
            this.RotationYLabel.AutoSize = true;
            this.RotationYLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationYLabel.ForeColor = System.Drawing.Color.White;
            this.RotationYLabel.Location = new System.Drawing.Point(170, 231);
            this.RotationYLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.RotationYLabel.Name = "RotationYLabel";
            this.RotationYLabel.Size = new System.Drawing.Size(42, 32);
            this.RotationYLabel.TabIndex = 18;
            this.RotationYLabel.Text = "y :";
            this.RotationYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RotationXLabel
            // 
            this.RotationXLabel.AutoSize = true;
            this.RotationXLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationXLabel.ForeColor = System.Drawing.Color.White;
            this.RotationXLabel.Location = new System.Drawing.Point(10, 231);
            this.RotationXLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.RotationXLabel.Name = "RotationXLabel";
            this.RotationXLabel.Size = new System.Drawing.Size(42, 32);
            this.RotationXLabel.TabIndex = 17;
            this.RotationXLabel.Text = "x :";
            this.RotationXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RotationLabel
            // 
            this.RotationLabel.AutoSize = true;
            this.RotationLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationLabel.ForeColor = System.Drawing.Color.White;
            this.RotationLabel.Location = new System.Drawing.Point(10, 187);
            this.RotationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.RotationLabel.Name = "RotationLabel";
            this.RotationLabel.Size = new System.Drawing.Size(113, 32);
            this.RotationLabel.TabIndex = 16;
            this.RotationLabel.Text = "Rotation";
            this.RotationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScaleZLabel
            // 
            this.ScaleZLabel.AutoSize = true;
            this.ScaleZLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleZLabel.ForeColor = System.Drawing.Color.White;
            this.ScaleZLabel.Location = new System.Drawing.Point(330, 327);
            this.ScaleZLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ScaleZLabel.Name = "ScaleZLabel";
            this.ScaleZLabel.Size = new System.Drawing.Size(41, 32);
            this.ScaleZLabel.TabIndex = 12;
            this.ScaleZLabel.Text = "z :";
            this.ScaleZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScaleYLabel
            // 
            this.ScaleYLabel.AutoSize = true;
            this.ScaleYLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleYLabel.ForeColor = System.Drawing.Color.White;
            this.ScaleYLabel.Location = new System.Drawing.Point(170, 327);
            this.ScaleYLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ScaleYLabel.Name = "ScaleYLabel";
            this.ScaleYLabel.Size = new System.Drawing.Size(42, 32);
            this.ScaleYLabel.TabIndex = 11;
            this.ScaleYLabel.Text = "y :";
            this.ScaleYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScaleXLabel
            // 
            this.ScaleXLabel.AutoSize = true;
            this.ScaleXLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleXLabel.ForeColor = System.Drawing.Color.White;
            this.ScaleXLabel.Location = new System.Drawing.Point(10, 327);
            this.ScaleXLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ScaleXLabel.Name = "ScaleXLabel";
            this.ScaleXLabel.Size = new System.Drawing.Size(42, 32);
            this.ScaleXLabel.TabIndex = 10;
            this.ScaleXLabel.Text = "x :";
            this.ScaleXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.AutoSize = true;
            this.ScaleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScaleLabel.ForeColor = System.Drawing.Color.White;
            this.ScaleLabel.Location = new System.Drawing.Point(10, 283);
            this.ScaleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(206, 32);
            this.ScaleLabel.TabIndex = 9;
            this.ScaleLabel.Text = "Facteur d\'échelle";
            this.ScaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Informations_Tutoriel
            // 
            this.Informations_Tutoriel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Informations_Tutoriel.ForeColor = System.Drawing.Color.White;
            this.Informations_Tutoriel.Name = "Informations_Tutoriel";
            this.Informations_Tutoriel.Size = new System.Drawing.Size(268, 38);
            this.Informations_Tutoriel.Text = "Tutoriel";
            // 
            // Editeur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1888, 1310);
            this.Controls.Add(this.Edition);
            this.MainMenuStrip = this.MenuBar;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Editeur";
            this.Text = "Jeu";
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            this.Edition.ResumeLayout(false);
            this.Edition.PerformLayout();
            this.userPanel.ResumeLayout(false);
            this.Panel_PropertiesBack.ResumeLayout(false);
            this.PropertiesEditPanel.ResumeLayout(false);
            this.PropertiesEditPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleXText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleYText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationYText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationXText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_Fichier;
        private System.Windows.Forms.ToolStripMenuItem Fichier_Nouveau;
        private System.Windows.Forms.ToolStripMenuItem Fichier_Ouvrir;
        private System.Windows.Forms.ToolStripMenuItem Fichier_Enregistrer;
        private System.Windows.Forms.ToolStripMenuItem Fichier_EnregistrerSous;
        private System.Windows.Forms.ToolStripMenuItem Fichier_Propriete;
        private System.Windows.Forms.ToolStripMenuItem Fichier_ModeTest;
        private System.Windows.Forms.ToolStripMenuItem Fichier_MenuPrincipal;
        private System.Windows.Forms.ToolStripMenuItem Menu_Edition;
        private System.Windows.Forms.ToolStripMenuItem Edition_Supprimer;
        private System.Windows.Forms.ToolStripMenuItem Menu_Outils;
        private System.Windows.Forms.ToolStripMenuItem Outils_Selection;
        private System.Windows.Forms.ToolStripMenuItem Outils_Deplacement;
        private System.Windows.Forms.ToolStripMenuItem Outils_Rotation;
        private System.Windows.Forms.ToolStripMenuItem Outils_Echelle;
        private System.Windows.Forms.ToolStripMenuItem Outils_Duplication;
        private System.Windows.Forms.ToolStripMenuItem Outils_Zoom;
        private System.Windows.Forms.ToolStripMenuItem Outils_Creation;
        private System.Windows.Forms.ToolStripMenuItem Outils_Creation_Accelerateur;
        private System.Windows.Forms.ToolStripMenuItem Outils_Creation_Muret;
        private System.Windows.Forms.ToolStripMenuItem Outils_Creation_Portail;
        private System.Windows.Forms.ToolStripMenuItem Outils_PointsControles;
        private System.Windows.Forms.ToolStripMenuItem Menu_Vues;
        private System.Windows.Forms.ToolStripMenuItem Vues_Orthographique;
        private System.Windows.Forms.ToolStripMenuItem Vue_Orbite;
        private System.Windows.Forms.ToolStripMenuItem Menu_Informations;
        private System.Windows.Forms.ToolStripMenuItem Informations_Aide;
        private System.Windows.Forms.ToolStrip Toolbar;
        private System.Windows.Forms.ToolStripSeparator Toolbar_SeparatorTop;
        private System.Windows.Forms.ToolStripButton Toolbar_Select;
        private System.Windows.Forms.ToolStripButton Toolbar_Move;
        private System.Windows.Forms.ToolStripButton Toolbar_Rotate;
        private System.Windows.Forms.ToolStripButton Toolbar_Scale;
        private System.Windows.Forms.ToolStripButton Toolbar_Duplicate;
        private System.Windows.Forms.ToolStripButton Toolbar_Zoom;
        private System.Windows.Forms.ToolStripButton Toolbar_ControlPoint;
        private System.Windows.Forms.ToolStripSeparator Toolbar_SeparatorMiddle;
        private System.Windows.Forms.ToolStripButton Toolbar_Booster;
        private System.Windows.Forms.ToolStripButton Toolbar_Wall;
        private System.Windows.Forms.ToolStripButton Toolbar_Portal;
        private System.Windows.Forms.ToolStripSeparator Toolbar_SeparatorBottom;
        private System.Windows.Forms.Panel Edition;
        private System.Windows.Forms.ToolStripMenuItem Fichier_EnregistrerSous_Ordinateur;
        private System.Windows.Forms.ToolStripMenuItem Fichier_EnregistrerSous_Serveur;
        private System.Windows.Forms.ToolStripMenuItem Fichier_OuvrirLocalement;
        private System.Windows.Forms.ToolStripMenuItem Fichier_OuvrirEnLigne;
        private System.Windows.Forms.Panel userPanel;
        private System.Windows.Forms.Integration.ElementHost userLists;
        private System.Windows.Forms.Panel Panel_PropertiesBack;
        private System.Windows.Forms.Panel PropertiesEditPanel;
        private System.Windows.Forms.NumericUpDown ScaleZText;
        private System.Windows.Forms.NumericUpDown ScaleXText;
        private System.Windows.Forms.NumericUpDown ScaleYText;
        private System.Windows.Forms.NumericUpDown RotationZText;
        private System.Windows.Forms.NumericUpDown RotationYText;
        private System.Windows.Forms.NumericUpDown RotationXText;
        private System.Windows.Forms.NumericUpDown PositionZText;
        private System.Windows.Forms.NumericUpDown PositionYText;
        private System.Windows.Forms.NumericUpDown PositionXText;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label PropertiesTitle;
        private System.Windows.Forms.Label PositionZLabel;
        private System.Windows.Forms.Label PositionYLabel;
        private System.Windows.Forms.Label PositionXLabel;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Label RotationZLabel;
        private System.Windows.Forms.Label RotationYLabel;
        private System.Windows.Forms.Label RotationXLabel;
        private System.Windows.Forms.Label RotationLabel;
        private System.Windows.Forms.Label ScaleZLabel;
        private System.Windows.Forms.Label ScaleYLabel;
        private System.Windows.Forms.Label ScaleXLabel;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.ToolStripMenuItem Informations_Tutoriel;
    }
}