using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Controls.WPF.Tournament
{
    public class TournamentViewModel : ViewModelBase
    {
        private WaitingRoomHub waitingRoomHub;

        public TournamentViewModel(WaitingRoomHub waitingRoomHub)
        {
            this.waitingRoomHub = waitingRoomHub;
        }
        public override void InitializeViewModel()
        {
            this.waitingRoomHub.JoinGame();
        }


    }
}
