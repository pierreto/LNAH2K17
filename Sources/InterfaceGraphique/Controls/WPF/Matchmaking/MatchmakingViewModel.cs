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
    public class MatchmakingViewModel : ViewModelBase
    {
        private MatchmakingHub matchmakingHub;
        private bool isStarted;

        public MatchmakingViewModel(MatchmakingHub matchmakingHub)
        {
            this.matchmakingHub = matchmakingHub;
            this.isStarted = false;
        }

      
        private ICommand matchmakingCommand;
        public ICommand MatchmakingCommand
        {
            get
            {
                return matchmakingCommand ??
                       (matchmakingCommand = new RelayCommandAsync(Matchmaking, (o) => CanStart()));
            }
        }
        private async Task Matchmaking()
        {
            if (this.isStarted)
            {
                 this.matchmakingHub.Cancel();
                this.IsStarted = false;

            }
            else
            {
                 this.matchmakingHub.Start();
                this.IsStarted = true;
            }

        }

        private bool CanStart()
        {

            return true;
        }

        public bool IsStarted
        {
            get => isStarted;
            set
            {
                isStarted = value;
                this.OnPropertyChanged();
            }
        }
    }
}
