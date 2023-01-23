using Synaplic.Inventory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Permissions;
using Synaplic.Inventory.Infrastructure.Models.Identity;

namespace Synaplic.Inventory.Server.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Name = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) + " " +
                   httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname);

            SiteId = httpContextAccessor.HttpContext?.User?.FindFirstValue(nameof(UniUser.SiteID)) ?? "*";
          
            Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
        }

        public string SiteId { get; }

        public string UserId { get; }

        public string Name { get; }

        public List<KeyValuePair<string, string>> Claims { get; set; }
    }
}