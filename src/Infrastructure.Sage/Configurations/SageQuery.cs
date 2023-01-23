using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.Sage.Configurations
{
    public class SageQuery
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Query { get; set; }
        public string Path { get; set; }
        public string Domain { get; set; }
    }
}
