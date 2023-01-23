using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Sage.Synaplic.Animatrice.Models
{
    public class ArticleDTO 
    {
        public string Laboratoire { get; set; }
        public string Famille { get; set; }
        public string FA_CodeFamille { get; set; }
        public string AR_Ref { get; set; }
        public string AR_Design { get; set; }
        public Nullable<short> AR_Sommeil { get; set; }
        public Nullable<short> AR_SuiviStock { get; set; }
        public string AR_CodeBarre { get; set; }
    }
}
