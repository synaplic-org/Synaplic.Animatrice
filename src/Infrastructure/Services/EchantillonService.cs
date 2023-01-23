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

    public interface IEchantillonService
    {
        Task<Result<int>> DeleteAsync(int id);
        Task<Result<List<EchantillonDTO>>> GetAllAsync();
        Task<Result<EchantillonDTO>> GetById(int id);
        Task<Result<int>> SaveAsync(EchantillonRequest request);
    }
    public class EchantillonService : IEchantillonService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUnitOfWork<string> _scanUnitOfWork;
        private readonly IStringLocalizer<InventorySessionService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly SageLogisticsService _logisticsService;

        public EchantillonService(IStringLocalizer<InventorySessionService> localizer,
            IUnitOfWork<int> unitOfWork,
            IUnitOfWork<string> scanUnitOfWork, SageLogisticsService logisticsService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _scanUnitOfWork = scanUnitOfWork;
            _logisticsService = logisticsService;
            _currentUserService = currentUserService;
        }



        public async Task<Result<int>> SaveAsync(EchantillonRequest request)
        {

            var echantillon = request.Adapt<Echantillon>();
            await _unitOfWork.Repository<Echantillon>().AddAsync(echantillon);
            await _unitOfWork.Commit(new CancellationToken());
            return await Result<int>.SuccessAsync(request.Numero, _localizer["echantillon créée"]);

        }

        public async Task<Result<int>> DeleteAsync(int id)
        {
            var echantillon = await _unitOfWork.Repository<Echantillon>().GetByIdAsync(id);
            if (echantillon != null)
            {
                await _unitOfWork.Repository<Echantillon>().DeleteAsync(echantillon);
                await _unitOfWork.Commit(new CancellationToken());
                return await Result<int>.SuccessAsync(id, _localizer["echantillon supprimé"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["echantillon Non trouvé!"]);
            }
        }


        public async Task<Result<EchantillonDTO>> GetById(int id)
        {
            var echantillon = await _unitOfWork.Repository<Echantillon>().GetByIdAsync(id);
            var mappedechantillon = echantillon.Adapt<EchantillonDTO>();

            return await Result<EchantillonDTO>.SuccessAsync(mappedechantillon);
        }


        public async Task<Result<List<EchantillonDTO>>> GetAllAsync()
        {
            var user = _currentUserService.UserId;
            var echantillonResponse = await _unitOfWork.Repository<Echantillon>().Entities.Where(o => (o.Animatrice == user)).ToListAsync();
            var results = echantillonResponse.Adapt<List<EchantillonDTO>>();
            return await Result<List<EchantillonDTO>>.SuccessAsync(results);
        }
    }
}
