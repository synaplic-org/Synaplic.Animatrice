using System.Collections.Generic;

namespace Synaplic.Inventory.Transfer.Responses.Identity
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}