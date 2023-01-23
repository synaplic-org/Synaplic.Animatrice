using Infrastructure.Bydesign.OData.LogisticsTasks;
using Soap.Byd.QuerySiteLogisticsTaskIn;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synaplic.Inventory.Infrastructure.ByDesign.Service;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.DataModel;
using Synaplic.Inventory.Transfer.Requests.Logistics;

namespace Infrastructure.ByDesign
{
    public class BydService
    {
        public static async Task<List<SiteLogisticsTask>> GetLogisticsTasksAsync() => await ODataService.GetLogisticsTasksAsync();

        public static async Task<List<ResponsibleEmployee>> GetResponsibleEmployeesAsync() => await ODataService.GetResponsibleEmployeesAsync();
        public static async Task<List<LogisticsArea>> GetLogisticsAreasAsync() => await ODataService.GetLogisticsAreasAsync();

        public static async Task<bool> SetLogisticsTaskResponsibleAsync(string taskObjectID, string employeeID) => await ODataService.SetLogisticsTaskResponsibleAsync(taskObjectID, employeeID);

        public static async Task<bool> RemoveTaskResponsibleAsync(string taskObjectID) => await ODataService.RemoveTaskResponsibleAsync(taskObjectID);

        public static async System.Threading.Tasks.Task<SiteLogisticsTaskByElementsResponse_sync> GetSiteLogisticsTaskAsync(string taskID)
        {
            return await SoapService.GetSiteLogisticsTaskAsync("my345234", "_UNIVERSCAN", "Azur@222324", taskID);
        }
        public static async Task<MaterialIdentification> GetMaterialAsync(string productID) => await ODataService.GetMaterialAsync(productID);

        public static async Task<IResult<bool>> SendInboundScans(List<LogisticTaskMaterialOutputDTO> outpus)
        {
            //return await SoapService.SendInboundScans(outpus,"my345234", "_UNIVERSCAN", "Azur@222324");
            return await Synaplic.Inventory.Infrastructure.ByDesign.Logistics.Inbound.InboundIntegrationService.SendInboundScans(outpus, "my345234", "_UNIVERSCAN", "Azur@222324");
        }
        public static async Task<IResult<List<SearchProductDTO>>> GetMaterialsBySearchAsync(ProductSearchRequest request) => await ODataService.GetMaterialsBySearchAsync(request);
        
    }
}