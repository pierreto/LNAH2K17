namespace InterfaceGraphique {
    partial class QuickPlay {
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
            this.Panel_EndBack = new System.Windows.Forms.Panel();
            this.Panel_EndFront = new System.Windows.Forms.Panel();
            this.pointsNb = new System.Windows.Forms.Label();
            this.pointsLabel = new System.Windows.Forms.Label();
            this.Label_ScoreText = new System.Windows.Forms.Label();
            this.Button_MainMenu = new System.Windows.Forms.Button();
            this.Button_PlayAgain = new System.Windows.Forms.Button();
            this.Label_Score = new System.Windows.Forms.Label();
            this.Label_ScoreSeparator = new System.Windows.Forms.Label();
            this.Label_GameEnded = new System.Windows.Forms.Label();
            this.MenuStrip_MenuBar = new System.Windows.Forms.MenuStrip();
            this.Menu_Fichier = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_MainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Vues = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_OrthoView = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_OrbitView = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Informations = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel_InGame.SuspendLayout();
            this.Panel_EndBack.SuspendLayout();
            this.Panel_EndFront.SuspendLayout();
            this.MenuStrip_MenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_InGame
            // 
            this.Panel_InGame.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Panel_InGame.Controls.Add(this.Panel_EndBack);
            this.Panel_InGame.Controls.Add(this.MenuStrip_MenuBar);
            this.Panel_InGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_InGame.Location = new System.Drawing.Point(0, 0);
            this.Panel_InGame.Name = "Panel_InGame";
            this.Panel_InGame.Size = new System.Drawing.Size(784, 561);
            this.Panel_InGame.TabIndex = 0;
            this.Panel_InGame.Visible = false;
            // 
            // Panel_EndBack
            // 
            this.Panel_EndBack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Panel_EndBack.BackColor = System.Drawing.Color.DarkViolet;
            this.Panel_EndBack.Controls.Add(this.Panel_EndFront);
            this.Panel_EndBack.Location = new System.Drawing.Point(229, 207);
            this.Panel_EndBack.Name = "Panel_EndBack";
            this.Panel_EndBack.Size = new System.Drawing.Size(325, 211);
            this.Panel_EndBack.TabIndex = 30;
            // 
            // Panel_EndFront
            // 
            this.Panel_EndFront.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_EndFront.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_EndFront.Controls.Add(this.pointsNb);
            this.Panel_EndFront.Controls.Add(this.pointsLabel);
            this.Panel_EndFront.Controls.Add(this.Label_ScoreText);
            this.Panel_EndFront.Controls.Add(this.Button_MainMenu);
            this.Panel_EndFront.Controls.Add(this.Button_PlayAgain);
            this.Panel_EndFront.Controls.Add(this.Label_Score);
            this.Panel_EndFront.Controls.Add(this.Label_ScoreSeparator);
            this.Panel_EndFront.Controls.Add(this.Label_GameEnded);
            this.Panel_EndFront.Location = new System.Drawing.Point(3, 1);
            this.Panel_EndFront.Name = "Panel_EndFront";
            this.Panel_EndFront.Size = new System.Drawing.Size(319, 205);
            this.Panel_EndFront.TabIndex = 29;
            // 
            // pointsNb
            // 
            this.pointsNb.AutoSize = true;
            this.pointsNb.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointsNb.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.pointsNb.Location = new System.Drawing.Point(194, 86);
            this.pointsNb.Name = "pointsNb";
            this.pointsNb.Size = new System.Drawing.Size(41, 30);
            this.pointsNb.TabIndex = 35;
            this.pointsNb.Text = "+0";
            this.pointsNb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pointsNb.Visible = false;
            // 
            // pointsLabel
            // 
            this.pointsLabel.AutoSize = true;
            this.pointsLabel.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointsLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.pointsLabel.Location = new System.Drawing.Point(60, 85);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Size = new System.Drawing.Size(109, 30);
            this.pointsLabel.TabIndex = 34;
            this.pointsLabel.Text = "Points    :";
            this.pointsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label_ScoreText
            // 
            this.Label_ScoreText.AutoSize = true;
            this.Label_ScoreText.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_ScoreText.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Label_ScoreText.Location = new System.Drawing.Point(60, 55);
            this.Label_ScoreText.Name = "Label_ScoreText";
            this.Label_ScoreText.Size = new System.Drawing.Size(80, 30);
            this.Label_ScoreText.TabIndex = 33;
            this.Label_ScoreText.Text = "SCORE";
            this.Label_ScoreText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Button_MainMenu
            // 
            this.Button_MainMenu.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_MainMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_MainMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_MainMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_MainMenu.ForeColor = System.Drawing.Color.White;
            this.Button_MainMenu.Location = new System.Drawing.Point(106, 128);
            this.Button_MainMenu.Name = "Button_MainMenu";
            this.Button_MainMenu.Size = new System.Drawing.Size(100, 30);
            this.Button_MainMenu.TabIndex = 32;
            this.Button_MainMenu.TabStop = false;
            this.Button_MainMenu.Text = "Menu principal";
            this.Button_MainMenu.UseVisualStyleBackColor = true;
            // 
            // Button_PlayAgain
            // 
            this.Button_PlayAgain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_PlayAgain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_PlayAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_PlayAgain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_PlayAgain.ForeColor = System.Drawing.Color.White;
            this.Button_PlayAgain.Location = new System.Drawing.Point(106, 164);
            this.Button_PlayAgain.Name = "Button_PlayAgain";
            this.Button_PlayAgain.Size = new System.Drawing.Size(100, 30);
            this.Button_PlayAgain.TabIndex = 31;
            this.Button_PlayAgain.TabStop = false;
            this.Button_PlayAgain.Text = "Rejouer";
            this.Button_PlayAgain.UseVisualStyleBackColor = true;
            // 
            // Label_Score
            // 
            this.Label_Score.AutoSize = true;
            this.Label_Score.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Score.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Label_Score.Location = new System.Drawing.Point(187, 55);
            this.Label_Score.Name = "Label_Score";
            this.Label_Score.Size = new System.Drawing.Size(60, 30);
            this.Label_Score.TabIndex = 30;
            this.Label_Score.Text = "0 - 0";
            this.Label_Score.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label_ScoreSeparator
            // 
            this.Label_ScoreSeparator.AutoSize = true;
            this.Label_ScoreSeparator.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_ScoreSeparator.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Label_ScoreSeparator.Location = new System.Drawing.Point(149, 55);
            this.Label_ScoreSeparator.Margin = new System.Windows.Forms.Padding(0);
            this.Label_ScoreSeparator.Name = "Label_ScoreSeparator";
            this.Label_ScoreSeparator.Size = new System.Drawing.Size(26, 30);
            this.Label_ScoreSeparator.TabIndex = 29;
            this.Label_ScoreSeparator.Text = ": ";
            this.Label_ScoreSeparator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_GameEnded
            // 
            this.Label_GameEnded.AutoSize = true;
            this.Label_GameEnded.Font = new System.Drawing.Font("Segoe UI Black", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_GameEnded.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Label_GameEnded.Location = new System.Drawing.Point(5, 5);
            this.Label_GameEnded.Name = "Label_GameEnded";
            this.Label_GameEnded.Size = new System.Drawing.Size(310, 45);
            this.Label_GameEnded.TabIndex = 28;
            this.Label_GameEnded.Text = "PARTIE TERMINÉE";
            this.Label_GameEnded.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.MenuStrip_MenuBar.TabIndex = 1;
            this.MenuStrip_MenuBar.Text = "menuStrip1";
            // 
            // Menu_Fichier
            // 
            this.Menu_Fichier.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_MainMenu});
            this.Menu_Fichier.ForeColor = System.Drawing.Color.White;
            this.Menu_Fichier.Name = "Menu_Fichier";
            this.Menu_Fichier.Size = new System.Drawing.Size(54, 20);
            this.Menu_Fichier.Text = "Fichier";
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
            // QuickPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.Panel_InGame);
            this.Name = "QuickPlay";
            this.Text = "InGame";
            this.Panel_InGame.ResumeLayout(false);
            this.Panel_InGame.PerformLayout();
            this.Panel_EndBack.ResumeLayout(false);
            this.Panel_EndFront.ResumeLayout(false);
            this.Panel_EndFront.PerformLayout();
            this.MenuStrip_MenuBar.ResumeLayout(false);
            this.MenuStrip_MenuBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_InGame;
        private System.Windows.Forms.MenuStrip MenuStrip_MenuBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_Fichier;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_MainMenu;
        private System.Windows.Forms.ToolStripMenuItem Menu_Vues;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_OrthoView;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_OrbitView;
        private System.Windows.Forms.ToolStripMenuItem Menu_Informations;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Help;
        private System.Windows.Forms.Panel Panel_EndBack;
        private System.Windows.Forms.Panel Panel_EndFront;
        private System.Windows.Forms.Label Label_ScoreText;
        private System.Windows.Forms.Button Button_MainMenu;
        private System.Windows.Forms.Button Button_PlayAgain;
        private System.Windows.Forms.Label Label_Score;
        private System.Windows.Forms.Label Label_ScoreSeparator;
        private System.Windows.Forms.Label Label_GameEnded;
        private System.Windows.Forms.Label pointsNb;
        private System.Windows.Forms.Label pointsLabel;
    }
}