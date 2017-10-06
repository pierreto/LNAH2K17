using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class TournementTree
    /// @brief UI de l'arbre de tournoi
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class TournementTree : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe TournementTree
        ///
        ////////////////////////////////////////////////////////////////////////
        public TournementTree() {
            InitializeComponent();
            InitializeEvents();
            InitializeUIElements();

            timer.Interval = 5000;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            this.Button_R1P1Type.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P2Type.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P3Type.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P4Type.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R2P1Type.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R2P2Type.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_WinnerType.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P1Score.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P2Score.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P3Score.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R1P4Score.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R2P1Score.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();
            this.Button_R2P2Score.GotFocus += (sender, e) => this.Label_R1P1Name.Focus();

            timer.Tick += new EventHandler(StartRound);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les éléments du UI avec les bonnes valeurs 
        /// de couleur pour les joueurs.
        ///
        /// @return Void.
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeUIElements() {
            this.Panel_R1P1Color.BackColor = Program.TournementMenu.Player1Color;
            this.Panel_R1P2Color.BackColor = Program.TournementMenu.Player2Color;
            this.Panel_R1P3Color.BackColor = Program.TournementMenu.Player3Color;
            this.Panel_R1P4Color.BackColor = Program.TournementMenu.Player4Color;

            this.Label_R1P1Name.ForeColor = Program.TournementMenu.Player1Color;
            this.Label_R1P2Name.ForeColor = Program.TournementMenu.Player2Color;
            this.Label_R1P3Name.ForeColor = Program.TournementMenu.Player3Color;
            this.Label_R1P4Name.ForeColor = Program.TournementMenu.Player4Color;

            this.Button_R1P1Type.ForeColor = Program.TournementMenu.Player1Color;
            this.Button_R1P2Type.ForeColor = Program.TournementMenu.Player2Color;
            this.Button_R1P3Type.ForeColor = Program.TournementMenu.Player3Color;
            this.Button_R1P4Type.ForeColor = Program.TournementMenu.Player4Color;

            this.Button_R1P1Score.ForeColor = Program.TournementMenu.Player1Color;
            this.Button_R1P2Score.ForeColor = Program.TournementMenu.Player2Color;
            this.Button_R1P3Score.ForeColor = Program.TournementMenu.Player3Color;
            this.Button_R1P4Score.ForeColor = Program.TournementMenu.Player4Color;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Fonction vide appelée sur toutes les forms de facon 
        /// constante sans se soucier du type
        /// 
        ///	@param[in]  tempsInterAffichage : Temps entre chaque affichage
        /// @return     Void.
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage) {

        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Met à jour la taille de la fenetre selon celle de la fenêtre parent
        /// 
        ///	@param[in]  sender : Objet qui a causé l'évènement
        /// @param[in]  e : Arguments de l'évènement
        /// @return     Void.
        ///
        ////////////////////////////////////////////////////////////////////////
        private void WindowSizeChanged(object sender, EventArgs e) {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Ajoute le panneau openGL à la Form en cours. Les controles sont
        /// modifiés afin d'ajouter les éléments visuels nécessaires et les 
        /// events sur le panel sont ajoutés.
        ///
        /// @return Void.
        ///
        ////////////////////////////////////////////////////////////////////////
        public void InitializeOpenGlPanel() {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);

            InitializeUIElements();
            timer.Start();
            roundWinner = (roundScore[0] > roundScore[1]) ? 1 : 2;

            if (currentRound == 0)
                ResetAllToDefault();
            else if (currentRound == 1)
                UpdateUIForSemiFinal();
            else if (currentRound == 2)
                UpdateUIForFinal();
            else if (currentRound == 3)
                UpdateUIForWinner();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction désinscrits les events de la form courante sur le 
        /// panneau openGL.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UnsuscribeEventHandlers() {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction est appelée si le timer atteint 5 secondes ou si 
        /// la touche espace est appuyée. Elle s'occupe de lancer la partie ou
        /// de quitter au menu si la victoire est déjà remportée.
        /// 
        ///	@param[in]  sender : Objet qui a causé l'évènement
        /// @param[in]  e      : Arguments de l'évènement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void StartRound(object sender, EventArgs e) {
            timer.Stop();
            currentRound++;

            if (currentRound == 4) {
                FonctionsNatives.resetNodeTree();
                FonctionsNatives.resetCameraPosition();
                Program.FormManager.CurrentForm = Program.MainMenu;
            }
            else {
                if (!isVirtualMatch()) {
                    SetVirtualProfile();
                    SetPlayerNamesAndColors();
                    Program.FormManager.CurrentForm = Program.QuickPlay;
                }
                else
                    StartVirtualMatch();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction assigne le profil du joueur virtuel si il y en a un
        /// pour la ronde à jouer. Si il y en a pas, le joueur virtuel est 
        /// désactivé.
        /// 
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void SetVirtualProfile() {
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.LOCAL_PLAYER);

            if (currentRound == 1) {
                if(this.Button_R1P1Type.Text == virtualPlayerText) {
                    PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(Program.TournementMenu.Player1Profile);
                    FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                }
                else if (this.Button_R1P2Type.Text == virtualPlayerText) {
                    PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(Program.TournementMenu.Player2Profile);
                    FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                }
            }

            else if (currentRound == 2) {
                if (this.Button_R1P3Type.Text == virtualPlayerText) {
                    PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(Program.TournementMenu.Player3Profile);
                    FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                }
                else if (this.Button_R1P4Type.Text == virtualPlayerText) {
                    PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(Program.TournementMenu.Player4Profile);
                    FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                }
            }

            else if (currentRound == 3) {
                if (this.Button_R2P1Type.Text == virtualPlayerText) {
                    PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(R2P1Profile);
                    FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                }
                else if (this.Button_R2P2Type.Text == virtualPlayerText) {
                    PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(R2P2Profile);
                    FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction détermine le nom et les couleurs des joueurs selon 
        /// la ronde en cours pour le prochaine partie.
        /// 
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void SetPlayerNamesAndColors() {
            if (currentRound == 1) {
                if (this.Button_R1P1Type.Text != virtualPlayerText)
                    ApplyPlayerNamesAndColors(this.Label_R1P1Name.Text, this.Label_R1P2Name.Text, this.Label_R1P1Name.ForeColor, this.Label_R1P2Name.ForeColor);
                else
                    ApplyPlayerNamesAndColors(this.Label_R1P2Name.Text, this.Label_R1P1Name.Text, this.Label_R1P2Name.ForeColor, this.Label_R1P1Name.ForeColor);
            }

            else if (currentRound == 2) {
                if (this.Button_R1P3Type.Text != virtualPlayerText)
                    ApplyPlayerNamesAndColors(this.Label_R1P3Name.Text, this.Label_R1P4Name.Text, this.Label_R1P3Name.ForeColor, this.Label_R1P4Name.ForeColor);
                else
                    ApplyPlayerNamesAndColors(this.Label_R1P4Name.Text, this.Label_R1P3Name.Text, this.Label_R1P4Name.ForeColor, this.Label_R1P3Name.ForeColor);
            }

            else if (currentRound == 3) {
                if (this.Button_R2P1Type.Text != virtualPlayerText)
                    ApplyPlayerNamesAndColors(this.Label_R2P1Name.Text, this.Label_R2P2Name.Text, this.Label_R2P1Name.ForeColor, this.Label_R2P2Name.ForeColor);
                else
                    ApplyPlayerNamesAndColors(this.Label_R2P2Name.Text, this.Label_R2P1Name.Text, this.Label_R2P2Name.ForeColor, this.Label_R2P1Name.ForeColor);
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction applique le nom et les couleurs des joueurs au
        /// contexte openGL.
        /// 
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void ApplyPlayerNamesAndColors(string p1Name, string p2Name, Color p1Color, Color p2Color) {
            StringBuilder player1Name = new StringBuilder(p1Name.Length);
            StringBuilder player2Name = new StringBuilder(p2Name.Length);
            player1Name.Append(p1Name);
            player2Name.Append(p2Name);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);

            float[] player1Color = new float[4] { p1Color.R, p1Color.G, p1Color.B, p1Color.A };
            float[] player2Color = new float[4] { p2Color.R, p2Color.G, p2Color.B, p2Color.A };
            FonctionsNatives.setPlayerColors(player1Color, player2Color);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction vérifie si les deux joueurs à jouer dans la 
        /// prochaine ronde sont tout deux virtuels.
        ///
        /// @return Vrai si les deux joueurs sont virtuels 
        ///
        ////////////////////////////////////////////////////////////////////////
        private bool isVirtualMatch() {
            switch (currentRound) {
                case 1:
                    if (this.Button_R1P1Type.Text == virtualPlayerText && this.Button_R1P2Type.Text == virtualPlayerText)
                        return true;
                    break;
                case 2:
                    if (this.Button_R1P3Type.Text == virtualPlayerText && this.Button_R1P4Type.Text == virtualPlayerText)
                        return true;
                    break;
                case 3:
                    if (this.Button_R2P1Type.Text == virtualPlayerText && this.Button_R2P2Type.Text == virtualPlayerText)
                        return true;
                    break;
            }
            return false;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction lance un match virtuel. Elle s'occuper donc de 
        /// randomiser le résultat et de préparer la prochaine ronde.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void StartVirtualMatch() {
            Random randomiser = new Random();
            do {
                roundScore[0] = randomiser.Next(1, Program.ConfigurationMenu.NeededGoalsToWin + 1);
                roundScore[1] = randomiser.Next(1, Program.ConfigurationMenu.NeededGoalsToWin + 1);
            } while ((roundScore[0] != Program.ConfigurationMenu.NeededGoalsToWin && roundScore[1] != Program.ConfigurationMenu.NeededGoalsToWin) || roundScore[0] == roundScore[1]);

            Program.FormManager.CurrentForm = Program.TournementTree;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction met à jour le UI de l'arbre pour présenter le 2e 
        /// match des semi-finales.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void UpdateUIForSemiFinal() {
            if (this.Button_R1P1Type.Text == virtualPlayerText && this.Button_R1P2Type.Text != virtualPlayerText) {
                roundWinner = (roundWinner % 2) + 1;
                int temp = roundScore[0]; roundScore[0] = roundScore[1]; roundScore[1] = temp;
            }

            switch (roundWinner) {
                case 1:
                    this.Line_R1P1_1.BackColor = this.Panel_R1P1Color.BackColor;
                    this.Line_R1P1_2.BackColor = this.Panel_R1P1Color.BackColor;
                    this.Line_R1P1_3.BackColor = this.Panel_R1P1Color.BackColor;
                    this.Panel_R2P1Color.BackColor = this.Panel_R1P1Color.BackColor;
                    this.Label_R2P1Name.ForeColor = this.Panel_R1P1Color.BackColor;
                    this.Button_R2P1Type.ForeColor = this.Panel_R1P1Color.BackColor;
                    this.Button_R2P1Score.ForeColor = this.Panel_R1P1Color.BackColor;
                    this.Label_R2P1Name.Text = this.Label_R1P1Name.Text;
                    this.Button_R2P1Type.Text = this.Button_R1P1Type.Text;
                    this.Button_R2P1Score.Text = defaultScoreText;
                    R2P1Profile = Program.TournementMenu.Player1Profile;
                    break;
                case 2:
                    this.Line_R1P2_1.BackColor = this.Panel_R1P2Color.BackColor;
                    this.Line_R1P2_2.BackColor = this.Panel_R1P2Color.BackColor;
                    this.Line_R1P2_3.BackColor = this.Panel_R1P2Color.BackColor;
                    this.Panel_R2P1Color.BackColor = this.Panel_R1P2Color.BackColor;
                    this.Label_R2P1Name.ForeColor = this.Panel_R1P2Color.BackColor;
                    this.Button_R2P1Type.ForeColor = this.Panel_R1P2Color.BackColor;
                    this.Button_R2P1Score.ForeColor = this.Panel_R1P2Color.BackColor;
                    this.Label_R2P1Name.Text = this.Label_R1P2Name.Text;
                    this.Button_R2P1Type.Text = this.Button_R1P2Type.Text;
                    this.Button_R2P1Score.Text = defaultScoreText;
                    R2P1Profile = Program.TournementMenu.Player2Profile;
                    break;
            }

            this.Label_R2P1Name.Visible = true;
            this.Button_R2P1Type.Visible = true;
            this.Button_R2P1Score.Visible = true;
            this.Button_R1P1Score.Text = "Score: " + roundScore[0];
            this.Button_R1P2Score.Text = "Score: " + roundScore[1];
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction met à jour le UI de l'arbre pour présenter le 
        /// match final.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void UpdateUIForFinal() {
            if (this.Button_R1P3Type.Text == virtualPlayerText && this.Button_R1P4Type.Text != virtualPlayerText) {
                roundWinner = (roundWinner % 2) + 1;
                int temp = roundScore[0]; roundScore[0] = roundScore[1]; roundScore[1] = temp;
            }

            switch (roundWinner) {
                case 1:
                    this.Line_R1P3_1.BackColor = this.Panel_R1P3Color.BackColor;
                    this.Line_R1P3_2.BackColor = this.Panel_R1P3Color.BackColor;
                    this.Line_R1P3_3.BackColor = this.Panel_R1P3Color.BackColor;
                    this.Panel_R2P2Color.BackColor = this.Panel_R1P3Color.BackColor;
                    this.Label_R2P2Name.ForeColor = this.Panel_R1P3Color.BackColor;
                    this.Button_R2P2Type.ForeColor = this.Panel_R1P3Color.BackColor;
                    this.Button_R2P2Score.ForeColor = this.Panel_R1P3Color.BackColor;
                    this.Label_R2P2Name.Text = this.Label_R1P3Name.Text;
                    this.Button_R2P2Type.Text = this.Button_R1P3Type.Text;
                    this.Button_R2P2Score.Text = defaultScoreText;
                    R2P2Profile = Program.TournementMenu.Player3Profile;
                    break;
                case 2:
                    this.Line_R1P4_1.BackColor = this.Panel_R1P4Color.BackColor;
                    this.Line_R1P4_2.BackColor = this.Panel_R1P4Color.BackColor;
                    this.Line_R1P4_3.BackColor = this.Panel_R1P4Color.BackColor;
                    this.Panel_R2P2Color.BackColor = this.Panel_R1P4Color.BackColor;
                    this.Label_R2P2Name.ForeColor = this.Panel_R1P4Color.BackColor;
                    this.Button_R2P2Type.ForeColor = this.Panel_R1P4Color.BackColor;
                    this.Button_R2P2Score.ForeColor = this.Panel_R1P4Color.BackColor;
                    this.Label_R2P2Name.Text = this.Label_R1P4Name.Text;
                    this.Button_R2P2Type.Text = this.Button_R1P4Type.Text;
                    this.Button_R2P2Score.Text = defaultScoreText;
                    R2P2Profile = Program.TournementMenu.Player4Profile;
                    break;
            }

            this.Label_R2P2Name.Visible = true;
            this.Button_R2P2Type.Visible = true;
            this.Button_R2P2Score.Visible = true;
            this.Button_R1P3Score.Text = "Score: " + roundScore[0];
            this.Button_R1P4Score.Text = "Score: " + roundScore[1];
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction met à jour le UI de l'arbre pour présenter le 
        /// gagnant du tournoi.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void UpdateUIForWinner() {
            if (this.Button_R2P1Type.Text == virtualPlayerText && this.Button_R2P2Type.Text != virtualPlayerText) {
                roundWinner = (roundWinner % 2) + 1;
                int temp = roundScore[0]; roundScore[0] = roundScore[1]; roundScore[1] = temp;
            }

            switch (roundWinner) {
                case 1:
                    this.Line_R2P1_1.BackColor = this.Panel_R2P1Color.BackColor;
                    this.Line_R2P1_2.BackColor = this.Panel_R2P1Color.BackColor;
                    this.Line_R2P1_3.BackColor = this.Panel_R2P1Color.BackColor;
                    this.Panel_WinnerColor.BackColor = this.Panel_R2P1Color.BackColor;
                    this.Label_WinnerName.ForeColor = this.Panel_R2P1Color.BackColor;
                    this.Button_WinnerType.ForeColor = this.Panel_R2P1Color.BackColor;
                    this.Label_WinnerText.ForeColor = this.Panel_R2P1Color.BackColor;
                    this.Label_WinnerName.Text = this.Label_R2P1Name.Text;
                    this.Button_WinnerType.Text = this.Button_R2P1Type.Text;
                    break;
                case 2:
                    this.Line_R2P2_1.BackColor = this.Panel_R2P2Color.BackColor;
                    this.Line_R2P2_2.BackColor = this.Panel_R2P2Color.BackColor;
                    this.Line_R2P2_3.BackColor = this.Panel_R2P2Color.BackColor;
                    this.Panel_WinnerColor.BackColor = this.Panel_R2P2Color.BackColor;
                    this.Label_WinnerName.ForeColor = this.Panel_R2P2Color.BackColor;
                    this.Button_WinnerType.ForeColor = this.Panel_R2P2Color.BackColor;
                    this.Label_WinnerText.ForeColor = this.Panel_R2P2Color.BackColor;
                    this.Label_WinnerName.Text = this.Label_R2P2Name.Text;
                    this.Button_WinnerType.Text = this.Button_R2P2Type.Text;
                    break;
            }

            this.Label_WinnerName.Visible = true;
            this.Button_WinnerType.Visible = true;
            this.Label_WinnerText.Visible = true;
            this.Button_R2P1Score.Text = "Score: " + roundScore[0];
            this.Button_R2P2Score.Text = "Score: " + roundScore[1];
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction réinitialise l'arbre de tournoi à son état initial.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ResetAllToDefault() {
            // Reset Text
            this.Label_R1P1Name.Text = Program.TournementMenu.Player1Name;
            this.Label_R1P2Name.Text = Program.TournementMenu.Player2Name;
            this.Label_R1P3Name.Text = Program.TournementMenu.Player3Name;
            this.Label_R1P4Name.Text = Program.TournementMenu.Player4Name;
            this.Button_R1P1Type.Text = Program.TournementMenu.Player1Type;
            this.Button_R1P2Type.Text = Program.TournementMenu.Player2Type;
            this.Button_R1P3Type.Text = Program.TournementMenu.Player3Type;
            this.Button_R1P4Type.Text = Program.TournementMenu.Player4Type;
            this.Button_R1P1Score.Text = defaultScoreText;
            this.Button_R1P2Score.Text = defaultScoreText;
            this.Button_R1P3Score.Text = defaultScoreText;
            this.Button_R1P4Score.Text = defaultScoreText;

            // Reset Visibility
            this.Label_R2P1Name.Visible = false;
            this.Label_R2P2Name.Visible = false;
            this.Label_WinnerName.Visible = false;
            this.Label_WinnerText.Visible = false;
            this.Button_R2P1Type.Visible = false;
            this.Button_R2P2Type.Visible = false;
            this.Button_WinnerType.Visible = false;
            this.Button_R2P1Score.Visible = false;
            this.Button_R2P2Score.Visible = false;

            // Reset Color
            this.Panel_R2P1Color.BackColor = Color.White;
            this.Panel_R2P2Color.BackColor = Color.White;
            this.Panel_WinnerColor.BackColor = Color.White;
            this.Line_R1P1_1.BackColor = Color.White;
            this.Line_R1P1_2.BackColor = Color.White;
            this.Line_R1P1_3.BackColor = Color.White;
            this.Line_R1P2_1.BackColor = Color.White;
            this.Line_R1P2_2.BackColor = Color.White;
            this.Line_R1P2_3.BackColor = Color.White;
            this.Line_R1P3_1.BackColor = Color.White;
            this.Line_R1P3_2.BackColor = Color.White;
            this.Line_R1P3_3.BackColor = Color.White;
            this.Line_R1P4_1.BackColor = Color.White;
            this.Line_R1P4_2.BackColor = Color.White;
            this.Line_R1P4_3.BackColor = Color.White;
            this.Line_R2P1_1.BackColor = Color.White;
            this.Line_R2P1_2.BackColor = Color.White;
            this.Line_R2P1_3.BackColor = Color.White;
            this.Line_R2P2_1.BackColor = Color.White;
            this.Line_R2P2_2.BackColor = Color.White;
            this.Line_R2P2_3.BackColor = Color.White;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction gère la touche d'espacement afin de devancer le 
        /// début de la manche (5 secondes d'attente).
        /// 
        /// @param[in]  msg       : Message de l'event
        /// @param[in]  keyData   : Touche appuyée
        /// @return     Vrai si la touche est gérée 
        ///
        ////////////////////////////////////////////////////////////////////////
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            switch (keyData) {
                case Keys.Space:
                    StartRound(null, null);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private Timer timer = new Timer();
        private int currentRound = 0;
        private int roundWinner = 0;
        private int[] roundScore = new int[2] { 0, 0 };
        private string defaultScoreText = "Score: ?";
        private string virtualPlayerText = "Virtuel";
        private string R2P1Profile;
        private string R2P2Profile;


        // Accessors
        public int[] RoundScore { set { roundScore = value; } }
        public int CurrentRound { set { currentRound = value; } }
    }
}
