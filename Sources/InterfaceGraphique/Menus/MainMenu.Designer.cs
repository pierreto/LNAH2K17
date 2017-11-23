using InterfaceGraphique.Controls;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Friends;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique
{
    partial class MainMenu
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.boutonPartieRapide = new System.Windows.Forms.Button();
            this.boutonTournoi = new System.Windows.Forms.Button();
            this.buttonConfiguration = new System.Windows.Forms.Button();
            this.buttonEditeur = new System.Windows.Forms.Button();
            this.buttonQuitter = new System.Windows.Forms.Button();
            this.Button_Credits = new System.Windows.Forms.Button();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.profileButton = new System.Windows.Forms.Button();
            this.storeButton = new System.Windows.Forms.Button();
            this.TutorielEditeur = new System.Windows.Forms.Button();
            this.TutorielGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // boutonPartieRapide
            // 
            this.boutonPartieRapide.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.boutonPartieRapide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boutonPartieRapide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boutonPartieRapide.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boutonPartieRapide.ForeColor = System.Drawing.Color.Red;
            this.boutonPartieRapide.Location = new System.Drawing.Point(49, 155);
            this.boutonPartieRapide.Name = "boutonPartieRapide";
            this.boutonPartieRapide.Size = new System.Drawing.Size(150, 50);
            this.boutonPartieRapide.TabIndex = 1;
            this.boutonPartieRapide.TabStop = false;
            this.boutonPartieRapide.Text = "Partie Rapide";
            this.boutonPartieRapide.UseVisualStyleBackColor = true;
            // 
            // boutonTournoi
            // 
            this.boutonTournoi.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.boutonTournoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boutonTournoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boutonTournoi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boutonTournoi.ForeColor = System.Drawing.Color.White;
            this.boutonTournoi.Location = new System.Drawing.Point(205, 155);
            this.boutonTournoi.Name = "boutonTournoi";
            this.boutonTournoi.Size = new System.Drawing.Size(150, 50);
            this.boutonTournoi.TabIndex = 2;
            this.boutonTournoi.TabStop = false;
            this.boutonTournoi.Text = "Tournoi";
            this.boutonTournoi.UseVisualStyleBackColor = true;
            // 
            // buttonConfiguration
            // 
            this.buttonConfiguration.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonConfiguration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonConfiguration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConfiguration.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConfiguration.ForeColor = System.Drawing.Color.Red;
            this.buttonConfiguration.Location = new System.Drawing.Point(361, 155);
            this.buttonConfiguration.Name = "buttonConfiguration";
            this.buttonConfiguration.Size = new System.Drawing.Size(150, 50);
            this.buttonConfiguration.TabIndex = 3;
            this.buttonConfiguration.TabStop = false;
            this.buttonConfiguration.Text = "Configuration";
            this.buttonConfiguration.UseVisualStyleBackColor = true;
            // 
            // buttonEditeur
            // 
            this.buttonEditeur.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonEditeur.AutoEllipsis = true;
            this.buttonEditeur.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonEditeur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditeur.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEditeur.ForeColor = System.Drawing.Color.White;
            this.buttonEditeur.Location = new System.Drawing.Point(517, 155);
            this.buttonEditeur.Name = "buttonEditeur";
            this.buttonEditeur.Size = new System.Drawing.Size(150, 50);
            this.buttonEditeur.TabIndex = 4;
            this.buttonEditeur.TabStop = false;
            this.buttonEditeur.Text = "Éditeur";
            this.buttonEditeur.UseVisualStyleBackColor = true;
            // 
            // buttonQuitter
            // 
            this.buttonQuitter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonQuitter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonQuitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQuitter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuitter.ForeColor = System.Drawing.Color.Red;
            this.buttonQuitter.Location = new System.Drawing.Point(985, 155);
            this.buttonQuitter.Name = "buttonQuitter";
            this.buttonQuitter.Size = new System.Drawing.Size(150, 50);
            this.buttonQuitter.TabIndex = 5;
            this.buttonQuitter.TabStop = false;
            this.buttonQuitter.Text = "Quitter";
            this.buttonQuitter.UseVisualStyleBackColor = true;
            // 
            // Button_Credits
            // 
            this.Button_Credits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_Credits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_Credits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Credits.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Credits.ForeColor = System.Drawing.Color.Red;
            this.Button_Credits.Location = new System.Drawing.Point(673, 155);
            this.Button_Credits.Name = "Button_Credits";
            this.Button_Credits.Size = new System.Drawing.Size(150, 50);
            this.Button_Credits.TabIndex = 7;
            this.Button_Credits.TabStop = false;
            this.Button_Credits.Text = "Crédits";
            this.Button_Credits.UseVisualStyleBackColor = true;
            // 
            // buttonLogout
            // 
            this.buttonLogout.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogout.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogout.ForeColor = System.Drawing.Color.White;
            this.buttonLogout.Location = new System.Drawing.Point(829, 155);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(150, 50);
            this.buttonLogout.TabIndex = 9;
            this.buttonLogout.TabStop = false;
            this.buttonLogout.Text = "Déconnexion";
            this.buttonLogout.UseVisualStyleBackColor = true;
            // 
            // profileButton
            // 
            this.profileButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.profileButton.AutoEllipsis = true;
            this.profileButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.profileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.profileButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profileButton.ForeColor = System.Drawing.Color.White;
            this.profileButton.Location = new System.Drawing.Point(517, 50);
            this.profileButton.Name = "profileButton";
            this.profileButton.Size = new System.Drawing.Size(150, 50);
            this.profileButton.TabIndex = 4;
            this.profileButton.TabStop = false;
            this.profileButton.Text = "Profil";
            this.profileButton.UseVisualStyleBackColor = true;
            // 
            // storeButton
            // 
            this.storeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.storeButton.AutoEllipsis = true;
            this.storeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.storeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.storeButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeButton.ForeColor = System.Drawing.Color.White;
            this.storeButton.Location = new System.Drawing.Point(985, 50);
            this.storeButton.Name = "storeButton";
            this.storeButton.Size = new System.Drawing.Size(150, 50);
            this.storeButton.TabIndex = 4;
            this.storeButton.TabStop = false;
            this.storeButton.Text = "Magasin";
            this.storeButton.UseVisualStyleBackColor = true;
            // 
            // TutorielEditeur
            // 
            this.TutorielEditeur.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TutorielEditeur.AutoEllipsis = true;
            this.TutorielEditeur.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TutorielEditeur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TutorielEditeur.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TutorielEditeur.ForeColor = System.Drawing.Color.White;
            this.TutorielEditeur.Location = new System.Drawing.Point(497, 244);
            this.TutorielEditeur.Name = "TutorielEditeur";
            this.TutorielEditeur.Size = new System.Drawing.Size(201, 50);
            this.TutorielEditeur.TabIndex = 10;
            this.TutorielEditeur.TabStop = false;
            this.TutorielEditeur.Text = "Tutoriel: Édition";
            this.TutorielEditeur.UseVisualStyleBackColor = true;
            // 
            // TutorielGame
            // 
            this.TutorielGame.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TutorielGame.AutoEllipsis = true;
            this.TutorielGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.TutorielGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TutorielGame.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TutorielGame.ForeColor = System.Drawing.Color.White;
            this.TutorielGame.Location = new System.Drawing.Point(31, 244);
            this.TutorielGame.Name = "TutorielGame";
            this.TutorielGame.Size = new System.Drawing.Size(201, 50);
            this.TutorielGame.TabIndex = 11;
            this.TutorielGame.TabStop = false;
            this.TutorielGame.Text = "Tutoriel: Partie en ligne";
            this.TutorielGame.UseVisualStyleBackColor = true;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1184, 648);
            this.Controls.Add(this.TutorielGame);
            this.Controls.Add(this.TutorielEditeur);
            this.Controls.Add(this.buttonLogout);
            this.Controls.Add(this.Button_Credits);
            this.Controls.Add(this.buttonQuitter);
            this.Controls.Add(this.buttonEditeur);
            this.Controls.Add(this.buttonConfiguration);
            this.Controls.Add(this.boutonTournoi);
            this.Controls.Add(this.boutonPartieRapide);
            this.Controls.Add(this.profileButton);
            this.Controls.Add(this.storeButton);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button boutonPartieRapide;
        private System.Windows.Forms.Button boutonTournoi;
        private System.Windows.Forms.Button buttonConfiguration;
        private System.Windows.Forms.Button buttonEditeur;
        private System.Windows.Forms.Button buttonQuitter;
        private System.Windows.Forms.Button Button_Credits;
        private System.Windows.Forms.Button profileButton;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Button storeButton;
        private Controls.WPF.Chat.TestChatView testChatView1;
        private FriendContentControl hostedComponent1;
        private System.Windows.Forms.Button TutorielEditeur;
        private System.Windows.Forms.Button TutorielGame;
    }
}