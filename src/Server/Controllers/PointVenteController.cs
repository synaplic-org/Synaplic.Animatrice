using Microsoft.AspNetCore.Mvc;
using Sage.Synaplic.Animatrice.Models;
using Synaplic.Inventory.Infrastructure.Services;
using Synaplic.Inventory.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Server.Controllers
{
    public class PointVenteController : BaseApiController<PointVenteController>
    {
        private readonly IPointVenteService _pointventeService;

        public PointVenteController(IPointVenteService pointventeService)
        {
            _pointventeService = pointventeService;
        }

        [HttpGet(nameof(GetAllAsync))]
        public async Task<Result<List<PointVenteDTO>>> GetAllAsync()
        {
            return await _pointventeService.GetAllAsync();
        }

    }
}
