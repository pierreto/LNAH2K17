﻿namespace InterfaceGraphique.Controls
{
    partial class BrowserHost
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
            this.browser = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(718, 529);
            this.browser.TabIndex = 0;
            this.browser.Text = "browser";
            this.browser.Child = null;
            // 
            // BrowserHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 529);
            this.Controls.Add(this.browser);
            this.Name = "BrowserHost";
            this.Text = "BrowserHost";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Integration.ElementHost browser;
    }
}