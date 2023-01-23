using System;
using System.Collections.Generic;

namespace Synaplic.Inventory.Domain.Entities
{
    public partial class Echantillon
    {
        public int Numero { get; set; }
        public string Article { get; set; }
        public string Client { get; set; }
        public decimal? Qte { get; set; }
        public decimal? Prix { get; set; }
        public string Commentaire { get; set; }
        public DateTime? DateEchantillon { get; set; }
        public string Animatrice { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
    }
}
