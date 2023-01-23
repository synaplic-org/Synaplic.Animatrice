using Synaplic.Inventory.Shared.Enums;

namespace Synaplic.Inventory.Transfer.Requests
{
    public class UploadRequest
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[] Data { get; set; }
    }
}