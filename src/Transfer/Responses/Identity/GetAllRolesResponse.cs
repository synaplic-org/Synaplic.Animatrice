using System.Collections.Generic;

namespace Synaplic.Inventory.Transfer.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}