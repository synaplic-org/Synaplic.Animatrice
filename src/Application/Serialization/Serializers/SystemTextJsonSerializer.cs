using System.Text.Json;
using Synaplic.Inventory.Application.Serialization.Options;
using Microsoft.Extensions.Options;
using Synaplic.Inventory.Shared.Interfaces.Serialization.Serializers;

namespace Synaplic.Inventory.Application.Serialization.Serializers
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<SystemTextJsonOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public T Deserialize<T>(string data)
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}