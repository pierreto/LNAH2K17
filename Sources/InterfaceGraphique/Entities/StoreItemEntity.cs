using System;
using System.Collections.Generic;
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

        public string ImageUrl { get; set; }

        public int Id { get; set; }

        public bool IsChecked { get; set; }

        public bool IsGameEnabled { get; set; }
    }
}
