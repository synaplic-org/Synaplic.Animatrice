using System;
using System.Collections.Generic;

namespace Synaplic.Inventory.Domain.Entities
{
    public partial class Affectation
    {
        public int Id { get; set; }
        public string Animatrice { get; set; }
        public string PointVente { get; set; }
    }
}
