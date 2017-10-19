namespace InterfaceGraphique.Menus
{
    partial class ConnectServerMenu
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
            this.IpAddressInput = new System.Windows.Forms.TextBox();
            this.ipAddressErrorLabel = new System.Windows.Forms.Label();
            this.ConnectServerButton = new System.Windows.Forms.Button();
            this.ipAddressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ipAddressInput
            // 
            this.IpAddressInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.IpAddressInput.Location = new System.Drawing.Point(133, 72);
            this.IpAddressInput.Name = "ipAddressInput";
            this.IpAddressInput.Size = new System.Drawing.Size(100, 20);
            this.IpAddressInput.TabIndex = 0;
            // 
            // ipAddressErrorLabel
            // 
            this.ipAddressErrorLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ipAddressErrorLabel.AutoSize = true;
            this.ipAddressErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.ipAddressErrorLabel.Location = new System.Drawing.Point(133, 99);
            this.ipAddressErrorLabel.Name = "ipAddressErrorLabel";
            this.ipAddressErrorLabel.Size = new System.Drawing.Size(75, 13);
            this.ipAddressErrorLabel.TabIndex = 1;
            this.ipAddressErrorLabel.Text = "Error Message";
            // 
            // connectServerButton
            // 
            this.ConnectServerButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConnectServerButton.Location = new System.Drawing.Point(109, 127);
            this.ConnectServerButton.Name = "connectServerButton";
            this.ConnectServerButton.Size = new System.Drawing.Size(99, 23);
            this.ConnectServerButton.TabIndex = 2;
            this.ConnectServerButton.Text = "Se connecter";
            this.ConnectServerButton.UseVisualStyleBackColor = true;
            // 
            // ipAddressLabel
            // 
            this.ipAddressLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ipAddressLabel.AutoSize = true;
            this.ipAddressLabel.Location = new System.Drawing.Point(63, 75);
            this.ipAddressLabel.Name = "ipAddressLabel";
            this.ipAddressLabel.Size = new System.Drawing.Size(64, 13);
            this.ipAddressLabel.TabIndex = 3;
            this.ipAddressLabel.Text = "Adresse IP :";
            // 
            // ConnectServerMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ipAddressLabel);
            this.Controls.Add(this.ConnectServerButton);
            this.Controls.Add(this.ipAddressErrorLabel);
            this.Controls.Add(this.IpAddressInput);
            this.Name = "ConnectServerMenu";
            this.Text = "ConnectServerMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IpAddressInput;
        private System.Windows.Forms.Label ipAddressErrorLabel;
        private System.Windows.Forms.Button ConnectServerButton;
        private System.Windows.Forms.Label ipAddressLabel;
    }
}