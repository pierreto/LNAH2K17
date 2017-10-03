using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Controls.WPF.Matchmaking
{
    public class MatchmakingViewModel
    {
        private MatchmakingHub matchmakingHub;
        private bool isStarted;

        public MatchmakingViewModel(MatchmakingHub matchmakingHub)
        {
            this.matchmakingHub = matchmakingHub;
            this.isStarted = false;
        }

      
        private ICommand startMatchmakingCommand;
        public ICommand StartMatchmakingCommand
        {
            get
            {
                return startMatchmakingCommand ??
                       (startMatchmakingCommand = new RelayCommandAsync(StartMatchmaking, (o) => CanStart()));
            }
        }
        private async Task StartMatchmaking()
        {
            this.matchmakingHub.Start();
        }

        private ICommand cancelMatchmakingCommand;
        public ICommand CancelMatchmakingCommand
        {
            get
            {
                return cancelMatchmakingCommand ?? (cancelMatchmakingCommand =
                           new RelayCommandAsync(CancelMatchmaking, (o) => !CanStart()));
            }
        }

        private async Task CancelMatchmaking()
        {
            this.matchmakingHub.Cancel();
        }

        private bool CanStart()
        {

            return !this.isStarted;
        }
    }
}
