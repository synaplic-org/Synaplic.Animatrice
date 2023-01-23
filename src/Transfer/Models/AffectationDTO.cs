using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Synaplic.Animatrice.Models
{
    public class AffectationDTO
    {
        public int id { get; set; }
        public string Animatrice { get; set; }
        public string PointVente { get; set; }
    }
}
