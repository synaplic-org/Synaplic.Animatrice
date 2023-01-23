using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Synaplic.Inventory.Transfer.SageModels
{
    public class SageArticleDTO
    {
        public string CodeArticle { get; set; }
        public string CodeBarre { get; set; }
        public string Designation  { get; set; }
        public string CodeFamille  { get; set; }
        public string SuiviStock  { get; set; }
        public string EnSommiel  { get; set; }

        [JsonIgnore]
        public decimal SumScannedQte { get; set; }
    }
}
