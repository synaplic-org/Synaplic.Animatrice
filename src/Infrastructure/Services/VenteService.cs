using Mapster;
using Microsoft.Extensions.Localization;
using Sage.Synaplic.Animatrice.Models;
using Sage.Synaplic.Animatrice.Transfer.Requests;
using Synaplic.Inventory.Application.Interfaces.Repositories;
using Synaplic.Inventory.Application.Interfaces.Services;
using Synaplic.Inventory.Domain.Entities;
using Synaplic.Inventory.Infrastructure.Sage.Service;
using Synaplic.Inventory.Infrastructure.Services.v1.Inventory;
using Synaplic.Inventory.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.Services
{

    public interface IVenteService
    {
        Task<Result<int>> DeleteAsync(int id);
        Task<Result<List<VentesDTO>>> GetAllAsync();
        Task<Result<VentesDTO>> GetById(int id);
        Task<Result<int>> SaveAsync(VentesRequest request);
    }
    public class VenteService : IVenteService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUnitOfWork<string> _scanUnitOfWork;
        private readonly IStringLocalizer<InventorySessionService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly SageLogisticsService _logisticsService;

        public VenteService(IStringLocalizer<InventorySessionService> localizer,
            IUnitOfWork<int> unitOfWork,
            IUnitOfWork<string> scanUnitOfWork, SageLogisticsService logisticsService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _scanUnitOfWork = scanUnitOfWork;
            _logisticsService = logisticsService;
            _currentUserService = currentUserService;
        }



        public async Task<Result<int>> SaveAsync(VentesRequest request)
        {

            var vente = request.Adapt<Vente>();
            await _unitOfWork.Repository<Vente>().AddAsync(vente);
            await _unitOfWork.Commit(new CancellationToken());
            return await Result<int>.SuccessAsync(request.Numero, _localizer["Vente créée"]);

        }

        public async Task<Result<int>> DeleteAsync(int id)
        {
            var vente = await _unitOfWork.Repository<Vente>().GetByIdAsync(id);
            if (vente != null)
            {
                await _unitOfWork.Repository<Vente>().DeleteAsync(vente);
                await _unitOfWork.Commit(new CancellationToken());
                return await Result<int>.SuccessAsync(id, _localizer["vente supprimé"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["vente Non trouvé!"]);
            }
        }


        public async Task<Result<VentesDTO>> GetById(int id)
        {
            var vente = await _unitOfWork.Repository<Vente>().GetByIdAsync(id);
            var mappedvente = vente.Adapt<VentesDTO>();

            return await Result<VentesDTO>.SuccessAsync(mappedvente);
        }


        public async Task<Result<List<VentesDTO>>> GetAllAsync()
        {
            var user = _currentUserService.UserId;
            var venteResponse = await _unitOfWork.Repository<Vente>().Entities.Where(o => (o.Animatrice == user)).ToListAsync();
            var results = venteResponse.Adapt<List<VentesDTO>>();
            return await Result<List<VentesDTO>>.SuccessAsync(results);
        }
    }
}
