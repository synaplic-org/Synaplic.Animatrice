using Synaplic.Inventory.Domain.Contracts;
using Synaplic.Inventory.Shared.Enums;
using System;
using System.Collections.Generic;



namespace Synaplic.Inventory.Domain.Entities.Inventory
{
    public class InventorySession : AuditableEntity<int>
    {
        public string Designation { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateDebutPrevisionnel { get; set; }
        public DateTime DateFin { get; set; }
        public SessionStatus  Status { get; set; }
        public string DepotId { get; set; }
        public string Filtre { get; set; }

        public string Log { get; set; }
        public virtual List<InventoryStock> InventoryStocks { get; set; }

        public virtual List<InventoryScan> InventoryScans { get; set; }
    }
}
