using Synaplic.Inventory.Application.Interfaces.Common;
using Synaplic.Inventory.Shared.Wrapper;
using System.Threading.Tasks;
using Synaplic.Inventory.Transfer.Requests.Identity;

namespace Synaplic.Inventory.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}