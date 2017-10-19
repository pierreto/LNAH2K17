namespace InterfaceGraphique.Menus
{
    partial class HomeMenu
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.onlineButton = new System.Windows.Forms.Button();
            this.offlineButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(59, 59);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(166, 31);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "LNAH 2K17";
            // 
            // onlineButton
            // 
            this.onlineButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.onlineButton.Location = new System.Drawing.Point(93, 118);
            this.onlineButton.Name = "onlineButton";
            this.onlineButton.Size = new System.Drawing.Size(98, 23);
            this.onlineButton.TabIndex = 1;
            this.onlineButton.Text = "Mode En Ligne";
            this.onlineButton.UseVisualStyleBackColor = true;
            // 
            // offlineButton
            // 
            this.offlineButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.offlineButton.Location = new System.Drawing.Point(93, 161);
            this.offlineButton.Name = "offlineButton";
            this.offlineButton.Size = new System.Drawing.Size(98, 23);
            this.offlineButton.TabIndex = 2;
            this.offlineButton.Text = "Mode Hors Ligne";
            this.offlineButton.UseVisualStyleBackColor = true;
            // 
            // HomeMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.offlineButton);
            this.Controls.Add(this.onlineButton);
            this.Controls.Add(this.titleLabel);
            this.Name = "HomeMenu";
            this.Text = "HomeMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button onlineButton;
        private System.Windows.Forms.Button offlineButton;
    }
}