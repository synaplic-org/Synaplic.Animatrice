using System.Collections.Generic;
using System.Threading.Tasks;
using Synaplic.Inventory.Application.Interfaces.Common;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.Requests.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}