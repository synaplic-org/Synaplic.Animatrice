using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Infrastructure.Routes
{
    public static class SageEndpoints
    {
        internal static string GetAllDepots = "api/v1/Sage/GetAllDepots";
        internal static string GetAllFamilles = "api/v1/Sage/GetAllFamilles";
        internal static string GetAllArticles = "api/v1/Sage/GetAllArticles";
        internal static string GetAllArticlesByFamille(string filter) => $"api/v1/Sage/GetAllArticlesByFamille?filter={Uri.EscapeDataString(filter)}";
        public static string? GetStockADate(int depot, DateTime date, string filter)
            => $"api/v1/Sage/GetStockADate?depot={depot}&date={date.Date.ToString(("s"))}&filter={Uri.EscapeDataString(filter)}" ;

    }
}
