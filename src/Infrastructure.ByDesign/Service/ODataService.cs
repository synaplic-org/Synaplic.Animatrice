using Infrastructure.Bydesign.OData.LogisticsTasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using Mapster;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synaplic.Inventory.Infrastructure.ByDesign.Extentions;
using Synaplic.Inventory.Infrastructure.ByDesign.Logistics.Stock;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.DataModel;
using Synaplic.Inventory.Transfer.Requests.Logistics;
using Microsoft.IdentityModel.Tokens;
using Soap.Byd.QuerySiteLogisticsTaskIn;

namespace Synaplic.Inventory.Infrastructure.ByDesign.Service
{
    public class ODataService
    {
        public static string OdataAdresse = "https://my345234.sapbydesign.com/sap/byd/odata/cust/v1/uniscan/";
        public static string OdataAuthorization = "Basic Ym91dGFtZW46V2VsY29tZTIy";

        private static void OdataBuildingRequest(object sender, BuildingRequestEventArgs e)
        {
            e.Headers.Add("Authorization", OdataAuthorization);
        }

        public static async Task<List<SiteLogisticsTask>> GetLogisticsTasksAsync()
        {
            var oDataClient = new uniscan(new Uri(OdataAdresse));
            oDataClient.BuildingRequest+= OdataBuildingRequest;

            var results = await oDataClient.SiteLogisticsTaskCollection
                .AddQueryOption("$expand", "ResponsibleEmployee,LogisticsRequest,LogisticsTaskFolder")
                .AddQueryOption("$filter", "ProcessingStatusCode eq '1' or ProcessingStatusCode eq '2'")
                .ExecuteAsync();
            return results.Item2.ToList();
        }

        class Results
        {
            public List<SearchProductDTO> results { get; set; } 
        }

        public static async Task<IResult<List<SearchProductDTO>>> GetMaterialsBySearchAsync(ProductSearchRequest request)
        {
            var oDataClient = new RestClient($"https://my345234.sapbydesign.com/sap/byd/odata/ana_businessanalytics_analytics.svc/");
            oDataClient.Authenticator = new HttpBasicAuthenticator("boutamen", "Welcome22");
            var ressource = $"RPSCMINBU03_Q0001QueryResults?$format=json&$select=CCATCP_UUID,CCUSTODIAN_UUID,CINV_TYPE,CINV_UNIT,CIN_CONSIGNMENT_IND,CIN_QUALITY_IND,CISTOCK_UUID,CLOCATION_UUID,CLOG_AREA_UUID,CMATERIAL_UUID,COWNER_UUID,CPARENT_LU_UUID,CPRS_VER_UUID,CRESTRICTED,CSITE_UUID,CSPA_UUID,FCENDING_QUANTITY,FCENDING_QUANTITY_NODIM,KCENDING_QUANTITY,KCENDING_QUANTITY_NODIM,TCATCP_UUID,TCUSTODIAN_UUID,TINV_TYPE,TINV_UNIT,TISTOCK_UUID,TLOCATION_UUID,TLOG_AREA_UUID,TMATERIAL_UUID,TOWNER_UUID,TPARENT_LU_UUID,TPRS_VER_UUID,TSITE_UUID&$orderby=CMATERIAL_UUID%20asc&$filter=(CSITE_UUID eq  '{request.SiteID}')";
            if (!string.IsNullOrEmpty(request.LogisticArea))
            {
                ressource.Remove(ressource.Length - 1);
                ressource = ressource + $" and (CLOG_AREA_UUID eq '{request.SiteID + "/"+ request.LogisticArea}')";
            }
            if (!string.IsNullOrEmpty(request.Product))
            {
                ressource.Remove(ressource.Length - 1);
                ressource = ressource + $" and (CMATERIAL_UUID eq '{request.Product}')";
            }

            if (!string.IsNullOrEmpty(request.IdentifiedStock))
            {
                ressource.Remove(ressource.Length - 1);
                ressource = ressource + $" and (CISTOCK_UUID eq '{request.Product+ "/"+ request.IdentifiedStock}')";
            }

            var result = await oDataClient.BydGetAsync(ressource);
            StockResponse stockResponse = System.Text.Json.JsonSerializer.Deserialize<StockResponse>(result.Content);

            var objects = stockResponse.D.results.Adapt<List<SearchProductDTO>>();
            return await Result < List < SearchProductDTO >>.SuccessAsync(objects);

        }

        public static async Task<List<ResponsibleEmployee>> GetResponsibleEmployeesAsync()
        {
            var oDataClient = new uniscan(new Uri(OdataAdresse));
            oDataClient.BuildingRequest+= OdataBuildingRequest;
            var results = await oDataClient.ResponsibleEmployeeCollection.ExecuteAsync();
            return results.Item2.ToList();
        }

        public static async Task<List<SiteLogisticsTask>> GetLogisticsTaskByIDAsync(string taskID)
        {
            var oDataClient = new uniscan(new Uri(OdataAdresse));
            oDataClient.BuildingRequest+= OdataBuildingRequest;
            var results = await oDataClient.SiteLogisticsTaskCollection
                .AddQueryOption("$expand", "ResponsibleEmployee,LogisticsRequest,LogisticsTaskFolder")
                .AddQueryOption("$filter", $"ID eq '{taskID}'").ExecuteAsync();
            return results.Item2.ToList();
        }
        
        public static async Task<List<LogisticsArea>> GetLogisticsAreasAsync()
        {
            var oDataClient = new uniscan(new Uri(OdataAdresse));
            oDataClient.BuildingRequest+= OdataBuildingRequest;
            var results = await oDataClient.LogisticsAreaCollection.ExecuteAsync();
            return results.Item2.ToList();
        }

        public static async Task<MaterialIdentification> GetMaterialAsync(string productID)
        {
            //var oDataClient = new RestClient(OdataAdresse);
            //oDataClient.Authenticator = new HttpBasicAuthenticator("boutamen", "Welcome22");
            //var ressource = $"MaterialIdentificationCollection?$format=json&$expand=Material&$filter= ProductID eq '{productID}'";
            //var result = await oDataClient.BydPostAsync(ressource);
            //var material = System.Text.Json.JsonSerializer.Deserialize<Material>(result.Content);

            var oDataClient = new uniscan(new Uri(OdataAdresse));
            oDataClient.BuildingRequest+= OdataBuildingRequest;

            var results = await oDataClient.MaterialIdentificationCollection
                .AddQueryOption("$expand", "Material")
                .AddQueryOption("$filter", $"ProductID eq '{productID}'")
                .ExecuteAsync();
            return results.Item2?.FirstOrDefault();

        }

        public static async Task<bool> SetLogisticsTaskResponsibleAsync(string taskID, string employeeID)
        {
            var oDataClient = new RestClient(OdataAdresse);
            oDataClient.Authenticator  = new HttpBasicAuthenticator("boutamen", "Welcome22");
            var ressource = $"SiteLogisticsTaskSetResponsible?ObjectID='{taskID}'&ResponsibleEmployeeID='{employeeID}'";
            var result = await oDataClient.BydPostAsync(ressource);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public static async Task<bool> RemoveTaskResponsibleAsync(string taskID)
        {
            var oDataClient = new RestClient(OdataAdresse);
            oDataClient.Authenticator  = new HttpBasicAuthenticator("boutamen", "Welcome22");
            var ressource = $"SiteLogisticsTaskRemoveResponsible?ObjectID='{taskID}'";
            var result = await oDataClient.BydPostAsync(ressource);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public static async Task<List<LogisticsArea>> GetStockIDAsync(string stockID, string ProductID)
        {
            var oDataClient = new uniscan(new Uri(OdataAdresse));
            oDataClient.BuildingRequest+= OdataBuildingRequest;

            var results = await oDataClient.IdentifiedStockCollection
                .AddQueryOption("$filter", $"ID eq '{stockID}' and ProductID eq '{ProductID}'")
                .ExecuteAsync();
             
            return results.Item2.Adapt<List<LogisticsArea>>() ;

        }

        public static async Task<bool> CreateStockID(string ProductID,string ID)
        {
            var oDataClient = new RestClient(OdataAdresse);
            oDataClient.Authenticator  = new HttpBasicAuthenticator("boutamen", "Welcome22");
            var ressource = $"IdentifiedStockCollection";
             
            var str = $"{{\"ID\": \"{ID}\",\"ProductID\": \"{ProductID}\",\"LifeCycleStatusCode\": \"2\"}}";
            var result = await oDataClient.BydPostJsonAsync(ressource,str);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

    }

    public class StockIDRequest 
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("ProductID")]
        public string ProductID { get; set; }
    }

}