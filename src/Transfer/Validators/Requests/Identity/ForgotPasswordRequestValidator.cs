using FluentValidation;
using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Transfer.Requests.Identity;

namespace Synaplic.Inventory.Transfer.Validators.Requests.Identity
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer)
        {
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["L'e-mail est requis"])
                .EmailAddress().WithMessage(x => localizer["L'e-mail n'est pas correct"]);
        }
    }
}