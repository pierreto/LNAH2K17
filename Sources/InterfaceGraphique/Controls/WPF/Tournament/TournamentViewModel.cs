using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Controls.WPF.Tournament
{
    public class TournamentViewModel
    {
        private WaitingRoomHub waitingRoomHub;

        public TournamentViewModel(WaitingRoomHub waitingRoomHub)
        {
            this.waitingRoomHub = waitingRoomHub;
        }


    }
}
