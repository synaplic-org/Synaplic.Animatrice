using System;
using System.Collections.Generic;

namespace Synaplic.Inventory.Domain.Entities
{
    public partial class VwVenteGl
    {
        public long IdNum { get; set; }
        public string Article { get; set; }
        public string PointVente { get; set; }
        public string Famille { get; set; }
        public string Animatrice { get; set; }
        public decimal? Qte { get; set; }
        public string Commentaire { get; set; }
        public DateTime? DateVente { get; set; }
    }
}
