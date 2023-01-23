using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Synaplic.Inventory.Infrastructure.Services.v1.Inventory;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Transfer.Models;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers.v1.Inventory
{
    [ApiController]
    public class InventorySessionController : BaseApiController<InventorySessionController>

    {
        private readonly IInventorySessionService _sessionService;

        public InventorySessionController(IInventorySessionService sessionService)
        {
            _sessionService = sessionService;
        }


        /// <summary>
        /// Add session
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpPost(nameof(Save))]
        public async Task<IActionResult> Save(SessionDTO request)
        {
            return Ok(await _sessionService.SaveAsync(request));
        }


        /// <summary>
        /// Delete session
        /// </summary> s
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _sessionService.DeleteAsync(id));
        }


        /// <summary>
        /// Get session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var sessions = await _sessionService.GetById(id);
            return Ok(sessions);
        }

        /// <summary>
        /// Get All Sessions
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sessionService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get All Sessions
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet(nameof(GetByStatus))]
        public async Task<IActionResult> GetByStatus(SessionStatus status)
        {
            var result = await _sessionService.GetByStatusAsync(status);
            return Ok(result);
        }


        [HttpPost(nameof(EditStatus))]
        public async Task<IActionResult> EditStatus(int id, SessionStatus status)
        {
            var result = await _sessionService.EditStatus(id, status);
            return Ok(result);
        }

        [HttpPost(nameof(SaveScans))]
        public async Task<IActionResult> SaveScans(List<ScanDTO> scanDtos)
        {
            var result = await _sessionService.SaveScansAsync(scanDtos);
            return Ok(result);
        }

        [HttpPost(nameof(UpdateScan))]
        public async Task<IActionResult> UpdateScan(ScanDTO scanDto)
        {
            var result = await _sessionService.UpdateScanAsync(scanDto);
            return Ok(result);
        }

        [HttpGet(nameof(GetAllScanBySession))]
        public async Task<IActionResult> GetAllScanBySession(int id)
        {
            var result = await _sessionService.GetAllScanBySessionAsync(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetAllStockBySession))]
        public async Task<IActionResult> GetAllStockBySession(int id)
        {
            var result = await _sessionService.GetAllStockBySessionAsync(id);
            return Ok(result);
        }

        [HttpPost(nameof(GenerateEcarts))]
        public async Task<IActionResult> GenerateEcarts(SessionDTO session)
        {
            var result = await _sessionService.GenerateEcartsAsync(session);
            return Ok(result);
        }


        [HttpPost(nameof(SaveStock))]
        public async Task<IActionResult> SaveStock(List<StockDTO> stockDtos)
        {
            var result = await _sessionService.SaveStockAsync(stockDtos);
            return Ok(result);
        }


    }
}