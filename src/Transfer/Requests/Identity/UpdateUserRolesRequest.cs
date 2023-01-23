using System.Collections.Generic;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Transfer.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}