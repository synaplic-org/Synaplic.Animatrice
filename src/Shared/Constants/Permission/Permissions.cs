using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Synaplic.Inventory.Shared.Constants.Permission
{
    public static class Permissions
    {
        public const string ClaimType = "Permission";


        public static class Inventory
        {
            public const string Home = "Permissions.Inventory.Home";
            public const string ViewSession = "Permissions.Inventory.ViewSession";
            public const string CreateSession = "Permissions.Inventory.CreateSession";
            public const string EditSession = "Permissions.Inventory.EditSession";
            public const string ValidateSession = "Permissions.Inventory.ValidateSession";
            public const string OpenSession = "Permissions.Inventory.OpenSession";
            public const string CloseSession = "Permissions.Inventory.CloseSession";
            public const string CancelSession = "Permissions.Inventory.CancelSession";
            public const string SearchSession = "Permissions.Inventory.SearchSession";
            public const string ViewStock = "Permissions.Inventory.ViewStock";
            public const string Scan = "Permissions.Inventory.Scan";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

       

        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";
            //TODO - add permissions
        }


        public static class Infrastructure
        {
            public const string Jobs = "Permissions.Infrastructure.Jobs";
            public const string Swagger = "Permissions.Infrastructure.Swagger";
        }

        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }
        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }
    }
}