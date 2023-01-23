using System.ComponentModel;

namespace Synaplic.Inventory.Shared.Enums
{
    public enum UploadType : byte
    {
        

        [Description(@"Images\ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document
    }
}