using InterfaceGraphique.Entities;
using Microsoft.Practices.Unity;

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
                channelEntity = value;
                Program.unityContainer.Resolve<ChatViewModel>().CurrentChannel = value;
                //foreach (var item in Program.unityContainer.Resolve<ChatListViewModel>().Items)
                //{
                //    item.IsSelected = false;
                //}
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
    }
}
