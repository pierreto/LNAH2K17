namespace InterfaceGraphique {
    partial class GeneralProperties {
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
            this.Input_CoefAcceleration = new System.Windows.Forms.NumericUpDown();
            this.Input_CoefRebound = new System.Windows.Forms.NumericUpDown();
            this.Input_CoefFriction = new System.Windows.Forms.NumericUpDown();
            this.Label_CoefAcceleration = new System.Windows.Forms.Label();
            this.Label_CoefRebound = new System.Windows.Forms.Label();
            this.Label_CoefFriction = new System.Windows.Forms.Label();
            this.Button_SaveChanges = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Panel_FillColor = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Input_CoefAcceleration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_CoefRebound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_CoefFriction)).BeginInit();
            this.SuspendLayout();
            // 
            // Input_CoefAcceleration
            // 
            this.Input_CoefAcceleration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_CoefAcceleration.DecimalPlaces = 2;
            this.Input_CoefAcceleration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_CoefAcceleration.ForeColor = System.Drawing.Color.White;
            this.Input_CoefAcceleration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Input_CoefAcceleration.Location = new System.Drawing.Point(290, 98);
            this.Input_CoefAcceleration.Name = "Input_CoefAcceleration";
            this.Input_CoefAcceleration.Size = new System.Drawing.Size(75, 23);
            this.Input_CoefAcceleration.TabIndex = 5;
            // 
            // Input_CoefRebound
            // 
            this.Input_CoefRebound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_CoefRebound.DecimalPlaces = 2;
            this.Input_CoefRebound.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_CoefRebound.ForeColor = System.Drawing.Color.White;
            this.Input_CoefRebound.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Input_CoefRebound.Location = new System.Drawing.Point(290, 63);
            this.Input_CoefRebound.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Input_CoefRebound.Name = "Input_CoefRebound";
            this.Input_CoefRebound.Size = new System.Drawing.Size(75, 23);
            this.Input_CoefRebound.TabIndex = 4;
            // 
            // Input_CoefFriction
            // 
            this.Input_CoefFriction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_CoefFriction.DecimalPlaces = 2;
            this.Input_CoefFriction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_CoefFriction.ForeColor = System.Drawing.Color.White;
            this.Input_CoefFriction.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.Input_CoefFriction.Location = new System.Drawing.Point(290, 28);
            this.Input_CoefFriction.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Input_CoefFriction.Name = "Input_CoefFriction";
            this.Input_CoefFriction.Size = new System.Drawing.Size(75, 23);
            this.Input_CoefFriction.TabIndex = 3;
            // 
            // Label_CoefAcceleration
            // 
            this.Label_CoefAcceleration.AutoSize = true;
            this.Label_CoefAcceleration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Label_CoefAcceleration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_CoefAcceleration.ForeColor = System.Drawing.Color.White;
            this.Label_CoefAcceleration.Location = new System.Drawing.Point(23, 100);
            this.Label_CoefAcceleration.Name = "Label_CoefAcceleration";
            this.Label_CoefAcceleration.Size = new System.Drawing.Size(189, 15);
            this.Label_CoefAcceleration.TabIndex = 2;
            this.Label_CoefAcceleration.Text = "Coefficient d\'accélération [0, 100] :";
            // 
            // Label_CoefRebound
            // 
            this.Label_CoefRebound.AutoSize = true;
            this.Label_CoefRebound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Label_CoefRebound.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_CoefRebound.ForeColor = System.Drawing.Color.White;
            this.Label_CoefRebound.Location = new System.Drawing.Point(23, 65);
            this.Label_CoefRebound.Name = "Label_CoefRebound";
            this.Label_CoefRebound.Size = new System.Drawing.Size(166, 15);
            this.Label_CoefRebound.TabIndex = 1;
            this.Label_CoefRebound.Text = "Coefficient de rebond [0, 15] : ";
            // 
            // Label_CoefFriction
            // 
            this.Label_CoefFriction.AutoSize = true;
            this.Label_CoefFriction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Label_CoefFriction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_CoefFriction.ForeColor = System.Drawing.Color.White;
            this.Label_CoefFriction.Location = new System.Drawing.Point(23, 30);
            this.Label_CoefFriction.Name = "Label_CoefFriction";
            this.Label_CoefFriction.Size = new System.Drawing.Size(163, 15);
            this.Label_CoefFriction.TabIndex = 0;
            this.Label_CoefFriction.Text = "Coefficient de friction [0, 10] :";
            // 
            // Button_SaveChanges
            // 
            this.Button_SaveChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_SaveChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_SaveChanges.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_SaveChanges.ForeColor = System.Drawing.Color.White;
            this.Button_SaveChanges.Location = new System.Drawing.Point(85, 145);
            this.Button_SaveChanges.Name = "Button_SaveChanges";
            this.Button_SaveChanges.Size = new System.Drawing.Size(100, 25);
            this.Button_SaveChanges.TabIndex = 6;
            this.Button_SaveChanges.TabStop = false;
            this.Button_SaveChanges.Text = "Sauvegarder";
            this.Button_SaveChanges.UseVisualStyleBackColor = false;
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Cancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Cancel.ForeColor = System.Drawing.Color.White;
            this.Button_Cancel.Location = new System.Drawing.Point(200, 145);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(100, 25);
            this.Button_Cancel.TabIndex = 7;
            this.Button_Cancel.TabStop = false;
            this.Button_Cancel.Text = "Annuler";
            this.Button_Cancel.UseVisualStyleBackColor = false;
            // 
            // Panel_FillColor
            // 
            this.Panel_FillColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_FillColor.Location = new System.Drawing.Point(3, 3);
            this.Panel_FillColor.Name = "Panel_FillColor";
            this.Panel_FillColor.Size = new System.Drawing.Size(378, 180);
            this.Panel_FillColor.TabIndex = 8;
            // 
            // GeneralProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Orange;
            this.ClientSize = new System.Drawing.Size(384, 186);
            this.Controls.Add(this.Label_CoefFriction);
            this.Controls.Add(this.Label_CoefRebound);
            this.Controls.Add(this.Label_CoefAcceleration);
            this.Controls.Add(this.Input_CoefFriction);
            this.Controls.Add(this.Input_CoefRebound);
            this.Controls.Add(this.Input_CoefAcceleration);
            this.Controls.Add(this.Button_SaveChanges);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Panel_FillColor);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeneralProperties";
            this.Text = "Propriétés générales";
            ((System.ComponentModel.ISupportInitialize)(this.Input_CoefAcceleration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_CoefRebound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_CoefFriction)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown Input_CoefAcceleration;
        private System.Windows.Forms.NumericUpDown Input_CoefRebound;
        private System.Windows.Forms.NumericUpDown Input_CoefFriction;
        private System.Windows.Forms.Label Label_CoefAcceleration;
        private System.Windows.Forms.Label Label_CoefRebound;
        private System.Windows.Forms.Label Label_CoefFriction;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.Button Button_SaveChanges;
        private System.Windows.Forms.Panel Panel_FillColor;
    }
}