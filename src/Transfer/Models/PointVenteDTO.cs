using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage.Synaplic.Animatrice.Models
{
    public class PointVenteDTO
    {
        public string CT_Num { get; set; }
        public string CT_Intitule { get; set; }
        public string CT_Adresse { get; set; }
        public string Email { get; set; }
        public string CT_Telephone { get; set; }
    }
}
