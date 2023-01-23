using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Polly;
using Synaplic.Inventory.Client.Extensions;
using Synaplic.Inventory.Client.Infrastructure.Managers.Inventory;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Client.Shared.Dialogs;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Shared.Extensions;
using Synaplic.Inventory.Shared.Wrapper;
using Synaplic.Inventory.Transfer.Models;
using Synaplic.Inventory.Transfer.SageModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Synaplic.Inventory.Client.Pages.InventoryMobile;

public partial class InventorySessionMobileDetails
{
    [Parameter] public string sessionId { get; set; }

    [Inject] private ISessionManager _sessionManager { get; set; }
    [Inject] private ISageManager _sageManager { get; set; }


    //private bool _showNavigationArrows = true;
    private bool _isLoading = true;
    private SessionDTO _mySession = new();
    //private int _lineCount = 0;
    //private int _currentIndex = 0;

    private SageArticleDTO _currentSageArticle = new();

    private MudTextField<string> barcodeInputText;
    private MudTextField<string> SearchInputText;

    private bool isJsScannerActive;

    private List<SageArticleDTO> _articleListe = new();
    private List<SageArticleDTO> _articleFiltredListe = new();

    private List<ScanDTO> _allScanListe = new();
    private List<ScanDTO> _scanListe = new();
    private MudFab _scanButton;
    private ClaimsPrincipal _userClaims;
    private string _currentUserId;
    private decimal _scanTotalQte { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        _userClaims = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        _currentUserId = _userClaims.GetUserId();
    }

    protected override async Task OnInitializedAsync()
    {
        SetLoading(true);
        await GetTaskDetails();
        await GetArticles();
        await GetAllScansAsync();

        if (!_articleListe.IsNullOrEmpty())
        {
            await MoveToAsync(_articleListe.FirstOrDefault()?.CodeArticle, true);
            RefreshScanGrid();
        }
        SetLoading(false);
    }

    private async Task GetArticles()
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
            BackToParent();
        }


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
            BackToParent();
        }
    }

    private async Task GetTaskDetails()
    {
        SetLoading(true);
        var response = await _sessionManager.GetBy(sessionId);
        if (response.Succeeded)
        {
            _mySession = response.Data;
            if (_mySession.Status != SessionStatus.Encours)
            {
                _snackBar.Add(Localizer["Status modifié"]);
                await BackToParent();
            }
        }
        else
        {
            _ = await ShowErrors(response.Messages);
            await BackToParent();
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


    private async Task MoveToAsync(string scan, bool firstMove = false)
    {
        try
        {
            scan = scan.Trim();
        var o = _articleListe.FirstOrDefault(x => scan.Equals(x.CodeBarre, StringComparison.OrdinalIgnoreCase)
                                                  || scan.Equals(x.CodeArticle,
                                                      StringComparison.OrdinalIgnoreCase));
        if (o != null)
        {
            await  MoveToAsync(o, firstMove);
        }
        else
        {
            _snackBar.Add(Localizer["Article non indexé: "] + scan, Severity.Error);
            await FocusScanInput();
        }
        }
        catch (Exception ex)
        {
            _snackBar.Add(ex.Message, Severity.Error);
        }
    }

    private async Task MoveToAsync(SageArticleDTO obj, bool firstMove = false)
    {
        
           
            
                _currentSageArticle = obj;
                StateHasChanged();
                if (!firstMove) await AddScan(_currentSageArticle.CodeArticle);
            
       
    }

    private async Task AddScan(string codeArtcile)
    {
        var s =
            await InventoryScanDialog.GetQuantiteAsync(_dialogService, codeArtcile, _currentSageArticle.Designation);
        decimal.TryParse(s[0].ToString(), out var qte);

        var test = s[2];
        if (qte > 0 )
        {
            var scan = new ScanDTO()
            {
                CodeArticle = codeArtcile,
                QuantiteStock = qte,
                Id = Guid.NewGuid().ToString("N"),
                SessionId = _mySession.Id,
                ScanTime = DateTime.Now,
                NumLot = s[1].ToString(),
                DatePeremption = Convert.ToDateTime(s[2]),

            };

            _allScanListe.Add(scan);
        }

        RefreshScanGrid();
    }

    private async Task BackToParent()
    {
        if (_allScanListe.Any(o => o.CreatedBy == null))
        {
            var saved = true;
            var confirmation = await ConfirmationDialog.ShowDialogAsync(_dialogService,
                Localizer["Attention"],
                Localizer["Voulez-vous enregistrer vos scans avant de quitter :"],
                Localizer["Enregistrer"],
                Localizer["Non"],
                Color.Secondary);
            if (confirmation == true)
            {
                saved = await SaveScans();
            }

            if (confirmation != null && saved)
            {
                SetLoading(true);
                _navigationManager.NavigateTo($"/Logistics/Mobile/Sessions");
            }

        }
        else
        {
            SetLoading(true);
            _navigationManager.NavigateTo($"/Logistics/Mobile/Sessions");
        }
    }

    private void SetLoading(bool val)
    {
        _isLoading = val;
        StateHasChanged();
    }


    private async Task ScanBarCode()
    {
        SetLoading(true);


        var parameters = new DialogParameters();

        var options = new DialogOptions { CloseButton = true, FullScreen = true, DisableBackdropClick = true };
        var dialog = _dialogService.Show<BarcodeScannerDialog>(Localizer["Scan"], parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            Console.WriteLine(result.Data.ToString());
            await MoveToAsync(result.Data.ToString());
        }


        SetLoading(false);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            await InitializeBarcodeScanner();
        }
    }

    private async Task InitializeBarcodeScanner()
    {
        if (isJsScannerActive)
        {
            var dotNetObjectReference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("InitBarcodeScannerEvent", dotNetObjectReference);
        }
    }

    [JSInvokable]
    public async Task OnScanned(string barcode)
    {
        /*_snackBar.Add($"code :[{barcode.ToString()}]");*/
        await MoveToAsync(barcode);
    }

    //private string _scanedString;
    private async Task ValueInputChanged(string _scanedString)
    {
        if (!string.IsNullOrEmpty(_scanedString))
        {
            await _scanButton.FocusAsync();
            await MoveToAsync(_scanedString);
            await barcodeInputText.Clear();
        }
    }

    private async Task FocusScanInput()
    {
        await barcodeInputText.FocusAsync();
    }

    private async Task DeleteScan(ScanDTO context)
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
            RefreshScanGrid();
        }
    }

    private void RefreshScanGrid()
    {
        _scanListe = _allScanListe.Where(o => o.CodeArticle == _currentSageArticle.CodeArticle && !o.Deleted)
            .OrderByDescending(o => o.ScanTime)
            .ToList();
        _currentSageArticle.SumScannedQte = _scanListe?.Sum(o => o.QuantiteStock) ?? 0;
      
        StateHasChanged();
    }

    private async Task<bool> SaveScans()
    {
        var nonsavedScan = _allScanListe.Where(o => o.CreatedBy == null).ToList();
        if (!nonsavedScan.IsNullOrEmpty())
        {
            SetLoading(true);

            try
            {
                var response = await _sessionManager.SaveScansAsync(nonsavedScan);
                if (response.Succeeded && response.Data)
                {
                    foreach (var scanDto in nonsavedScan)
                    {
                        scanDto.CreatedBy = _currentUserId;
                    }

                    return true;
                }
                else
                {
                    _ = await ShowErrors(response.Messages);
                }
            }
            catch (HttpRequestException e)
            {

                _snackBar.Add(Localizer["Erreur de connexion, veuillez vérifier votre connexion Internet puis réessayer!"], Severity.Error);
            }

            SetLoading(false);

        }
        else
        {
            return true;
        }
        return false;
    }

    private bool ShowListe;
    private async Task ToggleSearch()
    {
        ShowListe = !ShowListe;
        if (ShowListe)
        {
 
            SearchArticle(string.Empty);
            StateHasChanged();
            if (SearchInputText != null)
            {
                await SearchInputText.Clear();
                await SearchInputText.FocusAsync();
            }
            
        }

    }


    private void SearchArticle(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            _articleFiltredListe = _articleListe.Take(100).ToList();
        }
        else
        {
            _articleFiltredListe = _articleListe.Where(o => o.CodeArticle.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                                           || o.Designation.Contains(searchString, StringComparison.OrdinalIgnoreCase)).Take(100).ToList();
        }
    }

    private async Task ArticleSelected(SageArticleDTO arg)
    {
        ShowListe = false;
        await MoveToAsync(arg);
    }
}