using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using Synaplic.Inventory.Client.Extensions;
using Synaplic.Inventory.Client.Infrastructure.Managers.Inventory;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Shared.Constants.Application;
using Synaplic.Inventory.Transfer.Models;
using Synaplic.Inventory.Transfer.SageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Pages.Inventory
{
    public partial class InevntorySessionModal
    {
        [Inject] private ISessionManager _sessionManager { get; set; }
        [Inject] private ISageManager _sageManager { get; set; }



        //private List<DepotDTO> _depotList = new();
        private List<SageFamilleDTO> _familleList = new();

        [Parameter] public SessionDTO SessionModel { get; set; } = new();
        [Parameter] public List<SageDepotDTO> _depotList { get; set; } = new ();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
  

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }


        protected override async Task OnInitializedAsync()
        {

            //await GetDepots();
            await GetFiltres();
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
        private async Task GetFiltres()
        {
            var response = await _sageManager.GetFamillesAsync();
            if (response.Succeeded)
            {
                _familleList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task SaveAsync()
        {


                var response = await _sessionManager.SaveAsync(SessionModel);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    MudDialog.Close();
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
}