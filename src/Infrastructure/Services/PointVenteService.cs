using Microsoft.Extensions.Localization;
using Sage.Synaplic.Animatrice.Models;
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
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.Services
{

    public interface IPointVenteService
    {
        Task<Result<List<PointVenteDTO>>> GetAllAsync();

    }
    public class PointVenteService : IPointVenteService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUnitOfWork<string> _scanUnitOfWork;
        private readonly IStringLocalizer<InventorySessionService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly SageLogisticsService _logisticsService;

        public PointVenteService(IStringLocalizer<InventorySessionService> localizer,
            IUnitOfWork<int> unitOfWork,
            IUnitOfWork<string> scanUnitOfWork, SageLogisticsService logisticsService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _scanUnitOfWork = scanUnitOfWork;
            _logisticsService = logisticsService;
            _currentUserService = currentUserService;
        }


        public async Task<Result<List<PointVenteDTO>>> GetAllAsync()
        {
            var clientResponse = await _unitOfWork.Repository<PointVente>().GetAllAsync();
            var results = clientResponse.Adapt<List<PointVenteDTO>>();
            return await Result<List<PointVenteDTO>>.SuccessAsync(results);
        }
    }
}
