using Microsoft.Extensions.Options;
using Synaplic.Inventory.Infrastructure.Sage.Configurations;
using Synaplic.Inventory.Infrastructure.Sage.Manager;
using Synaplic.Inventory.Infrastructure.Sage.Repositories;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.SageModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.Sage.Service
{
    public class SageLogisticsService
    {
        private readonly SageRepository ctx;

        public SageLogisticsService(SageRepository repository)
        {
            ctx = repository;
        }

        public async Task<Result<List<SageDepotDTO>>> GetAllDepotsAsync()
        {
            var list = await ctx.QueryAsync<SageDepotDTO>("SELECT_DEPOT");
            return await Result<List<SageDepotDTO>>.SuccessAsync(list.ToList());
        }
        public async Task<Result<List<SageFamilleDTO>>> GetAllFamillesAsync()
        {
            var list = await ctx.QueryAsync<SageFamilleDTO>("SELECT_FAMILLE");
            return await Result<List<SageFamilleDTO>>.SuccessAsync(list.ToList());
        }

        public async Task<Result<List<SageArticleDTO>>> GetAllArticlesAsync()
        {
            var list = await ctx.QueryAsync<SageArticleDTO>("SELECT_ARTICLE");
            return await Result<List<SageArticleDTO>>.SuccessAsync(list.ToList());
        }

        public async Task<Result<List<SageArticleDTO>>> GetAllArticlesByFamilleAsync(string familleFilter)
        {
            var list = await ctx.QueryAsync<SageArticleDTO>("SELECT_ARTICLE");
            list = list.Where(o => familleFilter.Contains(o.CodeFamille));
            return await Result<List<SageArticleDTO>>.SuccessAsync(list.ToList());
        }

        public async Task<Result<List<SageStockDTO>>> GetStockADateAsync(int idDepot, DateTime dateStock, string familleFilter)
        {
            try
            {
                var list = await ctx.QueryAsync<SageStockDTO>("STOCK_A_DATE_BY_DEPOT", new { DepotNo = idDepot, DateInventaire = dateStock.Date });
                if (!string.IsNullOrWhiteSpace(familleFilter))
                {
                   list = list.Where(o => familleFilter.Contains(o.CodeFamille)).ToList(); 
                }
                
                return await Result<List<SageStockDTO>>.SuccessAsync(list.ToList());
            }
            catch (Exception e)
            {

                return await Result<List<SageStockDTO>>.FailAsync(e);
            }
           
        }
        
    }
}
