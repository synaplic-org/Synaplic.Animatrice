using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Synaplic.Inventory.Transfer.Models
{
	public class StockDTO
	{
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string CodeArticle { get; set; }
        public string Designation { get; set; }
        public string Famille { get; set; }
        public decimal QuantiteInventaire { get; set; }
        public decimal QuantiteStock { get; set; }
        
        public decimal QuantiteMouvement { get; set; }
        
        public decimal ValeurInventaire { get; set; }
        
        public decimal ValeurStock { get; set; }
        
        public string JusftifEcart { get; set; }
        
        public bool Exclure { get; set; }
        
        public DateTime DateExclusion { get; set; }
        
        public string ExcludeBy { get; set; }
        
        public string NumLot { get; set; }
        
        public string NumSerie  { get; set; }

        [JsonIgnore]
        public decimal EcartStock => QuantiteInventaire - QuantiteStock  ;
        
        [JsonIgnore]
        public decimal ValeurUnitaire => QuantiteInventaire!= 0 ?  ValeurInventaire / QuantiteInventaire : 0; 
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
