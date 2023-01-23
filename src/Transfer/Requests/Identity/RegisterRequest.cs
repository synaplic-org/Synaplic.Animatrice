using Synaplic.Inventory.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Synaplic.Inventory.Transfer.Requests.Identity
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string EmployeeID { get; set; }
        public string SiteID { get; set; } = "*";
        public string PhoneNumber { get; set; }

        public UserType UserType { get; set; } = UserType.Mobile;
        public bool ActivateUser { get; set; } = true;
        public bool AutoConfirmEmail { get; set; } = true;
    }
}