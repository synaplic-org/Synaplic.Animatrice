using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Synaplic.Inventory.Client.Infrastructure.Managers.Inventory;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Client.Shared.Dialogs;
using Synaplic.Inventory.Shared.Constants.Permission;
using Synaplic.Inventory.Transfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Pages.InventoryMobile;

public partial class InventorySessionMobilePage
{
    [Inject] private ISessionManager _sessionManager { get; set; }
    [Inject] private ISageManager _sageManager { get; set; }

    [Parameter] public string ProcessType { get; set; }

    private ClaimsPrincipal _currentUser;
    private bool enabled = true;
    private bool _loading;
    private string _searchString = "";
    private List<SessionDTO> _taskList = new();
    private List<SessionDTO> _mytaskList = new();
    private List<SessionDTO> _unassignedtaskList = new();
    private List<string> _MytaskIds = new();
    private bool scannerIsClosed = true;
    private string code = "";
    MudTabs tabs;
    MudTextField<string> scaninput;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }


    private bool _canScan, _canOpenSession;

    protected override async Task OnParametersSetAsync()
    {
        var _authenticationStateProviderUser = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        
        _canScan = (await _authorizationService.AuthorizeAsync(_authenticationStateProviderUser,
            Permissions.Inventory.Scan)).Succeeded;
        _canOpenSession =
            (await _authorizationService.AuthorizeAsync(_authenticationStateProviderUser,
                Permissions.Inventory.OpenSession)).Succeeded;
    }

    private void OpenTask(SessionDTO dto)
    {
        _navigationManager.NavigateTo($"/Logistics/Mobile/Sessions/Details/{dto.Id}");
    }

    private async Task LoadData()
    {
        _loading = true;
        _searchString = string.Empty;
        var myresponse = await _sessionManager.GetActiveSessions();
        var unassignReponse = await _sessionManager.GetWaitingSessions();
        if (myresponse.Succeeded && unassignReponse.Succeeded)
        {
            _mytaskList = myresponse.Data.ToList();
            _unassignedtaskList = unassignReponse.Data.ToList();
        }
        else
        {
            foreach (var message in myresponse.Messages)
            {
                _snackBar.Add(message, Severity.Error);
            }

            foreach (var message in unassignReponse.Messages)
            {
                _snackBar.Add(message, Severity.Error);
            }
        }

        scaninput.Reset();
        _loading = false;
    }

    private bool Search(SessionDTO taskDTO)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (taskDTO.Designation.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        return false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await FocusScanInput();
    }

    private async Task FocusScanInput()
    {
        await scaninput.FocusAsync();
    }

    private async Task OpenSessionAsync(SessionDTO session)
    {
        _loading = true;
        StateHasChanged();
        var response = await _sessionManager.OpenSessionAsync(session);
        if (response.Succeeded)
        {
            if (response.Data)
            {
                tabs.ActivatePanel(0);
                await LoadData();
            }
            else
            {
                _snackBar.Add(Localizer["Erreur de l'ouverture de la session"], Severity.Error);
            }
        }
        else
        {
            foreach (var message in response.Messages)
            {
                _snackBar.Add(message, Severity.Error);
            }
        }

        _loading = false;
        await FocusScanInput();
    }

    private void SetLoading(bool val)
    {
        _loading = val;
        StateHasChanged();
    }

    private async Task BackToParent()
    {
        SetLoading(true);

        _navigationManager.NavigateTo($"/Home");
        await Task.Delay(1);
        SetLoading(false);
    }

    private async Task ScanBarCode()
    {
        // SetLoading(true);


        var parameters = new DialogParameters();

        var options = new DialogOptions {CloseButton = true, FullScreen = true, DisableBackdropClick = true};
        var dialog = _dialogService.Show<BarcodeScannerDialog>(Localizer["Scan"], parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await scaninput.SetText(result.Data.ToString());
        }


        SetLoading(false);
    }

    private async Task Refresh()
    {
        SetLoading(true);


        await LoadData();


        SetLoading(false);
    }
}