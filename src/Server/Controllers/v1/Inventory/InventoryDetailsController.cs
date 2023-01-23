using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Synaplic.Inventory.Server.Services.Inventory;
using Synaplic.Inventory.Transfer.Features.Session;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers.Inventory
{
    
    [ApiController]

    public class InventoryDetailsController  :BaseApiController<InventoryDetailsController>

    {

        private readonly IInventoryDetailsService _detailsService;

        public InventoryDetailsController(IInventoryDetailsService detailsService)
        {
            _detailsService = detailsService;
        }


        /// <summary>
        /// Add details
        /// </summary>
        /// <returns>Status 200 OK</returns>

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SaveAsync(DetailsQuery request)
        {
            return Ok(await _detailsService.SaveAsync(request));
        }



        /// <summary>
        /// Delete details
        /// </summary> s
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await _detailsService.DeleteAsync(id));
        }


        /// <summary>
        /// Get details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var details = await _detailsService.GetById(id);
            return Ok(details);
        }

        /// <summary>
        /// Get All Details
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet(nameof(GetAllDetails))]
        public async Task<IActionResult> GetAllDetails()
        {
            var result = await _detailsService.GetAllAsync();
            return Ok(result);
        }
    }
}
