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
    public class ChannelEntity
    {
        public Guid Id { get; set; }

        public List<MemberEntity> Members { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsJoinable { get; set; }

        public int PrivateUserId { get; set; }

        public string Profile { get; set; }

        public ChannelEntity()
        {
            this.Members = new List<MemberEntity>();
            this.messages = new ObservableCollection<ChatMessage>();
        }

        private ObservableCollection<ChatMessage> messages;
        public ObservableCollection<ChatMessage> Messages
        {
            get => messages;
            set => messages = value;
        }
        
    }
}
