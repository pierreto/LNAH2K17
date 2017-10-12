namespace InterfaceGraphique {
    partial class TestMode {
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
            this.Panel_InGame = new System.Windows.Forms.Panel();
            this.MenuStrip_MenuBar = new System.Windows.Forms.MenuStrip();
            this.Menu_Fichier = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_EditorMode = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_MainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Vues = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_OrthoView = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_OrbitView = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Informations = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel_InGame.SuspendLayout();
            this.MenuStrip_MenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_InGame
            // 
            this.Panel_InGame.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Panel_InGame.Controls.Add(this.MenuStrip_MenuBar);
            this.Panel_InGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_InGame.Location = new System.Drawing.Point(0, 0);
            this.Panel_InGame.Name = "Panel_InGame";
            this.Panel_InGame.Size = new System.Drawing.Size(784, 561);
            this.Panel_InGame.TabIndex = 1;
            this.Panel_InGame.Visible = false;
            // 
            // MenuStrip_MenuBar
            // 
            this.MenuStrip_MenuBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuStrip_MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Fichier,
            this.Menu_Vues,
            this.Menu_Informations});
            this.MenuStrip_MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip_MenuBar.Name = "MenuStrip_MenuBar";
            this.MenuStrip_MenuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MenuStrip_MenuBar.Size = new System.Drawing.Size(784, 24);
            this.MenuStrip_MenuBar.TabIndex = 2;
            this.MenuStrip_MenuBar.Text = "menuStrip1";
            // 
            // Menu_Fichier
            // 
            this.Menu_Fichier.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_EditorMode,
            this.MenuItem_MainMenu});
            this.Menu_Fichier.ForeColor = System.Drawing.Color.White;
            this.Menu_Fichier.Name = "Menu_Fichier";
            this.Menu_Fichier.Size = new System.Drawing.Size(54, 20);
            this.Menu_Fichier.Text = "Fichier";
            // 
            // MenuItem_EditorMode
            // 
            this.MenuItem_EditorMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuItem_EditorMode.ForeColor = System.Drawing.Color.White;
            this.MenuItem_EditorMode.Name = "MenuItem_EditorMode";
            this.MenuItem_EditorMode.Size = new System.Drawing.Size(154, 22);
            this.MenuItem_EditorMode.Text = "Mode éditeur";
            // 
            // MenuItem_MainMenu
            // 
            this.MenuItem_MainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuItem_MainMenu.ForeColor = System.Drawing.Color.White;
            this.MenuItem_MainMenu.Name = "MenuItem_MainMenu";
            this.MenuItem_MainMenu.Size = new System.Drawing.Size(154, 22);
            this.MenuItem_MainMenu.Text = "Menu principal";
            // 
            // Menu_Vues
            // 
            this.Menu_Vues.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_OrthoView,
            this.MenuItem_OrbitView});
            this.Menu_Vues.ForeColor = System.Drawing.Color.White;
            this.Menu_Vues.Name = "Menu_Vues";
            this.Menu_Vues.Size = new System.Drawing.Size(44, 20);
            this.Menu_Vues.Text = "Vues";
            // 
            // MenuItem_OrthoView
            // 
            this.MenuItem_OrthoView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuItem_OrthoView.ForeColor = System.Drawing.Color.White;
            this.MenuItem_OrthoView.Name = "MenuItem_OrthoView";
            this.MenuItem_OrthoView.Size = new System.Drawing.Size(159, 22);
            this.MenuItem_OrthoView.Text = "Orthographique";
            // 
            // MenuItem_OrbitView
            // 
            this.MenuItem_OrbitView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuItem_OrbitView.Enabled = false;
            this.MenuItem_OrbitView.ForeColor = System.Drawing.Color.White;
            this.MenuItem_OrbitView.Name = "MenuItem_OrbitView";
            this.MenuItem_OrbitView.Size = new System.Drawing.Size(159, 22);
            this.MenuItem_OrbitView.Text = "Orbite";
            // 
            // Menu_Informations
            // 
            this.Menu_Informations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Help});
            this.Menu_Informations.ForeColor = System.Drawing.Color.White;
            this.Menu_Informations.Name = "Menu_Informations";
            this.Menu_Informations.Size = new System.Drawing.Size(87, 20);
            this.Menu_Informations.Text = "Informations";
            // 
            // MenuItem_Help
            // 
            this.MenuItem_Help.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MenuItem_Help.ForeColor = System.Drawing.Color.White;
            this.MenuItem_Help.Name = "MenuItem_Help";
            this.MenuItem_Help.Size = new System.Drawing.Size(98, 22);
            this.MenuItem_Help.Text = "Aide";
            // 
            // TestMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.Panel_InGame);
            this.Name = "TestMode";
            this.Text = "TestMode";
            this.Panel_InGame.ResumeLayout(false);
            this.Panel_InGame.PerformLayout();
            this.MenuStrip_MenuBar.ResumeLayout(false);
            this.MenuStrip_MenuBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_InGame;
        private System.Windows.Forms.MenuStrip MenuStrip_MenuBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_Fichier;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_EditorMode;
        private System.Windows.Forms.ToolStripMenuItem Menu_Vues;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_OrthoView;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_OrbitView;
        private System.Windows.Forms.ToolStripMenuItem Menu_Informations;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Help;
    }
}