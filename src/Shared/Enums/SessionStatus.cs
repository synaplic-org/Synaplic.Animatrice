using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Shared.Enums
{
	public enum SessionStatus
	{
        [Description(@"Brouillon")]
        Brouillon = 0,

        [Description(@"Activé")]
        Activee = 1,

        [Description(@"En cours")]
        Encours = 2,

        [Description(@"Terminée")]
        Terminee = 3,

        [Description(@"En Integration")]
        EnIntegration = 4,
        
       [Description(@"Cloturée")]
        Cloturee = 5,
        
        [Description(@"Erreure")]
        Erreure = 6,
        
        [Description(@"Annulée")]
        Annulee = 99,

    }
}
