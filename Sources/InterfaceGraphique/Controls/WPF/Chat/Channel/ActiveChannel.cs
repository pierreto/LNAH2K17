using InterfaceGraphique.Entities;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ActiveChannel
    {
        public static ActiveChannel instance;

        private ChannelEntity channelEntity;
        public ChannelEntity ChannelEntity
        {
            get
            {
                return channelEntity;
            }
            set
            {
                if(channelEntity != value)
                {
                    channelEntity = value;
                    Program.unityContainer.Resolve<ChatViewModel>().CurrentChannel = value;
                }
            }
        }

        public static ActiveChannel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActiveChannel();
                }
                return instance;
            }
        }

        private ChannelEntity joinChannelEntity;
        public ChannelEntity JoinChannelEntity { get; set; }
    }
}
