using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Tournament;

namespace InterfaceGraphique.Controls.WPF
{
    public class ViewModelLocator
    {
        private static ViewModelBase instance;

        private UnityContainer container;

        public ViewModelLocator()
        {
            container = Program.unityContainer;
        }

        public ChatViewModel ChatViewModel => container.Resolve<ChatViewModel>();
        public MatchmakingViewModel MatchmakingViewModel => container.Resolve<MatchmakingViewModel>();
        public TournamentViewModel TournamentViewModel => container.Resolve<TournamentViewModel>();
        public EditorViewModel EditorViewModel => container.Resolve<EditorViewModel>();
        
    }
}
