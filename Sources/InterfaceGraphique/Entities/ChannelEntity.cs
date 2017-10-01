using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF;

namespace InterfaceGraphique.Entities
{
    public class ChannelEntity : ViewModelBase
    {
        public Guid Id { get; set; }

        public List<MemberEntity> Members { get; set; }
        public ObservableCollection<ChatMessage> channelMessages;

        public string Name { get; set; }

        public ChannelEntity(ObservableCollection<ChatMessage> channelMessages)
        {
            this.Members = new List<MemberEntity>();
            this.channelMessages = channelMessages;
        }

        public ObservableCollection<ChatMessage> ChannelMessages
        {
            get => channelMessages;
            set => channelMessages = value;
        }
        
    }
}
