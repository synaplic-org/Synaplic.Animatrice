using System;
using System.Collections.Generic;

namespace Synaplic.Inventory.Domain.Entities
{
    public partial class Article
    {
        public string Laboratoire { get; set; }
        public string Famille { get; set; }
        public string FaCodeFamille { get; set; }
        public string ArRef { get; set; }
        public string ArDesign { get; set; }
        public short? ArSommeil { get; set; }
        public short? ArSuiviStock { get; set; }
        public string ArCodeBarre { get; set; }
    }
}
