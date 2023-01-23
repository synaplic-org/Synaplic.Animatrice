using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Synaplic.Inventory.Infrastructure.Sage.Service;
using Synaplic.Inventory.Infrastructure.Services.v1;
using Synaplic.Inventory.Shared.Constants.Permission;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.SageModels;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers.v1
{
    [ApiController]
    public class SageController : BaseApiController<SageController>
    {
        private readonly SageLogisticsService _SagelogisticsService;

        public SageController(SageLogisticsService logisticsService)
        {
            _SagelogisticsService = logisticsService;
        }

        /// <summary>
        /// Get Dashboard Data
        /// </summary>
        /// <returns>Status 200 OK </returns>
        //[Authorize(Policy = Permissions.Sage.View)]
        [HttpGet(nameof(GetAllDepots))]
        public async Task<IActionResult> GetAllDepots()
        {
            //dd
            var result = await _SagelogisticsService.GetAllDepotsAsync();
            return Ok(result);
        }


        [HttpGet(nameof(GetAllFamilles))]
        public async Task<IActionResult> GetAllFamilles()
        {
            //dd
            var result = await _SagelogisticsService.GetAllFamillesAsync();
            return Ok(result);
        }


        [HttpGet(nameof(GetAllArticles))]
        public async Task<IActionResult> GetAllArticles()
        {
            //dd
            var result = await _SagelogisticsService.GetAllArticlesAsync();
            return Ok(result);
        }

        [HttpGet(nameof(GetAllArticlesByFamille))]
        public async Task<IActionResult> GetAllArticlesByFamille(string filter)
        {
            //dd
            var result = await _SagelogisticsService.GetAllArticlesByFamilleAsync(filter);
            return Ok(result);
        }

        [HttpGet(nameof(GetStockADate))]
        public async Task<IActionResult> GetStockADate(int depot, DateTime date,string filter)
        {
            var result = await _SagelogisticsService.GetStockADateAsync(depot, date,filter);
            return Ok(result);
        }
    }
}