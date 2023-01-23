using Synaplic.Inventory.Infrastructure.Models.Identity;
using Synaplic.Inventory.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetAllPermissions(this List<RoleClaimResponse> allPermissions)
        {
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                        allPermissions.Add(new RoleClaimResponse { Value = propertyValue.ToString(), Type = Permissions.ClaimType, Group = module.Name });
                    //TODO - take descriptions from description attribute
                }
            }

        }

        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<UniRole> roleManager, UniRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == Permissions.ClaimType && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(Permissions.ClaimType, permission));
            }

            return IdentityResult.Failed();
        }
    }
}