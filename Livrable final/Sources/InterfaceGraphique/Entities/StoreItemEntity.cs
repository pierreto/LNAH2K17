using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class StoreItemEntity
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string TextureName { get; set; }

        private string imageUrl;
        public string ImageUrl
        {
            get
            {
                return Directory.GetCurrentDirectory() + imageUrl;
            }
            set
            {
                imageUrl = value;
            }
        }

        public int Id { get; set; }

        public bool IsGameEnabled { get; set; }
    }
}
