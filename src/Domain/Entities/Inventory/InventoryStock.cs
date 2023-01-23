using System;
using Synaplic.Inventory.Domain.Contracts;

namespace Synaplic.Inventory.Domain.Entities.Inventory
{
    public class InventoryStock : AuditableEntity<int>
    {
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
        
        public virtual InventorySession InventorySession { get; set; }   

    }
}
