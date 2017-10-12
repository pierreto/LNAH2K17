namespace InterfaceGraphique.Menus
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.sonicPuckLabel = new System.Windows.Forms.PictureBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.Passwordlabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sonicPuckLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UsernameLabel.Location = new System.Drawing.Point(259, 355);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(227, 37);
            this.UsernameLabel.TabIndex = 0;
            this.UsernameLabel.Text = "Nom d\'usager";
            this.UsernameLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // ServerLabel
            // 
            this.ServerLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ServerLabel.Location = new System.Drawing.Point(170, 306);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(316, 37);
            this.ServerLabel.TabIndex = 1;
            this.ServerLabel.Text = "Adresse du serveur";
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ServerTextBox.Font = new System.Drawing.Font("Arial Narrow", 20F);
            this.ServerTextBox.Location = new System.Drawing.Point(492, 305);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(177, 38);
            this.ServerTextBox.TabIndex = 2;
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UsernameTextBox.Font = new System.Drawing.Font("Arial Narrow", 20F);
            this.UsernameTextBox.Location = new System.Drawing.Point(492, 354);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(177, 38);
            this.UsernameTextBox.TabIndex = 3;
            // 
            // LoginButton
            // 
            this.LoginButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LoginButton.BackColor = System.Drawing.Color.Brown;
            this.LoginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LoginButton.Font = new System.Drawing.Font("Arial Narrow", 24F);
            this.LoginButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LoginButton.Location = new System.Drawing.Point(491, 457);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(188, 49);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "Se connecter";
            this.LoginButton.UseVisualStyleBackColor = false;
            // 
            // sonicPuckLabel
            // 
            this.sonicPuckLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sonicPuckLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.sonicPuckLabel.Image = ((System.Drawing.Image)(resources.GetObject("sonicPuckLabel.Image")));
            this.sonicPuckLabel.ImageLocation = "";
            this.sonicPuckLabel.Location = new System.Drawing.Point(288, -2);
            this.sonicPuckLabel.Name = "sonicPuckLabel";
            this.sonicPuckLabel.Size = new System.Drawing.Size(550, 350);
            this.sonicPuckLabel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.sonicPuckLabel.TabIndex = 7;
            this.sonicPuckLabel.TabStop = false;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PasswordTextBox.Font = new System.Drawing.Font("Arial Narrow", 20F);
            this.PasswordTextBox.Location = new System.Drawing.Point(492, 401);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(177, 38);
            this.PasswordTextBox.TabIndex = 8;
            // 
            // Passwordlabel
            // 
            this.Passwordlabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Passwordlabel.AutoSize = true;
            this.Passwordlabel.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Passwordlabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Passwordlabel.Location = new System.Drawing.Point(266, 402);
            this.Passwordlabel.Name = "Passwordlabel";
            this.Passwordlabel.Size = new System.Drawing.Size(220, 37);
            this.Passwordlabel.TabIndex = 9;
            this.Passwordlabel.Text = "Mot de passe";
            this.Passwordlabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1030, 622);
            this.Controls.Add(this.Passwordlabel);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.sonicPuckLabel);
            this.Name = "Login";
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.sonicPuckLabel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.PictureBox sonicPuckLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label Passwordlabel;
    }
}