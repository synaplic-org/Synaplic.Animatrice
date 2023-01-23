using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Synaplic.Inventory.Server.Services.Inventory;
using Synaplic.Inventory.Transfer.Features.Session;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers.Inventory
{
  
    [ApiController]

    public class InventoryStockController  :BaseApiController<InventoryStockController>

    {

        private readonly IInventoryStockService _stockService;

        public InventoryStockController(IInventoryStockService stockService)
        {
            _stockService = stockService;
        }


        /// <summary>
        /// Add stock
        /// </summary>
        /// <returns>Status 200 OK</returns>

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(StockQuery request)
        {
            return Ok(await _stockService.SaveAsync(request));
        }


        
        /// <summary>
        /// Delete a stock
        /// </summary> s
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await _stockService.DeleteAsync(id));
        }



        /// <summary>
        /// Get stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var stocks = await _stockService.GetById(id);
            return Ok(stocks);
        }


        /// <summary>
        /// Get All Stocks
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet(nameof(GetAllStocks))]
        public async Task<IActionResult> GetAllStocks()
        {
            var result = await _stockService.GetAllAsync();
            return Ok(result);
        }
    }
}