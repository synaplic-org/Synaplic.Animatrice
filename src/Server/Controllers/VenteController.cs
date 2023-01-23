using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sage.Synaplic.Animatrice.Models;
using Sage.Synaplic.Animatrice.Transfer.Requests;
using Synaplic.Inventory.Infrastructure.Services;
using Synaplic.Inventory.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers
{
    public class VenteController : BaseApiController<VenteController>
    {
        private readonly IVenteService _venteService;

        public VenteController(IVenteService venteService)
        {
            _venteService = venteService;
        }

       
        [HttpPost(nameof(SaveAsync))]
        public async Task<Result<int>> SaveAsync(VentesRequest request)
        {
            return await _venteService.SaveAsync(request);
        }



        [HttpDelete(nameof(DeleteAsync) + "/{id}")]
        public async Task<Result<int>> DeleteAsync(int id)
        {
            return await _venteService.DeleteAsync(id);
        }


    
        [HttpGet]
        public async Task<Result<VentesDTO>> GetById(int id)
        {
            return await _venteService.GetById(id);
        }

       
        [HttpGet(nameof(GetAllAsync))]
        public async Task<Result<List<VentesDTO>>> GetAllAsync()
        {
            return await _venteService.GetAllAsync();
        }
    }
}
