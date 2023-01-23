using Synaplic.Inventory.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Infrastructure.Routes
{
    public static class SessionEndpoints
    {
        public static string Delete(int id) => $"​api/v1/InventorySession/Delete/{id}";
        public static string GetBy(string id) => $"api/v1/InventorySession?id={id}";

        public static string GetAllSessions = "api/v1/InventorySession/GetAll";

        public static string Save = "api/v1/InventorySession/Save";
        
        public static string SaveScans = "api/v1/InventorySession/SaveScans";

        public static string GetByStatus(SessionStatus status) =>
            $"api/v1/InventorySession/GetByStatus?status={status}";

        internal static string EditStatus(int id, SessionStatus status) =>
            $"api/v1/InventorySession/EditStatus?id={id}&status={status}";

        public static string GetAllScanBySession(string sessionId)=>
            $"api/v1/InventorySession/GetAllScanBySession?id={sessionId}"; 
        public static string GetAllStockBySession(string sessionId)=>
            $"api/v1/InventorySession/GetAllStockBySession?id=0{sessionId}";

        public static string  GenerateEcarts = "api/v1/InventorySession/GenerateEcarts";
        
        public static string SaveStock= "api/v1/InventorySession/SaveStock";


        public static string UpdateScan = "api/v1/InventorySession/UpdateScan";
    }
}