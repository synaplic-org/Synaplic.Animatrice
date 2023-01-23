using Synaplic.Inventory.Domain.Contracts;
using System;

namespace Synaplic.Inventory.Domain.Entities.Inventory
{
    public class InventoryScan : AuditableEntity<string>
    {
        public int SessionId { get; set; }
        public string CodeArticle { get; set; }
        public decimal QuantiteStock { get; set; }
        public string NumLot { get; set; }
        public string NumSerie { get; set; }
        public DateTime? DatePeremption { get; set; }
        public DateTime ScanTime    { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedTime { get; set; }

        public virtual InventorySession InventorySession { get; set; }
    }
}
