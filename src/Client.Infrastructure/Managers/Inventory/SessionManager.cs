using Synaplic.Inventory.Client.Infrastructure.Extensions;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Infrastructure.Managers.Inventory
{
    public interface ISessionManager:IManager
    {
        Task<IResult<int>> Delete(int id);
        Task<IResult<List<SessionDTO>>> GetAllSessions();

        Task<IResult<List<SessionDTO>>> GetActiveSessions();

        Task<IResult<List<SessionDTO>>> GetWaitingSessions();

        Task<IResult<SessionDTO>> GetBy(string id);
        Task<IResult<int>> SaveAsync(SessionDTO session);
        Task<IResult<bool>> OpenSessionAsync(SessionDTO task);
        Task<IResult<bool>> SaveScansAsync(List<ScanDTO> nonsavedScan);
        Task<IResult<List<ScanDTO>>> GetAllScansAsync(string sessionId);
        
        Task<IResult<List<StockDTO>>> GetAllStocksAsync(string sessionId);
        Task<IResult<bool>> ActiveSession(int id);
        Task<IResult<bool>> CancelSession(int id);
        Task<IResult<bool>> CloseSession(int id);
        Task<IResult<bool>> UnlockSession(int id);
        Task<IResult<bool>>  SaveStockAsync(List<StockDTO> stockDtos);
        Task<IResult<bool>>  GenerateEcartsAsync(SessionDTO session);
        Task<IResult<bool>> UpdateScanAsync(ScanDTO context);
    }
 
    public class SessionManager : ISessionManager
    {
        private readonly HttpClient _httpClient;
        public SessionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
 
        public async Task<IResult<int>> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.SessionEndpoints.Delete(id));
            return await response.ToResult<int>();
        }
        public async Task<IResult<SessionDTO>> GetBy(string id)
        {
            var response = await _httpClient.GetAsync(Routes.SessionEndpoints.GetBy(id));
            return await response.ToResult<SessionDTO>();
        }

        public async Task<IResult<List<SessionDTO>>> GetAllSessions()
        {
            var response = await _httpClient.GetAsync(Routes.SessionEndpoints.GetAllSessions);
            return await response.ToResult<List<SessionDTO>>();
        }

        public async Task<IResult<int>> SaveAsync(SessionDTO session)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SessionEndpoints.Save, session);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<SessionDTO>>> GetActiveSessions()
        {
            var response = await _httpClient.GetAsync(Routes.SessionEndpoints.GetByStatus(Shared.Enums.SessionStatus.Encours));
            return await response.ToResult<List<SessionDTO>>();
        }

        public async Task<IResult<List<SessionDTO>>> GetWaitingSessions()
        {
            var response = await _httpClient.GetAsync(Routes.SessionEndpoints.GetByStatus(Shared.Enums.SessionStatus.Activee));
            return await response.ToResult<List<SessionDTO>>();
        }

        public async Task<IResult<bool>> OpenSessionAsync(SessionDTO task)
        {
            var response = await _httpClient.PostAsync(Routes.SessionEndpoints.EditStatus(task.Id, SessionStatus.Encours),null);
            return await response.ToResult<bool>();
        }
        public async Task<IResult<bool>> SaveScansAsync(List<ScanDTO> scanDtos)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SessionEndpoints.SaveScans,scanDtos);
            return await response.ToResult<bool>();
        }

        public async Task<IResult<List<ScanDTO>>> GetAllScansAsync(string sessionId)
        {
            var response = await _httpClient.GetAsync(Routes.SessionEndpoints.GetAllScanBySession(sessionId));
            return await response.ToResult<List<ScanDTO>>();
        }

        public async Task<IResult<List<StockDTO>>> GetAllStocksAsync(string sessionId)
        {
            var response = await _httpClient.GetAsync(Routes.SessionEndpoints.GetAllStockBySession(sessionId));
            return await response.ToResult<List<StockDTO>>();
        }

        public async Task<IResult<bool>> ActiveSession(int id)
        {
            var response = await _httpClient.PostAsync(Routes.SessionEndpoints.EditStatus(id, SessionStatus.Activee),null);
            return await response.ToResult<bool>();
        }

        public async Task<IResult<bool>> CancelSession(int id)
        {
            var response = await _httpClient.PostAsync(Routes.SessionEndpoints.EditStatus(id, SessionStatus.Annulee),null);
            return await response.ToResult<bool>();
        }

        public async Task<IResult<bool>> CloseSession(int id)
        {
            var response = await _httpClient.PostAsync(Routes.SessionEndpoints.EditStatus(id, SessionStatus.Terminee),null);
            return await response.ToResult<bool>();
        }

        public async Task<IResult<bool>> UnlockSession(int id)
        {
            var response = await _httpClient.PostAsync(Routes.SessionEndpoints.EditStatus(id, SessionStatus.Encours),null);
            return await response.ToResult<bool>();
        }

        public async Task<IResult<bool>> SaveStockAsync(List<StockDTO> stockDtos)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SessionEndpoints.SaveStock,stockDtos);
            return await response.ToResult<bool>();
        }
        
        

        public async Task<IResult<bool>> GenerateEcartsAsync(SessionDTO session)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SessionEndpoints.GenerateEcarts,session);
            return await response.ToResult<bool>();
        }

        public async Task<IResult<bool>> UpdateScanAsync(ScanDTO context)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SessionEndpoints.UpdateScan,context);
            return await response.ToResult<bool>();
        }
    }
}
