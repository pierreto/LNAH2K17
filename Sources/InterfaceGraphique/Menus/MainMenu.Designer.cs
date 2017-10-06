namespace InterfaceGraphique {
    partial class MainMenu {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.boutonPartieRapide = new System.Windows.Forms.Button();
            this.boutonTournoi = new System.Windows.Forms.Button();
            this.buttonConfiguration = new System.Windows.Forms.Button();
            this.buttonEditeur = new System.Windows.Forms.Button();
            this.buttonQuitter = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Button_Credits = new System.Windows.Forms.Button();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.chatView1 = new InterfaceGraphique.Controls.ChatView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // boutonPartieRapide
            // 
            this.boutonPartieRapide.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.boutonPartieRapide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boutonPartieRapide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.boutonPartieRapide.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boutonPartieRapide.ForeColor = System.Drawing.Color.Red;
            this.boutonPartieRapide.Location = new System.Drawing.Point(1022, 227);
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
            this.boutonTournoi.Location = new System.Drawing.Point(1022, 283);
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
            this.buttonConfiguration.Location = new System.Drawing.Point(1022, 339);
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
            this.buttonEditeur.Location = new System.Drawing.Point(1022, 407);
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
            this.buttonQuitter.Location = new System.Drawing.Point(1022, 584);
            this.buttonQuitter.Name = "buttonQuitter";
            this.buttonQuitter.Size = new System.Drawing.Size(150, 50);
            this.buttonQuitter.TabIndex = 5;
            this.buttonQuitter.TabStop = false;
            this.buttonQuitter.Text = "Quitter";
            this.buttonQuitter.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(-25, -35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(550, 300);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Button_Credits
            // 
            this.Button_Credits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button_Credits.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_Credits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Credits.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Credits.ForeColor = System.Drawing.Color.Red;
            this.Button_Credits.Location = new System.Drawing.Point(1022, 463);
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
            this.buttonLogout.Location = new System.Drawing.Point(1022, 528);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(150, 50);
            this.buttonLogout.TabIndex = 9;
            this.buttonLogout.TabStop = false;
            this.buttonLogout.Text = "Déconnexion";
            this.buttonLogout.UseVisualStyleBackColor = true;
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.elementHost1.Location = new System.Drawing.Point(12, 227);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1004, 407);
            this.elementHost1.TabIndex = 10;
            this.elementHost1.Text = "s";
            this.elementHost1.Child = this.chatView1;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1184, 648);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.buttonLogout);
            this.Controls.Add(this.Button_Credits);
            this.Controls.Add(this.buttonQuitter);
            this.Controls.Add(this.buttonEditeur);
            this.Controls.Add(this.buttonConfiguration);
            this.Controls.Add(this.boutonTournoi);
            this.Controls.Add(this.boutonPartieRapide);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button boutonPartieRapide;
        private System.Windows.Forms.Button boutonTournoi;
        private System.Windows.Forms.Button buttonConfiguration;
        private System.Windows.Forms.Button buttonEditeur;
        private System.Windows.Forms.Button buttonQuitter;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Button_Credits;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private Controls.ChatView chatView1;
    }
}