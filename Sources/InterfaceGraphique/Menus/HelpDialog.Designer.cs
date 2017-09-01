namespace InterfaceGraphique {
    partial class EditorHelp {
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
            this.Panel_FillColor = new System.Windows.Forms.Panel();
            this.HelpText = new System.Windows.Forms.Label();
            this.Panel_FillColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_FillColor
            // 
            this.Panel_FillColor.AutoSize = true;
            this.Panel_FillColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_FillColor.Controls.Add(this.HelpText);
            this.Panel_FillColor.Location = new System.Drawing.Point(3, 3);
            this.Panel_FillColor.Name = "Panel_FillColor";
            this.Panel_FillColor.Size = new System.Drawing.Size(278, 255);
            this.Panel_FillColor.TabIndex = 0;
            // 
            // HelpText
            // 
            this.HelpText.AutoSize = true;
            this.HelpText.Font = new System.Drawing.Font("Courier New", 9F);
            this.HelpText.ForeColor = System.Drawing.Color.White;
            this.HelpText.Location = new System.Drawing.Point(5, 5);
            this.HelpText.Margin = new System.Windows.Forms.Padding(0);
            this.HelpText.Name = "HelpText";
            this.HelpText.Padding = new System.Windows.Forms.Padding(15);
            this.HelpText.Size = new System.Drawing.Size(177, 45);
            this.HelpText.TabIndex = 0;
            this.HelpText.Text = "** Help text here **";
            // 
            // EditorHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Panel_FillColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpDialog";
            this.Panel_FillColor.ResumeLayout(false);
            this.Panel_FillColor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel_FillColor;
        private System.Windows.Forms.Label HelpText;
    }
}