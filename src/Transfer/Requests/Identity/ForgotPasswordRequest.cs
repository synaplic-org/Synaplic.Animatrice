using System.ComponentModel.DataAnnotations;

namespace Synaplic.Inventory.Transfer.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}