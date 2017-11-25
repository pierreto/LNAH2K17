using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.MainMenu;

namespace InterfaceGraphique.Menus
{
    public partial class OnlineTournementMenu : Form
    {
        public static List<Color> availableColors = new List<Color>() { Color.Chartreuse, Color.Blue, Color.Fuchsia, Color.DarkOrange, Color.Yellow, Color.Cyan, Color.Red, Color.DarkOrchid };

        private string player1Profile = "";
        private string player2Profile = "";
        private string player3Profile = "";
        private string player4Profile = "";
        private Color player1Color = DefaultValues.player1Color;
        private Color player2Color = DefaultValues.player2Color;
        private Color player3Color = DefaultValues.player3Color;
        private Color player4Color = DefaultValues.player4Color;
        private bool isPlayer1Virtual = DefaultValues.isplayerVirtual;
        private bool isPlayer2Virtual = DefaultValues.isplayerVirtual;
        private bool isPlayer3Virtual = DefaultValues.isplayerVirtual;
        private bool isPlayer4Virtual = DefaultValues.isplayerVirtual;

        public OnlineTournementMenu()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeAvailableColors();
        }

        private void InitializeEvents()
        {
            this.Button_Play.Click += new EventHandler(ValidateSettings);
            this.Button_MainMenu.Click += (sender, e) => { Program.FormManager.CurrentForm = Program.HomeMenu; Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>()); };

            this.Button_Player1Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player1Human, this.Button_Player1Virtual); this.List_VirtualProfile1.Enabled = false; };
            this.Button_Player1Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player1Virtual, this.Button_Player1Human); this.List_VirtualProfile1.Enabled = true; };
            this.Button_Player2Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player2Human, this.Button_Player2Virtual); this.List_VirtualProfile2.Enabled = false; };
            this.Button_Player2Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player2Virtual, this.Button_Player2Human); this.List_VirtualProfile2.Enabled = true; };
            this.Button_Player3Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player3Human, this.Button_Player3Virtual); this.List_VirtualProfile3.Enabled = false; };
            this.Button_Player3Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player3Virtual, this.Button_Player3Human); this.List_VirtualProfile3.Enabled = true; };
            this.Button_Player4Human.Click += (sender, e) => { SwitchButtonsState(this.Button_Player4Human, this.Button_Player4Virtual); this.List_VirtualProfile4.Enabled = false; };
            this.Button_Player4Virtual.Click += (sender, e) => { SwitchButtonsState(this.Button_Player4Virtual, this.Button_Player4Human); this.List_VirtualProfile4.Enabled = true; };

            // Window events
            this.List_VirtualProfile1.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.List_VirtualProfile2.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.List_VirtualProfile3.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();
            this.List_VirtualProfile4.DropDownClosed += (sender, e) => this.Label_TournementTitle.Focus();

            // Color events
            this.Label_Player1.Click += new EventHandler(CycleAvailableColors);
            this.Label_Player2.Click += new EventHandler(CycleAvailableColors);
            this.Label_Player3.Click += new EventHandler(CycleAvailableColors);
            this.Label_Player4.Click += new EventHandler(CycleAvailableColors);
        }

        private void InitializeAvailableColors()
        {
            foreach (Color color in availableColors)
            {
                if (color != player1Color && color != player2Color && color != player3Color && color != player4Color)
                    TournamentColors.Enqueue(color);
            }
        }
            
        private Queue<Color> TournamentColors = new Queue<Color>();
        private void CycleAvailableColors(object sender, EventArgs e)
        {
            TournamentColors.Enqueue(((Label)sender).ForeColor);
            Color newColor = TournamentColors.Dequeue();

            if ((Label)sender == this.Label_Player1)
            {
                this.Label_Player1.ForeColor = newColor;
                this.Panel_Player1BorderColor.BackColor = newColor;
                this.Button_Player1Human.ForeColor = (this.Button_Player1Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player1Virtual.ForeColor = (this.Button_Player1Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
            else if ((Label)sender == this.Label_Player2)
            {
                this.Label_Player2.ForeColor = newColor;
                this.Panel_Player2BorderColor.BackColor = newColor;
                this.Button_Player2Human.ForeColor = (this.Button_Player2Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player2Virtual.ForeColor = (this.Button_Player2Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
            else if ((Label)sender == this.Label_Player3)
            {
                this.Label_Player3.ForeColor = newColor;
                this.Panel_Player3BorderColor.BackColor = newColor;
                this.Button_Player3Human.ForeColor = (this.Button_Player3Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player3Virtual.ForeColor = (this.Button_Player3Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
            else if ((Label)sender == this.Label_Player4)
            {
                this.Label_Player4.ForeColor = newColor;
                this.Panel_Player4BorderColor.BackColor = newColor;
                this.Button_Player4Human.ForeColor = (this.Button_Player4Human.ForeColor != Color.White) ? newColor : Color.White;
                this.Button_Player4Virtual.ForeColor = (this.Button_Player4Virtual.ForeColor != Color.White) ? newColor : Color.White;
            }
        }


        public static Color defaultGrey = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        private void SwitchButtonsState(Button select, Button deselect)
        {
            if (select.ForeColor == Color.White)
            {
                select.ForeColor = deselect.ForeColor;
                deselect.ForeColor = Color.White;
                deselect.FlatAppearance.MouseDownBackColor = Color.Empty;
                deselect.FlatAppearance.MouseOverBackColor = Color.Empty;
                select.FlatAppearance.MouseDownBackColor = defaultGrey;
                select.FlatAppearance.MouseOverBackColor = defaultGrey;
                select.Cursor = Cursors.Default;
                deselect.Cursor = Cursors.Hand;
            }
        }

        private void ValidateSettings(object sender, EventArgs e)
        {
            if (this.Button_Player1Human.ForeColor == Color.White && this.Button_Player2Human.ForeColor == Color.White && this.Button_Player3Human.ForeColor == Color.White && this.Button_Player4Human.ForeColor == Color.White)
            {
                string warning = "La partie ne peut être lancée sans qu'il y ait au moins un joueur humain.";
                ErrorMessageDialog dialog = new ErrorMessageDialog(warning);
                dialog.ShowDialog();
                return;
            }

            LoadGame();
        }

        private void LoadGame()
        {
            SaveGameSettings();

            // CREATE AI OR ONLINE PLAYERS
            List<GamePlayerEntity> players = new List<GamePlayerEntity>();
            List<bool> IsVirtualPlayers = new List<bool> { isPlayer1Virtual, isPlayer2Virtual, isPlayer3Virtual, isPlayer4Virtual };

            IsVirtualPlayers.Where(x => x).ToList().ForEach(x => players.Add(new GamePlayerEntity { IsAi = true, Username = "AI" }));

            players.Add(new GamePlayerEntity(User.Instance.UserEntity));

            Program.QuickPlay.CurrentGameState.IsOnlineTournementMode = true;
            var vm = Program.unityContainer.Resolve<Controls.WPF.Tournament.TournamentViewModel>();
            vm.Initialize(players);
            Program.FormManager.CurrentForm = Program.OnlineTournament;

        }

        public void MettreAJour(double tempsInterAffichage)
        {

        }

        public void InitializeOpenGlPanel()
        {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);
            ResetProfileLists();
            LoadSavedSettings();
        }

        private void ResetProfileLists()
        {
            this.List_VirtualProfile1.Items.Clear();
            this.List_VirtualProfile2.Items.Clear();
            this.List_VirtualProfile3.Items.Clear();
            this.List_VirtualProfile4.Items.Clear();
            foreach (string profile in Program.ConfigurationMenu.GetProfileList())
            {
                this.List_VirtualProfile1.Items.Add(profile);
                this.List_VirtualProfile2.Items.Add(profile);
                this.List_VirtualProfile3.Items.Add(profile);
                this.List_VirtualProfile4.Items.Add(profile);
            }
            this.List_VirtualProfile1.SelectedIndex = 0;
            this.List_VirtualProfile2.SelectedIndex = 0;
            this.List_VirtualProfile3.SelectedIndex = 0;
            this.List_VirtualProfile4.SelectedIndex = 0;
        }

        private void LoadSavedSettings()
        {

            if (this.List_VirtualProfile1.Items.Contains(player1Profile)) this.List_VirtualProfile1.SelectedItem = player1Profile; else this.List_VirtualProfile1.SelectedIndex = 0;
            if (this.List_VirtualProfile2.Items.Contains(player2Profile)) this.List_VirtualProfile2.SelectedItem = player2Profile; else this.List_VirtualProfile2.SelectedIndex = 0;
            if (this.List_VirtualProfile3.Items.Contains(player3Profile)) this.List_VirtualProfile3.SelectedItem = player3Profile; else this.List_VirtualProfile3.SelectedIndex = 0;
            if (this.List_VirtualProfile4.Items.Contains(player4Profile)) this.List_VirtualProfile4.SelectedItem = player4Profile; else this.List_VirtualProfile4.SelectedIndex = 0;

            this.Label_Player1.ForeColor = player1Color;
            this.Panel_Player1BorderColor.BackColor = player1Color;
            this.Button_Player1Virtual.ForeColor = (isPlayer1Virtual) ? player1Color : Color.White;
            this.Button_Player1Human.ForeColor = (isPlayer1Virtual) ? Color.White : player1Color;

            this.Label_Player2.ForeColor = player2Color;
            this.Panel_Player2BorderColor.BackColor = player2Color;
            this.Button_Player2Virtual.ForeColor = (isPlayer2Virtual) ? player2Color : Color.White;
            this.Button_Player2Human.ForeColor = (isPlayer2Virtual) ? Color.White : player2Color;

            this.Label_Player3.ForeColor = player3Color;
            this.Panel_Player3BorderColor.BackColor = player3Color;
            this.Button_Player3Virtual.ForeColor = (isPlayer3Virtual) ? player3Color : Color.White;
            this.Button_Player3Human.ForeColor = (isPlayer3Virtual) ? Color.White : player3Color;

            this.Label_Player4.ForeColor = player4Color;
            this.Panel_Player4BorderColor.BackColor = player4Color;
            this.Button_Player4Virtual.ForeColor = (isPlayer4Virtual) ? player4Color : Color.White;
            this.Button_Player4Human.ForeColor = (isPlayer4Virtual) ? Color.White : player4Color;

            if (isPlayer1Virtual) this.Button_Player1Virtual.PerformClick(); else this.Button_Player1Human.PerformClick();
            if (isPlayer2Virtual) this.Button_Player2Virtual.PerformClick(); else this.Button_Player2Human.PerformClick();
            if (isPlayer3Virtual) this.Button_Player3Virtual.PerformClick(); else this.Button_Player3Human.PerformClick();
            if (isPlayer4Virtual) this.Button_Player4Virtual.PerformClick(); else this.Button_Player4Human.PerformClick();

        }

        private void WindowSizeChanged(object sender, EventArgs e)
        {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
        }
        public void UnsuscribeEventHandlers()
        {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }

        private void SaveGameSettings()
        {
            player1Profile = this.List_VirtualProfile1.SelectedItem.ToString();
            player2Profile = this.List_VirtualProfile2.SelectedItem.ToString();
            player3Profile = this.List_VirtualProfile3.SelectedItem.ToString();
            player4Profile = this.List_VirtualProfile4.SelectedItem.ToString();

            player1Color = this.Label_Player1.ForeColor;
            player2Color = this.Label_Player2.ForeColor;
            player3Color = this.Label_Player3.ForeColor;
            player4Color = this.Label_Player4.ForeColor;

            isPlayer1Virtual = (this.Button_Player1Human.ForeColor == Color.White) ? true : false;
            isPlayer2Virtual = (this.Button_Player2Human.ForeColor == Color.White) ? true : false;
            isPlayer3Virtual = (this.Button_Player3Human.ForeColor == Color.White) ? true : false;
            isPlayer4Virtual = (this.Button_Player4Human.ForeColor == Color.White) ? true : false;
        }

        private class DefaultValues
        {
            public static Color player1Color = Color.Chartreuse;
            public static Color player2Color = Color.Blue;
            public static Color player3Color = Color.Fuchsia;
            public static Color player4Color = Color.DarkOrange;
            public static bool isplayerVirtual = true;
            public static List<Color> availableColors = new List<Color>() { Color.Chartreuse, Color.Blue, Color.Fuchsia, Color.DarkOrange, Color.Yellow, Color.Cyan, Color.Red, Color.DarkOrchid };
        }
    }
}
