using Microsoft.AspNetCore.Mvc;
using Sage.Synaplic.Animatrice.Models;
using Sage.Synaplic.Animatrice.Transfer.Requests;
using Synaplic.Inventory.Infrastructure.Services;
using Synaplic.Inventory.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers
{

    public class EchantillonController : BaseApiController<EchantillonController>
    {

        private readonly IEchantillonService _echantillonService;

        public EchantillonController(IEchantillonService echantillonService)
        {
            _echantillonService = echantillonService;
        }


        [HttpPost(nameof(SaveAsync))]
        public async Task<Result<int>> SaveAsync(EchantillonRequest request)
        {
            return await _echantillonService.SaveAsync(request);
        }



        [HttpDelete(nameof(DeleteAsync) + "/{id}")]
        public async Task<Result<int>> DeleteAsync(int id)
        {
            return await _echantillonService.DeleteAsync(id);
        }



        [HttpGet]
        public async Task<Result<EchantillonDTO>> GetById(int id)
        {
            return await _echantillonService.GetById(id);
        }


        [HttpGet(nameof(GetAllAsync))]
        public async Task<Result<List<EchantillonDTO>>> GetAllAsync()
        {
            return await _echantillonService.GetAllAsync();
        }

    }
}
