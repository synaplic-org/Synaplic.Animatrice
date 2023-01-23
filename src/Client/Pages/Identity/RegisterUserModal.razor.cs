using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Synaplic.Inventory.Client.Infrastructure.Managers.Sage;
using Synaplic.Inventory.Shared.Enums;
using Synaplic.Inventory.Transfer.Requests.Identity;
using Synaplic.Inventory.Transfer.SageModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Client.Pages.Identity
{
    public partial class RegisterUserModal
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private readonly RegisterRequest _registerUserModel = new();
 
        private UserType usertype = UserType.Gestion;
       
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        protected override async void OnInitialized()
        {
            await GetDepotsAsync();
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
        private async Task SubmitAsync()
        {
            //_registerUserModel.EmployeeID = employee.EmployeeID;
            _registerUserModel.UserName = _registerUserModel.Email;
            var response = await _userManager.RegisterUserAsync(_registerUserModel);
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

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        
    }
}