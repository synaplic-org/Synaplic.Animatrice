using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Transfer.SageModels
{
    public class SageDepotDTO
    {
        public int CodeDepot { get; set; }
        public string Intitule { get; set; }
        public string Adresse { get; set; }
        public string Complement { get; set; }
        public string Ville { get; set; }
        public string Contact { get; set; }
        public string Pays { get; set; }
        public string Telephone { get; set; }
        public string Telecopie { get; set; }
        public bool Pricipal { get; set; }
    }
}
