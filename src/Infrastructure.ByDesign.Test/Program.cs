// See https://aka.ms/new-console-template for more information
using Infrastructure.ByDesign;
using Synaplic.Inventory.Infrastructure.ByDesign.Service;

Console.WriteLine("Hello, World!");






//var results = await BydService.SetLogisticsTaskResponsibleAsync("00163E7336F91EDA92A4CA64E2EB56A8", "7000032");


var results2 = await ODataService.CreateStockID("THE_MATCHA", "TST_ODATA");

//var results = await BydService.GetLogisticsTasksAsync();
//foreach (var item in results)
//{
//    Console.WriteLine("ID {0}; process {2} ; Type :{1} ", item.ID, item.ProcessingStatusCodeText, item.LogisticsRequest[0]?.SiteLogisticsProcessTypeCodeText);
//}

Console.ReadKey();
