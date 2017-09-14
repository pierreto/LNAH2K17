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
            this.loginName = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.loginNameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.serverAdress = new System.Windows.Forms.TextBox();
            this.serverAdressLabel = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginName
            // 
            this.loginName.Location = new System.Drawing.Point(125, 137);
            this.loginName.Name = "loginName";
            this.loginName.Size = new System.Drawing.Size(100, 20);
            this.loginName.TabIndex = 0;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(125, 166);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 1;
            // 
            // loginNameLabel
            // 
            this.loginNameLabel.AutoSize = true;
            this.loginNameLabel.Location = new System.Drawing.Point(51, 140);
            this.loginNameLabel.Name = "loginNameLabel";
            this.loginNameLabel.Size = new System.Drawing.Size(68, 13);
            this.loginNameLabel.TabIndex = 2;
            this.loginNameLabel.Text = "Pseudonyme";
            this.loginNameLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(48, 169);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(71, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Mot de passe";
            // 
            // serverAdress
            // 
            this.serverAdress.Location = new System.Drawing.Point(125, 102);
            this.serverAdress.Name = "serverAdress";
            this.serverAdress.Size = new System.Drawing.Size(100, 20);
            this.serverAdress.TabIndex = 4;
            this.serverAdress.TextChanged += new System.EventHandler(this.serverAdress_TextChanged);
            // 
            // serverAdressLabel
            // 
            this.serverAdressLabel.AutoSize = true;
            this.serverAdressLabel.Location = new System.Drawing.Point(21, 105);
            this.serverAdressLabel.Name = "serverAdressLabel";
            this.serverAdressLabel.Size = new System.Drawing.Size(98, 13);
            this.serverAdressLabel.TabIndex = 5;
            this.serverAdressLabel.Text = "Adresse du serveur";
            this.serverAdressLabel.Click += new System.EventHandler(this.label3_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(125, 212);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(107, 23);
            this.LoginButton.TabIndex = 6;
            this.LoginButton.Text = "Se connecter";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.serverAdressLabel);
            this.Controls.Add(this.serverAdress);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.loginNameLabel);
            this.Controls.Add(this.password);
            this.Controls.Add(this.loginName);
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginName;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label loginNameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox serverAdress;
        private System.Windows.Forms.Label serverAdressLabel;
        private System.Windows.Forms.Button LoginButton;
    }
}