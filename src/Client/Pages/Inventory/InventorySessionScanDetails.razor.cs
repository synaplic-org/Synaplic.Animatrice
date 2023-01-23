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
using Synaplic.Inventory.Transfer.Responses.Identity;

namespace Synaplic.Inventory.Client.Pages.Inventory
{
    public partial class InventorySessionScanDetails
    {
        [Parameter] public string sessionId { get; set; }


        [Inject] private ISessionManager _sessionManager { get; set; }
        [Inject] private ISageManager _sageManager { get; set; }

 
        private List<SageDepotDTO> _depotList = new();
 
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


        private bool _isLoading = true;
        private SessionDTO _mySession = new();
        //private List<SageArticleDTO> _articleListe = new();
        private List<ScanDTO> _allScanListe = new();
        private ClaimsPrincipal _userClaims;
        private string _currentUserId;
        private List<UserResponse> _userListe = new();

        private void SetLoading(bool val)
        {
            _isLoading = val;
            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            _userClaims = await _stateProvider.GetAuthenticationStateProviderUserAsync();
            _currentUserId = _userClaims.GetUserId();
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateSession =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.CreateSession))
                .Succeeded;
            _canSearchSession =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.SearchSession))
                .Succeeded;
            _canEditSession =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.EditSession)).Succeeded;
            _canViewSessions =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.ViewSession)).Succeeded;
            _canCancelSession =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.CancelSession))
                .Succeeded;
            _canValidateSession =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.ValidateSession))
                .Succeeded;
            _canAnnulerSession =
                (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.CancelSession))
                .Succeeded;
        }

        protected override async Task OnInitializedAsync()
        {
            SetLoading(true);
            await GetUsers();
            await GetSession();
            //await GetArticles();
            await GetAllScansAsync();
            await GetDepots();
            SetLoading(false);
        }

        //private async Task GetArticles()
        //{
        //    if (_mySession == null) return;

        //    SetLoading(true);
        //    var response = await _sageManager.GetArticlesByFamilleAsync(_mySession.Filtre);
        //    if (response.Succeeded)
        //    {
        //        _articleListe = response.Data;
        //    }
        //    else
        //    {
        //        _ = await ShowErrors(response.Messages);
        //    }
        //    SetLoading(false);
        //}
        
        private async Task GetUsers()
        {
            if (_mySession == null) return;

            SetLoading(true);
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _userListe = response.Data;
            }
            else
            {
                _ = await ShowErrors(response.Messages);
            }
            SetLoading(false);
        }

        private async Task GetAllScansAsync()
        {
            if (_mySession == null) return;
             
            SetLoading(true);
            var response = await _sessionManager.GetAllScansAsync(sessionId);
            if (response.Succeeded)
            {
                _allScanListe = response.Data;
            }
            else
            {
                _ = await ShowErrors(response.Messages);
            }
            SetLoading(false);
        }

        private async Task GetSession()
        {
            SetLoading(true);
            var response = await _sessionManager.GetBy(sessionId);
            if (response.Succeeded)
            {
                _mySession = response.Data;
            }
            else
            {
                _ = await ShowErrors(response.Messages);
            }
            SetLoading(false);
        }

        private async Task<DialogResult> ShowErrors(List<string> responseMessages)
        {
            string buttonText = Localizer["Fermer"];
            var parameters = new DialogParameters();
            parameters.Add("Errors", responseMessages);


            var dialog = _dialogService.Show<ErrorDialog>(Localizer["Errors"], parameters);
            var result = await dialog.Result;
            return result;
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

        internal string GetDepotName(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id == "0" || _depotList.IsNullOrEmpty()) return "?";
            return _depotList.FirstOrDefault(x => x.CodeDepot.ToString() == id)?.Intitule ?? "?";
        }

        internal string GetUserName(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id == "0" || _userListe.IsNullOrEmpty()) return "?";
            var usr = _userListe.FirstOrDefault(x => x.Id.Equals(id));
            if (usr == null) return "?";
            
            return usr.FirstName + " " + usr.LastName;
        }
     
        private bool Search(ScanDTO session)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
            { return true; }
            if (session.CodeArticle?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            

            return false;
        }


        private async Task DeleteScanDetails(ScanDTO context)
        {
            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Suppression"],
                Localizer["Veuillez confirmer la suppression de la Quantité suivante :"] +
                context.QuantiteStock.ToString("N2"),
                Localizer["Supprimer"],
                Localizer["Annuller"], Color.Error);
            if (confirmation == true)
            {
                context.Deleted = true;
                context.DeletedTime = DateTime.Now;
                SetLoading(true);
            
                var response = await _sessionManager.UpdateScanAsync(context);
                if (response.Succeeded && response.Data)
                {
                }
                else
                {
                    _ = await ShowErrors(response.Messages);
                }
                SetLoading(false);
            }
        }
        
        private async Task ShowEcartDetails()
        {
            if (_mySession.Status == SessionStatus.Terminee || _mySession.Status == SessionStatus.Cloturee)
            {
                _navigationManager.NavigateTo($"/Logistics/Invenotry/Sessions/Ecarts/{sessionId}");
            }
           
        }

       
    }
}