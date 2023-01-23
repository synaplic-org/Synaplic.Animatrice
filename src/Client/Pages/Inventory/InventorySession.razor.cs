using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using Synaplic.Inventory.Client.Extensions;
using Synaplic.Inventory.Client.Infrastructure.Managers.Inventory;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Client.Shared.Dialogs;
using Synaplic.Inventory.Shared.Constants.Application;
using Synaplic.Inventory.Shared.Constants.Permission;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Shared.Extensions;
using Synaplic.Inventory.Transfer.Models;
using Synaplic.Inventory.Transfer.SageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace Synaplic.Inventory.Client.Pages.Inventory
{
    public partial class InventorySession
    {
        [Inject] private ISessionManager _sessionManager { get; set; }
        [Inject] private ISageManager _sageManager { get; set; }


        private MudTable<SessionDTO> _table;

        private List<SessionDTO> _sessionList = new();
        private List<SageDepotDTO> _depotList = new();
        private SessionDTO _session = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateSession;
        private bool _canSearchSession;
        private bool _canEditSession;
        private bool _canViewSessions;
        private bool _canCancelSession;
        private bool _canValidateSession;
        private bool _canAnnulerSession;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateSession = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.CreateSession)).Succeeded;
            _canSearchSession = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.SearchSession)).Succeeded;
            _canEditSession = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.EditSession)).Succeeded;
            _canViewSessions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.ViewSession)).Succeeded;
            _canCancelSession = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.CancelSession)).Succeeded;
            _canValidateSession = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.ValidateSession)).Succeeded;
            _canAnnulerSession = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.CancelSession)).Succeeded;


            await GetDepots();
            await GetSessionAsync();
            
            _loaded = true;
            
        }

        private async Task DeleteAsync(int id)
        {
            string deleteContent = Localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(DeleteConfirmationDialog.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<DeleteConfirmationDialog>(Localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _sessionManager.Delete(id);
                if (response.Succeeded)
                {
                    await GetSessionAsync();
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task GetSessionAsync()
        {
            var response = await _sessionManager.GetAllSessions();
            if (response.Succeeded)
            {
                _sessionList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task GetDepots()
        {
            var response = await _sageManager.GetDepotsAsync();
            if (response.Succeeded)
            {
                _depotList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        internal string GetDepotName(string id )
        {
            if (string.IsNullOrWhiteSpace(id) || id == "0" ||_depotList.IsNullOrEmpty()) return "?";
            return _depotList.FirstOrDefault(x => x.CodeDepot.ToString() == id)?.Intitule ?? "?";
        }

        private async Task ActivateSession(int id)
        {
            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Attention"],
                Localizer["Voulez-vous activer cette session"],
                Localizer["Activer"],
                Localizer["Non"],
                Color.Success);
            if (confirmation == true)
            {
                var response = await _sessionManager.ActiveSession(id);
                if (response.Succeeded)
                {
                    await GetSessionAsync();
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        
      
        
        private async Task AnnulerSession(int id = 0)
        {


            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Attention"],
                Localizer["Voulez-vous annuler cette session"],
                Localizer["Annuler"],
                Localizer["Non"],
                Color.Warning);
            if (confirmation == true)
            {
                var response = await _sessionManager.CancelSession(id);
                if (response.Succeeded)
                {
                    await GetSessionAsync();
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }


        }


        private bool Search(SessionDTO session)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
            { return true; }
            if (session.Designation?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (session.Filtre?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _session = _sessionList.FirstOrDefault(c => c.Id == id);
                if (_session != null)
                {
                    parameters.Add(nameof(InevntorySessionModal.SessionModel),  _session);
                
                }
            }
            parameters.Add(nameof(InevntorySessionModal._depotList), _depotList);
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<InevntorySessionModal>(id == 0 ? Localizer["Create"] : Localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {

                await Reset();
            }
        }


        private async Task Reset()
        {
            _session = new SessionDTO();
            await GetSessionAsync();
        }


        private async Task TerminerSession(int contextId)
        {
            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Attention"],
                Localizer["Voulez-vous terminer cette session"],
                Localizer["Terminer"],
                Localizer["Non"],
                Color.Dark);
            if (confirmation == true)
            {
                var response = await _sessionManager.CloseSession(contextId);
                if (response.Succeeded)
                {
                    await GetSessionAsync();
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }

        }
        
        private async Task UnlockSession(int contextId)
        {
            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Attention"],
                Localizer["Voulez-vous remettre en cours cette session"],
                Localizer["Remise en cours"],
                Localizer["Non"],
                Color.Secondary);
            if (confirmation == true)
            {
                var response = await _sessionManager.UnlockSession(contextId);
                if (response.Succeeded)
                {
                    await GetSessionAsync();
                    await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ShowScanDetails(int contextId)
        {
            _navigationManager.NavigateTo($"/Logistics/Invenotry/Sessions/Details/{contextId}");
        }

        private async Task ShowErreurDetails(int contextId)
        {
            //_navigationManager.NavigateTo($"/Logistics/Mobile/Sessions/Erreurs/{contextId}");
        }

        private async Task ShowEcartDetails(int contextId)
        {
            _navigationManager.NavigateTo($"/Logistics/Invenotry/Sessions/Ecarts/{contextId}");
        }

       
    }
}
