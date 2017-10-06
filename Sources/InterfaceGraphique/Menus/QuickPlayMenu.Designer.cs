namespace InterfaceGraphique {
    partial class QuickPlayMenu {
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
            this.Text_MapName = new System.Windows.Forms.Label();
            this.Button_OpenMap = new System.Windows.Forms.Button();
            this.Button_DefaultMap = new System.Windows.Forms.Button();
            this.Text_Map = new System.Windows.Forms.Label();
            this.List_VirtualProfile = new System.Windows.Forms.ComboBox();
            this.Text_VirtualProfile = new System.Windows.Forms.Label();
            this.Button_PlayerHuman = new System.Windows.Forms.Button();
            this.Button_PlayerVirtual = new System.Windows.Forms.Button();
            this.Text_PlayerType = new System.Windows.Forms.Label();
            this.Text_Description = new System.Windows.Forms.Label();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Button_Play = new System.Windows.Forms.Button();
            this.Button_Online_Game = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.Button_Online_Game);
            this.panel1.Controls.Add(this.Text_MapName);
            this.panel1.Controls.Add(this.Button_OpenMap);
            this.panel1.Controls.Add(this.Button_DefaultMap);
            this.panel1.Controls.Add(this.Text_Map);
            this.panel1.Controls.Add(this.List_VirtualProfile);
            this.panel1.Controls.Add(this.Text_VirtualProfile);
            this.panel1.Controls.Add(this.Button_PlayerHuman);
            this.panel1.Controls.Add(this.Button_PlayerVirtual);
            this.panel1.Controls.Add(this.Text_PlayerType);
            this.panel1.Controls.Add(this.Text_Description);
            this.panel1.Controls.Add(this.Button_Cancel);
            this.panel1.Controls.Add(this.Button_Play);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1156, 490);
            this.panel1.TabIndex = 11;
            // 
            // Text_MapName
            // 
            this.Text_MapName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_MapName.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_MapName.ForeColor = System.Drawing.Color.White;
            this.Text_MapName.Location = new System.Drawing.Point(250, 279);
            this.Text_MapName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Text_MapName.Name = "Text_MapName";
            this.Text_MapName.Size = new System.Drawing.Size(353, 32);
            this.Text_MapName.TabIndex = 29;
            this.Text_MapName.Text = "DÉFAUT";
            // 
            // Button_OpenMap
            // 
            this.Button_OpenMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_OpenMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_OpenMap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_OpenMap.ForeColor = System.Drawing.Color.White;
            this.Button_OpenMap.Location = new System.Drawing.Point(634, 269);
            this.Button_OpenMap.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Button_OpenMap.Name = "Button_OpenMap";
            this.Button_OpenMap.Size = new System.Drawing.Size(150, 48);
            this.Button_OpenMap.TabIndex = 28;
            this.Button_OpenMap.TabStop = false;
            this.Button_OpenMap.Text = "Ouvrir";
            this.Button_OpenMap.UseVisualStyleBackColor = false;
            // 
            // Button_DefaultMap
            // 
            this.Button_DefaultMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_DefaultMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_DefaultMap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_DefaultMap.ForeColor = System.Drawing.Color.White;
            this.Button_DefaultMap.Location = new System.Drawing.Point(804, 269);
            this.Button_DefaultMap.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Button_DefaultMap.Name = "Button_DefaultMap";
            this.Button_DefaultMap.Size = new System.Drawing.Size(150, 48);
            this.Button_DefaultMap.TabIndex = 27;
            this.Button_DefaultMap.TabStop = false;
            this.Button_DefaultMap.Text = "Défaut";
            this.Button_DefaultMap.UseVisualStyleBackColor = false;
            // 
            // Text_Map
            // 
            this.Text_Map.AutoSize = true;
            this.Text_Map.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_Map.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_Map.ForeColor = System.Drawing.Color.White;
            this.Text_Map.Location = new System.Drawing.Point(50, 279);
            this.Text_Map.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Text_Map.Name = "Text_Map";
            this.Text_Map.Size = new System.Drawing.Size(157, 32);
            this.Text_Map.TabIndex = 26;
            this.Text_Map.Text = "Carte de jeu :";
            // 
            // List_VirtualProfile
            // 
            this.List_VirtualProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.List_VirtualProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.List_VirtualProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.List_VirtualProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.List_VirtualProfile.ForeColor = System.Drawing.Color.White;
            this.List_VirtualProfile.FormattingEnabled = true;
            this.List_VirtualProfile.Items.AddRange(new object[] {
            "Défaut"});
            this.List_VirtualProfile.Location = new System.Drawing.Point(634, 196);
            this.List_VirtualProfile.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.List_VirtualProfile.Name = "List_VirtualProfile";
            this.List_VirtualProfile.Size = new System.Drawing.Size(316, 40);
            this.List_VirtualProfile.TabIndex = 25;
            this.List_VirtualProfile.TabStop = false;
            // 
            // Text_VirtualProfile
            // 
            this.Text_VirtualProfile.AutoSize = true;
            this.Text_VirtualProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_VirtualProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_VirtualProfile.ForeColor = System.Drawing.Color.White;
            this.Text_VirtualProfile.Location = new System.Drawing.Point(50, 202);
            this.Text_VirtualProfile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Text_VirtualProfile.Name = "Text_VirtualProfile";
            this.Text_VirtualProfile.Size = new System.Drawing.Size(267, 32);
            this.Text_VirtualProfile.TabIndex = 24;
            this.Text_VirtualProfile.Text = "Profil du joueur virtuel :";
            // 
            // Button_PlayerHuman
            // 
            this.Button_PlayerHuman.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_PlayerHuman.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_PlayerHuman.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_PlayerHuman.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_PlayerHuman.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Button_PlayerHuman.Location = new System.Drawing.Point(634, 115);
            this.Button_PlayerHuman.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Button_PlayerHuman.Name = "Button_PlayerHuman";
            this.Button_PlayerHuman.Size = new System.Drawing.Size(150, 48);
            this.Button_PlayerHuman.TabIndex = 23;
            this.Button_PlayerHuman.TabStop = false;
            this.Button_PlayerHuman.Text = "Humain";
            this.Button_PlayerHuman.UseVisualStyleBackColor = false;
            // 
            // Button_PlayerVirtual
            // 
            this.Button_PlayerVirtual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_PlayerVirtual.Cursor = System.Windows.Forms.Cursors.Default;
            this.Button_PlayerVirtual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_PlayerVirtual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_PlayerVirtual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_PlayerVirtual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_PlayerVirtual.ForeColor = System.Drawing.Color.White;
            this.Button_PlayerVirtual.Location = new System.Drawing.Point(804, 115);
            this.Button_PlayerVirtual.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Button_PlayerVirtual.Name = "Button_PlayerVirtual";
            this.Button_PlayerVirtual.Size = new System.Drawing.Size(150, 48);
            this.Button_PlayerVirtual.TabIndex = 22;
            this.Button_PlayerVirtual.TabStop = false;
            this.Button_PlayerVirtual.Text = "Virtuel";
            this.Button_PlayerVirtual.UseVisualStyleBackColor = false;
            // 
            // Text_PlayerType
            // 
            this.Text_PlayerType.AutoSize = true;
            this.Text_PlayerType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_PlayerType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_PlayerType.ForeColor = System.Drawing.Color.White;
            this.Text_PlayerType.Location = new System.Drawing.Point(50, 125);
            this.Text_PlayerType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Text_PlayerType.Name = "Text_PlayerType";
            this.Text_PlayerType.Size = new System.Drawing.Size(301, 32);
            this.Text_PlayerType.TabIndex = 21;
            this.Text_PlayerType.Text = "Type du deuxième joueur :";
            // 
            // Text_Description
            // 
            this.Text_Description.AutoSize = true;
            this.Text_Description.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_Description.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_Description.ForeColor = System.Drawing.Color.White;
            this.Text_Description.Location = new System.Drawing.Point(50, 48);
            this.Text_Description.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Text_Description.Name = "Text_Description";
            this.Text_Description.Size = new System.Drawing.Size(374, 32);
            this.Text_Description.TabIndex = 14;
            this.Text_Description.Text = "Paramètres de la partie rapide :";
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Cancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Cancel.ForeColor = System.Drawing.Color.White;
            this.Button_Cancel.Location = new System.Drawing.Point(602, 394);
            this.Button_Cancel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(200, 48);
            this.Button_Cancel.TabIndex = 13;
            this.Button_Cancel.TabStop = false;
            this.Button_Cancel.Text = "Annuler";
            this.Button_Cancel.UseVisualStyleBackColor = false;
            // 
            // Button_Play
            // 
            this.Button_Play.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_Play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Play.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Play.ForeColor = System.Drawing.Color.White;
            this.Button_Play.Location = new System.Drawing.Point(352, 394);
            this.Button_Play.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Button_Play.Name = "Button_Play";
            this.Button_Play.Size = new System.Drawing.Size(200, 48);
            this.Button_Play.TabIndex = 12;
            this.Button_Play.TabStop = false;
            this.Button_Play.Text = "Débuter";
            this.Button_Play.UseVisualStyleBackColor = false;
            // 
            // Button_Online_Game
            // 
            this.Button_Online_Game.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_Online_Game.Cursor = System.Windows.Forms.Cursors.Default;
            this.Button_Online_Game.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_Online_Game.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_Online_Game.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Online_Game.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Online_Game.ForeColor = System.Drawing.Color.White;
            this.Button_Online_Game.Location = new System.Drawing.Point(972, 115);
            this.Button_Online_Game.Margin = new System.Windows.Forms.Padding(6);
            this.Button_Online_Game.Name = "Button_Online_Game";
            this.Button_Online_Game.Size = new System.Drawing.Size(150, 48);
            this.Button_Online_Game.TabIndex = 30;
            this.Button_Online_Game.TabStop = false;
            this.Button_Online_Game.Text = "En Ligne";
            this.Button_Online_Game.UseVisualStyleBackColor = false;
            // 
            // QuickPlayMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkViolet;
            this.ClientSize = new System.Drawing.Size(1168, 502);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickPlayMenu";
            this.Text = "QuickPlayMenu";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.Button Button_Play;
        private System.Windows.Forms.Label Text_Description;
        private System.Windows.Forms.Button Button_PlayerHuman;
        private System.Windows.Forms.Button Button_PlayerVirtual;
        private System.Windows.Forms.Label Text_PlayerType;
        private System.Windows.Forms.ComboBox List_VirtualProfile;
        private System.Windows.Forms.Label Text_VirtualProfile;
        private System.Windows.Forms.Label Text_MapName;
        private System.Windows.Forms.Button Button_OpenMap;
        private System.Windows.Forms.Button Button_DefaultMap;
        private System.Windows.Forms.Label Text_Map;
        private System.Windows.Forms.Button Button_Online_Game;
    }
}