using System;
using Synaplic.Inventory.Client.Infrastructure.Extensions;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.SageModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Infrastructure.Managers.Sage
{
    public interface ISageManager: IManager
    {
        Task<IResult<List<SageArticleDTO>>> GetArticlesAsync();
        Task<IResult<List<SageArticleDTO>>> GetArticlesByFamilleAsync(string filter);
        Task<IResult<List<SageDepotDTO>>> GetDepotsAsync();
        Task<IResult<List<SageFamilleDTO>>> GetFamillesAsync();
        Task<IResult<List<SageStockDTO>>> GetStockADate(int depot, DateTime date, string filter);
    }

    public class SageManager : ISageManager
    {
        private readonly HttpClient _httpClient;

        public SageManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<SageDepotDTO>>> GetDepotsAsync()
        {
            var response = await _httpClient.GetAsync(Routes.SageEndpoints.GetAllDepots);
            return await response.ToResult<List<SageDepotDTO>>();
        }


        public async Task<IResult<List<SageFamilleDTO>>> GetFamillesAsync()
        {
            var response = await _httpClient.GetAsync(Routes.SageEndpoints.GetAllFamilles);
            return await response.ToResult<List<SageFamilleDTO>>();
        }

        public async Task<IResult<List<SageArticleDTO>>> GetArticlesAsync()
        {
            var response = await _httpClient.GetAsync(Routes.SageEndpoints.GetAllArticles);
            return await response.ToResult<List<SageArticleDTO>>();
        }

        public async Task<IResult<List<SageArticleDTO>>> GetArticlesByFamilleAsync(string filter)
        {
            var response = await _httpClient.GetAsync(Routes.SageEndpoints.GetAllArticlesByFamille(filter));
            return await response.ToResult<List<SageArticleDTO>>();
        }
        
        public async Task<IResult<List<SageStockDTO>>>  GetStockADate(int depot, DateTime date,string filter)
        {
            var response = await _httpClient.GetAsync(Routes.SageEndpoints.GetStockADate(depot,date,filter));
            return await response.ToResult<List<SageStockDTO>>();
        }
    }
}
