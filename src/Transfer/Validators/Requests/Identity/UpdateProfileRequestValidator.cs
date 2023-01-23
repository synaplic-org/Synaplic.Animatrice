using FluentValidation;
using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Transfer.Requests.Identity;

namespace Synaplic.Inventory.Transfer.Validators.Requests.Identity
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator(IStringLocalizer<UpdateProfileRequestValidator> localizer)
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Le prénom est requis"]);
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Nom de famille est requis"]);
        }
    }
}