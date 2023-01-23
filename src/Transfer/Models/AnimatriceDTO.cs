using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Synaplic.Animatrice.Models
{
    public class AnimatriceDTO
    {
        public string Code { get; set; }
        public string Intitule { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Region { get; set; }
    }
}
