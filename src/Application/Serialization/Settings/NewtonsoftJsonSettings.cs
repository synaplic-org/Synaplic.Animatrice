using Newtonsoft.Json;
using Synaplic.Inventory.Shared.Interfaces.Serialization.Settings;

namespace Synaplic.Inventory.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}