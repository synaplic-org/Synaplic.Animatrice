using Synaplic.Inventory.Transfer.Requests;

namespace Synaplic.Inventory.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}