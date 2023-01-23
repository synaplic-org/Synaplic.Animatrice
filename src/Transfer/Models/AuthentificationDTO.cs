using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Synaplic.Animatrice.Models
{
    public class AuthentificationDTO 
    {
        public string login_ { get; set; }
        public string Password_ { get; set; }
        public string intitule { get; set; }
        public string Animatrice { get; set; }
        public Nullable<int> Active { get; set; }
        public string Email { get; set; }
    }
}
