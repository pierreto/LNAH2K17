namespace InterfaceGraphique {
    partial class ConfigurationMenu {
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
            this.TabHolder = new System.Windows.Forms.TabControl();
            this.Tab_Inputs = new System.Windows.Forms.TabPage();
            this.Button_ChangeKeyRight = new System.Windows.Forms.Button();
            this.Button_ChangeKeyLeft = new System.Windows.Forms.Button();
            this.Button_ChangeKeyDown = new System.Windows.Forms.Button();
            this.Button_ChangeKeyUp = new System.Windows.Forms.Button();
            this.Text_ControlKeyRight_Key = new System.Windows.Forms.Label();
            this.Text_ControlKeyLeft_Key = new System.Windows.Forms.Label();
            this.Text_ControlKeyDown_Key = new System.Windows.Forms.Label();
            this.Text_ControlKeyUp_Key = new System.Windows.Forms.Label();
            this.Text_ControlKeyUp = new System.Windows.Forms.Label();
            this.Text_ControlKeyDown = new System.Windows.Forms.Label();
            this.Text_ControlKeyLeft = new System.Windows.Forms.Label();
            this.Text_ControlKeyRight = new System.Windows.Forms.Label();
            this.Text_ControlsDescription = new System.Windows.Forms.Label();
            this.Button_ResetControls = new System.Windows.Forms.Button();
            this.Panel_InputTabFillColor = new System.Windows.Forms.Panel();
            this.Tab_GameOptions = new System.Windows.Forms.TabPage();
            this.Input_GoalsNeeded = new System.Windows.Forms.NumericUpDown();
            this.Button_PlayerHuman = new System.Windows.Forms.Button();
            this.Button_PlayerVirtual = new System.Windows.Forms.Button();
            this.Button_ResetOptions = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Text_GoalsNeeded = new System.Windows.Forms.Label();
            this.Text_OptionsDescription = new System.Windows.Forms.Label();
            this.Panel_OptionTabFillColor = new System.Windows.Forms.Panel();
            this.Tab_Debug = new System.Windows.Forms.TabPage();
            this.Checkbox_DebugActivation = new System.Windows.Forms.CheckBox();
            this.Checkbox_DebugPortal = new System.Windows.Forms.CheckBox();
            this.Checkbox_DebugLight = new System.Windows.Forms.CheckBox();
            this.Checkbox_DebugSpeed = new System.Windows.Forms.CheckBox();
            this.Checkbox_DebugCollision = new System.Windows.Forms.CheckBox();
            this.Text_DebugDescription = new System.Windows.Forms.Label();
            this.Panel_DebugTabFillColor = new System.Windows.Forms.Panel();
            this.Tab_Profiles = new System.Windows.Forms.TabPage();
            this.Input_PlayerPassivity = new System.Windows.Forms.NumericUpDown();
            this.Input_PlayerMoveSpeed = new System.Windows.Forms.NumericUpDown();
            this.Input_ProfileName = new System.Windows.Forms.TextBox();
            this.List_SavedProfileList = new System.Windows.Forms.ComboBox();
            this.Button_DeleteProfile = new System.Windows.Forms.Button();
            this.Button_SaveProfile = new System.Windows.Forms.Button();
            this.Text_PlayerPassivity = new System.Windows.Forms.Label();
            this.Text_PlayerMoveSpeed = new System.Windows.Forms.Label();
            this.Text_SavedProfileList = new System.Windows.Forms.Label();
            this.Text_ProfileName = new System.Windows.Forms.Label();
            this.Text_VirtualPlayerDescription = new System.Windows.Forms.Label();
            this.Panel_ProfileTabFillColor = new System.Windows.Forms.Panel();
            this.TabHolder.SuspendLayout();
            this.Tab_Inputs.SuspendLayout();
            this.Tab_GameOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Input_GoalsNeeded)).BeginInit();
            this.Tab_Debug.SuspendLayout();
            this.Tab_Profiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Input_PlayerPassivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_PlayerMoveSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // TabHolder
            // 
            this.TabHolder.Controls.Add(this.Tab_Inputs);
            this.TabHolder.Controls.Add(this.Tab_GameOptions);
            this.TabHolder.Controls.Add(this.Tab_Debug);
            this.TabHolder.Controls.Add(this.Tab_Profiles);
            this.TabHolder.Location = new System.Drawing.Point(-5, 0);
            this.TabHolder.Margin = new System.Windows.Forms.Padding(0);
            this.TabHolder.Name = "TabHolder";
            this.TabHolder.SelectedIndex = 0;
            this.TabHolder.Size = new System.Drawing.Size(594, 316);
            this.TabHolder.TabIndex = 0;
            // 
            // Tab_Inputs
            // 
            this.Tab_Inputs.BackColor = System.Drawing.Color.Chartreuse;
            this.Tab_Inputs.Controls.Add(this.Button_ChangeKeyRight);
            this.Tab_Inputs.Controls.Add(this.Button_ChangeKeyLeft);
            this.Tab_Inputs.Controls.Add(this.Button_ChangeKeyDown);
            this.Tab_Inputs.Controls.Add(this.Button_ChangeKeyUp);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyRight_Key);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyLeft_Key);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyDown_Key);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyUp_Key);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyUp);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyDown);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyLeft);
            this.Tab_Inputs.Controls.Add(this.Text_ControlKeyRight);
            this.Tab_Inputs.Controls.Add(this.Text_ControlsDescription);
            this.Tab_Inputs.Controls.Add(this.Button_ResetControls);
            this.Tab_Inputs.Controls.Add(this.Panel_InputTabFillColor);
            this.Tab_Inputs.Location = new System.Drawing.Point(4, 22);
            this.Tab_Inputs.Name = "Tab_Inputs";
            this.Tab_Inputs.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Inputs.Size = new System.Drawing.Size(586, 290);
            this.Tab_Inputs.TabIndex = 0;
            this.Tab_Inputs.Text = "Touches de contrôle";
            // 
            // Button_ChangeKeyRight
            // 
            this.Button_ChangeKeyRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_ChangeKeyRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ChangeKeyRight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ChangeKeyRight.ForeColor = System.Drawing.Color.White;
            this.Button_ChangeKeyRight.Location = new System.Drawing.Point(476, 180);
            this.Button_ChangeKeyRight.Name = "Button_ChangeKeyRight";
            this.Button_ChangeKeyRight.Size = new System.Drawing.Size(75, 25);
            this.Button_ChangeKeyRight.TabIndex = 21;
            this.Button_ChangeKeyRight.TabStop = false;
            this.Button_ChangeKeyRight.Text = "Changer";
            this.Button_ChangeKeyRight.UseVisualStyleBackColor = false;
            // 
            // Button_ChangeKeyLeft
            // 
            this.Button_ChangeKeyLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_ChangeKeyLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ChangeKeyLeft.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ChangeKeyLeft.ForeColor = System.Drawing.Color.White;
            this.Button_ChangeKeyLeft.Location = new System.Drawing.Point(476, 140);
            this.Button_ChangeKeyLeft.Name = "Button_ChangeKeyLeft";
            this.Button_ChangeKeyLeft.Size = new System.Drawing.Size(75, 25);
            this.Button_ChangeKeyLeft.TabIndex = 20;
            this.Button_ChangeKeyLeft.TabStop = false;
            this.Button_ChangeKeyLeft.Text = "Changer";
            this.Button_ChangeKeyLeft.UseVisualStyleBackColor = false;
            // 
            // Button_ChangeKeyDown
            // 
            this.Button_ChangeKeyDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_ChangeKeyDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ChangeKeyDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ChangeKeyDown.ForeColor = System.Drawing.Color.White;
            this.Button_ChangeKeyDown.Location = new System.Drawing.Point(476, 100);
            this.Button_ChangeKeyDown.Name = "Button_ChangeKeyDown";
            this.Button_ChangeKeyDown.Size = new System.Drawing.Size(75, 25);
            this.Button_ChangeKeyDown.TabIndex = 19;
            this.Button_ChangeKeyDown.TabStop = false;
            this.Button_ChangeKeyDown.Text = "Changer";
            this.Button_ChangeKeyDown.UseVisualStyleBackColor = false;
            // 
            // Button_ChangeKeyUp
            // 
            this.Button_ChangeKeyUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_ChangeKeyUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ChangeKeyUp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ChangeKeyUp.ForeColor = System.Drawing.Color.White;
            this.Button_ChangeKeyUp.Location = new System.Drawing.Point(476, 60);
            this.Button_ChangeKeyUp.Name = "Button_ChangeKeyUp";
            this.Button_ChangeKeyUp.Size = new System.Drawing.Size(75, 25);
            this.Button_ChangeKeyUp.TabIndex = 18;
            this.Button_ChangeKeyUp.TabStop = false;
            this.Button_ChangeKeyUp.Text = "Changer";
            this.Button_ChangeKeyUp.UseVisualStyleBackColor = false;
            // 
            // Text_ControlKeyRight_Key
            // 
            this.Text_ControlKeyRight_Key.AutoSize = true;
            this.Text_ControlKeyRight_Key.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyRight_Key.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyRight_Key.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyRight_Key.Location = new System.Drawing.Point(300, 185);
            this.Text_ControlKeyRight_Key.Name = "Text_ControlKeyRight_Key";
            this.Text_ControlKeyRight_Key.Size = new System.Drawing.Size(14, 15);
            this.Text_ControlKeyRight_Key.TabIndex = 17;
            this.Text_ControlKeyRight_Key.Text = "D";
            // 
            // Text_ControlKeyLeft_Key
            // 
            this.Text_ControlKeyLeft_Key.AutoSize = true;
            this.Text_ControlKeyLeft_Key.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyLeft_Key.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyLeft_Key.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyLeft_Key.Location = new System.Drawing.Point(300, 145);
            this.Text_ControlKeyLeft_Key.Name = "Text_ControlKeyLeft_Key";
            this.Text_ControlKeyLeft_Key.Size = new System.Drawing.Size(14, 15);
            this.Text_ControlKeyLeft_Key.TabIndex = 16;
            this.Text_ControlKeyLeft_Key.Text = "A";
            // 
            // Text_ControlKeyDown_Key
            // 
            this.Text_ControlKeyDown_Key.AutoSize = true;
            this.Text_ControlKeyDown_Key.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyDown_Key.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyDown_Key.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyDown_Key.Location = new System.Drawing.Point(300, 105);
            this.Text_ControlKeyDown_Key.Name = "Text_ControlKeyDown_Key";
            this.Text_ControlKeyDown_Key.Size = new System.Drawing.Size(14, 15);
            this.Text_ControlKeyDown_Key.TabIndex = 15;
            this.Text_ControlKeyDown_Key.Text = "S";
            // 
            // Text_ControlKeyUp_Key
            // 
            this.Text_ControlKeyUp_Key.AutoSize = true;
            this.Text_ControlKeyUp_Key.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyUp_Key.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyUp_Key.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyUp_Key.Location = new System.Drawing.Point(300, 65);
            this.Text_ControlKeyUp_Key.Name = "Text_ControlKeyUp_Key";
            this.Text_ControlKeyUp_Key.Size = new System.Drawing.Size(14, 15);
            this.Text_ControlKeyUp_Key.TabIndex = 14;
            this.Text_ControlKeyUp_Key.Text = "W";
            // 
            // Text_ControlKeyUp
            // 
            this.Text_ControlKeyUp.AutoSize = true;
            this.Text_ControlKeyUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyUp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyUp.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyUp.Location = new System.Drawing.Point(25, 65);
            this.Text_ControlKeyUp.Name = "Text_ControlKeyUp";
            this.Text_ControlKeyUp.Size = new System.Drawing.Size(146, 15);
            this.Text_ControlKeyUp.TabIndex = 13;
            this.Text_ControlKeyUp.Text = "Déplacement vers le haut :";
            // 
            // Text_ControlKeyDown
            // 
            this.Text_ControlKeyDown.AutoSize = true;
            this.Text_ControlKeyDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyDown.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyDown.Location = new System.Drawing.Point(25, 105);
            this.Text_ControlKeyDown.Name = "Text_ControlKeyDown";
            this.Text_ControlKeyDown.Size = new System.Drawing.Size(140, 15);
            this.Text_ControlKeyDown.TabIndex = 12;
            this.Text_ControlKeyDown.Text = "Déplacement vers le bas :";
            // 
            // Text_ControlKeyLeft
            // 
            this.Text_ControlKeyLeft.AutoSize = true;
            this.Text_ControlKeyLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyLeft.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyLeft.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyLeft.Location = new System.Drawing.Point(25, 145);
            this.Text_ControlKeyLeft.Name = "Text_ControlKeyLeft";
            this.Text_ControlKeyLeft.Size = new System.Drawing.Size(161, 15);
            this.Text_ControlKeyLeft.TabIndex = 11;
            this.Text_ControlKeyLeft.Text = "Déplacement vers la gauche :";
            // 
            // Text_ControlKeyRight
            // 
            this.Text_ControlKeyRight.AutoSize = true;
            this.Text_ControlKeyRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlKeyRight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlKeyRight.ForeColor = System.Drawing.Color.White;
            this.Text_ControlKeyRight.Location = new System.Drawing.Point(25, 185);
            this.Text_ControlKeyRight.Name = "Text_ControlKeyRight";
            this.Text_ControlKeyRight.Size = new System.Drawing.Size(153, 15);
            this.Text_ControlKeyRight.TabIndex = 10;
            this.Text_ControlKeyRight.Text = "Déplacement vers la droite :";
            // 
            // Text_ControlsDescription
            // 
            this.Text_ControlsDescription.AutoSize = true;
            this.Text_ControlsDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ControlsDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ControlsDescription.ForeColor = System.Drawing.Color.White;
            this.Text_ControlsDescription.Location = new System.Drawing.Point(25, 25);
            this.Text_ControlsDescription.Name = "Text_ControlsDescription";
            this.Text_ControlsDescription.Size = new System.Drawing.Size(298, 15);
            this.Text_ControlsDescription.TabIndex = 9;
            this.Text_ControlsDescription.Text = "Touches de contrôle du maillet du deuxième joueur :";
            // 
            // Button_ResetControls
            // 
            this.Button_ResetControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_ResetControls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ResetControls.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ResetControls.ForeColor = System.Drawing.Color.White;
            this.Button_ResetControls.Location = new System.Drawing.Point(239, 235);
            this.Button_ResetControls.Name = "Button_ResetControls";
            this.Button_ResetControls.Size = new System.Drawing.Size(100, 25);
            this.Button_ResetControls.TabIndex = 8;
            this.Button_ResetControls.TabStop = false;
            this.Button_ResetControls.Text = "Réinitialiser";
            this.Button_ResetControls.UseVisualStyleBackColor = false;
            // 
            // Panel_InputTabFillColor
            // 
            this.Panel_InputTabFillColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_InputTabFillColor.Location = new System.Drawing.Point(4, 3);
            this.Panel_InputTabFillColor.Name = "Panel_InputTabFillColor";
            this.Panel_InputTabFillColor.Size = new System.Drawing.Size(578, 283);
            this.Panel_InputTabFillColor.TabIndex = 22;
            // 
            // Tab_GameOptions
            // 
            this.Tab_GameOptions.BackColor = System.Drawing.Color.Fuchsia;
            this.Tab_GameOptions.Controls.Add(this.Input_GoalsNeeded);
            this.Tab_GameOptions.Controls.Add(this.Button_PlayerHuman);
            this.Tab_GameOptions.Controls.Add(this.Button_PlayerVirtual);
            this.Tab_GameOptions.Controls.Add(this.Button_ResetOptions);
            this.Tab_GameOptions.Controls.Add(this.label1);
            this.Tab_GameOptions.Controls.Add(this.Text_GoalsNeeded);
            this.Tab_GameOptions.Controls.Add(this.Text_OptionsDescription);
            this.Tab_GameOptions.Controls.Add(this.Panel_OptionTabFillColor);
            this.Tab_GameOptions.ForeColor = System.Drawing.Color.White;
            this.Tab_GameOptions.Location = new System.Drawing.Point(4, 22);
            this.Tab_GameOptions.Name = "Tab_GameOptions";
            this.Tab_GameOptions.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_GameOptions.Size = new System.Drawing.Size(586, 290);
            this.Tab_GameOptions.TabIndex = 1;
            this.Tab_GameOptions.Text = "Options de jeu";
            // 
            // Input_GoalsNeeded
            // 
            this.Input_GoalsNeeded.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_GoalsNeeded.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_GoalsNeeded.ForeColor = System.Drawing.Color.White;
            this.Input_GoalsNeeded.Location = new System.Drawing.Point(391, 63);
            this.Input_GoalsNeeded.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Input_GoalsNeeded.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Input_GoalsNeeded.Name = "Input_GoalsNeeded";
            this.Input_GoalsNeeded.Size = new System.Drawing.Size(160, 23);
            this.Input_GoalsNeeded.TabIndex = 21;
            this.Input_GoalsNeeded.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Input_GoalsNeeded.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Button_PlayerHuman
            // 
            this.Button_PlayerHuman.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_PlayerHuman.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Button_PlayerHuman.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_PlayerHuman.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_PlayerHuman.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Button_PlayerHuman.Location = new System.Drawing.Point(391, 100);
            this.Button_PlayerHuman.Name = "Button_PlayerHuman";
            this.Button_PlayerHuman.Size = new System.Drawing.Size(75, 25);
            this.Button_PlayerHuman.TabIndex = 20;
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
            this.Button_PlayerVirtual.Location = new System.Drawing.Point(476, 100);
            this.Button_PlayerVirtual.Name = "Button_PlayerVirtual";
            this.Button_PlayerVirtual.Size = new System.Drawing.Size(75, 25);
            this.Button_PlayerVirtual.TabIndex = 19;
            this.Button_PlayerVirtual.TabStop = false;
            this.Button_PlayerVirtual.Text = "Virtuel";
            this.Button_PlayerVirtual.UseVisualStyleBackColor = false;
            // 
            // Button_ResetOptions
            // 
            this.Button_ResetOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_ResetOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_ResetOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_ResetOptions.Location = new System.Drawing.Point(239, 235);
            this.Button_ResetOptions.Name = "Button_ResetOptions";
            this.Button_ResetOptions.Size = new System.Drawing.Size(100, 25);
            this.Button_ResetOptions.TabIndex = 17;
            this.Button_ResetOptions.TabStop = false;
            this.Button_ResetOptions.Text = "Réinitialiser";
            this.Button_ResetOptions.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(25, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "Type du deuxième joueur en mode éditeur :";
            // 
            // Text_GoalsNeeded
            // 
            this.Text_GoalsNeeded.AutoSize = true;
            this.Text_GoalsNeeded.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_GoalsNeeded.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_GoalsNeeded.ForeColor = System.Drawing.Color.White;
            this.Text_GoalsNeeded.Location = new System.Drawing.Point(25, 65);
            this.Text_GoalsNeeded.Name = "Text_GoalsNeeded";
            this.Text_GoalsNeeded.Size = new System.Drawing.Size(223, 15);
            this.Text_GoalsNeeded.TabIndex = 14;
            this.Text_GoalsNeeded.Text = "Nombre de buts pour gagner une partie :";
            // 
            // Text_OptionsDescription
            // 
            this.Text_OptionsDescription.AutoSize = true;
            this.Text_OptionsDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_OptionsDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_OptionsDescription.ForeColor = System.Drawing.Color.White;
            this.Text_OptionsDescription.Location = new System.Drawing.Point(25, 25);
            this.Text_OptionsDescription.Name = "Text_OptionsDescription";
            this.Text_OptionsDescription.Size = new System.Drawing.Size(192, 15);
            this.Text_OptionsDescription.TabIndex = 10;
            this.Text_OptionsDescription.Text = "Configuration des options de jeu :";
            // 
            // Panel_OptionTabFillColor
            // 
            this.Panel_OptionTabFillColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_OptionTabFillColor.Location = new System.Drawing.Point(4, 3);
            this.Panel_OptionTabFillColor.Name = "Panel_OptionTabFillColor";
            this.Panel_OptionTabFillColor.Size = new System.Drawing.Size(578, 283);
            this.Panel_OptionTabFillColor.TabIndex = 23;
            // 
            // Tab_Debug
            // 
            this.Tab_Debug.BackColor = System.Drawing.Color.Blue;
            this.Tab_Debug.Controls.Add(this.Checkbox_DebugActivation);
            this.Tab_Debug.Controls.Add(this.Checkbox_DebugPortal);
            this.Tab_Debug.Controls.Add(this.Checkbox_DebugLight);
            this.Tab_Debug.Controls.Add(this.Checkbox_DebugSpeed);
            this.Tab_Debug.Controls.Add(this.Checkbox_DebugCollision);
            this.Tab_Debug.Controls.Add(this.Text_DebugDescription);
            this.Tab_Debug.Controls.Add(this.Panel_DebugTabFillColor);
            this.Tab_Debug.Location = new System.Drawing.Point(4, 22);
            this.Tab_Debug.Name = "Tab_Debug";
            this.Tab_Debug.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Debug.Size = new System.Drawing.Size(586, 290);
            this.Tab_Debug.TabIndex = 2;
            this.Tab_Debug.Text = "Affichages de débogage";
            // 
            // Checkbox_DebugActivation
            // 
            this.Checkbox_DebugActivation.AutoSize = true;
            this.Checkbox_DebugActivation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Checkbox_DebugActivation.Checked = true;
            this.Checkbox_DebugActivation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Checkbox_DebugActivation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Checkbox_DebugActivation.ForeColor = System.Drawing.Color.White;
            this.Checkbox_DebugActivation.Location = new System.Drawing.Point(219, 235);
            this.Checkbox_DebugActivation.Name = "Checkbox_DebugActivation";
            this.Checkbox_DebugActivation.Size = new System.Drawing.Size(138, 19);
            this.Checkbox_DebugActivation.TabIndex = 24;
            this.Checkbox_DebugActivation.Text = "Activer le débogage";
            this.Checkbox_DebugActivation.UseVisualStyleBackColor = false;
            // 
            // Checkbox_DebugPortal
            // 
            this.Checkbox_DebugPortal.AutoSize = true;
            this.Checkbox_DebugPortal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Checkbox_DebugPortal.Checked = true;
            this.Checkbox_DebugPortal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Checkbox_DebugPortal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Checkbox_DebugPortal.ForeColor = System.Drawing.Color.White;
            this.Checkbox_DebugPortal.Location = new System.Drawing.Point(75, 185);
            this.Checkbox_DebugPortal.Name = "Checkbox_DebugPortal";
            this.Checkbox_DebugPortal.Size = new System.Drawing.Size(284, 19);
            this.Checkbox_DebugPortal.TabIndex = 22;
            this.Checkbox_DebugPortal.Text = "Identification de la limite d’attraction des portails";
            this.Checkbox_DebugPortal.UseVisualStyleBackColor = false;
            // 
            // Checkbox_DebugLight
            // 
            this.Checkbox_DebugLight.AutoSize = true;
            this.Checkbox_DebugLight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Checkbox_DebugLight.Checked = true;
            this.Checkbox_DebugLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Checkbox_DebugLight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Checkbox_DebugLight.ForeColor = System.Drawing.Color.White;
            this.Checkbox_DebugLight.Location = new System.Drawing.Point(75, 145);
            this.Checkbox_DebugLight.Name = "Checkbox_DebugLight";
            this.Checkbox_DebugLight.Size = new System.Drawing.Size(247, 19);
            this.Checkbox_DebugLight.TabIndex = 21;
            this.Checkbox_DebugLight.Text = "Activation ou désactivation d’un éclairage";
            this.Checkbox_DebugLight.UseVisualStyleBackColor = false;
            // 
            // Checkbox_DebugSpeed
            // 
            this.Checkbox_DebugSpeed.AutoSize = true;
            this.Checkbox_DebugSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Checkbox_DebugSpeed.Checked = true;
            this.Checkbox_DebugSpeed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Checkbox_DebugSpeed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Checkbox_DebugSpeed.ForeColor = System.Drawing.Color.White;
            this.Checkbox_DebugSpeed.Location = new System.Drawing.Point(75, 105);
            this.Checkbox_DebugSpeed.Name = "Checkbox_DebugSpeed";
            this.Checkbox_DebugSpeed.Size = new System.Drawing.Size(237, 19);
            this.Checkbox_DebugSpeed.TabIndex = 20;
            this.Checkbox_DebugSpeed.Text = "Vitesse de la rondelle après une collision";
            this.Checkbox_DebugSpeed.UseVisualStyleBackColor = false;
            // 
            // Checkbox_DebugCollision
            // 
            this.Checkbox_DebugCollision.AutoSize = true;
            this.Checkbox_DebugCollision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Checkbox_DebugCollision.Checked = true;
            this.Checkbox_DebugCollision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Checkbox_DebugCollision.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Checkbox_DebugCollision.ForeColor = System.Drawing.Color.White;
            this.Checkbox_DebugCollision.Location = new System.Drawing.Point(75, 65);
            this.Checkbox_DebugCollision.Name = "Checkbox_DebugCollision";
            this.Checkbox_DebugCollision.Size = new System.Drawing.Size(220, 19);
            this.Checkbox_DebugCollision.TabIndex = 19;
            this.Checkbox_DebugCollision.Text = "Collision entre la rondelle et un objet";
            this.Checkbox_DebugCollision.UseVisualStyleBackColor = false;
            // 
            // Text_DebugDescription
            // 
            this.Text_DebugDescription.AutoSize = true;
            this.Text_DebugDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_DebugDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_DebugDescription.ForeColor = System.Drawing.Color.White;
            this.Text_DebugDescription.Location = new System.Drawing.Point(25, 25);
            this.Text_DebugDescription.Name = "Text_DebugDescription";
            this.Text_DebugDescription.Size = new System.Drawing.Size(246, 15);
            this.Text_DebugDescription.TabIndex = 11;
            this.Text_DebugDescription.Text = "Configuration des affichages de débogage :";
            // 
            // Panel_DebugTabFillColor
            // 
            this.Panel_DebugTabFillColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_DebugTabFillColor.Location = new System.Drawing.Point(4, 4);
            this.Panel_DebugTabFillColor.Name = "Panel_DebugTabFillColor";
            this.Panel_DebugTabFillColor.Size = new System.Drawing.Size(578, 282);
            this.Panel_DebugTabFillColor.TabIndex = 25;
            // 
            // Tab_Profiles
            // 
            this.Tab_Profiles.BackColor = System.Drawing.Color.Orange;
            this.Tab_Profiles.Controls.Add(this.Input_PlayerPassivity);
            this.Tab_Profiles.Controls.Add(this.Input_PlayerMoveSpeed);
            this.Tab_Profiles.Controls.Add(this.Input_ProfileName);
            this.Tab_Profiles.Controls.Add(this.List_SavedProfileList);
            this.Tab_Profiles.Controls.Add(this.Button_DeleteProfile);
            this.Tab_Profiles.Controls.Add(this.Button_SaveProfile);
            this.Tab_Profiles.Controls.Add(this.Text_PlayerPassivity);
            this.Tab_Profiles.Controls.Add(this.Text_PlayerMoveSpeed);
            this.Tab_Profiles.Controls.Add(this.Text_SavedProfileList);
            this.Tab_Profiles.Controls.Add(this.Text_ProfileName);
            this.Tab_Profiles.Controls.Add(this.Text_VirtualPlayerDescription);
            this.Tab_Profiles.Controls.Add(this.Panel_ProfileTabFillColor);
            this.Tab_Profiles.Location = new System.Drawing.Point(4, 22);
            this.Tab_Profiles.Name = "Tab_Profiles";
            this.Tab_Profiles.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Profiles.Size = new System.Drawing.Size(586, 290);
            this.Tab_Profiles.TabIndex = 3;
            this.Tab_Profiles.Text = "Profils de joueurs virtuels";
            // 
            // Input_PlayerPassivity
            // 
            this.Input_PlayerPassivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_PlayerPassivity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Input_PlayerPassivity.Enabled = false;
            this.Input_PlayerPassivity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_PlayerPassivity.ForeColor = System.Drawing.Color.White;
            this.Input_PlayerPassivity.Location = new System.Drawing.Point(426, 183);
            this.Input_PlayerPassivity.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Input_PlayerPassivity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Input_PlayerPassivity.Name = "Input_PlayerPassivity";
            this.Input_PlayerPassivity.Size = new System.Drawing.Size(125, 23);
            this.Input_PlayerPassivity.TabIndex = 24;
            this.Input_PlayerPassivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Input_PlayerPassivity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Input_PlayerMoveSpeed
            // 
            this.Input_PlayerMoveSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_PlayerMoveSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Input_PlayerMoveSpeed.Enabled = false;
            this.Input_PlayerMoveSpeed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_PlayerMoveSpeed.ForeColor = System.Drawing.Color.White;
            this.Input_PlayerMoveSpeed.Location = new System.Drawing.Point(426, 143);
            this.Input_PlayerMoveSpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Input_PlayerMoveSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Input_PlayerMoveSpeed.Name = "Input_PlayerMoveSpeed";
            this.Input_PlayerMoveSpeed.Size = new System.Drawing.Size(125, 23);
            this.Input_PlayerMoveSpeed.TabIndex = 23;
            this.Input_PlayerMoveSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Input_PlayerMoveSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Input_ProfileName
            // 
            this.Input_ProfileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Input_ProfileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Input_ProfileName.Enabled = false;
            this.Input_ProfileName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_ProfileName.ForeColor = System.Drawing.Color.White;
            this.Input_ProfileName.Location = new System.Drawing.Point(426, 102);
            this.Input_ProfileName.Name = "Input_ProfileName";
            this.Input_ProfileName.Size = new System.Drawing.Size(125, 23);
            this.Input_ProfileName.TabIndex = 22;
            this.Input_ProfileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // List_SavedProfileList
            // 
            this.List_SavedProfileList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.List_SavedProfileList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.List_SavedProfileList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.List_SavedProfileList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.List_SavedProfileList.ForeColor = System.Drawing.Color.White;
            this.List_SavedProfileList.FormattingEnabled = true;
            this.List_SavedProfileList.Location = new System.Drawing.Point(401, 62);
            this.List_SavedProfileList.Name = "List_SavedProfileList";
            this.List_SavedProfileList.Size = new System.Drawing.Size(150, 23);
            this.List_SavedProfileList.TabIndex = 21;
            // 
            // Button_DeleteProfile
            // 
            this.Button_DeleteProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_DeleteProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_DeleteProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_DeleteProfile.ForeColor = System.Drawing.Color.White;
            this.Button_DeleteProfile.Location = new System.Drawing.Point(300, 235);
            this.Button_DeleteProfile.Name = "Button_DeleteProfile";
            this.Button_DeleteProfile.Size = new System.Drawing.Size(100, 25);
            this.Button_DeleteProfile.TabIndex = 20;
            this.Button_DeleteProfile.TabStop = false;
            this.Button_DeleteProfile.Text = "Supprimer";
            this.Button_DeleteProfile.UseVisualStyleBackColor = false;
            // 
            // Button_SaveProfile
            // 
            this.Button_SaveProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Button_SaveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_SaveProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_SaveProfile.ForeColor = System.Drawing.Color.White;
            this.Button_SaveProfile.Location = new System.Drawing.Point(175, 235);
            this.Button_SaveProfile.Name = "Button_SaveProfile";
            this.Button_SaveProfile.Size = new System.Drawing.Size(100, 25);
            this.Button_SaveProfile.TabIndex = 19;
            this.Button_SaveProfile.TabStop = false;
            this.Button_SaveProfile.Text = "Sauvegarder";
            this.Button_SaveProfile.UseVisualStyleBackColor = false;
            // 
            // Text_PlayerPassivity
            // 
            this.Text_PlayerPassivity.AutoSize = true;
            this.Text_PlayerPassivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_PlayerPassivity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_PlayerPassivity.ForeColor = System.Drawing.Color.White;
            this.Text_PlayerPassivity.Location = new System.Drawing.Point(25, 185);
            this.Text_PlayerPassivity.Name = "Text_PlayerPassivity";
            this.Text_PlayerPassivity.Size = new System.Drawing.Size(252, 15);
            this.Text_PlayerPassivity.TabIndex = 18;
            this.Text_PlayerPassivity.Text = "Taux de passivité lors d’une opportunité de tir :";
            // 
            // Text_PlayerMoveSpeed
            // 
            this.Text_PlayerMoveSpeed.AutoSize = true;
            this.Text_PlayerMoveSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_PlayerMoveSpeed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_PlayerMoveSpeed.ForeColor = System.Drawing.Color.White;
            this.Text_PlayerMoveSpeed.Location = new System.Drawing.Point(25, 145);
            this.Text_PlayerMoveSpeed.Name = "Text_PlayerMoveSpeed";
            this.Text_PlayerMoveSpeed.Size = new System.Drawing.Size(193, 15);
            this.Text_PlayerMoveSpeed.TabIndex = 17;
            this.Text_PlayerMoveSpeed.Text = "Vitesse de déplacement du maillet :";
            // 
            // Text_SavedProfileList
            // 
            this.Text_SavedProfileList.AutoSize = true;
            this.Text_SavedProfileList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_SavedProfileList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_SavedProfileList.ForeColor = System.Drawing.Color.White;
            this.Text_SavedProfileList.Location = new System.Drawing.Point(25, 65);
            this.Text_SavedProfileList.Name = "Text_SavedProfileList";
            this.Text_SavedProfileList.Size = new System.Drawing.Size(176, 15);
            this.Text_SavedProfileList.TabIndex = 16;
            this.Text_SavedProfileList.Text = "Profil en cours de modification :";
            // 
            // Text_ProfileName
            // 
            this.Text_ProfileName.AutoSize = true;
            this.Text_ProfileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_ProfileName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_ProfileName.ForeColor = System.Drawing.Color.White;
            this.Text_ProfileName.Location = new System.Drawing.Point(25, 105);
            this.Text_ProfileName.Name = "Text_ProfileName";
            this.Text_ProfileName.Size = new System.Drawing.Size(115, 15);
            this.Text_ProfileName.TabIndex = 15;
            this.Text_ProfileName.Text = "Identifiant du profil :";
            // 
            // Text_VirtualPlayerDescription
            // 
            this.Text_VirtualPlayerDescription.AutoSize = true;
            this.Text_VirtualPlayerDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Text_VirtualPlayerDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text_VirtualPlayerDescription.ForeColor = System.Drawing.Color.White;
            this.Text_VirtualPlayerDescription.Location = new System.Drawing.Point(25, 25);
            this.Text_VirtualPlayerDescription.Name = "Text_VirtualPlayerDescription";
            this.Text_VirtualPlayerDescription.Size = new System.Drawing.Size(222, 15);
            this.Text_VirtualPlayerDescription.TabIndex = 12;
            this.Text_VirtualPlayerDescription.Text = "Gestion des profils de joueurs virtuels :";
            // 
            // Panel_ProfileTabFillColor
            // 
            this.Panel_ProfileTabFillColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Panel_ProfileTabFillColor.Location = new System.Drawing.Point(4, 3);
            this.Panel_ProfileTabFillColor.Name = "Panel_ProfileTabFillColor";
            this.Panel_ProfileTabFillColor.Size = new System.Drawing.Size(578, 283);
            this.Panel_ProfileTabFillColor.TabIndex = 26;
            // 
            // ConfigurationMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 311);
            this.Controls.Add(this.TabHolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationMenu";
            this.Text = "Configuration";
            this.TabHolder.ResumeLayout(false);
            this.Tab_Inputs.ResumeLayout(false);
            this.Tab_Inputs.PerformLayout();
            this.Tab_GameOptions.ResumeLayout(false);
            this.Tab_GameOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Input_GoalsNeeded)).EndInit();
            this.Tab_Debug.ResumeLayout(false);
            this.Tab_Debug.PerformLayout();
            this.Tab_Profiles.ResumeLayout(false);
            this.Tab_Profiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Input_PlayerPassivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_PlayerMoveSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabHolder;
        private System.Windows.Forms.TabPage Tab_Inputs;
        private System.Windows.Forms.TabPage Tab_GameOptions;
        private System.Windows.Forms.TabPage Tab_Debug;
        private System.Windows.Forms.TabPage Tab_Profiles;
        private System.Windows.Forms.Button Button_ResetControls;
        private System.Windows.Forms.Label Text_ControlsDescription;
        private System.Windows.Forms.Label Text_ControlKeyUp;
        private System.Windows.Forms.Label Text_ControlKeyDown;
        private System.Windows.Forms.Label Text_ControlKeyLeft;
        private System.Windows.Forms.Label Text_ControlKeyRight;
        private System.Windows.Forms.Label Text_ControlKeyRight_Key;
        private System.Windows.Forms.Label Text_ControlKeyLeft_Key;
        private System.Windows.Forms.Label Text_ControlKeyDown_Key;
        private System.Windows.Forms.Label Text_ControlKeyUp_Key;
        private System.Windows.Forms.Button Button_ChangeKeyRight;
        private System.Windows.Forms.Button Button_ChangeKeyLeft;
        private System.Windows.Forms.Button Button_ChangeKeyDown;
        private System.Windows.Forms.Button Button_ChangeKeyUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Text_GoalsNeeded;
        private System.Windows.Forms.Label Text_OptionsDescription;
        private System.Windows.Forms.Button Button_PlayerHuman;
        private System.Windows.Forms.Button Button_PlayerVirtual;
        private System.Windows.Forms.Button Button_ResetOptions;
        private System.Windows.Forms.NumericUpDown Input_GoalsNeeded;
        private System.Windows.Forms.Label Text_DebugDescription;
        private System.Windows.Forms.CheckBox Checkbox_DebugActivation;
        private System.Windows.Forms.CheckBox Checkbox_DebugPortal;
        private System.Windows.Forms.CheckBox Checkbox_DebugLight;
        private System.Windows.Forms.CheckBox Checkbox_DebugSpeed;
        private System.Windows.Forms.CheckBox Checkbox_DebugCollision;
        private System.Windows.Forms.Label Text_VirtualPlayerDescription;
        private System.Windows.Forms.Label Text_ProfileName;
        private System.Windows.Forms.Label Text_SavedProfileList;
        private System.Windows.Forms.Label Text_PlayerPassivity;
        private System.Windows.Forms.Label Text_PlayerMoveSpeed;
        private System.Windows.Forms.Button Button_DeleteProfile;
        private System.Windows.Forms.Button Button_SaveProfile;
        private System.Windows.Forms.ComboBox List_SavedProfileList;
        private System.Windows.Forms.TextBox Input_ProfileName;
        private System.Windows.Forms.NumericUpDown Input_PlayerPassivity;
        private System.Windows.Forms.NumericUpDown Input_PlayerMoveSpeed;
        private System.Windows.Forms.Panel Panel_InputTabFillColor;
        private System.Windows.Forms.Panel Panel_OptionTabFillColor;
        private System.Windows.Forms.Panel Panel_DebugTabFillColor;
        private System.Windows.Forms.Panel Panel_ProfileTabFillColor;
    }
}