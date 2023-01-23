using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Synaplic.Inventory.Transfer.Requests.Identity
{
    public class TokenRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

   
}