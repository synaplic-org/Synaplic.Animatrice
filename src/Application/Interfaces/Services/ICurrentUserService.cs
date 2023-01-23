using Synaplic.Inventory.Application.Interfaces.Common;
using System.Collections.Generic;

namespace Synaplic.Inventory.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
        public string SiteId { get; }

        public string Name { get; }

        public List<KeyValuePair<string, string>> Claims { get; set; }
    }
}