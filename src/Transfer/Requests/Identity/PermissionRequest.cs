using System.Collections.Generic;

namespace Synaplic.Inventory.Transfer.Requests.Identity
{
    public class PermissionRequest
    {
        public string RoleId { get; set; }
        public IList<RoleClaimRequest> RoleClaims { get; set; }
    }
}