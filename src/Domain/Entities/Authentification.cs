using System;
using System.Collections.Generic;

namespace Synaplic.Inventory.Domain.Entities
{
    public partial class Authentification
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Intitule { get; set; }
        public string Animatrice { get; set; }
        public int? Active { get; set; }
        public string Email { get; set; }

        public virtual Animatrice AnimatriceNavigation { get; set; }
    }
}
