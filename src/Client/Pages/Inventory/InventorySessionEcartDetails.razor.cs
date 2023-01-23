using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Synaplic.Inventory.Client.Extensions;
using Synaplic.Inventory.Client.Infrastructure.Managers.Inventory;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Client.Shared.Dialogs;
using Synaplic.Inventory.Shared.Constants.Permission;
using Synaplic.Inventory.Shared.Extensions;
using Synaplic.Inventory.Transfer.Models;
using Synaplic.Inventory.Transfer.SageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Synaplic.Inventory.Client.Pages.Inventory
{
    public partial class InventorySessionEcartDetails 
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
        /*private List<SageArticleDTO> _articleListe = new();*/
        private List<StockDTO> _allScanListe = new();
        private List<StockDTO> _filtredScanListe = new();
        private ClaimsPrincipal _userClaims;
        private string _currentUserId;
        private int _filterStockType;


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
            await GetSession();
            /*await GetArticles();*/
            await GetAllStockAsync();
            await GetDepots();
            SetLoading(false);
        }

        /*private async Task GetArticles()
        {
            if (_mySession == null) return;

            SetLoading(true);
            var response = await _sageManager.GetArticlesByFamilleAsync(_mySession.Filtre);
            if (response.Succeeded)
            {
                _articleListe = response.Data;
            }
            else
            {
                _ = await ShowErrors(response.Messages);
            }
            SetLoading(false);
        }*/

        private async Task GetAllStockAsync()
        {
            if (_mySession == null) return;
             
            SetLoading(true);
            var response = await _sessionManager.GetAllStocksAsync(sessionId);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Data.Count().ToString());
                _allScanListe = response.Data;
                FilterGrid(_filterStockType);
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

     
        private bool Search(StockDTO session)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
            { return true; }
            if (session.CodeArticle?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            
            if (session.Designation?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            
            if  (session.Famille?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }


        private async Task ExcludeStock(StockDTO context)
        {
            bool? confirmation;
            if (context.Exclure)
            {
                confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                    Localizer["Inclusion"],
                    Localizer["Veuillez confirmer l'inclusion de l'article :"] +  context.Designation ,
                    Localizer["Inclure"],
                    Localizer["Annuler"], Color.Success);
            }
            else
            {
                confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                    Localizer["Exclusion"],
                    Localizer["Veuillez confirmer l'exclusion de l'article :"] +  context.Designation ,
                    Localizer["Exclure"],
                    Localizer["Annuler"], Color.Error);
            }
             
            if (confirmation == true)
            {
                context.Exclure = !context.Exclure;
                context.DateExclusion = DateTime.Now;
                SetLoading(true);
                try
                {
                    var response = await _sessionManager.SaveStockAsync(new List<StockDTO>() {context});
                    if (response.Succeeded && response.Data)
                    {
                    }
                    else
                    {
                        _ = await ShowErrors(response.Messages);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
               
                SetLoading(false);
            }
        }
        
       
        private async Task GenerateEcartDetails()
        {
            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Rgénération"],
                Localizer["La regeneration écraser toute modifification du rapport,êtes vous sûr de vouloir regénérer ?"]   ,
                Localizer["Regénérer"],
                Localizer["Annuler"], Color.Warning);
            if (confirmation == true)
            {
                var response = await _sessionManager.GenerateEcartsAsync(_mySession);
                if (response.Succeeded && response.Data)
                {
                    await GetAllStockAsync();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task UpdateJustification( StockDTO context , string justification)
        {
            if (context.JusftifEcart == Strings.Trim(justification)) return;
             
            try
            {
                context.JusftifEcart = Strings.Trim(justification);
                var response = await _sessionManager.SaveStockAsync(new List<StockDTO>() {context});
                if (response.Succeeded && response.Data)
                {
                }
                else
                {
                    _ = await ShowErrors(response.Messages);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            StateHasChanged();
        }
        
        private async Task UpdatePrice(StockDTO context, decimal value)
        {
            if (context.ValeurUnitaire == value) return;
             
            try
            {
                context.ValeurInventaire =  value * context.QuantiteInventaire;
                var response = await _sessionManager.SaveStockAsync(new List<StockDTO>() {context});
                if (response.Succeeded && response.Data)
                {
                }
                else
                {
                    _ = await ShowErrors(response.Messages);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            StateHasChanged();
        }

        private void FilterGrid(int i)
        {
            _filterStockType = i;
            switch (i)
            {
                case 1  : _filtredScanListe = _allScanListe.Where(o=> o.EcartStock != 0).ToList();
                    break;
                case 2  :  _filtredScanListe = _allScanListe.Where(o=> o.QuantiteMouvement != 0).ToList();
                    break;
                case 3  :  _filtredScanListe = _allScanListe.Where(o=> o.Exclure).ToList();
                    break;
                default : _filtredScanListe = _allScanListe;
                    break;
                    
            }
            StateHasChanged();
        }


      
    }
}