using System;
using System.Collections.Generic;

namespace Synaplic.Inventory.Domain.Entities
{
    public partial class Animatrice
    {
        public Animatrice()
        {
            Authentifications = new HashSet<Authentification>();
        }

        public string Code { get; set; }
        public string Intitule { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Region { get; set; }

        public virtual ICollection<Authentification> Authentifications { get; set; }
    }
}
