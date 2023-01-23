using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Transfer.Models
{
	public class ScanDTO
	{
        public string Id { get; set; }
        public int SessionId { get; set; }
        public string CodeArticle { get; set; }
        public decimal QuantiteStock { get; set; }
        public string NumLot { get; set; }
        public string NumSerie { get; set; }
        public DateTime? DatePeremption { get; set; }

        public DateTime ScanTime    { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedTime { get; set; }
	}
}
