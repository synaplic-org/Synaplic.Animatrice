using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Sage.Synaplic.Animatrice.Transfer.Requests
{
    public class EchantillonRequest
    {
        public int Numero { get; set; }
        public string Article { get; set; }
        public string client { get; set; }
        public Nullable<decimal> Qte { get; set; }
        public Nullable<decimal> Prix { get; set; }
        public string Commentaire { get; set; }
        public Nullable<System.DateTime> DateEchantillon { get; set; }
        public string Animatrice { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public string NameArticle { get; set; }
        public string Famille { get; set; }
    }
}
