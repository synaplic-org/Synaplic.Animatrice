using Synaplic.Inventory.Application.Interfaces.Common;
using Synaplic.Inventory.Shared.Wrapper;
using System.Threading.Tasks;
using Synaplic.Inventory.Transfer.Requests.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}