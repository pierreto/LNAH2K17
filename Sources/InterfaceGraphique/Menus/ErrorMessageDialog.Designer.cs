namespace InterfaceGraphique {
    partial class ErrorMessageDialog {
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Button_Close = new System.Windows.Forms.Button();
            this.Label_WarningText = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.Button_Close);
            this.panel1.Controls.Add(this.Label_WarningText);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 130);
            this.panel1.TabIndex = 0;
            // 
            // Button_Close
            // 
            this.Button_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Close.ForeColor = System.Drawing.Color.White;
            this.Button_Close.Location = new System.Drawing.Point(114, 87);
            this.Button_Close.Name = "Button_Close";
            this.Button_Close.Size = new System.Drawing.Size(100, 30);
            this.Button_Close.TabIndex = 1;
            this.Button_Close.Text = "Fermer";
            this.Button_Close.UseVisualStyleBackColor = true;
            // 
            // Label_WarningText
            // 
            this.Label_WarningText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_WarningText.ForeColor = System.Drawing.Color.White;
            this.Label_WarningText.Location = new System.Drawing.Point(15, 15);
            this.Label_WarningText.Name = "Label_WarningText";
            this.Label_WarningText.Size = new System.Drawing.Size(298, 50);
            this.Label_WarningText.TabIndex = 0;
            this.Label_WarningText.Text = "** WARNING TEXT HERE **";
            this.Label_WarningText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ErrorMessageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(334, 136);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorMessageDialog";
            this.Text = "Configuration incomplète";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Button_Close;
        private System.Windows.Forms.Label Label_WarningText;
    }
}