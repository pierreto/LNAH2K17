using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class Achievement
    {
        public string Name { get; set; }

        private string DisabledImageUrl { get; set; }

        private string EnabledImageUrl { get; set; }

        public string ImageUrl
        {
            get
            {
                return IsEnabled ? EnabledImageUrl : DisabledImageUrl;
            }
        }

        public bool IsEnabled { get; set; }

        public Achievement()
        {

        }

        public Achievement(string disabledImageUrl, string enabledImageUrl)
        {
            DisabledImageUrl = disabledImageUrl;
            EnabledImageUrl = enabledImageUrl;
        }
    }
}
