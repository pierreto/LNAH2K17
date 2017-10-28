using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListViewModel : ViewModelBase
    {
        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }

        public List<ChatListItemViewModel> Items { get; set; }
        public ChatListViewModel()
        {
            Items = new List<ChatListItemViewModel>
            {
                new ChatListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    ProfilePictureRGB = "3099c5",
                    NewContentAvailable = true
                },
                new ChatListItemViewModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    ProfilePictureRGB = "fe4503",
                    IsSelected = true
                },
                new ChatListItemViewModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    ProfilePictureRGB = "00d405",
                },
                new ChatListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    ProfilePictureRGB = "3099c5"
                },
                new ChatListItemViewModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    ProfilePictureRGB = "fe4503"
                },
                new ChatListItemViewModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    ProfilePictureRGB = "00d405"
                },
                new ChatListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    ProfilePictureRGB = "3099c5"
                },
                new ChatListItemViewModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    ProfilePictureRGB = "fe4503"
                },
                new ChatListItemViewModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    ProfilePictureRGB = "00d405"
                },
                new ChatListItemViewModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    ProfilePictureRGB = "3099c5"
                },
                new ChatListItemViewModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    ProfilePictureRGB = "fe4503"
                },
                new ChatListItemViewModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    ProfilePictureRGB = "00d405"
                }
            };
        }

    }
}
