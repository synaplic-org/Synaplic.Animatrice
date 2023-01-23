using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Transfer.SageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MudBlazor;
using Synaplic.Inventory.Client.Shared.Dialogs;
using Synaplic.Inventory.Shared.Extensions;

namespace Synaplic.Inventory.Client.Pages.Stock
{
    public partial class CurrentStock
    {
        [Inject] private ISageManager _logisticAreaManager { get; set; }
    
        [Inject] private ISageManager _sageManager { get; set; }
        
        private List<SageStockDTO> _stockDtos = new();
        private List<SageDepotDTO> _depotDtos = new();
        private List<SageFamilleDTO> _familleDtos = new();
        
        private ClaimsPrincipal _currentUser = new();
        
        private int _depoID = 0;
        private string _codeArticle = string.Empty;
        private string _designiationArticle = string.Empty;
        
        private string _familleFilter = string.Empty;
        
        private bool _isOpen = true;
        private bool _isLoading = true;
        
        protected override async Task OnParametersSetAsync()
        {
            _currentUser = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            SetLoading(true);
            await GetDepots();
            await GetFamilles();
            SetLoading(false);
        }
        
        private async Task GetDepots()
        {
            var response = await _sageManager.GetDepotsAsync();
            if (response.Succeeded)
            {
                _depotDtos = response.Data.ToList();
                if (_depoID == 0 )
                {
                    _depoID = _depotDtos.First()?.CodeDepot ?? 0;
                }
            }
            else
            {
                await  ShowErrors(response.Messages);
            }
        }

        private async Task GetFamilles()
        {
            var response = await _sageManager.GetFamillesAsync();
            if (response.Succeeded)
            {
                _familleDtos = response.Data.ToList();
            }
            else
            {
                await  ShowErrors(response.Messages);
            }
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
        
        private void SetLoading(bool val)
        {
            _isLoading = val;
            StateHasChanged();
        }
        
        private void Return()
        {
            _navigationManager.NavigateTo("/Home");
        }

        private async Task Search()
        {
            SetLoading(true);
            if(_depoID == 0)
            {
                _snackBar.Add(Localizer["Veuillez selectioner un dépôt!"], MudBlazor.Severity.Error);
                return;
            }
            _isOpen = false;
            _stockDtos = new List<SageStockDTO>();
            StateHasChanged();
            var response = await _sageManager.GetStockADate(_depoID,DateTime.Now,_familleFilter);
            if (response.Succeeded)
            {

                var query = response.Data.AsEnumerable();
                if (!string.IsNullOrWhiteSpace(_designiationArticle))
                {
                    query = query.Where(o => o.Designation.Contains(_designiationArticle));
                }
                
                if (!string.IsNullOrWhiteSpace(_codeArticle))
                {
                    query = query.Where(o => o.Reference.Contains(_designiationArticle));
                }
                _stockDtos = query.ToList();
            }
            else
            {
                await ShowErrors(response.Messages);
            }

            SetLoading(false);
        }
        
        internal string GetDepotName(string id )
        {
            if (string.IsNullOrWhiteSpace(id) || id == "0" || _depotDtos.IsNullOrEmpty()) return "?";
            return _depotDtos.FirstOrDefault(x => x.CodeDepot.ToString() == id)?.Intitule ?? "?";
        }
        
        internal string GetFamillyName(string id )
        {
            if (string.IsNullOrWhiteSpace(id) || id == "0" || _familleDtos.IsNullOrEmpty()) return "?";
            return _familleDtos.FirstOrDefault(x => x.CodeFamille.Equals(id,StringComparison.OrdinalIgnoreCase))?.Intitule ?? "?";
        }

        
        private string CardStyle(bool b)
        {
            if (b) return "margin: 5px; border-left: solid 3px #ff6700;";
            return "margin: 5px; border-left: solid 3px #25638d;";
        }
        public void ToggleOpen()
        {
            if (_isOpen)
                _isOpen = false;
            else
                _isOpen = true;
        }
    }
}
