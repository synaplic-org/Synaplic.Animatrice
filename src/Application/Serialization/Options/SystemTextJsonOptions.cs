using System.Text.Json;
using Synaplic.Inventory.Shared.Interfaces.Serialization.Options;

namespace Synaplic.Inventory.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}