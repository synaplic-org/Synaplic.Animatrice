using Microsoft.AspNetCore.Mvc;
using Sage.Synaplic.Animatrice.Models;
using Synaplic.Inventory.Infrastructure.Services;
using Synaplic.Inventory.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers
{
    public class ArticleController : BaseApiController<ArticleController>
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet(nameof(GetAllAsync))]
        public async Task<Result<List<ArticleDTO>>> GetAllAsync()
        {
            return await _articleService.GetAllAsync();
        }
    }
}
