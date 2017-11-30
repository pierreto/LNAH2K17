using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class JoinChannelListViewModel : ViewModelBase
    {

        #region Private Properties
        private ChatHub chatHub;
        private TaskFactory ctxTaskFactory;
        private ObservableCollection<ChatListItemViewModel> items;

        #endregion

        #region Public Properties
        public ObservableCollection<ChatListItemViewModel> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ICollectionView ItemsView
        {
            get { return CollectionViewSource.GetDefaultView(Items); }
        }
        #endregion

        #region Constructor
        public JoinChannelListViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
            this.chatHub.NewJoinableChannel += NewJoinableChannel;
            this.chatHub.ChannelDeleted += ChannelDeleted;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            Items = new ObservableCollection<ChatListItemViewModel>();
            ItemsView.Filter = new Predicate<object>(o => Filter(o as ChatListItemViewModel));
        }
        #endregion

        private bool Filter(ChatListItemViewModel clivm)
        {
            string channelName = Program.unityContainer.Resolve<JoinChannelViewModel>().ChannelName;
            return channelName == null || channelName == "" || clivm.Name.IndexOf(channelName) != -1;

        }

        private void ChannelDeleted(string channelName)
        {
            ctxTaskFactory.StartNew(() =>
            {
                this.Items.Remove(Items.FirstOrDefault(s => s.Name == channelName));
            }).Wait();
        }


        private void NewJoinableChannel(string channelName)
        {
            ctxTaskFactory.StartNew(() =>
            {
                this.Items.Add(new ChatListItemViewModel(new ChannelEntity { Name = channelName , IsJoinable = true }));
            }).Wait();
        }

        #region Overwritten Methods
        public override void InitializeViewModel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
