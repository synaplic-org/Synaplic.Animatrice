using System;
using Mapster;
using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Application.Interfaces.Repositories;
using Synaplic.Inventory.Shared.Constants.Application;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.Models;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Dashboard.Resources;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Serilog;
using Synaplic.Inventory.Infrastructure.Sage.Service;
using Synaplic.Inventory.Transfer.SageModels;
using Synaplic.Inventory.Application.Interfaces.Services;
using System.Security.Policy;

namespace Synaplic.Inventory.Infrastructure.Services.v1.Inventory
{
    public interface IInventorySessionService
    {
        //Task<Result<int>> DeleteAsync(int id);
        //Task<Result<bool>> EditStatus(int id, SessionStatus status);
        //Task<Result<List<SessionDTO>>> GetAllAsync();
        //Task<Result<SessionDTO>> GetById(int id);
        //Task<Result<List<SessionDTO>>> GetByStatusAsync(SessionStatus status);
        //Task<Result<int>> SaveAsync(SessionDTO request);
        //Task<Result<bool>> SaveScansAsync(List<ScanDTO> scanDtos);
        //Task<Result<bool>> UpdateScanAsync(ScanDTO scanDto);
        //Task<Result<List<ScanDTO>>> GetAllScanBySessionAsync(int id);
        //Task<Result<bool>> GenerateEcartsAsync(SessionDTO session);
        //Task<Result<List<StockDTO>>> GetAllStockBySessionAsync(int id);
        //Task<Result<bool>> SaveStockAsync(List<StockDTO> stockDtos);
  
    }

    public class InventorySessionService : IInventorySessionService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUnitOfWork<string> _scanUnitOfWork;
        private readonly IStringLocalizer<InventorySessionService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly SageLogisticsService _logisticsService;

        public InventorySessionService(IStringLocalizer<InventorySessionService> localizer,
            IUnitOfWork<int> unitOfWork,
            IUnitOfWork<string> scanUnitOfWork, SageLogisticsService logisticsService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _scanUnitOfWork = scanUnitOfWork;
            _logisticsService = logisticsService;
            _currentUserService = currentUserService;
        }


        //public async Task<Result<int>> DeleteAsync(int id)
        //{
        //    var session = await _unitOfWork.Repository<InventorySession>().GetByIdAsync(id);
        //    if (session != null)
        //    {
        //        await _unitOfWork.Repository<InventorySession>().DeleteAsync(session);
        //        await _unitOfWork.Commit(new CancellationToken());
        //        return await Result<int>.SuccessAsync(id, _localizer["session supprimé"]);
        //    }
        //    else
        //    {
        //        return await Result<int>.FailAsync(_localizer["session Non trouvé!"]);
        //    }
        //}

        //public async Task<Result<int>> SaveAsync(SessionDTO request)
        //{
        //    try
        //    {
        //        if (request.Id == 0)
        //        {
        //            var session = request.Adapt<InventorySession>();
        //            await _unitOfWork.Repository<InventorySession>().AddAsync(session);
        //            await _unitOfWork.Commit(new CancellationToken());
        //            return await Result<int>.SuccessAsync(session.Id, _localizer["session créée"]);
        //        }
        //        else
        //        {
        //            var session = await _unitOfWork.Repository<InventorySession>().GetByIdAsync(request.Id);
        //            session.Designation = request.Designation;
        //            session.DateDebut = request.DateDebut.Value;
        //            session.DateDebutPrevisionnel = request.DateDebut.Value;
        //            session.DateFin = request.DateFin.Value;
        //            session.Status = request.Status;
        //            session.DepotId = request.DepotId;
        //            session.Filtre = request.Filtre;



        //            await _unitOfWork.Repository<InventorySession>().UpdateAsync(session);
        //            await _unitOfWork.Commit(new CancellationToken());
        //            return await Result<int>.SuccessAsync(session.Id, _localizer["session mise à jour"]);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return await Result<int>.FailAsync(ex.Message);
        //    }
        //}

        //public async Task<Result<bool>> SaveScansAsync(List<ScanDTO> scanDtos)
        //{
        //    try
        //    {
        //        var sessionId = scanDtos.FirstOrDefault().SessionId;

        //        var session = await _unitOfWork.Repository<InventorySession>().GetByIdAsync(sessionId);

        //        if (session == null) throw new ApplicationException(_localizer["Session introuvable !"]);

        //        if (session.Status != SessionStatus.Encours)
        //            return await Result<bool>.FailAsync(_localizer["Action non autorisée, la session n'est plus en cours "]);

        //        foreach (var dto in scanDtos)
        //        {
        //            var scan = dto.Adapt<InventoryScan>();
        //            await _scanUnitOfWork.Repository<InventoryScan>().AddAsync(scan);
        //        }

        //        await _unitOfWork.Commit(new CancellationToken());
        //        return await Result<bool>.SuccessAsync(true, _localizer["Scans enregistrés"]);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Fatal("SaveScansAsync", e);
        //        return await Result<bool>.FailAsync(e);
        //    }
        //}

        //public async Task<Result<bool>> UpdateScanAsync(ScanDTO scanDto)
        //{
        //    try
        //    {
        //        var scan = await _scanUnitOfWork.Repository<InventoryScan>().GetByIdAsync(scanDto.Id);
        //        if (scan == null) await Result<bool>.FailAsync(_localizer["Scan introuvable"]);
        //        if (scan.Deleted != scanDto.Deleted)
        //        {
        //            scan.Deleted = scanDto.Deleted;
        //            scan.DeletedTime = scanDto.DeletedTime;
        //        }
        //        await _scanUnitOfWork.Repository<InventoryScan>().UpdateAsync(scan);
        //        await _scanUnitOfWork.Commit(new CancellationToken());
        //        return await Result<bool>.SuccessAsync(true, _localizer["Scan mis à jour"]);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return await Result<bool>.FailAsync(ex.Message);
        //    }
        //}

        //public async Task<Result<List<ScanDTO>>> GetAllScanBySessionAsync(int id)
        //{
        //    var sessionsResponse = await _scanUnitOfWork.Repository<InventoryScan>().Entities.Where(o => o.SessionId == id).ToListAsync();
        //    var session = sessionsResponse.Adapt<List<ScanDTO>>();

        //    return await Result<List<ScanDTO>>.SuccessAsync(session);
        //}

        //public async Task<Result<List<StockDTO>>> GetAllStockBySessionAsync(int id)
        //{
        //    var sessionsResponse = await _unitOfWork.Repository<InventoryStock>().Entities.Where(o => o.SessionId == id).ToListAsync();
        //    var session = sessionsResponse.Adapt<List<StockDTO>>();


        //    return await Result<List<StockDTO>>.SuccessAsync(session);
        //}

        //public async Task<Result<bool>> SaveStockAsync(List<StockDTO> stockDtos)
        //{
        //    foreach (var dto in stockDtos)
        //    {
        //        var dbStock = await _unitOfWork.Repository<InventoryStock>().GetByIdAsync(dto.Id);
        //        dbStock.ValeurInventaire = dto.ValeurInventaire;
        //        dbStock.ValeurStock = dto.ValeurStock;
        //        dbStock.NumLot = dto.NumLot;
        //        if (dbStock.Exclure != dto.Exclure)
        //        {
        //            dbStock.Exclure = dto.Exclure;
        //            dbStock.ExcludeBy = dto.ExcludeBy;
        //            dbStock.DateExclusion = dto.DateExclusion;
        //        }
        //        dbStock.JusftifEcart = dto.JusftifEcart ?? String.Empty;
        //        await _unitOfWork.Repository<InventoryStock>().UpdateAsync(dbStock);
        //    }
        //    try
        //    {
        //        await _unitOfWork.Commit(new CancellationToken());
        //        return await Result<bool>.SuccessAsync(true, _localizer["Entregistré avec succès !"]);
        //    }
        //    catch (Exception e)
        //    {
        //        return await Result<bool>.FailAsync(e);
        //    }

        //}




        //public async Task<Result<bool>> GenerateEcartsAsync(SessionDTO dto)
        //{
        //    var dbsession = await _unitOfWork.Repository<InventorySession>().GetByIdAsync(dto.Id);
        //    if (dbsession == null && dbsession.Status != SessionStatus.Terminee)
        //        return await Result<bool>.FailAsync(_localizer["Action non possible"]);

        //    var oldStocks = _unitOfWork.Repository<InventoryStock>().Entities.Where(o => o.SessionId == dbsession.Id)
        //        .ToList();

        //    foreach (var oldStock in oldStocks)
        //    {
        //        await _unitOfWork.Repository<InventoryStock>().DeleteAsync(oldStock);
        //    }

        //    var oScans = _scanUnitOfWork.Repository<InventoryScan>().Entities
        //        .Where(o => o.SessionId == dbsession.Id && o.Deleted == false)
        //        .GroupBy(p => new { p.CodeArticle, p.NumLot })

        //        .Select(y => new { CodeArticle = y.Key, NumLot = y.Key, Quantite = y.Sum(x => x.QuantiteStock) })
        //        .ToList();

        //    int.TryParse(dbsession.DepotId, out var depotId);

        //    var artciles = (await _logisticsService.GetAllArticlesByFamilleAsync(dbsession.Filtre)).Data ?? new List<SageArticleDTO>();


        //    var StockDatesResult = await _logisticsService.GetStockADateAsync(depotId, dbsession.DateDebut, dbsession.Filtre);
        //    if (!StockDatesResult.Succeeded) return await Result<bool>.FailAsync(StockDatesResult.Messages);

        //    artciles = artciles.OrderBy(o => o.CodeFamille).ThenBy(o => o.CodeArticle).ToList();
        //    var StockSage = StockDatesResult.Data.ToList();
        //    //InventoryStock newStk = new InventoryStock();
        //    foreach (var art in artciles)
        //    {
        //        var newStk = new InventoryStock()
        //        {
        //            CodeArticle = art.CodeArticle,
        //            Designation = art.Designation,
        //            Famille = art.CodeFamille,
        //            SessionId = dbsession.Id,
        //            QuantiteStock = StockDatesResult.Data.Where(o => o.Reference == art.CodeArticle).Sum(o => o.QteStockDate),
        //            QuantiteMouvement = StockDatesResult.Data.Where(o => o.Reference == art.CodeArticle).Sum(o => o.QteMouvement),
        //            QuantiteInventaire = oScans.Where(o => o.CodeArticle.CodeArticle == art.CodeArticle).Sum(o => o.Quantite)
        //        };

        //        if (newStk.QuantiteStock != 0 || newStk.QuantiteInventaire != 0)
        //        {
        //            await _unitOfWork.Repository<InventoryStock>().AddAsync(newStk);
        //        }
        //    }
        //    //foreach (var art in artciles)
        //    //{
        //    //    foreach (var item in StockSage)
        //    //    {
        //    //        if (art.CodeArticle == item.Reference)
        //    //        {
        //    //            var oScanInv = oScans.Where(x => x.CodeArticle.CodeArticle == item.Reference).ToList();
        //    //            foreach (var itemScan in oScanInv)
        //    //            {
        //    //                //if (item.Lot == item.Lot)
        //    //                //{
        //    //                    newStk.CodeArticle = art.CodeArticle;
        //    //                    newStk.Designation = art.Designation;
        //    //                    newStk.Famille = art.CodeFamille;
        //    //                    newStk.NumLot = item.Lot;
        //    //                    newStk.SessionId = dbsession.Id;
        //    //                    newStk.QuantiteStock = item.QteStockDate;
        //    //                    newStk.QuantiteMouvement = StockDatesResult.Data.Where(o => o.Reference == art.CodeArticle).Sum(o => o.QteMouvement);
        //    //                    //newStk.QuantiteInventaire = oScans.Where(o => o.CodeArticle.CodeArticle == art.CodeArticle).Sum(o => o.Quantite);
        //    //                    newStk.QuantiteInventaire =itemScan.Quantite;
        //    //                //}
        //    //                //else
        //    //                //{
        //    //                //    newStk.CodeArticle = art.CodeArticle;
        //    //                //    newStk.Designation = art.Designation;
        //    //                //    newStk.Famille = art.CodeFamille;
        //    //                //    newStk.NumLot = itemScan.NumLot.NumLot;
        //    //                //    newStk.SessionId = dbsession.Id;
        //    //                //    newStk.QuantiteStock = 0;
        //    //                //    newStk.QuantiteMouvement = StockDatesResult.Data.Where(o => o.Reference == art.CodeArticle).Sum(o => o.QteMouvement);
        //    //                //    //newStk.QuantiteInventaire = oScans.Where(o => o.CodeArticle.CodeArticle == art.CodeArticle).Sum(o => o.Quantite);
        //    //                //    newStk.QuantiteInventaire = itemScan.Quantite;
        //    //                //}
        //    //                if (!(newStk.QuantiteStock == 0 && newStk.QuantiteInventaire == 0))
        //    //                {
        //    //                    await _unitOfWork.Repository<InventoryStock>().AddAsync(newStk);
        //    //                }
        //    //            }

        //    //        }



        //    //    }
        //    // }

        //    try
        //    {
        //        await _unitOfWork.Commit(new CancellationToken());
        //        return await Result<bool>.SuccessAsync(true, _localizer["Rapport généré avec succès !"]);
        //    }
        //    catch (Exception e)
        //    {
        //        return await Result<bool>.FailAsync(e);
        //    }


        //}


        //public async Task<Result<SessionDTO>> GetById(int id)
        //{
        //    var session = await _unitOfWork.Repository<InventorySession>().GetByIdAsync(id);
        //    var mappedsession = session.Adapt<SessionDTO>();

        //    return await Result<SessionDTO>.SuccessAsync(mappedsession);
        //}


        //public async Task<Result<List<SessionDTO>>> GetAllAsync()
        //{
        //    var depot = _currentUserService.SiteId;
        //    var sessionsResponse = await _unitOfWork.Repository<InventorySession>().Entities.Where(o => (o.DepotId == _currentUserService.SiteId || _currentUserService.SiteId == "*")).ToListAsync();
        //    var results = sessionsResponse.Adapt<List<SessionDTO>>();
        //    return await Result<List<SessionDTO>>.SuccessAsync(results);
        //}

        //public async Task<Result<List<SessionDTO>>> GetByStatusAsync(SessionStatus status)
        //{

        //    var sessionsResponse = await _unitOfWork.Repository<InventorySession>().Entities
        //        .Where(o => o.Status == status && (o.DepotId == _currentUserService.SiteId || _currentUserService.SiteId == "*")).ToListAsync();
        //    var session = sessionsResponse.Adapt<List<SessionDTO>>();

        //    return await Result<List<SessionDTO>>.SuccessAsync(session);
        //}

        //public async Task<Result<bool>> EditStatus(int id, SessionStatus status)
        //{
        //    try
        //    {
        //        var session = await _unitOfWork.Repository<InventorySession>().GetByIdAsync(id);
        //        if (session == null) await Result<bool>.FailAsync(_localizer["session introuvable"]);
        //        session.Status = status;
        //        await _unitOfWork.Repository<InventorySession>().UpdateAsync(session);
        //        await _unitOfWork.Commit(new CancellationToken());
        //        return await Result<bool>.SuccessAsync(true, _localizer["session mise à jour"]);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return await Result<bool>.FailAsync(ex.Message);
        //    }
        //}
    }
}