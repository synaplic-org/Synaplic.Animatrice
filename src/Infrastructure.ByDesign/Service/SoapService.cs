using Soap.Byd.QuerySiteLogisticsTaskIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using Synaplic.Inventory.Shared.Extensions;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.DataModel;
using Infrastructure.Bydesign.OData.LogisticsTasks;

namespace Synaplic.Inventory.Infrastructure.ByDesign.Service
{
    public class SoapService
    {
        public static System.ServiceModel.BasicHttpBinding Binding
        {
            get
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var binding = new System.ServiceModel.BasicHttpBinding
                {
                    SendTimeout = TimeSpan.FromSeconds(900),
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    AllowCookies = true,
                    ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max
                };
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Basic;

                return binding;
            }
        }

        internal static EndpointAddress ManageSiteLogisticsTaskInAdresse(string tenantId)
        {
            return new EndpointAddress(
                $"https://{tenantId}.sapbydesign.com/sap/bc/srt/scs/sap/managesitelogisticstaskin?sap-vhost={tenantId}.sapbydesign.com");
        }

        public static EndpointAddress QuerySiteLogisticsTaskInAdresse(string tenantId)
        {
            return new EndpointAddress(
                $"https://{tenantId}.sapbydesign.com/sap/bc/srt/scs/sap/querysitelogisticstaskin?sap-vhost={tenantId}.sapbydesign.com");
        }

        public static async System.Threading.Tasks.Task<SiteLogisticsTaskByElementsResponse_sync>
            GetSiteLogisticsTaskAsync(string tenantId, string userName, string passWord, string taskID)
        {
            var TenantId = tenantId;
            var UserName = userName;
            var PassWord = passWord;

            var client = new QuerySiteLogisticsTaskInClient(Binding, QuerySiteLogisticsTaskInAdresse(TenantId));

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = PassWord;

            try
            {
                var conditions = new QueryProcessingConditions()
                { QueryHitsUnlimitedIndicator = false, QueryHitsMaximumNumberValue = 10 };
                var query = new SiteLogisticsTaskByElementsQueryMessage
                {
                    ProcessingConditions = conditions,
                    SiteLogisticsTaskSelectionByElements = new SiteLogisticsTaskSelectionByElements
                    {
                        SelectionBySiteLogisticsTaskID = new SiteLogisticsTaskSelectionByTaskId[]
                        {
                            new SiteLogisticsTaskSelectionByTaskId
                            {
                                IntervalBoundaryTypeCode = "1",
                                InclusionExclusionCode = "I",
                                LowerBoundarySiteLogisticsTaskID =
                                    new Soap.Byd.QuerySiteLogisticsTaskIn.BusinessTransactionDocumentID {Value = taskID}
                            }
                        }
                    }
                };

                var TaskResponse = await client.FindByElementsAsync(query);

                if (TaskResponse.SiteLogistcsTaskByElementsResponse_sync.SiteLogisticsTask != null)
                {
                    return TaskResponse.SiteLogistcsTaskByElementsResponse_sync.SiteLogisticsTask.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }


        public static async Task<IResult<bool>> SendInboundScans(List<LogisticTaskMaterialOutputDTO> outpus, string tenantId, string userName, string passWord)
        {
            var TenantId = tenantId;
            var UserName = userName;
            var PassWord = passWord;




            var oSendableOUtput = outpus.Where(o => o.OpenQuantity > 0 && o.ConfirmQuantity > 0).OrderBy(o => o.LineItemID).ToList();
            if (oSendableOUtput.IsNullOrEmpty()) return await Result<bool>.FailAsync("Nothing to send !");
            var oIds = oSendableOUtput.Select(o => o.LineItemID).Distinct().ToList();



            var oSiteLogisticsTask = new List<Saop.Byd.ManageSiteLogisticsTaskIn.SiteLogisticsTaskMaintainBundleRequest>();

            foreach (var oId in oIds)
            {
                var oLines = oSendableOUtput.Where(o => o.LineItemID == oId).OrderByDescending(o => o.OutputUUID).ToList();
                var oReqOuts = new List<Saop.Byd.ManageSiteLogisticsTaskIn.SLTMaterialOutputManageBundle_Request>();
                var oTaskID = string.Empty;
                var oTaskUID = string.Empty;

                foreach (var oLine in oLines)
                {
                    var oOut = new Saop.Byd.ManageSiteLogisticsTaskIn.SLTMaterialOutputManageBundle_Request()
                    {
                        MaterialOutputUUID = new Saop.Byd.ManageSiteLogisticsTaskIn.UUID() { Value = oLine.OutputUUID },
                        ProductID = new Saop.Byd.ManageSiteLogisticsTaskIn.ProductID() { Value = oLine.ProductID },
                        ActualQuantity = new Saop.Byd.ManageSiteLogisticsTaskIn.Quantity() { Value = oLine.ConfirmQuantity, unitCode = oLine.ConfirmQuantityUnitCode },
                        TargetLogisticsAreaID = oLine.TargetLogisticsAreaID
                    };
                    if (!string.IsNullOrEmpty(oLine.IdentifiedStockID))
                    {
                        oOut.IdentifiedStockID =    new Saop.Byd.ManageSiteLogisticsTaskIn.IdentifiedStockID() { Value = oLine.IdentifiedStockID };
                    }
                    if (!string.IsNullOrWhiteSpace(oLine.OutputUUID))
                    {
                        oTaskID = oLine.TaskId;
                        oTaskUID = oLine.OutputUUID;

                        if (oLines.Count> 1)
                        {
                            oOut.SplitIndicatorSpecified =    oOut.SplitIndicator = true;
                        }

                    }


                    //LogisticsDeviationReasonCode =  new Saop.Byd.ManageSiteLogisticsTaskIn.LogisticsDeviationReasonCode() { Value = oLine.LogisticsDeviationReasonCode }

                    if (oLine.ConfirmQuantity <  oLine.OpenQuantity &&  (string.IsNullOrWhiteSpace(oLine.LogisticsDeviationReasonCode) || oLine.LogisticsDeviationReasonCode == "1"))
                    {
                        oOut.SplitIndicatorSpecified =    oOut.SplitIndicator = true;
                    }



                    // oOut.SerialNumber = new[] { "", "", "", "", "" },
                    oReqOuts.Add(oOut);
                }
                var oReq = new Saop.Byd.ManageSiteLogisticsTaskIn.SiteLogisticsTaskMaintainBundleRequest()
                {
                    SiteLogisticTaskID = new Saop.Byd.ManageSiteLogisticsTaskIn.BusinessTransactionDocumentID() { Value = oTaskID },
                    SiteLogisticTaskUUID = new Saop.Byd.ManageSiteLogisticsTaskIn.UUID() { Value = oTaskUID },
                    ReferenceObject = new[] {
                        new Saop.Byd.ManageSiteLogisticsTaskIn.SiteLogisticsTaskReferenceObjectManageBundle_Request()
                        {
                            ReferenceObjectUUID = new Saop.Byd.ManageSiteLogisticsTaskIn.UUID() { Value = oTaskUID },
                            OperationActivity = new[]
                                    {
                                new Saop.Byd.ManageSiteLogisticsTaskIn.SiteLogisticsLotOperationActivityManageBundle_Request()
                                {
                                    OperationActivityUUID = new Saop.Byd.ManageSiteLogisticsTaskIn.UUID() { Value = oTaskUID },
                                    MaterialOutput = oReqOuts.ToArray()
                                }
                            }
                        }
                    }
                };
                oSiteLogisticsTask.Add(oReq);
            }




            try
            {
                var conditions = new QueryProcessingConditions()
                { QueryHitsUnlimitedIndicator = false, QueryHitsMaximumNumberValue = 10 };
                var request = new Saop.Byd.ManageSiteLogisticsTaskIn.SiteLogisticsTaskMaintainRequestBundleMessage()
                {
                    BasicMessageHeader = new Saop.Byd.ManageSiteLogisticsTaskIn.BusinessDocumentBasicMessageHeader()
                    {
                        //UUID = new    UUID() {Value = ""},
                        //ReferenceID = new BusinessDocumentMessageID(){Value = ""},
                        ID = new Saop.Byd.ManageSiteLogisticsTaskIn.BusinessDocumentMessageID() { Value = DateTime.Now.ToString("yyMMddhhmmssfffffff") },
                        //ReferenceUUID = new Saop.Byd.ManageSiteLogisticsTaskIn.UUID(){Value = "" }
                    },
                    SiteLogisticsTask = oSiteLogisticsTask.ToArray()
                };



                var client = new Saop.Byd.ManageSiteLogisticsTaskIn.ManageSiteLogisticsTaskInClient(Binding, ManageSiteLogisticsTaskInAdresse(TenantId));

                client.ClientCredentials.UserName.UserName = UserName;
                client.ClientCredentials.UserName.Password = PassWord;
                await client.OpenAsync();
                var TaskResponse = await client.MaintainBundle_V1Async(request);

                if (TaskResponse.SiteLogisticsTaskBundleMaintainResponse_sync_V1.SiteLogisticsTaskResponse != null)
                {
                    var erros = new List<string>();
                    foreach (var res in TaskResponse.SiteLogisticsTaskBundleMaintainResponse_sync_V1.SiteLogisticsTaskResponse)
                    {
                        foreach (var log in res.SiteLogisticsTaskLog)
                        {
                            if (log.SiteLogisticsTaskSeverityCode.StartsWith("E", StringComparison.InvariantCultureIgnoreCase))
                            {
                                erros.Add($"{res.TaskID}:{log.SiteLogisticsTaskNodeName}:{log.SiteLogisticsTaskNote}");
                            }

                            //S	Success
                            //E	Error
                            //I	Information
                            //W	Warning
                            //A	Abort
                        }
                    }

                    if (erros.IsNullOrEmpty() ==false)
                    {
                        return await Result<bool>.FailAsync(erros);
                    }

                }
            }
            catch (Exception e)
            {
                return await Result<bool>.SuccessAsync(false, e.Message);
            }

            return await Result<bool>.SuccessAsync(true);
        }



    }
}