using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Synaplic.Inventory.Shared.Constants.Application;
using Synaplic.Inventory.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Synaplic.Inventory.Shared.Constants.Identity;
using Synaplic.Inventory.Transfer.Responses.Identity;
using Microsoft.AspNetCore.Components;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Transfer.SageModels;

namespace Synaplic.Inventory.Client.Pages.Identity
{
    public partial class Users
    {
        

        private List<UserResponse> _userList = new();
      

        private UserResponse _user = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateUsers;
        private bool _canSearchUsers;
        private bool _canExportUsers;
        private bool _canViewRoles;
        private bool _loaded;
       
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create)).Succeeded;
            _canSearchUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Search)).Succeeded;
            _canExportUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Export)).Succeeded;
            _canViewRoles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.View)).Succeeded;

            await GetDepotsAsync();
            await GetUsersAsync();
            _loaded = true;
        }

        private async Task GetUsersAsync()
        {
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _userList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        [Inject] private ISageManager _sageManager { get; set; }
        private List<SageDepotDTO> _depotList = new();
        private async Task GetDepotsAsync()
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

        private string GetDepotName(string SiteID)
        {
            if (SiteID == "*" ) return "#TOUS#";
            return _depotList.FirstOrDefault(o => o.CodeDepot.ToString() == SiteID)?.Intitule;
            
        }

        private bool Search(UserResponse user)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (user.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private async Task ExportToExcel()
        {
            var base64 = await _userManager.ExportToExcelAsync(_searchString);
            await _jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = $"{nameof(Users).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
            _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                ? Localizer["Users exported"]
                : Localizer["Filtered Users exported"], Severity.Success);
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<RegisterUserModal>(Localizer["Register New User"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetUsersAsync();
            }
        }

        private void ViewProfile(string userId)
        {
            _navigationManager.NavigateTo($"/user-profile/{userId}");
        }

        private void ManageRoles(string userId, string email)
        {
            if (email == UserConstants.SuperAdminEmail) _snackBar.Add(Localizer["Not Allowed."], Severity.Error);
            else _navigationManager.NavigateTo($"/identity/user-roles/{userId}");
        }
    }
}